# Steller V2 Backlog

**Status:** Canonical Source of Truth for Project Momentum  
**Last Updated:** 2026-02-22  
**PRD:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`  
**Execution Plan:** `docs/PLG_EXECUTION_PLAN.md`

---

### How to use this backlog

- **Next action** = Top 4 items to pick up next (one per owner or by priority).
- **Status:** `Done` = shipped; `To do` = not started; `Optional` = backlog, do when capacity allows.
- **Forecasted / Deviation:** Not set for backlog items (AI-driven execution; no fixed dates). `—` = not applicable.
- **Done when:** Every "To do" has a short acceptance in Notes or a "Done when" cell — complete when that is met. If something is unclear (no owner, no acceptance), update this doc so it does not remain unknown.

---

### Next action

| Owner | Task | Reference |
|-------|------|-----------|
| **QA Agent** | GTM-T1: E2E test signup → first order | §7a GTM — Done 2026-02-20 (GTM_T1 in UserFlowIntegrationTests) |
| **QA Agent** | GTM-T2: Webhook delivery test | §7a GTM — Done (existing SendOrderUpdate_WithValidPartnerWebhook) |
| **Dev Agent** | GTM-P1: Audit log + admin audit API | §7a GTM — Done 2026-02-20 |
| **Dev Agent** | B5/B6 or B12 optional (expose Bamboo APIs, webhook handler) | §7 Backlog |

---

### Top priority (P0) — Bamboo catalog v2

| # | Item | Owner | Status | Completed At | Notes |
|---|------|-------|--------|:---:|-------|
| **P0** | **Bamboo catalog v2: paginated DTO + full sync** | Dev Agent | 🟢 Done | 2026-02-20 | v2 returns `pageIndex`, `pageSize`, `count`, `items[]`. Added `BambooCatalogV2Response` / `BambooBrandV2Item` / `BambooProductV2Item`; `BambooApiClient.GetCatalogAsync()` now paginates (pageSize=100), maps to `BambooCatalogResponse`, 1.1s delay between pages (Bamboo 1 req/sec). Fixes 429 from v1 (2 req/hour). |

---

## 1. Project Overview

- **Initiative:** Steller V2 AI-First Migration
- **Current Active:** Tier 4 COMPLETED (items 17–20). Next: GTM tests (GTM-T1, GTM-T2), audit log (GTM-P1), optional B12 follow-up (expose Bamboo APIs, webhook handler).
- **Overall Status:** 🟢 Tier 1–4 COMPLETED; User-flow tests 8/8; Bamboo client full integration (Phases A–E) done. Backlog: GTM tests/platform, Agency (B5–B11), B12 optional.

---

## 2. Phase Summary (Historical)

| Phase | Name | Status | Owner | Completed At | Forecasted | Deviation |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| Phase 1 | Foundation & Architectural Constraints | 🟢 COMPLETED | TPM Agent | 2026-02-18 | — | — |
| Phase 2 | Bamboo Integration Migration | 🟢 COMPLETED | Dev Agent | 2026-02-19 | — | — |
| Phase 3 | Core Order Flow Migration (Hangfire) | 🟢 COMPLETED | Dev Agent | 2026-02-19 | — | — |
| Phase 4 | E2E QA & Financial Reconciliation | 🟢 COMPLETED | QA Agent | 2026-02-19 | — | — |
| Phase 5 | Stabilization (QA P1+P2) | 🟢 COMPLETED | QA Agent | 2026-02-20 | — | — |
| Phase 6 | PLG Foundation (Phase 1) | 🟢 COMPLETED | Dev Agent | 2026-02-20 | — | — |
| Phase 7 | PLG Growth (Phase 2) | 🟢 COMPLETED | Dev Agent | 2026-02-17 | — | — |

---

## 2a. Phase Completion Details (Phases 1–7)

### Phase 1: Foundation & Architectural Constraints (Completed 2026-02-18)
- [x] `docs/INDEX.yaml` (Entry Point)
- [x] `docs/architecture/systems.yaml` (Boundaries)
- [x] `docs/architecture/containers.yaml` (Ports & Services)
- [x] `docs/STELLER_QA_AGENT_PROTOCOL_V2.md` (QA Rulebook)
- [x] `docs/architecture/atlas/apis.yaml` (API Contracts)
- [x] `docs/architecture/blueprints/data-flow.md` (Background job rules)

### Phase 2: Bamboo Integration Migration (Completed 2026-02-19)
- [x] Catalog + Categories: Single caller (BrandBackgroundService), ExternalApiService only, 429 Retry-After handled
- [x] Orders: PlaceBambooOrderJob/PollBambooOrderJob with WaitForSlotAsync; BambooVendorAdapter SetRetryAfter on 429
- [x] Account/Balance: N/A at migration time (Legacy). **Later:** Get Accounts, Exchange rates, Orders list, Transactions, Notification added per B12 / BAMBOO_FULL_INTEGRATION_PLAN (client done).
- [x] Notifications: N/A at migration time; Bamboo notification GET/POST client added in B12.

### Phase 3: Core Order Flow Migration (Completed 2026-02-19)
- [x] Order creation path: POST /api/orders → CreateOrderAsync → Enqueue PlaceBambooOrderJob(order.Id)
- [x] Place job, Poll job: Rate limiter + IVendorAdapter; Polly AddStandardResilienceHandler
- [x] Idempotency: referenceId → RequestId; duplicate returns existing order
- [x] Backfill path: OrderQueueService enqueues by Order.Id (fix deployed)

### Phase 4: E2E QA & Financial Reconciliation (Completed 2026-02-19)
- [x] Phase 1 Audit: Endpoints, schema — PASSED
- [x] Phase A Stimulus: POST /api/orders → 202 Accepted — PASSED
- [x] Phase B Observation: Job ran, Bamboo rejected (catalog SKU not found)
- [x] Financial Reconciliation: Refund path confirmed (MarkFailedAndRefundAsync fix deployed 2026-02-17; QA re-run 2026-02-19)

### Phase 5: Stabilization (Completed 2026-02-20)
- [x] QA P1 – Financial: 10 OrderServiceTests passed (debit, insufficient funds, vendor reject refund, double-refund guard, idempotency, concurrent duplicate)
- [x] QA P2 – Order flow: 8 tests passed (PlaceBambooOrderJob skip/success/failure, PollBambooOrderJob Succeeded/Failed/Pending/partial, E2E happy path)

### Phase 6: PLG Foundation (Completed 2026-02-20)
- [x] Self-service signup, API logging, analytics infrastructure, Swagger/docs (Tier 2 items 3–7).

### Phase 7: PLG Growth (Completed 2026-02-17)
- [x] Developer portal, sandbox, activation tracking, growth metrics dashboard (Tier 3 items 11–14).

---

## 3. Tier 1: Stabilization (Critical Path)

| # | Item | Owner | Status | Completed At | Forecasted | Deviation | Notes |
|---|------|-------|--------|:---:|:---:|:---:|-------|
| 1 | QA P1 – Financial (10 tests) | QA Agent | 🟢 Done | 2026-02-20 | — | — | OrderServiceTests P1_01–P1_06: debit, insufficient funds, vendor reject refund, double-refund guard, idempotency, concurrent duplicate |
| 2 | QA P2 – Order flow (8 tests) | QA Agent | 🟢 Done | 2026-02-20 | — | — | P2_01–P2_08: PlaceBambooOrderJob skip/success/failure; PollBambooOrderJob Succeeded/Failed/Pending/partial; E2E CreateOrder→Place→Poll. MockBambooClient SetGetOrderDetailsScenario; MockBackgroundJobClient GetEnqueuedPollOrderIds. 18 tests total, ~3 m 27 s. |

**Gate:** ✅ Tier 1 complete. PLG Phase 1 could proceed (and did).

---

## 4. Tier 2: PLG Foundation + QA Completion

| # | Item | Owner | Status | Completed At | Forecasted | Deviation | Notes |
|---|------|-------|--------|:---:|:---:|:---:|-------|
| 3 | Resolve PLG P0 gaps (GAP-001–005) | Dev Agent | 🟢 Done | 2026-02-20 | — | — | Verified; see docs/PLG_P0_GAP_RESOLUTION_REPORT.md. Email, Partners, Wallet, Redis, Wallet API |
| 4 | PLG Phase 1: self_service_signup | Dev Agent | 🟢 Done | 2026-02-20 | — | — | POST /api/public/signup (PublicController, PartnerOnboardingService); apis.yaml, components/dependencies; ApiKeyMiddleware skips /api/public; ADR-006 |
| 5 | PLG Phase 1: api_logging | Dev Agent | 🟢 Done | 2026-02-20 | — | — | AnalyticsMiddleware → Redis list (fire-and-forget); IApiRequestLogQueue, RedisApiRequestLogQueue, NoOpApiRequestLogQueue; ADR-005, components/dependencies/data-flow updated |
| 6 | PLG Phase 1: analytics_infrastructure | Dev Agent | 🟢 Done | 2026-02-20 | — | — | ApiRequestLog entity, migration AddApiRequestLogs; ProcessAnalyticsQueueJob (Redis → batch insert), recurring */2 * * * *; data-flow.md updated |
| 7 | PLG Phase 1: api_documentation | Dev Agent | 🟢 Done | 2026-02-20 | — | — | Swagger “Steller v2 API” v1 at /swagger; INDEX.yaml developer role (apis.yaml, security-auth, docs/integration/) |
| 8 | QA P3 – Bamboo integration (6 tests) | QA Agent | 🟢 Done | 2026-02-18 | — | — | 6/6: WaitForSlotAsync, 429 SetRetryAfter, VendorApiCall RequestPayload/OrderId |
| 9 | QA P4 – Operational jobs (6 tests) | QA Agent | 🟢 Done | 2026-02-18 | — | — | 6/6: OrderQueueService (24h/date/pending), ReconciliationJob, WalletConsistencyJob |
| 10 | QA P5 – Auth (6 tests) | QA Agent | 🟢 Done | 2026-02-18 | — | — | 6/6: 401/403/valid key, catalog/orders require key, admin 401 or 404. **Tier 2 gate: 18/18 passed ~1 m 48 s** |

**Gate:** ✅ Tier 2 QA complete (P3+P4+P5: 18/18). See `docs/qa/QA_AGENT_STATUS.md`, `docs/qa/QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md`. User-flow tests (8/8) in B7; GTM doc in §7a.

---

## 5. Tier 3: PLG Phase 2 + QA Docs

**Dashboard prep (Admin + Partner):** Full scope, APIs, auth, and backlog mapping → `docs/architecture/blueprints/dashboards-prep.md`. Items 11, 12, 14 feed Admin and Partner dashboard readiness; item 18 (partner_stats) aligns with Partner dashboard.

| # | Item | Owner | Status | Completed At | Forecasted | Deviation | Notes |
|---|------|-------|--------|:---:|:---:|:---:|-------|
| 11 | PLG Phase 2: developer_portal | Dev Agent | 🟢 Done | 2026-02-17 | — | — | Landing page, docs, signup CTA; prep: dashboards-prep.md (Admin + Partner) |
| 12 | PLG Phase 2: sandbox_environment | Dev Agent | 🟢 Done | 2026-02-17 | — | — | Sandbox API, mock vendor, test keys; Partner dashboard sandbox mode |
| 13 | PLG Phase 2: activation_tracking | Dev Agent | 🟢 Done | 2026-02-17 | — | — | Onboarding state, milestone detection, email triggers |
| 14 | PLG Phase 2: growth_metrics_dashboard | Dev Agent | 🟢 Done | 2026-02-17 | — | — | Admin API: GET /api/admin/metrics/growth (signups, activations, revenue); apis.yaml placeholder added |
| 15 | QA workflow docs (Place Order, Poll Order, etc.) | QA Agent | 🟢 Done | 2026-02-19 | — | — | 7 workflow docs (Poll Order, Order Creation, Refund, Backfill, Wallet, Partner Auth, Catalog Retrieval); index updated |
| 16 | MockBamboo / Sandbox alignment | Dev Agent | Backlog (low) | — | — | — | **Done when:** Mock and sandbox behavior aligned; doc updated. Optional. See `docs/DEV_TICKET_MOCK_BAMBOO_SANDBOX_ALIGNMENT.md`. |

---

## 6. Tier 4: PLG Phase 3–4

**Recent (2026-02-20):** Migrations applied — AllowedIpAddresses (IP allowlisting), PartnerRole (multi-user), ApiRequestLogs (partner log explorer), Referrals + Partners.ReferralCode/ReferredByPartnerId/OnboardingState (referral schema). API rebuilt and restarted.

| # | Item | Owner | Status | Completed At | Forecasted | Deviation | Notes |
|---|------|-------|--------|:---:|:---:|:---:|-------|
| 17 | PLG Phase 3: webhooks | Dev Agent | 🟢 Done | 2026-02-20 | — | — | SendWebhookJob on order Completed/Failed; HMAC signature; /api/partner/webhook |
| 18 | PLG Phase 3: partner_stats_api | Dev Agent | 🟢 Done | 2026-02-20 | — | — | GET /api/partner/stats (implemented) |
| 19 | PLG Phase 4: referral_program | Dev Agent | 🟢 Done | 2026-02-20 | — | — | Schema + APIs: referral-code, referrals, signup with ReferralCode; activation tracking; RewardAmount in DTO |
| 20 | PLG Phase 4: re_engagement | Dev Agent | 🟢 Done | 2026-02-20 | — | — | ReEngagementJob + ReEngagementNotifierService; email/webhook to inactive partners; RE_ENGAGEMENT_INACTIVE_DAYS env |

---

## 7. Backlog (prioritized when scheduled)

**Agency Network:** See `docs/product/AGENCY_NETWORK_PRODUCT_BRIEF.md`. Order: B5 → B6, then B8–B11. **Done when** = acceptance met.

| # | Item | Owner | Status | Completed At | Priority | Notes / Done when |
|---|------|-------|--------|:---:|:---:|-------|
| B5 | Scrape/remove referral module | Dev Agent | To do | — | P1 (before B6) | **Done when:** Referral schema/APIs/signup flow removed; ADR-008 applied; apis.yaml updated. See ADR-008, apis.yaml referral-code/referrals. |
| B6 | Prepare for agency dashboards | Dev Agent | To do | — | P1 | **Done when:** Backend/schema/APIs for ParentPartnerId hierarchy, wallet financing, revenue share, financial reporting. No UI; feeds B8–B11. |
| B8 | Steller admin dashboard | Dev Agent | To do | — | P2 | **Done when:** Admin UI: tree view, metrics, controls, onboarding; configure agent card list; manual credit. Depends on B6. |
| B9 | Partner dashboard | Dev Agent | To do | — | P2 | **Done when:** Partner (B2B API user) dashboard delivered. |
| B10 | Agent experience (Telegram) | Dev Agent | To do | — | P2 | **Done when:** Telegram bot: agents join, manage sub-agents, allocate wallets, order from card list; top-up manual. Same codebase as B11, role-based. |
| B11 | Sub-agent experience (Telegram) | Dev Agent | To do | — | P2 | **Done when:** Telegram bot: sub-agents join via agent link; order from list; own wallet/reports. Same codebase as B10. |
| B1 | Agent Responsibility Matrix | TPM | To do | — | P2 | **Done when:** Single doc published: what each agent (TPM, QA, Dev) owns. |
| B2 | QA workflow docs (full set) | QA Agent | 🟢 Done | 2026-02-19 | — | All 8 workflows documented; index workflows 4–10 + #11 Pricing (B4). |
| B3 | Bede integration planning | TPM | To do | — | P2 | **Done when:** Technical plan or brief for Bede integration (separate initiative). |
| B4 | PRICING_LEGACY_VS_V2 investigation | QA Agent | 🟢 Done | 2026-02-19 | — | PRICING_LEGACY_VS_V2_INVESTIGATION.md; workflow index #11. |
| B7 | User-flow integration tests | QA Agent | 🟢 Done | 2026-02-20 | — | 8/8 passed. `docs/qa/USER_FLOW_TEST_RUN_REPORT_WITH_TRAILS_20260220.md`. |
| B12 | **Bamboo full integration** | Dev Agent | 🟢 Done (client) | 2026-02-20 | — | Client: IBambooApiClient + DTOs (Exchange rates, Accounts, Orders list, Transactions, Notification). Optional: expose via APIs; Bamboo webhook handler. `steller-backend/docs/BAMBOO_FULL_INTEGRATION_PLAN.md`. |
| B13 | **WF-A10/WF-A11 backend** | Dev Agent | 🟢 Done | 2026-02-22 | — | PUT /api/admin/partners/{id}/status (toggle IsActive); POST /api/admin/orders/{id}/retry-webhook (re-enqueue SendWebhookJob); ApiKeyMiddleware rejects inactive partners (403 PARTNER_SUSPENDED). |

---

## 7a. GTM / B2B readiness (from GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md)

**Source:** `docs/qa/GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md` § Actionable items. PRD: `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md` § 6a.

### Tests

| ID | Item | Owner | Status | Priority | Done when |
|----|------|-------|--------|:---:|-------|
| GTM-T1 | E2E: public signup → admin funds → partner getCatalog → place order → completed | QA Agent | Done | P1 | Test uses POST /api/public/signup then admin credit/discount/key; partner getCatalog + POST /api/orders; order completed. No TestDataFactory for partner. |
| GTM-T2 | Integration test: webhook delivery (order completed/failed, HMAC) | QA Agent | Done | P1 | Test asserts webhook POST sent to partner URL with valid HMAC for order completed or failed. |
| GTM-T3 | Catalog-filtering test (partner A sees N, partner B sees M products) | QA Agent | To do | Optional | Assert catalog count per partner when PartnerProductPricing differs. |
| GTM-T4 | Sandbox E2E in CI (Bamboo sandbox + Steller) | Dev Agent | To do | Optional | CI job runs against Bamboo sandbox + Steller; documented. |
| GTM-T5 | Rate-limit / load test (429, retry-after) | Dev/QA | To do | Optional | Test or doc: Steller/Bamboo rate limits and retry-after behavior. |

### Documentation (executed)

| ID | Item | Owner | Status | Deliverable |
|----|------|-------|--------|-------|
| GTM-D1 | Partner onboarding runbook | QA/TPM | Done | `docs/qa/PARTNER_ONBOARDING_RUNBOOK.md` |
| GTM-D2 | Integration guide: rate limits & retries | Dev Agent | Done | `steller-backend/docs/integration/STELLER_INTEGRATION_GUIDE.md` §8 |
| GTM-D3 | Integration guide: webhook self-service | Dev Agent | Done | Same guide §8a |

### Platform / product (backlog)

| ID | Item | Owner | Status | Priority | Done when |
|----|------|-------|--------|:---:|-------|
| GTM-P1 | Audit log for admin actions + GET /api/admin/audit-log | Dev Agent | Done | P1 | All sensitive admin actions write to AuditLog; GET /api/admin/audit-log with filters (actor, resource, date). GAP_CLOSURE_PLAN P2. |
| GTM-P2 | Partner self-service API key rotation | Dev Agent | To do | P2 | Partner can generate new key and revoke old (via API or dashboard) without admin. |
| GTM-P3 | Partner usage/revenue summary API | Dev Agent | To do | P2 | e.g. GET /api/partner/summary or /stats extended: last 30 days orders count, spend, top products. |
| GTM-P4 | Catalog visibility: “approved product list” per partner | Product/Dev | To do | Optional | Document or implement: partner sees only products with PartnerProductPricing (or explicit allow list). |
| GTM-P5 | OpenAPI spec or Postman collection | Dev Agent | To do | Optional | Machine-readable partner API spec or Postman collection published. |
| GTM-P6 | Webhook replay / last N events API | Dev Agent | To do | Optional | Partner can fetch last N webhook events or replay. |
| GTM-P7 | Partner-facing status page / SLA | Product/Ops | To do | Optional | Public or partner status page; SLA doc or visibility. |

---

## 8. Column Legend

| Column | Description |
|--------|--------------|
| **Completed At** | Date (YYYY-MM-DD) when item was finished. `—` = not yet done. |
| **Forecasted** | Not set for backlog (no fixed dates). Used only in Phase Summary where applicable. |
| **Deviation** | Not set for backlog. Used only when comparing actual vs forecasted date. |
| **Priority** | P1 = next to pick up; P2 = when capacity; Optional = backlog, do when needed. `—` = done or N/A. |
| **Done when** | Acceptance criteria so the item is not "unknown" — complete when this is met. |

---

## 9. Quick Reference

| Tier | Focus | Status |
|------|-------|--------|
| 1 | Stabilization (QA P1+P2) | ✅ Done |
| 2 | PLG Phase 1 + QA P3–P5 | ✅ Done |
| 3 | PLG Phase 2 + QA docs | ✅ Done |
| 4 | PLG Phase 3–4 (webhooks, partner_stats, referral, re_engagement) | ✅ Done |
| — | Next: GTM tests (T1, T2), audit log (P1), backlog B5/B6/B8–B11 | §7, §7a |

---

## 10. References

- **PLG Execution Plan:** `docs/PLG_EXECUTION_PLAN.md` (phased next steps)
- **PRD:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`
- **PLG Plan:** `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md`
- **QA Protocol:** `docs/STELLER_QA_AGENT_PROTOCOL_V2.md`
- **QA Workflow Index:** `docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md`
- **Phase 4 Report:** `docs/qa/STELLER_QA_RUN_20260219.md`
- **Dashboard prep (Admin + Partner):** `docs/architecture/blueprints/dashboards-prep.md`
- **Agency Network (product brief):** `docs/product/AGENCY_NETWORK_PRODUCT_BRIEF.md`
- **User-flow integration test plan:** `docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md`
- **User-flow test report (2026-02-20):** `docs/qa/USER_FLOW_INTEGRATION_TEST_REPORT_20260220.md`
- **User-flow test run with trails (2026-02-20):** `docs/qa/USER_FLOW_TEST_RUN_REPORT_WITH_TRAILS_20260220.md`
- **Bamboo catalog v2 (P0):** `steller-backend/docs/BAMBOO_CATALOG_WHY_IT_FAILS.md`; DTOs: `BambooCatalogV2Response.cs`; client: `BambooApiClient.GetCatalogAsync()` pagination.
- **Repos (latest version):** Backend code → [manni17/steller-backend](https://github.com/manni17/steller-backend). This doc + deploy + INDEX → [manni17/steller-v2-ops](https://github.com/manni17/steller-v2-ops). See `docs/INDEX.yaml` → `repos:` for canonical list.
- **Bamboo full integration plan:** `steller-backend/docs/BAMBOO_FULL_INTEGRATION_PLAN.md` — Phases A–E: Exchange rates, Get Accounts, Orders list, Transactions, Notification (webhook); checklist and refs.
- **GTM / B2B readiness:** `docs/qa/GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md` — actionable items, tests (GTM-T1–T5), docs (GTM-D1–D3), platform (GTM-P1–P7); runbook: `docs/qa/PARTNER_ONBOARDING_RUNBOOK.md`.
