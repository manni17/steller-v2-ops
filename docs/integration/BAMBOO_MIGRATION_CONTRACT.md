# Bamboo Migration Contract

**Purpose:** Canonical source for migrating **all** Bamboo-related logic from Legacy to V2.  
**Scope:** Catalog, Categories, Orders, Account/Balance, Notifications, and any other Bamboo API usage.  
**Principle:** Legacy behavior → V2 implementation. No over-engineering.

---

## 1. Migration Rule

| Source | What to take |
|--------|--------------|
| **Legacy** | Behavior: URLs, auth, rate limits, 429 handling, single caller per API |
| **V2** | Implementation: Hangfire, Polly, typed clients, `referenceId` idempotency |

**Before V2 code:** Extract the exact Legacy behavior for that Bamboo flow. Then implement it in V2’s stack without extra call paths or legacy anti-patterns.

---

## 2. Parity Checklist (per Bamboo flow)

Before porting or changing any Bamboo integration, verify:

- [ ] **URLs:** Same base + suffix rules as legacy (or documented exception).
- [ ] **Auth:** Same credentials pattern as legacy (ClientId:ClientSecret or Username:Password per Bamboo docs).
- [ ] **Callers:** One logical consumer per Bamboo endpoint, or all consumers share the same rate limiter.
- [ ] **429 handling:** Bamboo "wait X seconds" parsed and applied to the shared rate limiter key.

---

## 3. Legacy Reference

- **Legacy location:** `/opt/steller-apps/Steller/` (Legacy Steller codebase).
- **Catalog sync:** See `docs/CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md` for legacy vs v2 comparison and root causes.
- **Orders (Legacy):** OrderService uses ExternalApiService `{BaseUrlV1}/orders/checkout` (POST), `{BaseUrlV1}/orders/{orderId}` (GET); rate limiter keys `bamboo_place_order`, `bamboo_get_order`; 429 → ExtractRetrySeconds + SetRetryAfter.
- **Account/Balance:** No Bamboo account or balance API in Legacy; wallet is Steller-internal.
- **Notifications:** No Bamboo notification API found in Legacy.
- **Incident:** V2 over-engineered sync (dual paths, dual callers, broken fallback) caused rate limits and failures; legacy’s simpler flow worked.

---

## 4. Flows in Scope

| Flow | Legacy behavior | V2 implementation | Status |
|------|-----------------|-------------------|--------|
| Catalog sync | ExternalApiService, BaseUrlV1 + `/catalog`, single caller, SetRetryAfter on 429 | ExternalApiService only, WaitForSlot + SetRetryAfter | Done |
| Categories | BaseUrlV2 + `/categories`, same auth | Same (ExternalApiService only) | Done |
| Orders | ExternalApiService BaseUrlV1 + `/orders/checkout`, `/orders/{id}`; rate limiter + 429 | PlaceBambooOrderJob/PollBambooOrderJob: WaitForSlot; BambooVendorAdapter: SetRetryAfter on 429 | Done |
| Account/balance | No Legacy Bamboo API (Steller wallet is internal) | N/A | N/A |
| Notifications | No Legacy Bamboo API found | N/A | N/A |
| *(Others)* | Extract first | Then implement | — |

---

## 5. Anti-Patterns to Block

- Dual code paths for the same Bamboo endpoint (primary + fallback with different URL rules).
- Multiple callers of the same Bamboo endpoint without a shared rate limiter.
- Ignoring Bamboo 429 Retry-After.
- Swallowed EF Core exceptions, hidden background threads, duplicated HTTP clients.
