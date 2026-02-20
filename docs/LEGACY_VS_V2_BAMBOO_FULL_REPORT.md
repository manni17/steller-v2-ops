# Legacy Steller vs Steller v2: Bamboo Integration — Full Report

**Purpose:** Single document with legacy Bamboo integration in detail, why v2 fails, and a complete comparison. Aligns with [AI-Agent-First Architecture](.cursor/plans/ai-agent-first_architecture_docs_b05def99.plan.md): single source of truth, machine-parseable facts, and explicit guardrails for agents.

**Related:** [CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md](CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md), [CATALOG_SYNC_RUN_REPORT_AFTER_IMMEDIATE_FIX.md](CATALOG_SYNC_RUN_REPORT_AFTER_IMMEDIATE_FIX.md).

---

## 0. Link to AI-Agent-First Architecture Plan

The plan at [.cursor/plans/ai-agent-first_architecture_docs_b05def99.plan.md](.cursor/plans/ai-agent-first_architecture_docs_b05def99.plan.md) defines:

- **Single source of truth** — One canonical fact per concern (e.g. Bamboo URLs, auth, rate limits). This report is the canonical comparison for “how legacy talks to Bamboo vs v2.”
- **Machine-parseable / stable structure** — Tables and fixed section names so agents can extract “Legacy config,” “V2 failures,” “Comparison matrix.”
- **Role-based entry** — Integration agents: read this doc + Bamboo catalog contract (when added) for any Bamboo-related change.
- **Guardrails** — Parity checklist and “do not” rules are in the same doc as the facts.

**Agent rule:** When changing catalog sync, orders, or any Bamboo integration, diff against **Legacy (Section 1)** and **Comparison (Section 4)** so URL/auth/callers/429/DB-seed stay aligned.

---

## 1. Legacy Steller — How It Deals With Bamboo (Full Detail)

**Codebase:** `/opt/steller-apps/Steller/`

### 1.1 Configuration

| Source | Key | Example value |
|-------|-----|----------------|
| Env / Program.cs | ExternalApi:BaseUrlV1 | `https://api.bamboocardportal.com/api/integration/v1.0` |
| | ExternalApi:BaseUrlV2 | `https://api.bamboocardportal.com/api/integration/v2.0` |
| | ExternalApi:MyClientId | `STELLER-TECHNOLOGY-FOR-COMMUNICATIONS-INFORMATION---CLIENT-SANDBOX` |
| | ExternalApi:MyClientSecret | (client secret; e.g. in .env.docker) |

- **Single auth for Bamboo:** Basic auth using `ClientId:ClientSecret` for all Bamboo calls (catalog, categories, orders, get order).
- **No separate “Bamboo” env vars** for catalog; only ExternalApi base URLs and client credentials.

### 1.2 HTTP Client — ExternalApiService

**File:** `Steller.EF/Services/ExternalApiService.cs`

- **Constructor:** Reads `ExternalApi:BaseUrlV1`, `MyClientId`, `MyClientSecret`. Builds `_authToken = Base64(ClientId + ":" + ClientSecret)` and sets `HttpClient.DefaultRequestHeaders.Authorization = Basic _authToken`.
- **All requests:** `CreateRequest(method, url)` adds the same Basic header. No per-request auth change.
- **GetApiResponse&lt;T&gt;(url):** GET to full `url`, returns `ServiceResponse<T>` with Status, Message, StatusCode (so 429 and message body are available to callers).
- **PostApiResponse&lt;TRequest,TResponse&gt;(url, data):** POST JSON to full `url`, same auth, same response shape.
- **URL usage:** Callers pass **full URL** (e.g. `_baseUrl + "/catalog"`). HttpClient has no BaseAddress override; `_baseUrl` is only used by callers to build URLs.

**Contract:** One client, one auth (Basic ClientId:ClientSecret), any Bamboo endpoint as long as URL is correct.

### 1.3 Rate Limiter — RateLimiterService

**File:** `Steller.Api/Services/RateLimiterService.cs`

- **Keys and limits:**  
  - `bamboo_place_order`: 2 per second  
  - `bamboo_get_order`: 120 per minute  
  - `bamboo_catalog`: 2 per hour  
  - `bamboo_exchange_rate`: 20 per minute  
  - `bamboo_transactions`: 4 per hour  
  - `bamboo_other`: 1 per second  
- **WaitForSlotAsync(key):** Enforces the window; if Bamboo returned 429 and the caller called `SetRetryAfter(key, seconds)`, the next wait checks `_retryAfterTimes` and can block until that time.
- **SetRetryAfter(key, seconds):** Sets `_retryAfterTimes[key] = UtcNow + seconds`. Used by callers when they receive 429 and parse “wait for X seconds.”

**Contract:** Every Bamboo call that is rate-limited goes through `WaitForSlotAsync`; on 429 the caller parses the message and calls `SetRetryAfter`.

### 1.4 Catalog Sync — BrandBackgroundService + BrandService

**Files:** `Steller.Api/Services/BrandBackgroundService.cs`, `Steller.Api/Services/BrandService.cs`

**Flow (every 30 min):**

1. **FetchCategoriesFromBambo()**
   - URL: `_baseUrlV2 + "/categories"` → `https://api.bamboocardportal.com/api/integration/v2.0/categories`
   - `WaitForSlotAsync("bamboo_categories")` → `GetApiResponse<List<Category>>(url)`.
   - On success: scope → ICategoryService.AddCategories(data, 1).
   - On failure: if StatusCode == TooManyRequests, `ExtractRetrySeconds(Message)` → `SetRetryAfter("bamboo_categories", retrySeconds)`.

2. **FetchBrandsFromBambo()**
   - URL: `_baseUrl + "/catalog"` → `https://api.bamboocardportal.com/api/integration/v1.0/catalog`
   - `WaitForSlotAsync("bamboo_catalog")` → `GetApiResponse<BrandData>(url)`.
   - On success: `brandService.AddBrands(result.Data?.Brands!, 1)`.
   - On failure: if 429, `ExtractRetrySeconds` → `SetRetryAfter("bamboo_catalog", retrySeconds)`.

**ExtractRetrySeconds:** Regex `wait\s+(?:for\s+)?(\d+)\s+seconds?` on the error message (Bamboo returns e.g. “Too many requests. Please wait for 3406 seconds.”).

**AddBrands (BrandService):** Upserts Brands, Products, ProductPricing, ProductPricingConfigurations, BrandCategories. For **ProductPricingConfiguration** it sets `CommissionTypeId = (MaxFaceValue - Price.Max) > 0 ? 1 : 2`. So legacy **assumes CommissionTypes has rows with Id 1 and 2**. Legacy **seeds** these via SeedController (CommissionType Id 1 “Commission Included”, Id 2 “Commission Extra”).

**Contract:** One caller (BrandBackgroundService), one URL rule (base + `/catalog` or `/categories`), one auth, 429 → SetRetryAfter. DB seed includes CommissionTypes so AddBrands never hits FK on CommissionTypeId.

### 1.5 Orders — OrderService

**File:** `Steller.Api/Services/OrderService.cs`

- **Place order:** URL `_baseUrl + "/orders/checkout"` → `https://api.bamboocardportal.com/api/integration/v1.0/orders/checkout`.  
  `WaitForSlotAsync("bamboo_place_order")` → `PostApiResponse<OrderDto, string>(url, requestData)`.  
  On 429: ExtractRetrySeconds → SetRetryAfter("bamboo_place_order", retrySeconds).
- **Get order:** URL `_baseUrl + "/orders/{orderId}"`.  
  `WaitForSlotAsync("bamboo_get_order")` → `GetApiResponse<OrderBambo>(url)`.  
  On 429: SetRetryAfter("bamboo_get_order", retrySeconds).

Same ExternalApiService, same auth, same 429 handling pattern.

### 1.6 Legacy Summary Table

| Concern | Legacy implementation |
|---------|------------------------|
| **Bamboo base** | BaseUrlV1 = full path including `/api/integration/v1.0`; BaseUrlV2 = full path including `/api/integration/v2.0`. |
| **Catalog URL** | BaseUrlV1 + `/catalog`. |
| **Categories URL** | BaseUrlV2 + `/categories`. |
| **Orders checkout URL** | BaseUrlV1 + `/orders/checkout`. |
| **Get order URL** | BaseUrlV1 + `/orders/{id}`. |
| **Auth** | Basic; single set of credentials (ClientId:ClientSecret) for all Bamboo calls. |
| **Client** | ExternalApiService only (no separate Bamboo API client). |
| **Catalog caller** | Only BrandBackgroundService (no separate hourly job calling catalog). |
| **429** | Caller checks StatusCode and Message; parses seconds; calls SetRetryAfter(key, seconds). |
| **AddBrands dependency** | CommissionTypes seeded (Id 1, 2) via SeedController so ProductPricingConfigurations FK succeeds. |

---

## 2. Steller v2 — How It Deals With Bamboo and Where It Fails

**Codebase:** `/opt/steller-v2/steller-backend/`

### 2.1 Configuration

- **Dual credentials:** BAMBOO_USERNAME, BAMBOO_PASSWORD (for BambooApiClient) and EXTERNAL_API_CLIENT_ID, EXTERNAL_API_CLIENT_SECRET (for ExternalApiService). BaseUrlV1/V2 same as legacy.
- **Immediate Fix (current):** Catalog uses **only** ExternalApiService (ClientId:ClientSecret); CatalogSyncJob is **disabled**; catalog URL is `_baseUrl.TrimEnd('/') + "/catalog"` (no double path).

### 2.2 Two HTTP Paths

- **BambooApiClient (orders, optional categories/catalog):** Basic Username:Password, BaseAddress = BAMBOO_BASE_URL (domain only), paths like `/api/integration/v1.0/catalog`. Used for place order, get order, and (before Immediate Fix) catalog/categories.
- **ExternalApiService:** Same as legacy (Basic ClientId:ClientSecret; URLs built by callers). Used for catalog after Immediate Fix and for fallback.

### 2.3 Catalog Sync (Post–Immediate Fix)

- **BrandBackgroundService** → **BrandService.SyncCatalogFromBambooAsync**.
- Categories: still can use BambooApiClient or ExternalApiService fallback.
- **Catalog:** Only ExternalApiService path: `WaitForSlotAsync("bamboo_catalog")` → `GetApiResponse<BrandData>(brandUrl)` with `brandUrl = _baseUrl.TrimEnd('/') + "/catalog"`.
- **CatalogSyncJob:** Disabled (commented out in Program.cs) so only one catalog caller.

### 2.4 Where v2 Fails (Catalog Populate)

1. **Bamboo/catalog call (fixed):** After Immediate Fix, catalog GET returns 200; no 429 in the last run.
2. **AddBrands / DB (current blocker):** During SaveChanges, insert into **ProductPricingConfigurations** fails with:
   - `23503: insert or update on table "ProductPricingConfigurations" violates foreign key constraint "FK_ProductPricingConfigurations_CommissionTypes_CommissionType..."`.
   - **Cause:** AddBrands sets `CommissionTypeId = 1` or `2` (same logic as legacy), but v2’s **RealDataSeedService does not seed CommissionTypes**. So CommissionTypes table is empty (or missing Id 1 and 2), and the FK fails.
3. **Legacy vs v2 seed:** Legacy SeedController seeds CommissionTypes (Id 1, 2). V2 RealDataSeedService seeds only Currencies, WalletTypes, TransactionTypes, OrderStatuses — **no CommissionTypes**. Hence v2 fails at first-time catalog persist even when Bamboo returns 200.

### 2.5 v2 Summary Table

| Concern | V2 implementation | Status / issue |
|---------|-------------------|----------------|
| **Catalog URL** | base + `/catalog` (Immediate Fix) | OK |
| **Catalog auth** | ExternalApiService only (Immediate Fix) | OK |
| **Catalog caller** | Only BrandBackgroundService (CatalogSyncJob disabled) | OK |
| **429** | Rate limiter exists; SetRetryAfter from response not wired in BambooApiClient for catalog (N/A when catalog uses only ExternalApiService) | Acceptable for current path |
| **AddBrands** | Same CommissionTypeId 1/2 logic as legacy | **Fails:** CommissionTypes not seeded in v2 → FK violation |

---

## 3. Complete Comparison: Legacy vs v2

| Aspect | Legacy | V2 (post–Immediate Fix) | Match? |
|--------|--------|--------------------------|--------|
| **Catalog URL** | BaseUrlV1 + `/catalog` | base + `/catalog` | Yes |
| **Categories URL** | BaseUrlV2 + `/categories` | BaseUrlV2 + `/categories` (or Bamboo client) | Yes |
| **Catalog auth** | Basic ClientId:ClientSecret | Basic ClientId:ClientSecret (catalog only) | Yes |
| **Catalog HTTP client** | ExternalApiService only | ExternalApiService only for catalog | Yes |
| **Who calls catalog** | BrandBackgroundService only | BrandBackgroundService only (job disabled) | Yes |
| **Rate limit key catalog** | bamboo_catalog, 2/hour | Same | Yes |
| **429 handling catalog** | ExtractRetrySeconds + SetRetryAfter | Not invoked when catalog returns 200; if 429 on fallback path, v2 could add same logic | Partial (OK while no 429) |
| **Order place** | ExternalApiService POST BaseUrlV1 + `/orders/checkout` | BambooApiClient (Username:Password) POST `/api/integration/v2.0/orders/checkout` | Different client/auth; both work if credentials valid |
| **Order get** | ExternalApiService GET BaseUrlV1 + `/orders/{id}` | BambooApiClient GET `/api/integration/v1.0/orders/{id}` | Same difference |
| **CommissionTypes seed** | SeedController seeds Id 1, 2 | **Not seeded** in RealDataSeedService | No — v2 fails AddBrands |
| **AddBrands → ProductPricingConfigurations** | CommissionTypeId 1 or 2 | Same; requires CommissionTypes rows 1 and 2 | Fails in v2 until CommissionTypes seeded |

---

## 4. Why v2 Fails (Summary)

1. **Catalog/Bamboo (addressed):** URL double path and two callers were fixed by Immediate Fix; auth unified to ExternalApiService for catalog. Catalog GET now succeeds (200).
2. **DB/seed (current):** AddBrands inserts ProductPricingConfigurations with CommissionTypeId 1 or 2. Legacy has CommissionTypes Id 1 and 2 seeded; v2 does not. So v2 hits FK violation and returns 400; no products persist.
3. **Design lesson:** v2 duplicated catalog logic and added a second caller without treating legacy as the contract; v2 also did not carry over the **seed dependency** (CommissionTypes) required by AddBrands, so first-time populate fails at persist.

---

## 5. Recommendations

1. **Seed CommissionTypes in v2** — In RealDataSeedService (or equivalent startup seed), insert CommissionTypes with Id 1 and 2 (e.g. “Commission Included”, “Commission Extra”) so AddBrands can insert ProductPricingConfigurations without FK violation. Re-run catalog sync and confirm products persist.
2. **Keep catalog contract documented** — Maintain one “Bamboo catalog contract” doc (URLs, auth, single caller, 429 handling) and reference it from this report so future changes stay aligned with legacy.
3. **Parity checklist** — For any Bamboo or catalog change, run: same URL rule as legacy, same auth for catalog, single catalog caller (or shared rate limiter), 429 → SetRetryAfter, and **seed data required by AddBrands (CommissionTypes)**.
4. **Agent rule** — When modifying Bamboo integration or catalog sync, load this doc and the Bamboo catalog contract; diff against Legacy (Section 1) and Comparison (Section 3); ensure DB seed (e.g. CommissionTypes) is present for any code path that writes ProductPricingConfigurations.

---

## 6. Document Index (for agents)

| Role / task | Docs |
|-------------|------|
| Bamboo integration parity | This file; CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md; CATALOG_SYNC_RUN_REPORT_AFTER_IMMEDIATE_FIX.md |
| Architecture / agent-first | .cursor/plans/ai-agent-first_architecture_docs_b05def99.plan.md |
| Catalog sync fix proposal | CATALOG_SYNC_FIX_PROPOSAL.md |

**Do not:** Change catalog URL construction without checking legacy (base + `/catalog` only when base already contains path). Do not add a second catalog caller without shared rate limiter. Do not assume ProductPricingConfigurations can be inserted without CommissionTypes Id 1 and 2 seeded.
