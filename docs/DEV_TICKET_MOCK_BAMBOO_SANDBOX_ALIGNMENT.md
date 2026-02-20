# Dev Ticket: MockBamboo ↔ Sandbox Bamboo Alignment

**Created:** 2026-02-19  
**Status:** Backlog — Nice to have (bottom of backlog; not a priority)  
**Assignee:** Dev Agent  
**Priority:** Low  

---

## Summary

Align MockBamboo (MockVendorAdapter) with sandbox Bamboo behavior so that offline testing is equivalent to sandbox testing, and the switch from mock → sandbox is credential-only (no code change).

---

## Design Intent

1. **MockBamboo** = sandbox Bamboo simulator for offline testing (no network, no credentials).
2. **Unplug mock → plug sandbox** = set `BAMBOO_USERNAME`, `BAMBOO_PASSWORD`, `BAMBOO_ACCOUNT_ID` (and `BAMBOO_ACCOUNT_ID_INT` if needed); restart. No code change.

---

## Current State

| Component | Behavior |
|-----------|----------|
| MockVendorAdapter | Catalog: hardcoded MOCK-ITUNES-25, MOCK-GOOGLE-50, MOCK-AMAZON-100. PlaceOrder: succeeds for any SKU unless `MOCK_VENDOR_REJECT_FOR_SKU` env matches. |
| BambooVendorAdapter | Calls real Bamboo API. PlaceOrder: success/failure per Bamboo response (e.g. "Product with sku X not found"). VendorApiCall now sets OrderId (fix deployed). |
| Switch | `BAMBOO_USERNAME` set → BambooVendorAdapter; unset → MockVendorAdapter. |

---

## Tasks

### 1. Catalog Alignment (optional)
- [ ] Confirm mock catalog SKUs match sandbox test products. If sandbox uses different SKUs, update MockVendorAdapter to match or document the mapping.
- [ ] Document: "Mock catalog = sandbox-equivalent; these SKUs work in both offline (mock) and sandbox (credentials)."

### 2. PlaceOrder Rejection (optional refinement)
- [ ] Consider: Mock PlaceOrder rejects SKUs **not in catalog** (natural sandbox behavior), instead of env-based `MOCK_VENDOR_REJECT_FOR_SKU`.
- [ ] If adopted, remove `MOCK_VENDOR_REJECT_FOR_SKU`; rejection becomes catalog-driven.
- [ ] Keep `MOCK_VENDOR_REJECT_FOR_SKU` if explicit E2E refund-path testing is preferred.

### 3. CheckAvailability (optional)
- [ ] Mock CheckAvailability: return true only for SKUs in catalog; false otherwise (align with sandbox).

### 4. Documentation
- [ ] Add/update section in README or UNPLUG_LEGACY_AND_BAMBOO_SANDBOX.md:
  - Mock (offline): `BAMBOO_USERNAME` unset.
  - Sandbox: set `BAMBOO_USERNAME`, `BAMBOO_PASSWORD`, `BAMBOO_ACCOUNT_ID`, optionally `BAMBOO_ACCOUNT_ID_INT`, `BAMBOO_BASE_URL`.
  - Production: same vars, production credentials.

---

## Acceptance Criteria

- [ ] MockBamboo behavior is documented as sandbox-equivalent.
- [ ] Switch from mock → sandbox requires only credential env vars and restart.
- [ ] No code change needed when moving from offline mock to sandbox.

---

## Notes

- Captured so we don't forget. Not a priority; do when capacity allows.

## Related

- Phase 4 E2E refund path confirmed with MockVendorAdapter + `MOCK_VENDOR_REJECT_FOR_SKU` (2026-02-19).
- BambooVendorAdapter VendorApiCall OrderId fix already deployed (OrderId passed in VendorOrderRequest).
- Catalog sync: see `docs/CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md` if related over-engineering concerns apply.
