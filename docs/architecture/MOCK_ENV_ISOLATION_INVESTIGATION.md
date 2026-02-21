# Mock Environment Isolation — Investigation Report

**Date:** 2026-02-21  
**Scope:** Trace mock data (MOCK-ITUNES-25, etc.) to source; isolate mock vs Bamboo Sandbox env.

---

## 1. Executive Summary

| Mode | Trigger | IVendorAdapter | IBambooApiClient | Catalog Source |
|------|---------|----------------|------------------|----------------|
| **Mock** | `BAMBOO_USERNAME` **unset** | MockVendorAdapter | **Not registered** | MockVendorAdapter returns MOCK-ITUNES-25, MOCK-GOOGLE-50, MOCK-AMAZON-100 |
| **Sandbox** | `BAMBOO_USERNAME` **set** | BambooVendorAdapter | BambooApiClient (real HTTP) | Bamboo API → real products only |

**Finding:** Mock SKUs (MOCK-ITUNES-25, etc.) come from **MockVendorAdapter**. Sandbox should only provide real cards. If MOCK products appear while connected to Sandbox, they are **leftover DB rows** from a prior mock run.

---

## 2. Environment Switch (Program.cs)

```
BAMBOO_USERNAME set?
├── YES → BambooVendorAdapter, BambooApiClient, bamboo-catalog, bamboo-transaction
└── NO  → MockVendorAdapter ONLY (IBambooApiClient NOT registered)
```

**Code path:** `Steller.Api/Program.cs` lines 403–473

- **Sandbox:** Registers `IVendorAdapter` = BambooVendorAdapter, `IBambooApiClient` = BambooApiClient, `bamboo-catalog` and `bamboo-transaction` HttpClients.
- **Mock:** Registers only `IVendorAdapter` = MockVendorAdapter. `IBambooApiClient` and Bamboo HttpClients are not registered.

---

## 3. Catalog Data Flow

### 3.1 Primary catalog sync (Brands/Products)

| Component | Uses | Behavior |
|-----------|------|----------|
| BrandService.SyncCatalogFromBambooAsync | IBambooApiClient | Fetches from Bamboo API, persists to Brands/Products |
| BrandBackgroundService | BrandService (→ IBambooApiClient) | Runs every 24h |
| POST /api/brand/sync-catalog | BrandService | Manual trigger |

**Mock mode:** IBambooApiClient is not registered, so BrandService cannot be resolved. BrandBackgroundService and sync endpoints will fail in mock mode.

### 3.2 CatalogSyncJob (cost updates)

| Component | Uses | Behavior |
|-----------|------|----------|
| CatalogSyncJob | IVendorAdapter.GetCatalogAsync | **Updates existing** products only; does not create new products |

- **Mock:** IVendorAdapter = MockVendorAdapter → returns MOCK-ITUNES-25, etc.
- **Sandbox:** IVendorAdapter = BambooVendorAdapter → calls IBambooApiClient → real Bamboo catalog.

CatalogSyncJob only updates SKUs that already exist in the DB; it does not insert new products.

### 3.3 Partner-facing catalog

| Component | Uses | Behavior |
|-----------|------|----------|
| BrandService.GetCatalogAsync | DB (Brands, Products) | Reads products from DB |

Products are read from the database. MOCK SKUs appear if they were previously synced or seeded and never removed.

---

## 4. Mock Data Sources

| Source | SKUs | When Used |
|--------|------|-----------|
| MockVendorAdapter.GetCatalogAsync | MOCK-ITUNES-25, MOCK-GOOGLE-50, MOCK-AMAZON-100 | BAMBOO_USERNAME unset |
| MockVendorAdapter.PlaceOrderAsync | Same SKUs (rejects others) | BAMBOO_USERNAME unset |
| MockBambooClient (tests) | Configurable | Tests.Integration only |
| MockBambooApiClient (tests) | GetCatalogAsync returns empty Brands | Tests only |

**File:** `Steller.Infrastructure/Adapters/MockVendorAdapter.cs` lines 22–38

---

## 5. How MOCK Products Reach the DB

1. **BrandService.SyncCatalogFromBambooAsync** is the only code path that creates Brands/Products from a vendor catalog.
2. BrandService uses **IBambooApiClient** (not IVendorAdapter).
3. In mock mode, IBambooApiClient is not registered, so SyncCatalogFromBambooAsync cannot run.
4. Conclusion: MOCK products are not inserted by the current mock setup. They can only come from:
   - An older version that registered MockBambooApiClient for IBambooApiClient and returned mock catalog, or
   - DB persistence from a past sync when mock/sandbox behavior differed, or
   - Bamboo Sandbox returning MOCK SKUs (less likely if Sandbox only returns real cards).

---

## 6. Isolation Recommendations

### 6.1 Sandbox mode: exclude mock products

When `BAMBOO_USERNAME` is set, avoid persisting or exposing mock products:

- **Option A (sync):** In BrandService.MapBambooCatalogToBrands or AddBrands, skip products where `p.Sku.StartsWith("MOCK-")`.
- **Option B (cleanup):** After sync, delete products with `Sku.StartsWith("MOCK-")`.
- **Option C (DB migration/script):** One-time delete:  
  `DELETE FROM "Products" WHERE "Sku" LIKE 'MOCK-%';`

### 6.2 Mock mode: allow IBambooApiClient and catalog sync

Today, BrandService and BrandBackgroundService fail in mock mode because IBambooApiClient is missing:

- In the `else` block (mock mode), register:
  - `IBambooApiClient` = `MockBambooApiClient` (returns empty catalog for sync), and
  - Minimal/void `bamboo-catalog` and `bamboo-transaction` HttpClients if needed.
- Alternatively, make BrandBackgroundService conditional on BAMBOO_USERNAME so it does not run in mock mode.

### 6.3 Environment guard

Add a startup or runtime check:

- If `BAMBOO_USERNAME` is set and any product has `Sku.StartsWith("MOCK-")`, log a warning or run a cleanup.

---

## 7. Quick Reference

| Env Var | Mock Mode | Sandbox Mode |
|---------|-----------|--------------|
| BAMBOO_USERNAME | Unset | Set (sandbox user) |
| IVendorAdapter | MockVendorAdapter | BambooVendorAdapter |
| IBambooApiClient | Not registered | BambooApiClient |
| Catalog source | MockVendorAdapter (MOCK-*) | Bamboo API (real) |
| Orders | MockVendorAdapter (fake cards) | Bamboo API (real) |

---

## 8. Related Docs

- `docs/DEV_TICKET_MOCK_BAMBOO_SANDBOX_ALIGNMENT.md`
- `docs/MOCK_AND_SANDBOX_SWITCH.md`
- `docs/SANDBOX.md`
- `Steller.Infrastructure/Adapters/MockVendorAdapter.cs`
