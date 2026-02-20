# Steller v2 — System-Wide QA Plan

**Status:** Approved (with constraint adjustments)  
**Last Updated:** 2026-02-18  
**Authority:** Single source of truth for QA strategy; execution follows [STELLER_QA_AGENT_PROTOCOL_V2.md](STELLER_QA_AGENT_PROTOCOL_V2.md) and [qa/QA_CRITICAL_PATH_AND_PIPELINE.md](qa/QA_CRITICAL_PATH_AND_PIPELINE.md).

---

## 1. Testing Strategy

### 1.1 Test Pyramid (Constraint-Adjusted)

In transactional financial systems, logic errors occur during **state transitions** (Order → Wallet → DB). Unit tests that mock the database hide locking and concurrency bugs. Integration tests with real PostgreSQL are prioritized.

| Layer | Target | Purpose |
|-------|--------|---------|
| **Unit** | **40%** | Business logic in isolation (pricing, validation, DTOs). Fast, no DB. |
| **Integration** | **50%** | State transitions, locking, rollback, real DB via `CustomWebApplicationFactory` + PostgreSQL container. |
| **E2E** | **10%** | Full API contract, Autonomous QA Agent, sandbox vendor. Grey-box. |

**Rationale:** Shift left on integration. The Critical 4 (Atomic Rollback, Profit Guard, Idempotency, Concurrency) are integration tests; they are **gatekeepers** and run on every commit.

### 1.2 The Critical 4 — Non-Negotiable Gatekeepers

These four tests are **not** optional. They run on **every commit**. If any fails, the **build is rejected immediately**.

| Test | Purpose | Failure = Build Rejected |
|------|---------|--------------------------|
| **T_01 Atomic Rollback** | Insufficient funds → order rejected, wallet unchanged | Yes |
| **T_02 Profit Guard** | Negative margin → order rejected | Yes |
| **T_03 Idempotency** | Duplicate RequestId → single debit, same order returned | Yes |
| **T_04 Concurrency** | Thread-safe wallet debits via DB locking | Yes |

**Implementation:** [qa/QA_CRITICAL_PATH_AND_PIPELINE.md](qa/QA_CRITICAL_PATH_AND_PIPELINE.md) defines the pipeline. Gate 1 is the Critical 4 against a real DB container; **HARD STOP** on failure.

### 1.3 Verified Pipeline (Summary)

1. **Commit** → Light unit tests (business logic only).
2. **Gate 1 (Critical 4)** → Integration tests vs real DB container. **HARD STOP** if failed.
3. **Build** → Docker image creation.
4. **Gate 2 (E2E)** → Autonomous QA Agent: real orders vs sandbox vendor, API contract verification.
5. **Deploy** → Production release.

Full diagram and directives: [qa/QA_CRITICAL_PATH_AND_PIPELINE.md](qa/QA_CRITICAL_PATH_AND_PIPELINE.md).

---

## 2. Test Categories

### 2.1 Functional

- **API contract:** Endpoints, status codes, JSON shape, error codes (`API_KEY_MISSING`, `API_KEY_INVALID`).
- **Business logic:** Order lifecycle, wallet credit/debit/refund, pricing, idempotency.
- **Integration:** Order → Wallet → DB → Hangfire job; vendor adapter (Bamboo vs Mock).

### 2.2 Financial Invariants

- **Ledger consistency:** `Sum(WalletHistory) == Wallet.AvailableBalance`. If this breaks, nothing else matters (see QMS).
- **Refund safety:** Double-refund prevention; refund amount = SaleTotal.
- **Profit guard:** No negative-margin orders accepted.

### 2.3 Non-Functional

- **Reliability:** Circuit breaker, retries, auto-refund on vendor failure.
- **Security:** API key, JWT, tenant isolation (PartnerId from claims only).
- **Performance:** Target p95 <200ms; measured in E2E/monitoring, not as a gate on every commit.

---

## 3. Test Automation

### 3.1 Unit (40%)

- **Location:** `Tests.Unit/` (to be created where needed).
- **Scope:** Pricing calculator, validators, DTO mapping, pure business rules.
- **Tools:** xUnit, Moq, FluentAssertions.
- **No DB:** All dependencies mocked.

### 3.2 Integration (50%)

- **Location:** [Steller backend] `Tests.Integration/`.
- **Scope:** OrderService, WalletService, tenant isolation, webhooks; real PostgreSQL via `CustomWebApplicationFactory`.
- **Tools:** xUnit, CustomWebApplicationFactory, RespawnDatabaseCleaner, MockBambooClient.
- **Critical 4:** Part of this layer; run on every commit (Gate 1).

### 3.3 E2E (10%)

- **Location:** Protocol-driven; agent runs per [STELLER_QA_AGENT_PROTOCOL_V2.md](STELLER_QA_AGENT_PROTOCOL_V2.md).
- **Scope:** Source of Truth audit, `POST /api/orders`, logs, DB observation, sandbox vendor.
- **Trigger:** After build (Gate 2) before deploy.

### 3.4 UI Testing (Separate Layer)

- **Location:** `Tests.UI/` (to be created) or separate repo for Vue dashboards.
- **Scope:** Critical user flows in Admin Dashboard (port 8080) and Consumer Dashboard (port 8081).
- **Tools:** Playwright (recommended) or Cypress. Headless browser automation.
- **Strategy:** **Critical path only** — test user journeys that touch financial operations, not visual regressions.
- **Priority flows:**
  - **Admin:** Login, view orders, view wallet balance, create API key, view metrics.
  - **Consumer/Partner:** Login, view catalog, place order, view order status, view wallet balance.
- **Trigger:** After Gate 2 (E2E API), before deploy. **Not blocking** for API-only changes; **blocking** for UI changes.
- **Note:** UI tests are **separate** from API E2E. API E2E validates contracts; UI tests validate user-facing workflows.

### 3.5 Test Data

- **Scenario-based generation:** [qa/TEST_DATA_FACTORY_SPEC.md](qa/TEST_DATA_FACTORY_SPEC.md).
- **Examples:** `CreatePartner(balance: 0)` → expect order failure; `CreatePartner(balance: 1000)` → expect success.
- **Isolation:** Each test gets a clean or explicitly seeded state; no shared mutable data.

---

## 4. Quality Gates (Summary)

| Gate | Priority | When | What | On Failure |
|------|----------|------|------|------------|
| **Gate 1** | **P0** | Every commit | Critical 4 integration tests | Build rejected |
| **Gate 2** | **P0** | After build, before deploy | Autonomous QA Agent E2E (API) | Deploy blocked |
| **Gate 3** | **P1** | After Gate 2, before deploy | UI critical flows (if UI changed) | Deploy blocked (UI changes only) |
| **Ledger** | **P0** | Hourly (production) | WalletConsistencyJob | Alert; see QMS |

**Priority Definitions:**
- **P0 (Critical):** API testing, financial invariants, ledger consistency. **Always blocking.** Cannot skip.
- **P1 (Important):** UI testing. **Conditional blocking** (only if UI changed). Can be deferred if UI unchanged.

**Gate 3 (UI) Notes:**
- **Conditional:** Only runs if UI code changed (detected via git diff or CI path filters).
- **Scope:** Critical user journeys only (login, orders, wallet, API key management).
- **Not blocking for:** API-only changes, backend-only deployments.
- **Blocking for:** UI changes, frontend deployments.

---

## 5. UI Testing (P1 Priority)

**Priority:** **P1** (Important, not critical). API testing (P0) takes precedence.

**Separate layer:** Admin Dashboard (port 8080) and Consumer Dashboard (port 8081) are Vue.js apps that consume the Steller v2 API.

**Strategy:** Test **critical user journeys** (login, order placement, wallet viewing, API key management), not visual regressions.

**Gate 3:** UI tests run conditionally (only if UI code changed), after Gate 2, before deploy. **Blocking for UI changes only.** Can be deferred if UI unchanged.

**Full strategy:** [qa/UI_TESTING_STRATEGY.md](qa/UI_TESTING_STRATEGY.md)

**Note:** UI testing is **not** a blocker for API-only deployments. Focus remains on **P0: API correctness and financial invariants**.

---

## 6. References

- **Critical path & pipeline:** [qa/QA_CRITICAL_PATH_AND_PIPELINE.md](qa/QA_CRITICAL_PATH_AND_PIPELINE.md)
- **Test data factory:** [qa/TEST_DATA_FACTORY_SPEC.md](qa/TEST_DATA_FACTORY_SPEC.md)
- **E2E protocol (API):** [STELLER_QA_AGENT_PROTOCOL_V2.md](STELLER_QA_AGENT_PROTOCOL_V2.md)
- **UI testing strategy:** [qa/UI_TESTING_STRATEGY.md](qa/UI_TESTING_STRATEGY.md)
- **QMS (Ledger focus):** [STELLER_V2_QMS_PLAN.md](STELLER_V2_QMS_PLAN.md)
- **Gray-box monitoring:** Steller backend `docs/integration/PHASE_4_GRAYBOX_MONITORING.md`
