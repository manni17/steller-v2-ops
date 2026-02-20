# Steller V2 Backlog

**Status:** Canonical Source of Truth for Project Momentum  
**Last Updated:** 2026-02-20 (catalog v2)  
**PRD:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`  
**Execution Plan:** `docs/PLG_EXECUTION_PLAN.md`

---

### Next action

| Owner | Task | Reference |
|-------|------|-----------|
| **Dev Agent** | **P0: Bamboo catalog v2 pagination + DTO** | § Top priority — Done 2026-02-20 |
| **Dev Agent** | Tier 4 complete (18–20) | §6 items 18–20 Done |

---

### Top priority (P0) — Bamboo catalog v2

| # | Item | Owner | Status | Completed At | Notes |
|---|------|-------|--------|:---:|-------|
| **P0** | **Bamboo catalog v2: paginated DTO + full sync** | Dev Agent | 🟢 Done | 2026-02-20 | v2 returns `pageIndex`, `pageSize`, `count`, `items[]`. Added `BambooCatalogV2Response` / `BambooBrandV2Item` / `BambooProductV2Item`; `BambooApiClient.GetCatalogAsync()` now paginates (pageSize=100), maps to `BambooCatalogResponse`, 1.1s delay between pages (Bamboo 1 req/sec). Fixes 429 from v1 (2 req/hour). |

---

## 1. Project Overview

- **Initiative:** Steller V2 AI-First Migration
- **Current Active:** Tier 4 — Phase 3 Growth (multi-user, log explorer, IP allowlisting, webhooks) done; referral/partner_stats/re_engagement next
- **Overall Status:** 🟢 Tier 1–3 QA COMPLETED; PLG Phase 2 COMPLETED; Phase 3 Growth (multi-user, log explorer, IP allowlisting, webhooks) COMPLETED

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

## 2a. Phase Completion Details (Phases 1–4)

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
- [x] Account/Balance: N/A (no Legacy Bamboo API)
- [x] Notifications: N/A (no Legacy Bamboo API found)

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

---

## 3. Tier 1: Stabilization (Critical Path)

| # | Item | Owner | Status | Completed At | Forecasted | Deviation | Notes |
|---|------|-------|--------|:---:|:---:|:---:|-------|
| 1 | QA P1 – Financial (10 tests) | QA Agent | 🟢 Done | 2026-02-20 | — | — | OrderServiceTests P1_01–P1_06: debit, insufficient funds, vendor reject refund, double-refund guard, idempotency, concurrent duplicate |
| 2 | QA P2 – Order flow (8 tests) | QA Agent | 🟢 Done | 2026-02-20 | — | — | P2_01–P2_08: PlaceBambooOrderJob skip/success/failure; PollBambooOrderJob Succeeded/Failed/Pending/partial; E2E CreateOrder→Place→Poll. MockBambooClient SetGetOrderDetailsScenario; MockBackgroundJobClient GetEnqueuedPollOrderIds. 18 tests total, ~3 m 27 s. |

**Gate:** ✅ Tier 1 complete. PLG Phase 1 can proceed.

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

**Gate:** ✅ Tier 2 QA complete (P3+P4+P5: 18/18). See `docs/qa/QA_AGENT_STATUS.md`, `docs/qa/QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md`.

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
| 16 | MockBamboo / Sandbox alignment | Dev Agent | Backlog (low) | — | TBD | — | See `docs/DEV_TICKET_MOCK_BAMBOO_SANDBOX_ALIGNMENT.md` |

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

## 7. Backlog (Low Priority)

**Agency Network:** See `docs/product/AGENCY_NETWORK_PRODUCT_BRIEF.md`. Order: B5 → B6, then dashboard jobs (B8–B11) as separate work.

| # | Item | Owner | Status | Completed At | Forecasted | Deviation | Notes |
|---|------|-------|--------|:---:|:---:|:---:|-------|
| B5 | Scrape/remove referral module | Dev Agent | To do | — | TBD | — | Remove referral schema/APIs/signup flow; see ADR-008, apis.yaml referral-code/referrals |
| B6 | Prepare for agency dashboards | Dev Agent | To do | — | TBD | — | Backend/schema/APIs: ParentPartnerId hierarchy, wallet financing, revenue share, financial reporting APIs. No UI; feeds B8–B11. |
| B8 | Steller admin dashboard | Dev Agent | To do | — | TBD | — | Admin: tree view, metrics, controls, onboarding. Separate job from B6. |
| B9 | Partner dashboard | Dev Agent | To do | — | TBD | — | Partner (B2B API user) dashboard. Separate job. |
| B10 | Agent dashboard | Dev Agent | To do | — | TBD | — | Agent: manage account + sub-agents; top-up wallets; financial reporting. Separate job. |
| B11 | Sub-agent dashboard | Dev Agent | To do | — | TBD | — | Sub-agent: order cards, own reports. Separate job. |
| B1 | Agent Responsibility Matrix | TPM | To do | — | TBD | — | Single doc: what each agent owns |
| B2 | QA workflow docs (full set) | QA Agent | 🟢 Done | 2026-02-19 | — | — | All 8 workflows documented; index workflows 4–10 + #11 Pricing (B4) Done |
| B3 | Bede integration planning | TPM | To do | — | TBD | — | Technical focus; separate initiative |
| B4 | PRICING_LEGACY_VS_V2 investigation | QA Agent | 🟢 Done | 2026-02-19 | — | — | PRICING_LEGACY_VS_V2_INVESTIGATION.md; workflow index #11 added |
| B7 | User-flow integration tests | QA Agent | 🟢 Done | 2026-02-20 | — | — | 8/8 passed. Trails report: `docs/qa/USER_FLOW_TEST_RUN_REPORT_WITH_TRAILS_20260220.md`. |

---

## 8. Column Legend

| Column | Description |
|--------|--------------|
| **Completed At** | Date (YYYY-MM-DD) when item was finished. Blank (—) if not yet done. |
| **Forecasted** | TBD for all pending items. AI-driven development — no fixed dates; items complete when agents execute. |
| **Deviation** | Schedule variance vs forecast. Blank until completed; update when actual date differs from forecast (if forecast is set). |

---

## 9. Quick Reference

| Tier | Focus | Blocking |
|------|-------|----------|
| 1 | Stabilization (QA P1+P2) | PLG Phase 1 |
| 2 | PLG Phase 1 + QA P3–P5 | — |
| 3 | PLG Phase 2 + QA docs | — |
| 4 | PLG Phase 3–4 | — |

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
