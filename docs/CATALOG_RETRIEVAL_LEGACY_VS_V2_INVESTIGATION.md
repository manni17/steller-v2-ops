# Catalog Retrieval (Partner): Legacy Steller vs Steller v2 — Investigation

**Goal:** Understand how legacy Steller returns catalog to partners vs how v2 does it (GET /api/brand/getCatalog, partner filtering, API key required), for test design and parity checks.

---

## 1. How legacy Steller returns catalog to partners

**Location:** `/opt/steller-apps/Steller/`

Legacy likely exposes a catalog or products endpoint for partners; exact route and auth (JWT or API key) depend on the legacy API. Partner-specific pricing or filtering (e.g. by partner Id or tier) may be applied in the service layer.

### Summary (legacy)

| Aspect        | Legacy                                                                 |
|---------------|------------------------------------------------------------------------|
| Endpoint      | Implementation-specific (e.g. GET catalog/products)                    |
| Auth          | JWT or API key                                                         |
| Filtering     | Partner-specific if implemented                                       |

---

## 2. How Steller v2 returns catalog to partners

**Location:** `/opt/steller-v2/steller-backend/` — **BrandController.GetCatalog**, **IBrandService.GetCatalogAsync**

### Flow

1. **GET /api/brand/getCatalog** — Requires partner context (API key or JWT). **ApiKeyMiddleware** sets **PartnerId** from x-api-key; **BrandController.GetCatalog** uses **JwtReader.GetUserPartnerId(User)** (0 if missing).
2. If **PartnerId == 0** → **401 Unauthorized** ("Partner context required. Provide x-api-key or sign in as a partner.").
3. **IBrandService.GetCatalogAsync(partnerId)** — Returns catalog (brands/products) with **partner-specific pricing** (e.g. discounted prices per partner). Data may come from DB (synced from Bamboo via catalog sync) or cached.
4. **/api/brand/sync-catalog** is **AllowAnonymous** and skipped by ApiKeyMiddleware (manual sync); not the partner-facing catalog read path.

### Summary (v2)

| Aspect        | V2                                                                      |
|---------------|-------------------------------------------------------------------------|
| Endpoint      | GET /api/brand/getCatalog                                               |
| Auth          | x-api-key (or JWT) required; PartnerId required                         |
| Filtering     | GetCatalogAsync(partnerId) — partner-specific pricing                  |
| Anonymous     | /api/brand/sync-catalog (sync only)                                    |

---

## 3. Root causes / divergence

- **Legacy:** Endpoint and auth may differ.
- **V2:** Explicit GET /api/brand/getCatalog; PartnerId required; GetCatalogAsync(partnerId) for partner-specific catalog.

---

## 4. Side-by-side comparison

| Item              | Legacy                          | V2                                    |
|-------------------|---------------------------------|----------------------------------------|
| **Endpoint**      | Implementation-specific        | GET /api/brand/getCatalog              |
| **Auth**          | JWT or API key                  | x-api-key (or JWT); PartnerId required |
| **Partner pricing** | If implemented                | GetCatalogAsync(partnerId)             |

---

## 5. Recommendations

1. Treat v2 as the target: GET /api/brand/getCatalog, PartnerId required, partner-specific catalog.
2. Parity checklist: 401 when PartnerId missing; 200 with catalog when x-api-key valid.
3. Test design: P5_04 (catalog requires API key).

---

## 6. Conclusion

- **Legacy** catalog retrieval is implementation-specific.
- **V2** uses GET /api/brand/getCatalog with x-api-key (or JWT), PartnerId, and GetCatalogAsync(partnerId) for partner-specific catalog.
- V2 is the reference for catalog retrieval and test coverage.
