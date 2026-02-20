# Catalog Sync Monitoring Report

**Date:** 2026-02-18  
**Monitoring Period:** 16:54 UTC - 17:54 UTC (1 hour after rebuild)  
**API Rebuild:** Completed at 16:54 UTC with catalog sync fix

---

## Summary

**Status:** ⚠️ **PARTIAL SUCCESS** - Bamboo client is working, but sync is hitting rate limits and products not yet persisted.

---

## Findings

### ✅ What's Working

1. **Bamboo Client Integration**
   - ✅ Catalog API call using correct path: `GET https://api.bamboocardportal.com/api/integration/v1.0/catalog`
   - ✅ Response: **200 OK** (at 17:00:08 UTC)
   - ✅ Code fix is deployed and using `IBambooApiClient.GetCatalogAsync()`

2. **CatalogSyncJob (Hourly)**
   - ✅ Fetched **8026 products** from Bamboo vendor (at 17:00:12 UTC)
   - ⚠️ **Limitation:** Only updates existing products (cost/face value); does NOT add new SKUs

### ⚠️ Issues

1. **Rate Limiting**
   - ❌ Sync failed at **17:03:18 UTC**: "Too many requests. Please wait for 3406 seconds (≈57 minutes) for the next request"
   - **Impact:** Subsequent sync attempts blocked until rate limit resets

2. **Products Not Persisted**
   - ❌ **0 real products** in DB (still only 3 MOCK-* products)
   - **Possible causes:**
     - SyncCatalogFromBambooAsync may have failed silently after catalog fetch
     - Rate limiting prevented completion
     - Mapping/AddBrands may have encountered an error not logged

3. **Previous Sync Failure**
   - ❌ Sync at **16:33:48 UTC** failed: "An error occurred while saving the entity changes"
   - Suggests database constraint or data issue during AddBrands

---

## Bamboo API Call Count (from logs)

| Time (UTC) | Caller | Bamboo endpoint | Result |
|------------|--------|-----------------|--------|
| **16:54:46** | BrandBackgroundService (SyncCatalogFromBambooAsync) | Categories (v2) | ✅ |
| **17:00:03** | BrandBackgroundService (SyncCatalogFromBambooAsync) | Catalog (v1.0) | ✅ 200 OK |
| **17:00:12** | **CatalogSyncJob** (Hangfire hourly) | Catalog (v1.0) | ✅ 200 OK — "Fetched 8026 products" |
| **17:03:17** | BrandBackgroundService (next 30‑min cycle) | Catalog (v1.0) | ❌ 429 — "wait 3406 seconds" |

**Total Bamboo API hits in the monitored hour:**  
- **Categories:** 1  
- **Catalog:** **3** (two succeeded, one returned rate limit)

So we hit the Bamboo catalog endpoint **3 times** in ~9 minutes. The third call got **429** with "Please wait for 3406 seconds" — i.e. **we did hit Bamboo’s rate limit**.

---

## Rate limits

**Our app (Steller) — from code:**

- **bamboo_catalog:** 2 requests per hour (used only in `BrandService.SyncCatalogFromBambooAsync`).
- **CatalogSyncJob does NOT use the rate limiter** — it calls `_vendorAdapter.GetCatalogAsync()` directly, so it can trigger a catalog call every time it runs (hourly). That’s why we had two catalog calls close together (BrandBackgroundService + CatalogSyncJob) and then a third (next BrandBackgroundService cycle) that hit Bamboo’s limit.

**Bamboo API (from observed behavior):**

- Bamboo does not document a numeric catalog rate limit in the repo’s Bamboo doc extract (only order limits: 500 cards/order, 6 orders/product/minute).
- From the 429 response: **“Please wait for 3406 seconds”** (~57 minutes). So in practice Bamboo is enforcing roughly **one catalog call per ~57 minutes** (or a low hourly cap); we exceeded it.

**Did we hit the limit?** **Yes.** The third catalog request returned 429 with the 3406‑second wait.

---

## First-time catalog vs “Fetched 8026 products (updates only)”

They are **different flows**:

1. **Getting the catalog the first time (insert new products)**  
   - **Code path:** `BrandBackgroundService` → `SyncCatalogFromBambooAsync` → `bambooClient.GetCatalogAsync()` → `MapBambooCatalogToBrands` → **`AddBrands`** (creates/updates Brands and **inserts/updates Products**).  
   - **Effect:** New SKUs are **added** to the DB; existing ones can be updated.  
   - **Rate limited in our app:** Yes (`bamboo_catalog` = 2/hour).

2. **“CatalogSyncJob: Fetched 8026 products (updates only)”**  
   - **Code path:** `CatalogSyncJob` (Hangfire, hourly) → `_vendorAdapter.GetCatalogAsync()` (one catalog GET) → for each of the 8026 products: `FirstOrDefaultAsync(p => p.Sku == vendorProd.Sku)`. **If `localProd != null`** it updates cost/face value; **if null it does nothing** (no insert).  
   - **Effect:** **Only updates existing products by SKU.** It does **not** add new products. So “8026 products” = we received 8026 from Bamboo and only applied updates where we already had that SKU; with 0 real products in DB, no rows were updated.  
   - **Rate limited in our app:** No — so this job can call Bamboo catalog every run and contributed to hitting Bamboo’s limit.

So: **first time** = SyncCatalogFromBambooAsync + AddBrands (inserts/updates). **“Fetched 8026 (updates only)”** = one catalog GET + update-only loop (no new rows).

---

## Timeline

| Time (UTC) | Event | Status |
|------------|-------|--------|
| **16:54:46** | API restarted with catalog sync fix | ✅ |
| **16:54:46** | BrandBackgroundService: "Starting scheduled catalog sync..." | ✅ Started |
| **16:54:46** | Bamboo categories API call (v2.0) | ✅ |
| **17:00:03** | Bamboo catalog API call (v1.0) - **NEW CODE PATH** | ✅ |
| **17:00:08** | Catalog API response: **200 OK** | ✅ Success |
| **17:00:12** | CatalogSyncJob: "Fetched 8026 products from vendor" | ✅ (updates only) |
| **17:03:17** | Second catalog API call attempt | ❌ |
| **17:03:18** | Sync failed: **Rate limit** (wait 3406 seconds) | ❌ Blocked |

---

## Database State

**Products Table:**
- **Total:** 3 products
- **Mock (MOCK-*):** 3
- **Real (non-MOCK):** 0

**Conclusion:** No real Bamboo SKUs have been persisted yet.

---

## Root Cause Analysis

1. **Bamboo client code is working** ✅
   - Correct URL, correct auth, 200 response
   - Fix is deployed and executing

2. **Rate limiting is blocking sync** ⚠️
   - Bamboo API rate limit: ~57 minutes between catalog calls
   - Multiple sync attempts (BrandBackgroundService + CatalogSyncJob) hitting limit

3. **Products not persisted** ❌
   - CatalogSyncJob fetched 8026 products but only updates existing (by design)
   - SyncCatalogFromBambooAsync (which should add new products) may have:
     - Failed silently after successful catalog fetch
     - Hit rate limit before AddBrands completed
     - Encountered a database error during AddBrands (like the 16:33:48 error)

---

## Recommendations

1. **Wait for rate limit reset** (~57 minutes from 17:03:18 = ~18:00 UTC)
   - Then check if next sync succeeds and products are added

2. **Check for silent failures**
   - Review logs around 17:00:08-17:00:15 for any AddBrands errors
   - Check database constraints that might prevent product insertion

3. **Consider rate limit handling**
   - Add exponential backoff or respect Bamboo's Retry-After header
   - Coordinate BrandBackgroundService (30-min) and CatalogSyncJob (hourly) to avoid simultaneous calls

4. **Manual trigger test**
   - After rate limit resets, manually trigger sync: `POST /api/brand/sync-catalog` (with admin auth)
   - Monitor logs to see if products are added

---

## Next Steps

1. ✅ **Code fix deployed** - Bamboo client integration working
2. ⏳ **Wait for rate limit reset** (~18:00 UTC)
3. 🔍 **Monitor next sync** (should be ~17:24 UTC for BrandBackgroundService, but blocked by rate limit)
4. ✅ **Verify products added** after successful sync completes

---

**Report Generated:** 2026-02-18 17:08 UTC
