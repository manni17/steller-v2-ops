# Steller Testing & QA Index

**Owner:** QA Agent  
**Purpose:** Single entry point for all testing and QA. Load this first when running tests, adding tests, or executing QA.  
**Last Updated:** 2026-02-20

---

## How to Use

**Quick guide:** Load this index as QA Agent. Pick role `qa` from `docs/INDEX.yaml`. Use the links below for run instructions, flow specs, and invariants. For spec-driven work: spec → test → update SPEC_INDEX.yaml.

### Top 10 Common Prompts

| # | Prompt |
|---|--------|
| 1 | Run all integration tests; follow QA_INTEGRATION_TEST_RUN_INSTRUCTIONS |
| 2 | Run only user-flow tests (UF-P1 through UF-A3) |
| 3 | Run the Critical 4 tests (T_01, T_02, T_03, T_04) |
| 4 | Add a new test for flow UF-P6 (or UF-A4); follow USER_FLOW_INTEGRATION_TEST_PLAN |
| 5 | Which tests enforce INV-WALLET-BALANCE? → Check INVARIANTS.md |
| 6 | What spec ID maps to test P1_05? → Check SPEC_INDEX.yaml |
| 7 | Run QA Phase 1 audit (endpoint discovery, schema validation) per STELLER_QA_AGENT_PROTOCOL_V2 |
| 8 | Verify idempotency tests pass (P1_05, P1_06, T_03, UF_P3) |
| 9 | Document a new workflow (Legacy vs V2); follow QA_WORKFLOW_DOCUMENTATION_INDEX template |
| 10 | Add a new invariant; update INVARIANTS.md and SPEC_INDEX.yaml |

---

## 1. Start Here

| Task | Load first | Then |
|------|------------|------|
| **Run tests** | This index | [docs/qa/QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md](QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md) |
| **Add a new test** | [docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md](USER_FLOW_INTEGRATION_TEST_PLAN.md) | Backend [AGENTS.md](/opt/steller-v2/steller-backend/AGENTS.md) § Test commands |
| **QA protocol (env, canonical sources)** | [docs/STELLER_QA_AGENT_PROTOCOL_V2.md](../STELLER_QA_AGENT_PROTOCOL_V2.md) | — |
| **Workflow investigations (Legacy vs V2)** | [docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md](../QA_WORKFLOW_DOCUMENTATION_INDEX.md) | — |
| **Backlog / GTM test items** | [docs/BACKLOG_V2.md](../BACKLOG_V2.md) §7a | [docs/qa/GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md](GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md) |

---

## 2. Test Code Location

**All automated tests live in one place:**

- **Path:** `/opt/steller-v2/steller-backend/Tests.Integration/`
- **Entry:** [steller-backend/AGENTS.md](/opt/steller-v2/steller-backend/AGENTS.md) — structure, conventions, test commands
- **Fixture:** `CustomWebApplicationFactory` (in-memory API, test DB, mock vendor)
- **Factory:** `TestDataFactory` (partners, products, balances)
- **Test classes:** UserFlowIntegrationTests, WebhookTests, OrderServiceTests, AuthTests, BambooIntegrationTests, OperationalJobsTests, WalletServiceTests

**Rule:** All new tests use the existing fixture and TestDataFactory. Add tests to existing test classes following the plan; do not create new test projects or duplicate fixtures for the same scope.

---

## 3. How to Run Tests

**Canonical run doc:** [docs/qa/QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md](QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md)

- Integration tests (P1, P2, P3, P4, P5, user-flow, webhook)
- Prerequisites (Postgres, `.env`, migrations)
- Filter examples (Critical 4, Tier 2, full suite)
- Alternative: standalone test Postgres

**API smoke tests / credentials:** [steller-backend/docs/RUNNING_API_TESTS.md](/opt/steller-v2/steller-backend/docs/RUNNING_API_TESTS.md) — DevCredentialsSeeder, `.dev-credentials.json`, curl examples.

---

## 4. Test Plans & Specs

| Doc | Content |
|-----|---------|
| [USER_FLOW_INTEGRATION_TEST_PLAN.md](USER_FLOW_INTEGRATION_TEST_PLAN.md) | Flow specs, steps, naming (UF-P1, UF-A1, etc.), infrastructure, determinism rules |
| [SPEC_INDEX.yaml](SPEC_INDEX.yaml) | Spec ID → test name, invariant refs (traceability) |
| [INVARIANTS.md](INVARIANTS.md) | Formal invariants (financial, wallet, idempotency), linked to enforcing tests |
| [TEST_DATA_FACTORY_SPEC.md](TEST_DATA_FACTORY_SPEC.md) | TestDataFactory usage, scenarios |
| [QA_WORKFLOW_DOCUMENTATION_INDEX.md](../QA_WORKFLOW_DOCUMENTATION_INDEX.md) | Workflow investigations (Legacy vs V2), links to test plan |
| [GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md](GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md) | GTM readiness, GTM-T1–T5, GTM-P1–P7 |

---

## 5. Backlog & GTM Test Items

- **Backlog:** [docs/BACKLOG_V2.md](../BACKLOG_V2.md) §7a (GTM tests, platform items)
- **GTM-T1 (Done):** E2E signup→first order in `UserFlowIntegrationTests.GTM_T1_E2E_PublicSignup_AdminFunds_PartnerCatalog_PlaceOrder_Completed`
- **GTM-T2 (Done):** Webhook delivery covered by `WebhookTests.SendOrderUpdate_WithValidPartnerWebhook_SendsCorrectPayloadAndSignature`
- **GTM-P1 (Done):** Audit log + `GET /api/admin/audit-log` (AdminAuditController); admin actions logged in Wallet, AdminOrders, ApiKeys
- Coverage for new backlog items: extend existing tests per USER_FLOW_INTEGRATION_TEST_PLAN; no new test classes unless the plan specifies.

---

## 6. Related

- **INDEX.yaml:** Role `qa` loads this index and the linked docs.
- **Architecture:** [docs/architecture/systems.yaml](../architecture/systems.yaml), [containers.yaml](../architecture/containers.yaml), [apis.yaml](../architecture/atlas/apis.yaml)
