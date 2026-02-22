# User-Flow Integration Test Plan

**Owner:** QA Agent  
**Purpose:** Define a detailed plan for user-flow integration tests — multi-step API journeys that simulate real partner and admin behavior.  
**Scope:** Partner API, Admin API, Wallet, Order lifecycle.  
**Last Updated:** 2026-02-18  
**Reference:** `docs/INDEX.yaml`, `docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md`, `docs/architecture/atlas/apis.yaml`

---

## 1. Overview

### 1.1 Definition

**User-flow integration test:** A test that executes multiple API calls in sequence, simulating a real user journey (partner or admin). Each flow represents one user goal, asserts side effects (wallet deduction, order completion, gift cards), and does not change Steller application code.

### 1.2 Goals

- Validate end-to-end behavior from the user perspective (what partners and admins actually do).
- Assert implications: wallet deduction, gift card serial/PIN, transaction history — not just HTTP status codes.
- Run as part of CI/regression without requiring a live vendor or UI.
- Complement existing service-level integration tests (P1–P5) with API-level journey tests.

### 1.3 Key Principle

**No Steller code changes.** Tests use only HTTP calls (and, where necessary, job execution to simulate async completion). No modifications to controllers, services, or DTOs.

---

## 2. Test Infrastructure

### 2.1 Existing Components

| Component | Location | Role |
|-----------|----------|------|
| `CustomWebApplicationFactory` | `Tests.Integration/CustomWebApplicationFactory.cs` | In-memory API host, test DB, mock vendor |
| `TestDataFactory` | `Tests.Integration/TestDataFactory.cs` | Create partners, products, balances |
| `MockBambooClient` | `Tests.Mocks/MockBambooClient.cs` | Mock vendor (PlaceOrder, GetOrderDetails) |
| `MockBackgroundJobClient` | `Tests.Mocks/MockBackgroundJobClient.cs` | Captures enqueued jobs; does not run them |
| `AuthTests` | `Tests.Integration/AuthTests.cs` | Pattern for `HttpClient` + API key |

### 2.2 Job Execution Strategy

Orders are fulfilled asynchronously: `POST /api/orders` enqueues `PlaceBambooOrderJob` and `PollBambooOrderJob`. In tests, `MockBackgroundJobClient` captures jobs but does not execute them.

**Approach:** After `POST /api/orders`, the test must simulate job completion by resolving and invoking the jobs directly (same as `OrderServiceTests.P2_08`). This keeps the test focused on API responses while ensuring order completion.

```csharp
// After POST /api/orders (202), simulate async fulfillment:
var placeJob = _serviceProvider.GetRequiredService<PlaceBambooOrderJob>();
await placeJob.ExecuteAsync(orderId);
var pollJob = _serviceProvider.GetRequiredService<PollBambooOrderJob>();
await pollJob.ExecuteAsync(orderId);
```

**Reusable rule:** All new tests use the existing fixture and TestDataFactory. Add tests to existing test classes (e.g. `UserFlowIntegrationTests`, `WebhookTests`) following the plan; do not create new test projects or duplicate fixtures for the same scope. See `docs/qa/TESTING_INDEX.md`.

### 2.3 Auth Model

| Flow Type | Auth | Source |
|-----------|------|--------|
| Partner | `x-api-key` | `IApiKeyService.GenerateKeyAsync(partnerId)` |
| Admin | `Authorization: Bearer <JWT>` | `POST /api/auth/login` → access token |

---

## 3. Partner User-Flow Specifications

### 3.1 UF-P1: Partner Creates Order and Receives Gift Card (Happy Path)

**User goal:** Partner places an order and receives serial + PIN when order completes.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Create partner (balance ≥ 500), product SKU, API key, mock vendor success | Partner and product exist |
| 1 | `GET /api/wallet/me` (x-api-key) | 200, balance = expected (e.g. 500) |
| 2 | `POST /api/orders` (sku, faceValue, quantity, referenceId) | 202, `id` present, `status` = "Processing", `cards` = [] |
| 3 | Run PlaceBambooOrderJob, PollBambooOrderJob (simulate async) | — |
| 4 | `GET /api/orders/{orderId}` | 200, `status` = "Completed", `cards.Count` ≥ 1 |
| 5 | Assert `cards[0].serial` non-empty, `cards[0].pin` non-empty | Serial and PIN present |
| 6 | `GET /api/wallet/me` (x-api-key) | 200, balance decreased by `saleTotal` |

**Test name:** `UF_P1_Partner_CreateOrder_ReceivesGiftCard`  
**Dependencies:** MockBambooClient.SetScenario("success"), SetGetOrderDetailsScenario("Succeeded", cardCount: 1)

---

### 3.2 UF-P2: Partner Checks Wallet After Order (Side Effect)

**User goal:** Partner verifies wallet deduction after placing an order.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Partner (500), product, API key | — |
| 1 | `GET /api/wallet/me` (x-api-key) | 200, initial balance recorded |
| 2 | `POST /api/orders` | 202, orderId, saleTotal captured |
| 3 | Run PlaceBambooOrderJob, PollBambooOrderJob | — |
| 4 | `GET /api/wallet/me` (x-api-key) | 200, balance = initial - saleTotal |
| 5 | `GET /api/wallet/transactions` (x-api-key) | Debit entry for order present |

**Test name:** `UF_P2_Partner_WalletDeductionAfterOrder`  
**Note:** If no GET transactions endpoint exists, assert only wallet balance.

---

### 3.3 UF-P3: Partner Idempotency (Duplicate referenceId)

**User goal:** Partner retries order with same referenceId; no double charge.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Partner (500), product, API key, referenceId = Guid | — |
| 1 | `POST /api/orders` (referenceId) | 202, orderId1, saleTotal |
| 2 | `GET /api/wallet/me` (x-api-key) | balance1 = initial - saleTotal |
| 3 | `POST /api/orders` (same referenceId) | 202, same orderId1 |
| 4 | `GET /api/wallet/me` (x-api-key) | balance = balance1 (unchanged) |

**Test name:** `UF_P3_Partner_Idempotency_NoDoubleCharge`

---

### 3.4 UF-P4: Partner Catalog → Order (Full Journey)

**User goal:** Partner fetches catalog, picks a product, places order, gets cards.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Partner, product with SKU, API key | — |
| 1 | `GET /api/brand/getCatalog` | 200, products contain expected SKU |
| 2 | `POST /api/orders` (sku from catalog, faceValue, quantity) | 202 |
| 3 | Run PlaceBambooOrderJob, PollBambooOrderJob | — |
| 4 | `GET /api/orders/{orderId}` | 200, status = Completed, cards.Count ≥ 1 |

**Test name:** `UF_P4_Partner_CatalogToOrder_FullJourney`

---

### 3.5 UF-P5: Partner Insufficient Funds (Rejected)

**User goal:** Partner with low balance cannot place order; wallet unchanged.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Partner (20), product (faceValue 50), API key | — |
| 1 | `GET /api/wallet/me` (x-api-key) | balance = 20 |
| 2 | `POST /api/orders` (sku, faceValue 50, quantity 1) | 400, message contains "Insufficient" |
| 3 | `GET /api/wallet/me` (x-api-key) | balance = 20 (unchanged) |

**Test name:** `UF_P5_Partner_InsufficientFunds_OrderRejected`

---

## 4. Admin User-Flow Specifications

### 4.1 UF-A1: Admin Logs In and Credits Partner Wallet

**User goal:** Admin supports a partner by crediting their wallet.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Partner, admin user | — |
| 1 | `POST /api/auth/login` (admin creds) | 200, access_token |
| 2 | `GET /api/wallet/{partnerId}` (Authorization: Bearer) | 200, initial balance |
| 3 | `POST /api/wallet/{partnerId}/credit` (body: amount, description, referenceId) | 200 |
| 4 | `GET /api/wallet/{partnerId}` (Authorization: Bearer) | Balance increased by credit amount |

**Test name:** `UF_A1_Admin_Login_CreditPartnerWallet`  
**Auth:** Admin JWT required for GET/POST /api/wallet/{partnerId}.

---

### 4.2 UF-A2: Admin Cancels Order and Partner Gets Refund

**User goal:** Admin cancels an order; partner wallet is refunded.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Partner (500), product, API key, admin JWT; create order (Processing) | orderId, saleTotal |
| 1 | `GET /api/wallet/me` (x-api-key) | balance = 500 - saleTotal |
| 2 | `POST /api/auth/login` (admin) | 200, token |
| 3 | `POST /api/admin/orders/{orderId}/cancel` (body: reason) | 200 |
| 4 | `GET /api/wallet/me` (x-api-key) | balance = 500 (refunded) |

**Test name:** `UF_A2_Admin_CancelOrder_PartnerRefunded`

---

### 4.3 UF-A3: Admin Creates API Key for Partner

**User goal:** Admin provisions API key; partner can use it for catalog/orders.

| Step | API Call | Assertion |
|------|----------|-----------|
| 0 | Setup: Partner, admin JWT | — |
| 1 | `POST /api/auth/login` (admin) | 200, token |
| 2 | `POST /api/admin/partners/{partnerId}/keys` | 200/201, plain key returned (once) |
| 3 | `GET /api/brand/getCatalog` (x-api-key: new key) | 200 |

**Test name:** `UF_A3_Admin_CreateApiKey_PartnerCanUse`

---

## 5. Implementation Structure

### 5.1 Project and Namespace

- **Project:** `Tests.Integration` (existing)
- **Class:** `UserFlowIntegrationTests` (new)
- **Namespace:** `Steller.Tests.Integration`

### 5.2 Test Naming Convention

- **Prefix:** `UF_` (User Flow)
- **Pattern:** `UF_{P|A}{n}_{Persona}_{Action}_{Outcome}`
- **Examples:** `UF_P1_Partner_CreateOrder_ReceivesGiftCard`, `UF_A2_Admin_CancelOrder_PartnerRefunded`

### 5.3 Shared Helpers (New)

| Helper | Purpose |
|--------|---------|
| `GetValidApiKeyAsync(int partnerId)` | Generate API key via IApiKeyService |
| `GetAdminJwtAsync()` | Login admin, return Bearer token |
| `RunOrderFulfillmentJobsAsync(Guid orderId)` | Resolve PlaceBambooOrderJob + PollBambooOrderJob, run ExecuteAsync |
| `CreatePartnerWithBalanceAsync(decimal balance)` | Use TestDataFactory, return (partnerId, apiKey) |
| `EnsureProductExistsAsync(string sku, ...)` | Use TestDataFactory |

### 5.4 File Layout

```
Tests.Integration/
├── AuthTests.cs
├── OrderServiceTests.cs
├── OperationalJobsTests.cs
├── BambooIntegrationTests.cs
├── UserFlowIntegrationTests.cs   ← NEW
├── UserFlowTestHelpers.cs        ← NEW (optional, or inline in class)
├── CustomWebApplicationFactory.cs
└── TestDataFactory.cs
```

---

## 6. Execution Model

### 6.1 Run Command

```bash
cd /opt/steller-v2/steller-backend
dotnet test Tests.Integration/Tests.Integration.csproj \
  --filter "FullyQualifiedName~UserFlowIntegrationTests" \
  --no-build
```

### 6.2 Environment

- **Database:** `TEST_DB_CONNECTION` or default `Host=localhost;Port=5432;Database=StellerTestDB;...`
- **Vendor:** MockBambooClient (no Bamboo credentials)
- **Jobs:** MockBackgroundJobClient; tests invoke jobs directly for fulfillment flows

### 6.3 Run and verify (catalog, admin, partner)

To run the user-flow tests locally (including catalog and admin/partner flows), a PostgreSQL instance is required. Example with Docker:

```bash
# Start Postgres (if not already running)
docker run -d --name steller-test-pg \
  -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=StellerTestDB \
  -p 5433:5432 postgres:16-alpine

# Wait for readiness, then run tests
export TEST_DB_CONNECTION="Host=localhost;Port=5433;Database=StellerTestDB;Username=postgres;Password=postgres;Pooling=false;"
cd /path/to/steller-backend
dotnet test Tests.Integration/Tests.Integration.csproj \
  --filter "FullyQualifiedName~UserFlowIntegrationTests" --no-build -v n
```

**Tests that cover catalog and admin/partner (per plan):**

| Test | What it verifies |
|------|------------------|
| **UF_P4_Partner_CatalogToOrder_FullJourney** | Partner gets catalog (`GET /api/brand/getCatalog`), places order, receives cards. |
| **UF_A3_Admin_CreateApiKey_PartnerCanUse** | Admin creates API key for partner; partner uses new key to call catalog successfully. |
| **UF_A1_Admin_Login_CreditPartnerWallet** | Admin login + credit partner wallet. |
| **UF_A2_Admin_CancelOrder_PartnerRefunded** | Admin cancels order; partner wallet refunded. |

**Verified (2026-02):** Full suite of 8 user-flow tests passed (Partner: UF-P1–P5; Admin: UF-A1–A3). Catalog is returned to partner (x-api-key) and to partner using admin-created key.

### 6.4 CI Integration

- Add user-flow tests to existing integration test job.
- Run after service-level tests (P1–P5).
- Expected duration: ~1–2 min for full suite (estimate).

---

## 7. Phased Rollout

| Phase | Scope | Deliverable |
|-------|-------|-------------|
| **1** | UF-P1, UF-P2, UF-P5 | Partner happy path, wallet side effect, insufficient funds |
| **2** | UF-P3, UF-P4 | Idempotency, catalog→order journey |
| **3** | UF-A1, UF-A2, UF-A3 | Admin flows (login, cancel/refund, API key) |

---

## 8. Success Criteria

- [x] All user-flow tests pass with MockBambooClient (8/8 passed when run with local Postgres).
- [ ] No changes to Steller.Api or Steller.Infrastructure for these tests.
- [ ] Tests are deterministic (no flakiness from timing).
- [ ] Documentation updated: `docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md` (add User-Flow Integration Tests section).

---

## 9. Determinism

**Goal:** No flakiness from timing; tests are repeatable and deterministic.

### 9.1 Rules

| Rule | Why |
|------|-----|
| **No `Thread.Sleep`** | Sleep introduces timing variability; tests may pass or fail depending on load. |
| **No `DateTime.Now` in assertions** | Use fixed dates or inject `IDateTimeProvider`; avoid time-of-day in test logic. |
| **Use `MockBambooClient` scenarios** | Vendor responses must be deterministic (e.g. `SetGetOrderDetailsScenario("Succeeded", cardCount: 1)`). |
| **Run jobs synchronously** | After `POST /api/orders`, resolve `PlaceBambooOrderJob` and `PollBambooOrderJob` and call `ExecuteAsync` directly; do not rely on background scheduler timing. |
| **Respawn / `ResetDatabaseAsync`** | Each test starts from a known DB state; no cross-test pollution. |
| **No shared mutable state** | Use `TestDataFactory` and fixture; avoid static or shared variables that change between runs. |

### 9.2 Checklist for New Tests

- [ ] No `Thread.Sleep`, `Task.Delay` (unless strictly required and documented)
- [ ] Mock vendor (MockBambooClient) returns deterministic responses
- [ ] Jobs invoked directly via `ExecuteAsync`, not by Hangfire scheduler
- [ ] DB reset before test or test class
- [ ] Assertions do not depend on wall-clock time

### 9.3 Reference

Spec index and invariants: [SPEC_INDEX.yaml](SPEC_INDEX.yaml), [INVARIANTS.md](INVARIANTS.md).

---

## 10. References

| Doc | Purpose |
|-----|---------|
| `docs/architecture/atlas/apis.yaml` | API endpoints, auth |
| `docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md` | Workflow docs |
| `docs/qa/SPEC_INDEX.yaml` | Spec ID → test name, invariant refs |
| `docs/qa/INVARIANTS.md` | Formal invariants (financial, wallet, idempotency) |
| `docs/integration/STELLER_INTEGRATION_GUIDE.md` | Partner API contract |
| `docs/RUNNING_API_TESTS.md` | How to run tests, credentials |
| `Tests.Integration/AuthTests.cs` | HttpClient + API key pattern |
| `Tests.Integration/OrderServiceTests.cs` | Job execution pattern (P2_08) |
