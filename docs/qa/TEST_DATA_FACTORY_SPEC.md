# Steller v2 — Test Data Factory Specification

**Status:** Approved  
**Last Updated:** 2026-02-18  
**Purpose:** Define **scenario-based** test data generation for integration tests. The factory must support explicit scenarios (e.g. partner with zero balance → expect failure; partner with sufficient balance → expect success) so that tests are repeatable and self-documenting.

---

## 1. Requirements

### 1.1 Scenario-Based Generation

- **No ad-hoc only data:** Tests that need a partner, wallet, or order should use factory methods that express **intent** (e.g. “partner that can place one order”, “partner that must fail on order”).
- **Repeatable:** Same scenario yields consistent state; tests do not depend on leftover data from other tests.
- **Isolation:** Each test or test class can get a clean DB state (via existing `ResetDatabaseAsync` / Respawn) and then apply only the scenarios it needs.

### 1.2 Critical Scenarios (Must Support)

| Scenario | Factory usage | Expected outcome in test |
|----------|----------------|---------------------------|
| Partner with **zero** balance | `CreatePartner(balance: 0)` (or equivalent) | Order creation **fails**; wallet unchanged. |
| Partner with **sufficient** balance | `CreatePartner(balance: 1000)` (or equivalent) | Order creation **succeeds** (subject to product/margin rules). |
| Partner with **low** balance | `CreatePartner(balance: 20)` with product requiring e.g. 50 | Order creation **fails** (insufficient funds). |
| Idempotency | Same `referenceId` / `RequestId` twice | Second request returns same order; **single** debit. |
| Concurrency | One partner, multiple concurrent debits | All debits succeed; final balance = initial − sum(debits). |

---

## 2. Factory Interface (Conceptual)

The following is a **spec** for what the factory must support. Implementation lives in the Steller backend (e.g. `Tests.Integration/TestDataFactory.cs` or similar).

### 2.1 Partner and Wallet

- **CreatePartner(balance: decimal)**  
  - Ensures a partner exists (e.g. PartnerId = 1 or a dedicated test partner).  
  - Ensures the partner has a wallet with **AvailableBalance = balance**.  
  - Optional: accept `partnerId` to create multiple partners.  
  - Returns partner id and wallet id for use in tests.

- **CreatePartnerWithBalanceZero()**  
  - Shorthand for `CreatePartner(balance: 0)`.  
  - Used in Atomic Rollback–style tests: place order → expect failure, wallet still 0.

- **CreatePartnerWithBalanceSufficient(amount = 1000)**  
  - Shorthand for `CreatePartner(balance: amount)`.  
  - Used in success-path tests (e.g. idempotency, concurrency, happy path order).

### 2.2 Products and Catalog

- **EnsureProductExists(sku, minFaceValue, maxFaceValue, ...)**  
  - Ensures a product exists (and optionally partner-specific pricing) so that order creation can target it.  
  - Used so tests are not dependent on arbitrary seed data; they declare the product they need.

- **EnsureCatalogForPartner(partnerId, productSkus)**  
  - Ensures the partner has catalog/pricing for the given SKUs (so getCatalog and order creation align).

### 2.3 Orders (Optional)

- **CreateOrder(partnerId, sku, faceValue, quantity, referenceId)**  
  - Creates an order in the DB with given parameters (for tests that need to assert on existing order state or run jobs).  
  - Use sparingly; prefer “create via API” in integration tests where the goal is to test the full flow.

---

## 3. Usage in Tests (Examples)

### 3.1 Atomic Rollback (T_01 style)

- Reset DB; seed reference data (currencies, order statuses, etc.).
- `CreatePartner(balance: 20)`.
- Ensure product exists with sell price > 20 (e.g. face value 50).
- Call order creation (via service or API) → expect **failure**.
- Assert wallet balance still **20** (no debit).

### 3.2 Success Path (e.g. Idempotency T_03)

- Reset DB; seed reference data.
- `CreatePartner(balance: 1000)`.
- Ensure product exists (e.g. MOCK-ITUNES-25 or test SKU).
- First order creation with `referenceId = "idem-1"` → success.
- Second order creation with same `referenceId = "idem-1"` → same order returned; wallet debited **once**.
- Assert wallet balance = 1000 − (one order’s sale total).

### 3.3 Concurrency (T_04 style)

- Reset DB; seed reference data.
- `CreatePartner(balance: 1000)`.
- Run N concurrent debits (e.g. 2 × 100) from separate scopes/DbContexts.
- Assert both debits succeed and final balance = 1000 − 200 = 800.

---

## 4. Implementation Notes

### 4.1 Where to Implement

- **Location:** Steller backend, e.g. `Tests.Integration/TestDataFactory.cs` (or `IntegrationTestHelpers.cs` extension).  
- **Dependencies:** `ApplicationDbContext`, `IWalletService` (or direct DB for wallet/partner seed).  
- **Lifetime:** Factory can be created per test or shared within a test class; must not rely on shared mutable state across different test classes.

### 4.2 DB State

- **Reset:** Use existing `CustomWebApplicationFactory` + `ResetDatabaseAsync` (or Respawn) so each test starts from a known state.  
- **Seeding:** Factory may call into existing seed logic (currencies, order statuses, wallet types) or assume it has already been run after reset.  
- **Partner/Wallet:** Factory creates or updates Partner and Wallet so that `AvailableBalance` matches the requested balance (via credit or direct set, depending on invariants and constraints).

### 4.3 API Key (E2E)

- For **E2E** tests that call the API, partner API key is obtained via Admin API or pre-provisioned secret; the factory does not need to expose API keys unless a test explicitly needs to simulate a partner request.

---

## 5. References

- **QA plan:** [../STELLER_V2_QA_PLAN.md](../STELLER_V2_QA_PLAN.md)
- **Critical path:** [QA_CRITICAL_PATH_AND_PIPELINE.md](QA_CRITICAL_PATH_AND_PIPELINE.md)
- **Critical 4 tests:** Steller backend `Tests.Integration/OrderServiceTests.cs`, `WalletServiceTests.cs`
- **CustomWebApplicationFactory / Respawn:** Steller backend `Tests.Integration/CustomWebApplicationFactory.cs`, `RespawnDatabaseCleaner.cs`
