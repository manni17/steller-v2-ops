# Catalog Sync: Two Services, Limits, Rules & Strategy

**Purpose:** Clarify the two catalog flows — first-time population vs change-scan — their limits, rules, and recommended usage.

---

## 1. First-time catalog population (populate catalog)

**What it is:** Inserts/updates **Brands** and **Products** from Bamboo into the DB. This is the only path that **adds new SKUs**.

| Aspect | Details |
|--------|---------|
| **Code** | `BrandService.SyncCatalogFromBambooAsync()` |
| **Triggered by** | (1) **BrandBackgroundService** (every 30 min), (2) **Manual**: `POST /api/brand/sync-catalog` |
| **Bamboo calls** | Categories (v2) once + Catalog (v1.0) once per run |
| **Our app limit** | **bamboo_catalog:** 2 requests per hour (only this path uses the rate limiter) |
| **Bamboo limit (observed)** | ~1 catalog call per ~57 minutes (429: "wait 3406 seconds") — stricter than our 2/hour |

### How many times can we use it?

- **Our limiter:** Up to **2 times per hour** (when only this service calls catalog).
- **Bamboo:** In practice **~1 successful catalog call per hour** (second call in same hour often gets 429).
- **So:** Treat as **at most 1 full run per hour** to stay under Bamboo’s limit.

### Rules to use it

1. **Single source of catalog writes** — Use this for “load catalog” and “refresh full catalog” (AddBrands inserts/updates).
2. **Respect Bamboo** — Don’t trigger manually right after a run; wait ~1 hour if you’ve already done a catalog call.
3. **Manual trigger** — `POST /api/brand/sync-catalog` (admin JWT recommended); counts against the same Bamboo catalog limit.
4. **Background schedule** — BrandBackgroundService runs every 30 min; each run that calls Bamboo catalog consumes one “slot” (and Bamboo may still 429 if we already did a catalog call in that hour).

### Best approach

- Use **one** of:
  - **Scheduled only:** Let BrandBackgroundService run (every 30 min); accept that only ~1 run per hour will get a successful catalog response from Bamboo.
  - **Manual for first time:** Call `POST /api/brand/sync-catalog` once when the environment is new, then rely on schedule or change-scan.
- Don’t mix heavy manual sync with the 30-min background run in the same hour.

### Best strategy for first-time sync

1. **New environment / empty catalog**
   - Run **one** full sync: either wait for BrandBackgroundService to run or call `POST /api/brand/sync-catalog` once.
   - Ensure no other catalog caller (e.g. CatalogSyncJob) runs in the same 1-hour window (or coordinate so only one catalog call per hour).
2. **After first time**
   - Use this service sparingly: **at most once per hour** (e.g. one successful BrandBackgroundService run, or one manual sync).
   - Rely on **CatalogSyncJob** for ongoing cost/face-value updates (see below).

---

## 2. Change-scan catalog sync (scan for changes)

**What it is:** Fetches the full catalog from Bamboo **once** per run, then **only updates existing** products (by SKU): cost/face value, margin guard. **Does not insert** new products.

| Aspect | Details |
|--------|---------|
| **Code** | `CatalogSyncJob.ExecuteAsync()` → `IVendorAdapter.GetCatalogAsync()` (Bamboo) |
| **Triggered by** | Hangfire recurring job **"catalog-sync"**: `0 */1 * * *` → **every hour at :00** |
| **Bamboo calls** | Catalog (v1.0) **once** per run |
| **Our app limit** | **None** — this job does **not** use the rate limiter |
| **Bamboo limit (observed)** | Same as above: ~1 catalog call per ~57 minutes |

### How many times can we use it?

- **Our limiter:** Not applied; the job can run every hour.
- **Bamboo:** Still **~1 catalog call per hour** across the whole app. So if BrandBackgroundService (or manual sync) already did a catalog call in that hour, this job’s catalog call can get **429**.
- **So:** Use at most **1 catalog call per hour** across **both** services combined.

### Rules to use it

1. **Update-only** — Use only for scanning for **changes** to existing products (cost/face value). It will not add new SKUs.
2. **Same Bamboo quota** — Shares Bamboo’s catalog rate limit with SyncCatalogFromBambooAsync. Total catalog calls (population + change-scan) should stay **~1 per hour**.
3. **Schedule** — Currently hourly at :00. If you also run first-time population in that hour, one of the two will likely get 429.

### Best approach

- Run **once per hour** (current schedule is fine).
- **Coordinate with first-time service:** Ensure only **one** of these runs in any given hour:
  - Option A: Run CatalogSyncJob at :00; run BrandBackgroundService at :30 but **skip** catalog call if we already did one in that hour (would require code change to check “catalog already called this hour”).
  - Option B: Keep both as-is and accept that some runs will get 429; next run after ~57 min will succeed.
- **First time:** Ensure the **first-time population** has run at least once (so products exist) before relying on this job for “changes”; otherwise it has nothing to update.

### Best strategy for change-scan sync

1. **After first-time population has run**
   - Let CatalogSyncJob run **hourly** to refresh cost/face value for existing products.
2. **Avoid double catalog in same hour**
   - Either stagger: e.g. first-time at :00, change-scan at :30 (or vice versa), **or** add a shared “catalog slot” so only one catalog call per hour (e.g. CatalogSyncJob or BrandBackgroundService checks rate limiter before calling Bamboo).
3. **If you get 429**
   - Bamboo says wait 3406 seconds (~57 min). Don’t retry catalog until that window has passed.

---

## Summary table

| | First-time population | Change-scan |
|--|------------------------|-------------|
| **Service** | SyncCatalogFromBambooAsync (BrandBackgroundService + manual) | CatalogSyncJob (Hangfire) |
| **Effect** | Inserts/updates Brands and Products (adds new SKUs) | Updates existing products only (cost/face value) |
| **Our limit** | 2 catalog calls/hour (rate limiter) | No app limit |
| **Bamboo limit** | ~1 catalog/hour (observed) | Same (shared) |
| **Max safe use** | ~1 full run per hour | ~1 run per hour (and not in same hour as first-time) |
| **Best first-time strategy** | One full sync when empty; then at most 1/hour | Run only after first-time has populated; then hourly |
| **Best ongoing strategy** | Use sparingly (e.g. 1/hour or less) | Hourly; coordinate so only one catalog call per hour across both |

---

## Recommendation: single catalog call per hour

To avoid 429s:

1. **First-time:** Run `POST /api/brand/sync-catalog` once (or let BrandBackgroundService run once) when the catalog is empty; wait ~1 hour before any other catalog call.
2. **Ongoing:** Have **only one** caller perform a catalog GET per hour:
   - **Option A (code change):** Make CatalogSyncJob use the same `bamboo_catalog` rate limiter so both first-time and change-scan share one “2 per hour” (and in practice 1/hour) quota.
   - **Option B (schedule):** Disable BrandBackgroundService’s catalog run and use only CatalogSyncJob for hourly catalog fetch; run full population (SyncCatalogFromBambooAsync) only manually or on a daily schedule.
   - **Option C (accept 429):** Keep current behavior; after 429 wait ~57 minutes; next run succeeds.

Implementing **Option A** is the cleanest: both flows use `WaitForSlotAsync("bamboo_catalog")` before calling Bamboo catalog, so combined catalog calls stay within our 2/hour and reduce risk of hitting Bamboo’s limit.
