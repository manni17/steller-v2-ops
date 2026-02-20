# Data Flow Blueprint

**Purpose:** Describes how data flows through the Steller v2 system, from partner requests to vendor orders. Includes background jobs, catalog sync, and Bamboo calls.

**Last Updated:** 2026-02-20

**Source:** Extracted from `/opt/steller-v2/steller-backend/`

---

## Overview

Steller v2 processes gift card orders through a multi-stage pipeline: Partner API → Order Creation → Direct Hangfire Enqueue → PlaceBambooOrderJob → Bamboo API → Database Updates. Catalog sync runs via BrandBackgroundService (every 30 min) and optional manual POST /api/brand/sync-catalog.

---

## High-Level Flow

```
Partner → POST /api/orders (x-api-key) → OrderService → DB commit → Enqueue PlaceBambooOrderJob
                ↓                                                              ↓
            PostgreSQL                                              Hangfire (PlaceBambooOrderJob)
                                                                              ↓
                                                                   IVendorAdapter.PlaceOrderAsync
                                                                              ↓
                                                                    Bamboo POST /checkout
                                                                              ↓
                                                                    PollBambooOrderJob (if success)
                                                                              ↓
                                                                    Bamboo GET /orders/{id}
                                                                              ↓
                                                                    DB: Status Completed/Failed
```

---

## Detailed Flow

### 1. Order Creation (Synchronous)

**Flow:**
1. Partner sends `POST /api/orders` with `x-api-key` header
2. ApiKeyMiddleware validates API key and sets `context.User` with `PartnerId`
3. OrderService.CreateOrderAsync: idempotency check → pricing → wallet debit → DB insert → commit
4. **Immediately after commit:** `PlaceBambooOrderJob` is enqueued to Hangfire (order.Id)
5. API returns `202 Accepted` with order details

**Data Written:**
- `Orders` table: Order record with `Status = Processing`
- `RequestId` column stores `referenceId` (idempotency key)

**Components:**
- `steller-v2-api` → `steller-v2-postgres`

---

### 2. Order Background Processing (Hangfire)

**Primary Path (POST /api/orders):**
1. OrderService directly enqueues `PlaceBambooOrderJob` after transaction commit
2. Hangfire picks up job
3. PlaceBambooOrderJob calls `IVendorAdapter.PlaceOrderAsync` → Bamboo `POST /api/integration/v2.0/orders/checkout`
4. On success: enqueues `PollBambooOrderJob` to poll Bamboo for order status
5. On failure: Order status set to `Failed`, wallet refunded (if applicable)

**Backfill Path (OrderQueueBackgroundService):**
1. `OrderQueueBackgroundService` runs every **1 minute** (IHostedService)
2. Finds orders from last 24h with `OrderId == 0`, status not Failed/Succeeded
3. Enqueues `OrderProcessingJob` per order with **Order.Id** (PK); PlaceBambooOrderJob looks up by Id
4. OrderProcessingJob enqueues `PlaceBambooOrderJob(order.Id)` (same as primary path)
5. Admin can trigger backfill via `POST /api/orderqueue/queue-by-date-range`

**Components:**
- `steller-v2-hangfire` (embedded in API) → `steller-v2-postgres`
- `steller-v2-hangfire` → `IVendorAdapter` (BambooVendorAdapter) → Bamboo API

---

### 3. Catalog Sync

**BrandBackgroundService (Primary Source of Truth):**
- Runs every **30 minutes** (IHostedService)
- Calls `SyncCatalogFromBambooAsync(1)` via BrandService
- Uses ExternalApiService only (single path, Legacy parity): `GET {BaseUrlV1}/catalog`, `GET {BaseUrlV2}/categories`
- Auth: Basic ClientId:ClientSecret (same as Legacy)
- 429 handling: Parses Bamboo "wait for X seconds" and calls `SetRetryAfter("bamboo_catalog"|"bamboo_categories", X)` (Legacy parity)
- Updates Products and Brands in PostgreSQL

**Manual Trigger:**
- `POST /api/brand/sync-catalog` — AllowAnonymous, ApiKeyMiddleware skips
- Same logic as BrandBackgroundService

**CatalogSyncJob (Hangfire Recurring — DISABLED):**
- Was scheduled every 1 hour (`0 */1 * * *`)
- Disabled so BrandBackgroundService is sole source (Immediate Fix)
- Re-enable when adding rate limiter to job

---

### 4. Catalog Retrieval (Synchronous)

**Flow:**
1. Partner sends `GET /api/brand/getCatalog` with `x-api-key`
2. ApiKeyMiddleware validates API key
3. Brand controller queries PostgreSQL for partner-specific products
4. Catalog filtered by `PartnerId` (from API key claims)
5. Returns JSON catalog

**Data Read:**
- `Products` table
- `PartnerProductPricings` table (partner-specific pricing)

**Components:**
- `steller-v2-api` → `steller-v2-postgres`

---

## Hangfire Recurring Jobs

| Job | Schedule | Purpose |
|-----|----------|---------|
| ReconciliationJob | `*/5 * * * *` (every 5 min) | Reconciliation engine |
| CatalogSyncJob | **DISABLED** | Was catalog sync; now BrandBackgroundService only |
| AdminAlertJob | `*/10 * * * *` (every 10 min) | Admin alert checks |
| WalletConsistencyJob | `0 * * * *` (every hour) | Wallet reconciliation |

---

## Bamboo Calls

| Call | Endpoint | Used By |
|------|----------|---------|
| Place order | `POST /api/integration/v2.0/orders/checkout` | PlaceBambooOrderJob via IVendorAdapter |
| Get order | `GET /api/integration/v1.0/orders/{requestId}` | PollBambooOrderJob |
| Catalog | `GET /api/integration/v1.0/catalog` | BrandBackgroundService, SyncCatalogFromBamboo, BrandService |
| Categories | `GET /api/integration/v2.0/categories` | BrandService (when fetching catalog) |

**Auth:** Bamboo uses Basic Authentication (username/password configured in Steller v2).

**Circuit Breaker:**
- Implemented to prevent cascading failures
- If Bamboo unavailable, circuit opens; orders fail with `Status = Failed`
- Expected behavior (not a bug)

---

## Database Schema (Key Tables)

| Table | Purpose | Key Fields |
|-------|---------|------------|
| Orders | Order records | Id, OrderNumber, Status, RequestId, PartnerId, OrderId (Bamboo) |
| Products | Product catalog | Id, SKU, Name, MinFaceValue, MaxFaceValue |
| PartnerProductPricings | Partner-specific pricing | PartnerId, ProductId, Price |
| ApiClientSecrets | API key storage | PartnerId, KeyHash (HMAC-SHA256) |
| Partners | Partner information | Id, Name |

**Guardrails:**
- API keys stored as `KeyHash` - never plain text
- Do not query for plain API key (does not exist)
- `referenceId` from request stored as `RequestId` in Orders table

---

## Hangfire Job Processing

**Job Name:** `PlaceBambooOrderJob`

**Execution:**
- Runs inside `steller-v2-api` process (not separate container)
- Enqueued directly by OrderService after DB commit (primary path)
- Or by OrderProcessingJob (backfill via OrderQueueBackgroundService)
- Retries on failure (Hangfire retry policy)
- On success: enqueues `PollBambooOrderJob`

**Monitoring:**
- Check logs: `docker logs steller-v2-api | grep PlaceBambooOrderJob`
- Hangfire dashboard: typically at `/hangfire` (auth required)

---

## Agent-Relevant

### Observing Data Flow

**Black Box (API):**
- Send requests: `curl -X POST http://localhost:6091/api/orders ...`
- Check responses: HTTP status codes, JSON bodies

**White Box (Database):**
- Query orders: `docker exec -t steller-v2-postgres psql -U steller_v2_user -d steller_v2 -c 'SELECT * FROM "Orders" ORDER BY "CreatedAt" DESC LIMIT 5;'`
- Check schema: `SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';`

**Logs:**
- API logs: `docker logs steller-v2-api`
- Look for: "PlaceBambooOrderJob", "Vendor order placed", circuit breaker messages

### Common Patterns

**Idempotency:**
- Use unique `referenceId` for each test order
- Same `referenceId` should return same order (if within idempotency window)

**Order Lifecycle:**
- `Pending` → Job picks up → `Completed` or `Failed`
- Check status in database after job processing

---

### API Request Analytics (Async, PLG)

**Flow:** API request → AnalyticsMiddleware (fire-and-forget) → Redis list `analytics:request_logs` → ProcessAnalyticsQueueJob (every 2 min, when Redis configured) → PostgreSQL `ApiRequestLogs`.

**Rule:** Middleware MUST NOT write to PostgreSQL synchronously (ADR-005). Queue and job decouple logging from request latency.

---

## Quick Reference

| Stage | Component | Data Store | Status Field |
|-------|-----------|------------|--------------|
| Order Creation | steller-v2-api | PostgreSQL (Orders) | Processing |
| Job Processing | steller-v2-hangfire | PostgreSQL (Orders) | Processing |
| Vendor Call | IVendorAdapter → Bamboo API | External | N/A |
| Status Update | steller-v2-hangfire | PostgreSQL (Orders) | Completed/Failed |

---

## Related Documentation

- Components: `docs/architecture/atlas/components.yaml`
- Dependencies: `docs/architecture/atlas/dependencies.yaml`
- APIs: `docs/architecture/atlas/apis.yaml`
- Security: `docs/architecture/blueprints/security-auth.md`
