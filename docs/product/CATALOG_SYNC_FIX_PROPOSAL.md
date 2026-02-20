# Catalog Sync Fix — Proposal (do not implement)

**Purpose:** Proposed code and process changes to fix first-time catalog population and ongoing sync in Steller v2. **This document is a proposal only; do not implement from this doc without explicit approval.**

---

## 1. CatalogSyncJob: use shared rate limiter

**File:** `Steller.Infrastructure/Jobs/CatalogSyncJob.cs`

**Current behavior:**  
- Calls `_vendorAdapter.GetCatalogAsync()` with no rate limiting.  
- Runs hourly (Hangfire).  
- Can trigger a catalog request in the same hour as BrandBackgroundService, exceeding Bamboo’s ~1 catalog request per hour.

**Proposed change:**  
- Inject `IRateLimiterService`.  
- Before `_vendorAdapter.GetCatalogAsync()`, call `await _rateLimiter.WaitForSlotAsync("bamboo_catalog")`.  
- Optionally: on 429 (e.g. catch exception from adapter and parse message), call `_rateLimiter.SetRetryAfter("bamboo_catalog", retrySeconds)` (see §3 for parsing).  
- No other change to job logic (still update-only by SKU).

**Rationale:** One shared quota for all catalog callers (BrandBackgroundService + CatalogSyncJob), matching legacy’s single-caller behavior.

---

## 2. BambooApiClient: set Retry-After on 429 for catalog (and categories)

**File:** `Steller.Infrastructure/Integrations/Bamboo/BambooApiClient.cs`

**Current behavior:**  
- On non–2xx (including 429), logs and throws `BambooApiException`.  
- Callers do not set the rate limiter’s Retry-After, so the next attempt may run too soon.

**Proposed change:**  
- Inject `IRateLimiterService` (or an interface that exposes `SetRetryAfter(string key, int seconds)`).  
- In `GetCatalogAsync()`: when `response.StatusCode == HttpStatusCode.TooManyRequests` (429), before throwing:  
  - Parse `responseBody` for “wait for X seconds” (same regex as legacy: `wait\s+(?:for\s+)?(\d+)\s+seconds?`).  
  - If `X > 0`, call `_rateLimiter.SetRetryAfter("bamboo_catalog", X)`.  
  - Then throw as today.  
- Optionally in `GetCategoriesAsync()`: on 429 from either v2.0 or v1.0 call, parse and call `SetRetryAfter("bamboo_categories", X)` if that key is defined and used.

**Rationale:** Same 429 handling as legacy (ExtractRetrySeconds + SetRetryAfter) so all callers that use the limiter benefit.

**Note:** BambooApiClient is registered with `AddHttpClient`; ensure the rate limiter can be injected (e.g. from the same DI scope as the client or a factory).

---

## 3. BrandService.SyncCatalogFromBambooAsync: SetRetryAfter when fallback gets 429

**File:** `Steller.Api/Services/BrandService.cs`

**Current behavior:**  
- Fallback uses `_externalApiService.GetApiResponse<BrandData>(brandUrl)`.  
- If result is not success, returns `ServiceResponse<bool>.Fail(...)`.  
- Does not check for 429 or call `SetRetryAfter`.

**Proposed change:**  
- After `brandResult` is received and `!brandResult.Status`:  
  - If `brandResult.StatusCode == HttpStatusCode.TooManyRequests` and `brandResult.Message` is not empty:  
    - Parse retry seconds from `brandResult.Message` (same regex as legacy).  
    - If parsed value > 0, call `_rateLimiter.SetRetryAfter("bamboo_catalog", retrySeconds)`.  
  - Then return Fail as today.

**Rationale:** When primary path uses BambooApiClient and fallback uses ExternalApiService, both 429 paths set Retry-After so the shared limiter backs off correctly.

---

## 4. AddBrands / DB: debug and fix “An error occurred while saving the entity changes”

**Scope:** BrandService.AddBrands (and related EF code), DB constraints, mapping from Bamboo to entities.

**Current behavior:**  
- Sync can return 200 from Bamboo but then fail with “An error occurred while saving the entity changes,” and no real products are persisted.

**Proposed change (investigation + fix):**  
1. **Reproduce:** Run manual sync when Bamboo catalog returns 200; capture full exception (including inner exceptions and stack trace).  
2. **Log:** Ensure the exception and any DbUpdateException details (e.g. constraint names, duplicate key values) are logged.  
3. **Inspect:** Check for unique/check constraints on Brands, Products, BrandCategories, ProductPricings; null/required columns; type mismatches between Bamboo DTOs and entities.  
4. **Fix:** Based on findings—e.g. adjust mapping (MapBambooCatalogToBrands / AddBrands), add defaults for nulls, or change constraint/insert strategy—so that first-time full catalog insert succeeds without error.  
5. **Optional:** Add a small integration test or runbook that runs SyncCatalogFromBambooAsync (or AddBrands with a minimal Bamboo-shaped payload) and asserts that products are inserted.

**Rationale:** Ensures first-time populate completes end-to-end once catalog and rate limiting are correct.

---

## 5. Bamboo catalog contract doc (process)

**Location:** e.g. `steller-backend/docs/BAMBOO_CATALOG_CONTRACT.md` (or a section in an existing integration doc).

**Proposed content (short):**  
- **Catalog URL:** `{BaseUrlV1}/catalog` with BaseUrlV1 = `https://api.bamboocardportal.com/api/integration/v1.0` (no double path).  
- **Categories URL:** `{BaseUrlV2}/categories`.  
- **Auth:** Basic (ClientId:ClientSecret or Username:Password per Bamboo).  
- **Callers:** BrandBackgroundService (first-time + periodic) and CatalogSyncJob (update-only). Both must use the same rate limiter key `bamboo_catalog`.  
- **Rate limit:** Effectively ~1 catalog request per hour (Bamboo); app-side 2/hour with Retry-After on 429.  
- **429:** Parse “wait for X seconds” and call `SetRetryAfter("bamboo_catalog", X)`.

**Rationale:** Single place for “how we talk to Bamboo” for catalog; reduces drift when AI or humans change code.

---

## 6. Parity checklist (process)

**Proposed:** Before merging any change that touches catalog sync, Bamboo client, or ExternalApiService catalog usage, run a short checklist:

- [ ] Catalog URL remains `base + "/catalog"` (no double `/api/integration/v1.0`).  
- [ ] All catalog callers use `WaitForSlotAsync("bamboo_catalog")` before calling Bamboo catalog.  
- [ ] On 429 from Bamboo catalog, Retry-After is parsed and `SetRetryAfter("bamboo_catalog", X)` is called.  
- [ ] No new catalog caller is added without using the shared rate limiter.

**Rationale:** Prevents reintroducing URL/caller/429 drift.

---

## 7. Summary table

| # | Item | File / scope | Proposal |
|---|------|----------------|----------|
| 1 | CatalogSyncJob rate limit | `CatalogSyncJob.cs` | Inject `IRateLimiterService`; call `WaitForSlotAsync("bamboo_catalog")` before GetCatalogAsync; optionally SetRetryAfter on 429. |
| 2 | BambooApiClient 429 | `BambooApiClient.cs` | Inject rate limiter; on 429 in GetCatalogAsync (and optionally GetCategoriesAsync), parse “wait X seconds” and call SetRetryAfter, then throw. |
| 3 | BrandService fallback 429 | `BrandService.cs` | On fallback GetApiResponse and 429, parse message and call SetRetryAfter("bamboo_catalog", X). |
| 4 | AddBrands / DB | BrandService, EF, DB | Reproduce save error, log full exception, fix constraints/mapping so first-time insert succeeds. |
| 5 | Contract doc | `docs/` | Add BAMBOO_CATALOG_CONTRACT.md (URLs, auth, callers, rate limit, 429). |
| 6 | Parity checklist | Process | Checklist for any catalog/Bamboo/ExternalApi catalog change. |

---

**Status:** Proposal only. Do not implement without explicit approval.
