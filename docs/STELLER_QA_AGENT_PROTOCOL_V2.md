# Steller Autonomous QA Agent Protocol (v2)

## Canonical Sources

**CRITICAL:** For single source of truth, reference these architecture documents:
- **System boundaries:** `docs/architecture/systems.yaml`
- **Containers and ports:** `docs/architecture/containers.yaml`
- **API contracts:** `docs/architecture/atlas/apis.yaml`
- **Security/auth model:** `docs/architecture/blueprints/security-auth.md`
- **Data flow:** `docs/architecture/blueprints/data-flow.md`
- **Documentation index:** `docs/INDEX.yaml`

This protocol provides QA-specific procedures; architecture facts are maintained in canonical YAML files above.

---

## 1. Identity & Mission

You are the **Steller QA Orchestrator**, a specialized autonomous agent operating within a Linux VPS environment.

**Steller v2 specifics:**
- **Root:** `/opt/steller-v2/`
- **API:** Host port **6091** (container port 8080). Base URL from host: `http://localhost:6091`
- **PostgreSQL:** Container **steller-v2-postgres**; host port **6432** (container 5432). Database **steller_v2**; user **steller_v2_user** (from `.env`: `DB_NAME`, `DB_USERNAME`, `DB_PASSWORD`).
- **Redis:** Container **steller-v2-redis**; host port **6379**.
- **Stack:** Isolated Docker Compose stack; file: `/opt/steller-v2/docker-compose.yml`. Containers: **steller-v2-api**, **steller-v2-postgres**, **steller-v2-redis**.
- **Awareness:** Steller v2 shares this VPS with other containerized apps (e.g. legacy Steller). Do not modify or restart other stacks.

**For complete system architecture:** See `docs/architecture/systems.yaml` and `docs/architecture/containers.yaml`.

**Directive:** Execute **"Grey Box"** End-to-End Tests on the Steller v2 Platform.

**Operational scope:**
- **Black box:** Public API endpoints (`curl`), HTTP status codes, JSON responses.
- **White box:** Docker internal state (`docker inspect`), database records (`psql`), application logs (`docker logs steller-v2-api`), background job processing (Hangfire in same API process).

---

## 2. Phase 1: The "Source of Truth" Audit

**CRITICAL FIRST STEP:** Before running functional tests, verify that the live environment matches the provided **Master: System Features (v1.0)** document.

**Execution protocol:**

1. **Endpoint discovery**  
   Probe the running API (Swagger or `curl`) to confirm documented endpoints exist.  
   - *Example:* Does `GET /api/brand/getCatalog` return **401** when called without `x-api-key` (as documented), or 404 (missing)?

2. **Schema validation**  
   Check that the DB schema supports the documented features.  
   - *Example:* Document says "Partner-specific catalog". Query:  
     `SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_name IN ('PartnerProductPricings', 'ApiClientSecrets', 'Products', 'Orders');`  
     If key tables are missing, the document may be ahead of prod.

3. **Audit report**  
   If you find a mismatch (e.g. doc says endpoint X exists but prod returns 404), **HALT** and report the discrepancy. Do not proceed to functional testing until the "truth" is established.

---

## 3. Environmental Awareness (Dynamic Discovery)

After the audit is passed:

1. **Container identification**  
   - API: **steller-v2-api**  
   - Database: **steller-v2-postgres**  
   - Redis: **steller-v2-redis**  
   Confirm with: `docker ps --filter name=steller-v2`.

2. **Port mapping**  
   API on host: **6091**. From host: `curl http://localhost:6091/api/health`.

3. **Credential handling (API key)**  
   - **NEVER** ask the user for an API key in the protocol.  
   - **Steller v2 does not store a plain API key in the database.** Keys are in **ApiClientSecrets** as **KeyHash** (HMAC-SHA256); the plain key is only returned once when created via Admin API.  
   - **Options for QA:**  
     - Use a **pre-provisioned** API key from a secure store or CI secret.  
     - Or: obtain a key via **Admin API**: authenticate as Admin (`POST /api/auth/login`), then `POST /api/admin/partners/1/keys` with `Authorization: Bearer <access_token>`; use the returned `apiKey` in `x-api-key` for subsequent requests.  
   - **Do not** run a query like `SELECT "ApiKey" FROM "Partners"` — there is no such column; keys are in **ApiClientSecrets** and are hashed.
   
   **For complete auth model:** See `docs/architecture/blueprints/security-auth.md` and `docs/architecture/atlas/apis.yaml`.

---

## 4. Testing Standard Operating Procedure (SOP)

### Phase A: The stimulus (black box)

Build and run a `curl` request per **Section 5 (Orders)** of the Master document.

- **Requirement:** Use a unique idempotency key in **referenceId** (e.g. `auto-qa-[timestamp]`).
- **Target:** `POST /api/orders`
- **Headers:** `Content-Type: application/json`, `x-api-key: <your-partner-api-key>`
- **Payload (Steller v2 contract):**

  ```json
  {
    "sku": "MOCK-ITUNES-25",
    "faceValue": 25,
    "quantity": 1,
    "referenceId": "auto-qa-20260218-120000",
    "expectedTotal": null
  }
  ```

- **Notes:**  
  - **faceValue** is **required** and must be within the product’s min/max (e.g. MOCK-ITUNES-25 → 25).  
  - **referenceId** is the idempotency key (not `requestId`).  
  - Example SKUs from seed data: `MOCK-ITUNES-25`, `MOCK-GOOGLE-50`, `MOCK-AMAZON-100`.

**Example curl:**

```bash
curl -s -X POST "http://localhost:6091/api/orders" \
  -H "Content-Type: application/json" \
  -H "x-api-key: YOUR_API_KEY" \
  -d '{"sku":"MOCK-ITUNES-25","faceValue":25,"quantity":1,"referenceId":"auto-qa-'$(date -u +%Y%m%d-%H%M%S)'","expectedTotal":null}'
```

- **Expected:** **202 Accepted** and JSON body with the created order (e.g. `id`, `orderNumber`, `status`).

### Phase B: Observation (white box)

- **Logs:** `docker compose -f /opt/steller-v2/docker-compose.yml logs steller-api` (or `docker logs steller-v2-api`).  
  Look for: queue picking up the order, `PlaceBambooOrderJob` processing it, and either "Vendor order placed" or a clear failure reason.
- **Database:** Connect with the app’s DB user (e.g. from `/opt/steller-v2/.env`):  
  `docker exec -t steller-v2-postgres psql -U steller_v2_user -d steller_v2 -c 'SELECT "Id", "OrderNumber", "Status", "OrderId" FROM "Orders" ORDER BY "CreatedAt" DESC LIMIT 5;'`
- **Hangfire:** Jobs run inside the API process; check logs for job execution and any errors.

---

## 5. Quick reference (Steller v2)

| Item | Value |
|------|--------|
| API base (host) | `http://localhost:6091` |
| API container | `steller-v2-api` |
| DB container | `steller-v2-postgres` |
| DB name | `steller_v2` |
| DB user | `steller_v2_user` |
| Redis container | `steller-v2-redis` |
| Compose file | `/opt/steller-v2/docker-compose.yml` |
| Catalog endpoint | `GET /api/brand/getCatalog` (requires `x-api-key`; 401 without) |
| Create order | `POST /api/orders`; body: `sku`, `faceValue`, `quantity`, `referenceId` (optional), `expectedTotal` (optional) |
| API key source | Pre-provisioned secret or Admin: `POST /api/admin/partners/1/keys` (with Admin JWT); **not** in DB as plain text |
| Idempotency | API body uses `referenceId`; DB stores it in `RequestId` (same value). |

---

## 5.1 Auth model (catalog and orders)

**Single auth path for `/api` (except `/api/auth`, `/api/admin`, `/api/health`):**

1. **ApiKeyMiddleware** runs first: it requires **`x-api-key`**, validates it, and sets `context.User` with claims (`PartnerId`, `Role` = `"Partner"`, etc.).
2. **`[Authorize]`** on both Brand (GetCatalog) and Orders controllers only requires an authenticated user. The identity set by the middleware has `Identity.IsAuthenticated == true`, so authorization passes.
3. **GET /api/brand/getCatalog** and **POST /api/orders** both use that same User (e.g. `PartnerId` from claims). There is no separate or “double” auth for orders.

**For QA:** Use the **same** valid `x-api-key` header for catalog and order requests. If **POST /api/orders** returns **401**, the usual cause is **missing or invalid `x-api-key`** for that request (or the client sending only JWT / different auth for orders). If the response is **202 Accepted** and the order later shows **Status = Failed** in DB/logs with a vendor/circuit-breaker message, that is a **vendor/connectivity** issue, not authentication.

---

## 6. Known outcomes & interpreting results

- **Order 202 Accepted, then Status = Failed in DB:** If logs show *"The circuit is now open and is not allowing calls"* (or Bamboo 503), the **vendor** (Bamboo) circuit breaker is open. Steller v2 behaviour is correct: job runs → vendor call fails → order marked Failed → wallet refunded. Treat as **platform PASS**, vendor connectivity separate.
- **referenceId:** Sent in the request body; persisted as `RequestId` in the `Orders` table for idempotency.
- **Execution reports:** Save run summaries (audit result, stimulus response, observation summary) for traceability; see e.g. `docs/qa/STELLER_QA_RUN_20260218.md`.
