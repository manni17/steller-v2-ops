# Catalog Sync Run Report — After Immediate Fix

**Date:** 2026-02-19  
**Sequence:** 30 min wait → manual POST `/api/brand/sync-catalog` (max-time 300s). Curl timed out; server had already responded.

---

## Summary

- **Bamboo/catalog:** Catalog request succeeded (no 429; ExternalApiService path used).
- **Sync result:** **400** after ~3 min. Failure is in **AddBrands** (DB), not in calling Bamboo.
- **DB state:** Still **0 real products**, 3 brands (unchanged).

---

## What happened

1. **Wait:** 30 min sleep completed; sync fired.
2. **Request:** `POST /api/brand/sync-catalog` ran on the server. Response: **400** in **180729 ms** (~3 min).
3. **Curl:** Timed out after 300 s with 0 bytes (client timeout; server had already returned 400).
4. **Logs:** Manual sync at **23:05:49** UTC: `HTTP POST /api/brand/sync-catalog responded 400 in 180729.5980 ms`.

---

## Root cause of 400

**PostgresException during SaveChanges:**

```
23503: insert or update on table "ProductPricingConfigurations" violates foreign key constraint "FK_ProductPricingConfigurations_CommissionTypes_CommissionType..."
```

So:

- Bamboo catalog was fetched (200).
- Categories/brands path and ExternalApiService (ClientId:ClientSecret) are working.
- Failure is in **AddBrands**: inserting **ProductPricingConfigurations** with a **CommissionTypeId** that does not exist in **CommissionTypes** (or is null where NOT NULL).

---

## DB state after run

| Table              | Count |
|--------------------|-------|
| Products           | 3 (all MOCK) |
| Real products      | 0     |
| Brands             | 3     |

No new products or brands were persisted due to the FK error.

---

## Conclusions

1. **Immediate Fix is effective for “talking to Bamboo”:** Single caller, ExternalApiService, URL and auth are correct; no rate limit hit this run.
2. **Next blocker is AddBrands/DB:** Seed or ensure **CommissionTypes** has the IDs that BrandService uses when creating ProductPricingConfigurations, or make the mapping use an existing CommissionTypeId / allow null if the schema permits.

---

## Recommended next step

- Fix **ProductPricingConfigurations** ↔ **CommissionTypes** FK: either seed CommissionTypes with the expected IDs, or change AddBrands mapping to use an existing CommissionTypeId (or omit/default the column if allowed). Then re-run sync and re-check product count.
