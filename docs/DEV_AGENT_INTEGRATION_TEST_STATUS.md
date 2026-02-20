# Dev Agent: Integration Test Status (Internal)

**Owner:** Dev Agent  
**Read-only for Dev:** `docs/BACKLOG_V2.md` is owned by TPM; do not edit.

---

## Current perspective

- **P1 (OrderServiceTests):** Complete. 10 tests, 10/10 passed.
- **Next:** QA P2 (8 order-flow tests per BACKLOG_V2 Tier 1 — TPM tracks there).

---

## Tier 2 — PLG P0 gaps (GAP-001–005)

- **Status:** All five P0 gaps verified 2026-02-20.
- **Report:** `docs/PLG_P0_GAP_RESOLUTION_REPORT.md` (email service, Partners schema, Wallet schema, Redis queue, Wallet API).
- **Implication:** PLG Phase 1 (self-service signup, api_logging, analytics_infrastructure, api_documentation) is unblocked; implementation can proceed per PLG plan.

## PLG Phase 1 (started 2026-02-20)

- **api_logging:** Done. AnalyticsMiddleware enqueues to Redis list; NoOp when Redis not configured. ADR-005, components/dependencies updated.
- **analytics_infrastructure:** Done. ApiRequestLog entity + migration, ProcessAnalyticsQueueJob (Redis → ApiRequestLogs), recurring every 2 min when Redis configured. Data-flow.md updated.
- **self_service_signup:** Done. POST /api/public/signup (no auth); PublicController, PartnerOnboardingService (create Partner, CreditWalletAsync, IApiKeyService.GenerateKeyAsync); ApiKeyMiddleware skips /api/public. Config: PartnerOnboarding uses sandbox + fake USD only; real money via top-up request only (no real-money signup credit). apis.yaml and components updated.
- **api_documentation:** Done. SwaggerDoc v1 title/description; INDEX.yaml developer role (apis.yaml, integration guides, public signup).

---

## P1 fixes applied (for our reference)

1. **Products sequence** (`CustomWebApplicationFactory.cs`)  
   After seeding Product with Id=1, PostgreSQL sequence stayed at 1; `TestDataFactory.EnsureProductExists` hit `PK_Products` on insert. Added `setval` to sync sequence with max Product Id.

2. **P1_01 wallet verification** (`OrderServiceTests.cs`)  
   Use a fresh scope for wallet checks after order creation; DbContext was serving cached wallet data (raw SQL updates don’t refresh the change tracker).

3. **P1_06 concurrent test** (`OrderServiceTests.cs`)  
   Use separate scopes per `CreateOrderAsync` (DbContext not thread-safe). Added `using Steller.Core.Common` for `Result<T>`.

---

## Shared with QA

- **Run instructions + status:** `docs/qa/QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md` (Status section + commands).
- **Contributing/test options:** `steller-backend/CONTRIBUTING.md`.
