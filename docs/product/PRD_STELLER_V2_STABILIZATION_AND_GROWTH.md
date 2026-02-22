# PRD: Steller V2 Stabilization & Product Lead Growth

**Version:** 1.1  
**Date:** 2026-02-19  
**Status:** Active  
**Owner:** TPM Agent  
**Reference:** `docs/BACKLOG_V2.md`, `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md`

---

## 1. Executive Summary

**Goal:** Stabilize Steller V2 through comprehensive QA coverage, then layer Product Lead Growth (PLG) features to enable self-service partner acquisition and usage-driven growth.

**Principle:** Stabilize first, then grow. Block PLG Phase 1 until QA P1+P2 (financial + order flow) tests pass.

---

## 2. Context

**Current State (as of 2026-02-19):**
- Migration Phases 1–4: **COMPLETED** (Foundation, Bamboo, Order Flow, E2E QA)
- Financial core: Debit, refund-on-vendor-failure, idempotency — verified
- **QA P1–P5: 36 tests passing** (financial, order flow, Bamboo, operational jobs, auth); automated regression suite in place
- **PLG Phase 1: COMPLETED** (self-service signup, api_logging, analytics_infrastructure, api_documentation)
- **PLG P0 gaps (GAP-001–005): Verified** (2026-02-20); see `docs/PLG_P0_GAP_RESOLUTION_REPORT.md`
- **QA workflow docs: COMPLETED** (8 workflows + pricing investigation); see `docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md`
- **Dashboard prep: COMPLETED** (Admin + Partner scope, APIs, backlog mapping); see `docs/architecture/blueprints/dashboards-prep.md`

**Remaining Gaps:**
- PLG Phase 2 (developer_portal, sandbox, activation_tracking, growth_metrics_dashboard) — **✅ COMPLETED** (2026-02-17)
- PLG Phase 3 Growth (multi-user, partner log explorer, IP allowlisting, webhooks) — **✅ COMPLETED** (2026-02-20; migrations applied, schema in place)
- PLG Phase 4 (referral_program schema applied; API in progress; re_engagement, partner_stats_api pending)
- TPM backlog: Agent Responsibility Matrix (B1), Bede integration planning (B3)

---

## 3. Objectives

| # | Objective | Success Criteria |
|---|-----------|------------------|
| 1 | **Stabilization** | P1+P2 QA tests passing; no financial/order-flow regressions |
| 2 | **PLG Foundation** | Self-service signup, API logging, analytics, API docs live |
| 3 | **PLG Growth** | Developer portal, sandbox, activation tracking, metrics dashboard |
| 4 | **PLG Scale** | Webhooks, partner stats, referral (to be replaced by agency), re-engagement |
| 5 | **Agency Network** | Agent/sub-agent hierarchy, dashboard-only B2C, wallet financing, financial reporting — see `docs/product/AGENCY_NETWORK_PRODUCT_BRIEF.md` |

---

## 4. Scope

### In Scope
- QA comprehensive test plan (36 tests, P1–P5) — **implemented and passing**
- PLG Phase 1–4 per PLG strategy plan
- QA workflow documentation (Legacy vs V2)
- MockBamboo / Sandbox alignment (when PLG sandbox starts)
- **Agency Network** (post-referral): Agent/sub-agent hierarchy, dashboard-only, wallet financing, financial reporting. Plan: remove referral module → add agency module. See `docs/product/AGENCY_NETWORK_PRODUCT_BRIEF.md`.

### Out of Scope
- Legacy system changes
- UI/Consumer dashboard rewrite
- New vendor integrations (e.g. Bede) — separate initiative

---

## 5. Prioritization Plan

### Tier 1: Stabilization (Critical Path) — ✅ COMPLETED
| Priority | Item | Owner | Gate | Status |
|----------|------|-------|------|--------|
| 1 | QA P1 – Financial (10 tests) | QA Agent | Block release until pass | ✅ Done 2026-02-20 |
| 2 | QA P2 – Order flow (8 tests) | QA Agent | Block release until pass | ✅ Done 2026-02-20 |

**Exit:** P1+P2 pass → financial and order flow stable. **Gate met.**  
**Block:** No PLG Phase 1 until Tier 1 complete. (Resolved.)

---

### Tier 2: PLG Foundation + QA Completion — ✅ COMPLETED
| Priority | Item | Owner | Notes | Status |
|----------|------|-------|-------|--------|
| 3 | Resolve PLG P0 gaps (GAP-001–005) | Dev Agent | Email, Partners, Wallet, Redis, Wallet API | ✅ Verified 2026-02-20 |
| 4 | PLG Phase 1 (signup, api_logging, analytics, api_docs) | Dev Agent | Start only after P0 gaps resolved | ✅ Done 2026-02-20 |
| 5 | QA P3 – Bamboo integration (6 tests) | QA Agent | Rate limit, 429, payload | ✅ Done 2026-02-18 |
| 6 | QA P4 – Operational jobs (6 tests) | QA Agent | Backfill, reconciliation, wallet consistency | ✅ Done 2026-02-18 |
| 7 | QA P5 – Auth (6 tests) | QA Agent | API key middleware | ✅ Done 2026-02-18 |

**Gate:** Tier 2 QA (P3+P4+P5) 18/18 passed. See `docs/BACKLOG_V2.md`.

---

### Tier 3: PLG Phase 2 + QA Docs — ✅ COMPLETED
| Priority | Item | Owner | Notes | Status |
|----------|------|-------|-------|--------|
| 8 | PLG Phase 2 (developer portal, sandbox, activation, metrics) | Dev Agent | Depends on Phase 1 (done) | ✅ Done 2026-02-17 |
| 9 | QA workflow docs (Place Order, Poll Order, Order Creation, etc.) | QA Agent | For regression and parity | ✅ Done 2026-02-19 |
| 10 | MockBamboo / Sandbox alignment | Dev Agent | Optional; align with PLG sandbox | Backlog (low) |

**Next:** Dev — PLG Phase 3 (multi-user workspaces, partner log explorer, IP allowlisting, webhooks). See `docs/PLG_EXECUTION_PLAN.md`, `docs/architecture/blueprints/dashboards-prep.md`.

---

### Tier 4: PLG Phase 3–4
| Priority | Item | Owner |
|----------|------|-------|
| 11 | PLG Phase 3 (webhooks, partner_stats_api) | Dev Agent |
| 12 | PLG Phase 4 (referral, re-engagement) | Dev Agent |

---

## 6. Release Criteria

| Stage | Criteria | Status |
|-------|----------|--------|
| **Staging / Production** | P1+P2 QA tests pass; no critical financial/order-flow failures | ✅ Met |
| **PLG Phase 1** | P1+P2 pass; P0 gaps resolved; self-service signup + analytics live | ✅ Met |
| **Production-Ready** | P1–P5 pass; PLG Phase 1 validated | ✅ Met |
| **Full PLG** | P1–P5 pass; PLG Phase 1–4 complete per PLG plan | Pending (Phase 3–4) |

---

### 6a. GTM / B2B readiness (from GTM assessment)

**Source:** `docs/qa/GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md`. Backlog: `docs/BACKLOG_V2.md` § 7a (GTM).

| Objective | Status | Notes |
|-----------|--------|-------|
| **Controlled B2B launch** | ✅ Ready | API-first, prepaid wallet, partner catalog/pricing, admin support, Bamboo integrated; user-flow tests + money trails documented. |
| **Tests for stronger GTM** | Backlog | GTM-T1 (signup→first order E2E), GTM-T2 (webhook delivery test); optional T3–T5 (catalog filter, sandbox E2E, rate/load). |
| **Documentation** | ✅ Done | GTM-D1 Partner onboarding runbook; GTM-D2/D3 integration guide (rate limits, retries, webhook self-service). |
| **Platform gaps** | Backlog | GTM-P1 Audit log; GTM-P2 self-service key rotation; GTM-P3 partner usage summary; P4–P7 optional (catalog visibility, OpenAPI, webhook replay, status page). |

**PRD scope:** GTM actionable items are tracked in BACKLOG_V2 § 7a; no change to in-scope/out-of-scope in §4. Execution: docs (GTM-D1–D3) completed; tests and platform items picked up per backlog priority.

---

## 7. Dependencies

| Dependency | Owner | Notes |
|------------|-------|-------|
| Migration Phases 1–4 | ✅ Done | Foundation, Bamboo, Order Flow, E2E QA |
| PLG P0 gaps (GAP-001–005) | ✅ Verified | 2026-02-20; PLG Phase 1 unblocked |
| MockVendorAdapter / MockBambooClient | Dev | Exists; used for QA tests |
| Hangfire, Polly, IRateLimiterService | Dev | In place |
| Redis | Infra | Exists; verify queue support for analytics |

---

## 8. Risks & Mitigations

| Risk | Impact | Mitigation | Status |
|------|--------|------------|--------|
| PLG Phase 1 before stabilization | Financial regressions | Block PLG Phase 1 until P1+P2 pass | ✅ Mitigated (Tier 1 complete before Phase 1) |
| Analytics middleware blocks API | Latency, failures | MUST use Redis queue or Hangfire fire-and-forget; never sync PostgreSQL | Ongoing guardrail |
| P0 gaps unresolved | PLG Phase 1 blocked | TPM/Dev prioritize gap verification | ✅ Mitigated (verified 2026-02-20) |
| Scope creep | Timeline slip | Phase-by-phase delivery; validate before next tier | Ongoing |

---

## 9. Timeline (Indicative)

| Phase | Focus | Status |
|-------|-------|--------|
| 1–2 | Tier 1 — QA P1 + P2 tests | ✅ Done (2026-02-20) |
| 3 | Resolve PLG P0 gaps | ✅ Verified (2026-02-20) |
| 4–6 | Tier 2 — PLG Phase 1 + QA P3–P5 | ✅ Done (2026-02-18–20) |
| 7–10 | Tier 3 — PLG Phase 2 + QA workflow docs | ✅ Done 2026-02-17 |
| 11+ | Tier 4 — PLG Phase 3–4 | Pending |

---

## 10. References

- `docs/BACKLOG_V2.md` — **Canonical** phase status and unified backlog (TPM-owned)
- `docs/PLG_EXECUTION_PLAN.md` — Phased PLG execution (Phase 1–3, next steps)
- `docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md` — QA workflow docs (8 workflows + pricing; complete)
- `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md` — PLG plan
- `docs/STELLER_QA_AGENT_PROTOCOL_V2.md` — QA protocol
- `docs/TPM_AGENT_PROTOCOL.md` — TPM protocol
- `docs/architecture/blueprints/dashboards-prep.md` — Admin + Partner dashboard prep (complete)
- `docs/product/AGENCY_NETWORK_PRODUCT_BRIEF.md` — Agency network product brief (planning)
- `docs/qa/GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md` — GTM assessment; actionable items (tests GTM-T1–T5, docs GTM-D1–D3, platform GTM-P1–P7); backlog § 7a
- `docs/qa/PARTNER_ONBOARDING_RUNBOOK.md` — Partner onboarding runbook (signup → first order)
