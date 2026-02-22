# PLG Execution Plan

**Owner:** Dev Agent / Product  
**Purpose:** Phased execution plan for Steller v2 Product Lead Growth strategy. Maps PLG plan to actionable steps and tracks progress.  
**Source:** `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md`, `docs/BACKLOG_V2.md`  
**Last Updated:** 2026-02-20

---

## How to Use

**Quick guide:** Load this plan for PLG execution. Pick role `developer` or `product_strategy` from `docs/INDEX.yaml`. Phase 1–2 complete; Phase 3 (Scale) next. Track referral_program, re_engagement; check dependencies before starting.

### Top 10 Common Prompts

| # | Prompt |
|---|--------|
| 1 | What is the current PLG phase and next action? → Phase 3 (Scale); referral_program |
| 2 | Is Phase 2 exit criteria met? → Check developer_portal, webhooks, multi_user, partner_log_explorer |
| 3 | Add referral_program; check schema, API scope, and stakeholder alignment |
| 4 | Implement re_engagement job; define RE_ENGAGEMENT_INACTIVE_DAYS, thresholds |
| 5 | What PLG dependencies exist? → API logging, admin metrics, sandbox, developer portal |
| 6 | Map BACKLOG_V2 item to PLG phase; update PLG plan if scope changes |
| 7 | Verify zero-code TTFV and sandbox are operational |
| 8 | Check PLG P0 gaps (GAP-001–005) resolved before Phase 2 |
| 9 | Update PLG Execution Plan when phase item completes |
| 10 | Link PLG item to apis.yaml and data-flow.md if new endpoint or job |

---

## 1. Phase Overview

| Phase | Focus | Status | Exit Criteria |
|-------|-------|--------|---------------|
| **Phase 1** | Foundation | ✅ Complete | Self-service signup; Sandbox; Zero-code TTFV; API logging; Basic analytics |
| **Phase 2** | Growth | ✅ Complete | Developer portal; Multi-user workspaces; Partner log explorer; IP allowlisting; Growth metrics dashboard; Webhooks |
| **Phase 3** | Scale | 🚧 Next | Referral program; Re-engagement; Content marketing; API marketplace listings |

---

## 2. Phase 1 (Foundation) — ✅ COMPLETE

| Item | Status | Notes |
|------|--------|-------|
| self_service_signup | ✅ Done | POST /api/public/signup; sandbox + fake USD; ADR-006 |
| sandbox_environment | ✅ Done | Test keys, mock vendor, fake USD |
| zero_code_ttfv | ✅ Done | Interactive API console / "Generate Test Gift Card" |
| analytics_infrastructure | ✅ Done | API request logging (async); growth metrics data |
| Basic analytics | ✅ Done | Request logs for partner/endpoint/status |

**Gate met:** New partners can sign up, get sandbox access, and see immediate value via embedded console.

---

## 3. Phase 2 (Growth) — ✅ COMPLETE

| Item | Status | Notes |
|------|--------|-------|
| developer_portal | ✅ Done | Landing, docs, signup CTA; logged-in = Partner dashboard |
| growth_metrics_dashboard | ✅ Done | Admin API: signups, activations, revenue |
| activation_tracking | ✅ Done | Onboarding state, milestone detection |
| multi_user_workspaces | ✅ Done | User.PartnerRole (Developer/Finance); invite via register-user; logs restricted to Developer |
| partner_log_explorer | ✅ Done | GET /api/partner/logs; filters status, path, date; pagination |
| ip_allowlisting | ✅ Done | ApiClientSecret.AllowedIpAddresses; middleware checks remote IP |
| webhooks | ✅ Done | SendWebhookJob on order Completed/Failed; HMAC signature; config via /api/partner/webhook |

**Gate:** Developer portal live; Admin growth metrics; sandbox environment operational.

---

## 4. Phase 3 (Scale) — 🚧 NEXT

| Item | Status | Notes |
|------|--------|-------|
| referral_program | Pending | Codes, tracking, rewards (format TBD) |
| re_engagement | Pending | Inactive partner outreach; thresholds TBD |
| Content marketing | Backlog | Separate initiative |
| API marketplace listings | Backlog | Separate initiative |

**Next actions:** referral_program (API; schema applied 2026-02-20); re_engagement. Multi-user, partner log explorer, IP allowlisting, webhooks — done.

---

## 5. Immediate Execution Order (Phase 2 Growth — ✅ COMPLETE)

1. ~~multi_user_workspaces~~ — ✅ RBAC (Developer, Finance); invite via POST /api/auth/register-user with PartnerRole  
2. ~~partner_log_explorer~~ — ✅ GET /api/partner/logs; filters statusCode, path, dateFrom, dateTo; pagination  
3. ~~ip_allowlisting~~ — ✅ ApiClientSecret.AllowedIpAddresses; Admin POST keys accepts optional allowedIpAddresses  
4. ~~webhooks~~ — ✅ Already implemented; SendWebhookJob on order Completed/Failed  
5. **referral_program** — Scope and rewards TBD with stakeholders (next)

---

## 6. Dependencies

| Dependency | Status |
|------------|--------|
| PLG P0 gaps (GAP-001–005) | ✅ Verified |
| API logging (async) | ✅ In place |
| Admin growth metrics API | ✅ In place |
| Sandbox / mock vendor | ✅ In place |
| Developer portal | ✅ In place |

---

## 7. References

- **PLG strategy:** `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md`
- **Backlog:** `docs/BACKLOG_V2.md`
- **PRD:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`
- **Architecture:** `docs/architecture/systems.yaml`, `docs/architecture/atlas/apis.yaml`
- **Dashboards prep:** `docs/architecture/blueprints/dashboards-prep.md`
