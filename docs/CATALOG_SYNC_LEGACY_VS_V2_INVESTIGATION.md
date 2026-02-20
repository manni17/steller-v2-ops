# Catalog Sync: Legacy Steller vs Steller v2 — Why v2 Fails to Populate First Time

**Goal:** Understand how legacy Steller talks to Bamboo for catalog sync vs how v2 does it, and why v2 is still failing to populate the catalog the first time.

---

## 1. How legacy Steller syncs catalog (and why it works)

**Location:** `/opt/steller-apps/Steller/`

### Flow

- **BrandBackgroundService** (runs every 30 min):
  1. **FetchCategoriesFromBambo()**  
     - URL: `{BaseUrlV2}/categories` → `https://api.bamboocardportal.com/api/integration/v2.0/categories`  
     - Auth: **Basic** with `ClientId:ClientSecret` (ExternalApiService).
  2. **FetchBrandsFromBambo()**  
     - URL: `{BaseUrlV1}/catalog` → `https://api.bamboocardportal.com/api/integration/v1.0/catalog`  
     - Auth: same **Basic** `ClientId:ClientSecret`.  
     - Then calls `brandService.AddBrands(result.Data?.Brands!, 1)` and saves.

### Configuration (legacy .env.docker)

```env
EXTERNAL_API_BASE_URL_V1=https://api.bamboocardportal.com/api/integration/v1.0
EXTERNAL_API_BASE_URL_V2=https://api.bamboocardportal.com/api/integration/v2.0
EXTERNAL_API_CLIENT_ID=STELLER-TECHNOLOGY-FOR-COMMUNICATIONS-INFORMATION---CLIENT-SANDBOX
EXTERNAL_API_CLIENT_SECRET=0fea3fab-21eb-4350-b222-6f95911384f7
```

- **Single code path:** only **ExternalApiService** (no separate Bamboo client).
- **URLs:** BaseUrlV1/V2 already include `/api/integration/v1.0` and `/api/integration/v2.0`, so:
  - Catalog = BaseUrlV1 + `/catalog` = correct full path.
- **Auth:** Bamboo is called with **Basic auth** using **ClientId:ClientSecret** (Bamboo treats these as the catalog/categories credentials).
- **Rate limiting:** Legacy parses 429 response and calls `_rateLimiter.SetRetryAfter("bamboo_catalog", retrySeconds)` so it backs off correctly.

### Summary (legacy)

| Aspect | Legacy |
|--------|--------|
| Catalog URL | `BaseUrlV1 + "/catalog"` → correct |
| Categories URL | `BaseUrlV2 + "/categories"` → correct |
| Auth | Basic `ClientId:ClientSecret` (one set of credentials for catalog) |
| Client | ExternalApiService only |
| 429 handling | ExtractRetrySeconds + SetRetryAfter |

---

## 2. How Steller v2 syncs catalog (and where it diverges)

**Location:** `/opt/steller-v2/steller-backend/`

### Flow

- **BrandBackgroundService** → **BrandService.SyncCatalogFromBambooAsync()**:
  1. **Categories:**  
     - If **IBambooApiClient** is registered: `bambooClient.GetCategoriesAsync()` (Basic **Username:Password**).  
     - Else: ExternalApiService GET `{BaseUrlV2}/categories` (Basic **ClientId:ClientSecret**).
  2. **Catalog:**  
     - If **IBambooApiClient** is registered: tries `bambooClient.GetCatalogAsync()` (Basic **Username:Password**), then `MapBambooCatalogToBrands` → **AddBrands**.  
     - On exception: **fallback** to ExternalApiService with a **constructed catalog URL** (see bug below).

### Configuration (v2 .env)

```env
BAMBOO_BASE_URL=https://api.bamboocardportal.com
BAMBOO_USERNAME=STELLER-TECHNOLOGY-FOR-COMMUNICATIONS-INFORMATION---CLIENT-SANDBOX
BAMBOO_PASSWORD=2930e792-1186-4b9b-9782-f68a5a771567
EXTERNAL_API_BASE_URL_V1=https://api.bamboocardportal.com/api/integration/v1.0
EXTERNAL_API_BASE_URL_V2=https://api.bamboocardportal.com/api/integration/v2.0
EXTERNAL_API_CLIENT_ID=STELLER-TECHNOLOGY-FOR-COMMUNICATIONS-INFORMATION---CLIENT-SANDBOX
EXTERNAL_API_CLIENT_SECRET=2930e792-1186-4b9b-9782-f68a5a771567
```

- **Two code paths:** **BambooApiClient** (username/password) **and** ExternalApiService (client id/secret).
- **BambooApiClient:** BaseUrl = `https://api.bamboocardportal.com`, path = `/api/integration/v1.0/catalog` → **correct**.
- **Fallback catalog URL in BrandService is wrong** when BaseUrlV1 already contains the path (see below).

### Summary (v2)

| Aspect | V2 |
|--------|----|
| Catalog (primary) | BambooApiClient: Basic **Username:Password**, path `/api/integration/v1.0/catalog` ✅ |
| Catalog (fallback) | ExternalApiService: URL built as `_baseUrl + "/api/integration/v1.0/catalog"` → **wrong** when _baseUrl is already `.../v1.0` ❌ |
| Categories | BambooApiClient or ExternalApiService `BaseUrlV2 + "/categories"` ✅ |
| 429 handling | Rate limiter exists; Retry-After from Bamboo response may not be applied to catalog (BambooApiClient throws, fallback then uses wrong URL) |

---

## 3. Root causes why v2 fails to populate the first time

### 3.1 Fallback catalog URL is wrong (bug)

In **BrandService.SyncCatalogFromBambooAsync** (v2):

```csharp
string brandUrl = (_baseUrl?.Contains("bamboocardportal", StringComparison.OrdinalIgnoreCase) == true)
    ? $"{_baseUrl.TrimEnd('/')}/api/integration/v1.0/catalog"
    : $"{_baseUrl}/catalog";
```

- When `_baseUrl` = `https://api.bamboocardportal.com/api/integration/v1.0` (from config), this becomes:
  - `https://api.bamboocardportal.com/api/integration/v1.0/api/integration/v1.0/catalog` ❌
- **Legacy** uses `_baseUrl + "/catalog"` only, so it gets `.../v1.0/catalog` ✅

**Fix:** When base already contains the integration path, append only `/catalog`. For example:

- If `_baseUrl` already contains `api/integration/v1.0`, use `$"{_baseUrl.TrimEnd('/')}/catalog"`.
- Otherwise use `$"{_baseUrl.TrimEnd('/')}/api/integration/v1.0/catalog"`.

So: **legacy “knows” the right URL because it never doubles the path; v2’s fallback doubles it.**

### 3.2 Two auth methods and two callers

- **Legacy:** One auth (ClientId:ClientSecret), one client, one catalog call per sync.
- **V2:**  
  - Primary: BambooApiClient (Username:Password).  
  - Fallback: ExternalApiService (ClientId:ClientSecret).  
  - If Bamboo treats these the same for catalog, primary can work; if not, fallback is still broken by the URL bug.  
  - **CatalogSyncJob** also calls Bamboo catalog (no rate limiter), so v2 can make **two** catalog calls in a short window (BrandBackgroundService + CatalogSyncJob) and hit **Bamboo’s** rate limit (~1/hour), causing 429. Legacy only has the background service (and respects retry-after).

So: **legacy talks to Bamboo once per sync with one auth; v2 adds a second caller and a broken fallback.**

### 3.3 Rate limit and Retry-After

- Legacy: On 429, parses “wait for X seconds” and calls `SetRetryAfter("bamboo_catalog", retrySeconds)` so the next attempt backs off.
- V2: BambooApiClient throws on non-2xx; the code falls back to ExternalApiService. If the fallback URL is wrong, fallback fails. Even if the primary gets 429, v2 may not be setting the rate limiter’s Retry-After from the Bamboo response body, so the next attempt can fire too soon and hit 429 again.

So: **legacy “knows” how to back off after 429; v2 is less aligned with Bamboo’s rate limit.**

### 3.4 AddBrands and DB errors

- Both legacy and v2 use **AddBrands** to persist brands/products.  
- V2 has seen “An error occurred while saving the entity changes” during sync, which suggests constraint or data issues during **AddBrands** (e.g. duplicate key, null required field, or mapping difference).  
- So even when the catalog is fetched (200), v2 can fail to **persist** the first time due to DB/entity issues, not only due to “how we talk to Bamboo.”

---

## 4. Side-by-side comparison

| Item | Legacy | V2 |
|------|--------|-----|
| **Catalog URL** | BaseUrlV1 + `/catalog` → correct | BambooApiClient: correct path. Fallback: **wrong** (double path). |
| **Categories URL** | BaseUrlV2 + `/categories` → correct | Correct (BambooApiClient or ExternalApiService). |
| **Auth** | Single: Basic ClientId:ClientSecret | Primary: Basic Username:Password. Fallback: Basic ClientId:ClientSecret. |
| **Who calls catalog** | Only BrandBackgroundService | BrandBackgroundService **and** CatalogSyncJob (no rate limiter on job). |
| **429 handling** | SetRetryAfter from response body | Rate limiter exists; Retry-After from Bamboo may not be applied for catalog. |
| **First-time populate** | One path, correct URL, one caller | Primary path can work; fallback broken; extra caller increases 429 risk; AddBrands can fail (DB). |

---

## 5. Recommendations

1. **Fix v2 fallback catalog URL**  
   Use `$"{_baseUrl.TrimEnd('/')}/catalog"` when `_baseUrl` already contains `api/integration/v1.0` (or equivalent); otherwise keep full path. This makes v2 behave like legacy when using ExternalApiService.

2. **Align v2 with Bamboo’s rate limit**  
   - Have **CatalogSyncJob** use the same `bamboo_catalog` rate limiter before calling Bamboo.  
   - On 429 from Bamboo, parse “wait for X seconds” and call `SetRetryAfter("bamboo_catalog", retrySeconds)` so both BrandBackgroundService and CatalogSyncJob back off.

3. **Optional: Prefer legacy-style single path**  
   For first-time populate, consider using only ExternalApiService (ClientId:ClientSecret) and the same URL construction as legacy, until BambooApiClient + rate limiting and AddBrands are fully stable.

4. **Debug AddBrands on v2**  
   Reproduce “An error occurred while saving the entity changes” (e.g. run sync when catalog returns 200), capture full exception and DB constraint errors, and fix mapping/constraints so first-time insert succeeds.

---

## 6. Conclusion

- **Legacy Steller** uses a single, simple flow: one client (ExternalApiService), correct URLs (base + `/catalog` or `/categories`), Basic ClientId:ClientSecret, and 429 handling with SetRetryAfter. It “knows how to talk to Bamboo” in the sense of correct path and backoff.
- **Steller v2** adds a Bamboo client and a fallback, but the **fallback catalog URL is wrong** when BaseUrlV1 already includes the path (double path), and **two callers** (BrandBackgroundService + CatalogSyncJob) can hit Bamboo’s rate limit. AddBrands can also fail with DB errors.  
- Fixing the fallback URL, unifying rate limiting and Retry-After for catalog, and fixing AddBrands/DB issues will bring v2 in line with legacy behavior and allow the catalog to populate the first time.

---

## 7. Why the AI failed to design the catalog service (and how to avoid it)

### 7.1 Why the design failed

1. **No single source of truth for “how we talk to Bamboo”**  
   The AI introduced a **new** client (BambooApiClient) and a **fallback** (ExternalApiService) without treating the **legacy implementation as the contract**. The fallback URL was invented (base + `/api/integration/v1.0/catalog`) instead of reusing legacy’s rule: base already contains the path, so append only `/catalog`. That led to the double-path bug when BaseUrlV1 was the full prefix.

2. **Inconsistent URL construction**  
   Legacy uses “base + suffix” consistently (BaseUrlV1 + `/catalog`, BaseUrlV2 + `/categories`). The AI assumed a different convention for “Bamboo” (domain-only base + full path) and applied it in the fallback without checking that config still uses the legacy-style base (with path included). So one code path was correct, the other wrong.

3. **Multiple callers, no shared quota**  
   The AI added CatalogSyncJob as a second consumer of the catalog API but did not wire it to the same rate limiter as BrandBackgroundService. Legacy has only one caller; the design did not preserve that “one logical consumer” constraint or make both callers share one quota.

4. **429 handling not carried over**  
   Legacy parses Bamboo’s “wait for X seconds” and sets `SetRetryAfter`. The v2 design did not explicitly require “on 429 from Bamboo catalog, set retry-after for the same key the limiter uses,” so the behavior was not guaranteed.

5. **No design-time check against legacy**  
   There was no step that said: “For catalog sync, diff against legacy: URLs, auth, who calls, rate limit, 429 handling.” So drift (wrong URL, extra caller, missing retry-after) was not caught at design time.

### 7.2 How to avoid this in the future

1. **Treat legacy as the contract for external integrations**  
   For any flow that already works in production (e.g. catalog sync with Bamboo), **define the contract** from legacy: URLs, auth, which app component calls, rate limits, 429 behavior. New or AI-generated code must either reuse that contract or explicitly document and test every divergence.

2. **Single, explicit “Bamboo catalog” contract**  
   Maintain one place (e.g. a short doc or ADR) that states:  
   - Catalog URL = `{BaseUrlV1}/catalog` (with BaseUrlV1 including `/api/integration/v1.0` if applicable).  
   - Auth = Basic (ClientId:ClientSecret or Username:Password per Bamboo docs).  
   - Only one logical “consumer” of catalog per hour, or all consumers share the same rate limiter and Retry-After.  
   Any new service or job that calls catalog must be checked against this.

3. **Mandatory parity checklist for rewrites**  
   When (re)designing a service that replaces or mirrors legacy:  
   - [ ] URLs: same base + suffix rules as legacy (or documented exception).  
   - [ ] Auth: same credentials / same client as legacy (or documented exception).  
   - [ ] Callers: list all callers; if more than one, they share rate limit and Retry-After.  
   - [ ] 429: Bamboo “wait X seconds” is parsed and applied to the same key used by the limiter.  
   Run this checklist before considering the design done.

4. **AI prompts and context**  
   When using AI to design or refactor integration code:  
   - Provide **legacy code or a short spec** (URLs, auth, who calls, 429 handling) as required context.  
   - Ask for “same URL construction as legacy” or “diff against legacy and list differences.”  
   - Require that any fallback path use the **exact same** URL rule as the primary path when they target the same API.

5. **Tests that encode the contract**  
   Add tests (or at least runbooks) that assert:  
   - Catalog request uses the intended URL (e.g. no double path).  
   - All catalog callers go through the same rate limiter (or are explicitly excluded with a reason).  
   - A 429 response leads to Retry-After being set for the catalog key.  
   These tests guard against regressions when AI or humans change the design.

### 7.3 Proposed resolution

| Area | Resolution |
|------|------------|
| **Catalog URL** | ✅ **Done:** Fallback uses `_baseUrl.TrimEnd('/') + "/catalog"` (same as legacy). No double path. |
| **Rate limit** | **Do:** Make CatalogSyncJob call `WaitForSlotAsync("bamboo_catalog")` before fetching catalog so both BrandBackgroundService and CatalogSyncJob share one quota. |
| **429 / Retry-After** | **Do:** When Bamboo catalog returns 429, parse “wait for X seconds” and call `SetRetryAfter("bamboo_catalog", X)` (same as legacy). Ensure BambooApiClient or a central handler does this so all callers benefit. |
| **AddBrands / DB** | **Do:** Reproduce “An error occurred while saving the entity changes” with full exception and DB logs; fix constraints or mapping so first-time catalog insert succeeds. |
| **Contract doc** | **Do:** Add a short “Bamboo catalog contract” (URLs, auth, callers, rate limit, 429) to the repo and point AI and developers to it for any catalog-related change. |
| **Parity checklist** | **Do:** Use the checklist above (or a one-pager) for any future “replace legacy” or “new integration” design so AI and humans don’t reintroduce URL/caller/429 drift. |

**Summary:** The AI failed by not using legacy as the single source of truth for URLs, callers, and 429 behavior. The resolution is to fix the remaining code (rate limiter for CatalogSyncJob, Retry-After, AddBrands), and to **institutionalize** the contract and parity checklist so future designs—AI or not—don’t repeat the same mistakes.
