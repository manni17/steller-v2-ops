# Agency Network — Product Brief

**Status:** Planning  
**Owner:** Product  
**Last Updated:** 2026-02-20  
**Reference:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`, `docs/BACKLOG_V2.md`, `docs/architecture/blueprints/dashboards-prep.md`

---

## 1. Problem / Opportunity

Steller needs a **B2B2C agency network**: partners (agents) recruit and manage sub-agents who sell via dashboard. Agents finance sub-agent wallets; everyone gets cards from Steller. This replaces the flat referral model with a hierarchy that supports revenue share, visibility, and financial reporting. Treated as B2C because agents and sub-agents use dashboards only (no API access).

---

## 2. Agency Model

**Hierarchy:** Steller → Partner (B2B, API) → Agent (dashboard) → Sub-agent (dashboard).

- **Partner (B2B):** API access; integrates with Steller programmatically.
- **Agent (parent):** Dashboard-only. Manages own account and all sub-agents. Tops up own wallet and all sub-agent wallets. Earns revenue share on sub-agent sales.
- **Sub-agent:** Dashboard-only. Gets cards from dashboard. Wallet funded by agent. Sees only reports related to themselves.

**Key rule:** Agent does not hold or dispense cards; agent **finances** sub-agent wallets. All cards come from Steller. Commission rates differ by level (agent vs sub-agent). Revenue share applies only to direct children (no cascading to grandchildren).

---

## 3. Revenue Model

| Actor | Gets cards from | Who tops up their wallet | Commission |
|-------|-----------------|--------------------------|------------|
| Agent | Steller (via dashboard) | Agent (self) | Agent rate |
| Sub-agent | Steller (via dashboard) | **Agent** (parent) | Sub-agent rate (different) |

- **Flow:** Agent tops up at Steller; credit can go to agent wallet and/or sub-agent wallets. Sub-agents order cards from dashboard using balance funded by agent.
- **Revenue share:** Parent earns share on direct sub-agent sales only. Calculated monthly; payout at month end.
- **Commission rules:** Configured by Steller (not by agent).
- **Deliverable:** Revenue flow diagram + commission rules (who gets what, when, how).

---

## 4. Visibility Model

| Who | Sees |
|-----|------|
| **Agent** | Own + all sub-agents: orders, GMV, wallet, API usage. Full financial reporting. |
| **Sub-agent** | Only own: wallet, orders, commission. Reports related to them only. |
| **Hidden from agent** | Sub-agent API keys (N/A — no API), raw PII, Steller cost/margin. |
| **Sub-agent sees of parent** | Limited: parent identity, support contact, own commission/revenue-share info. |

Parents see only direct children (no grandchildren). Deliverable: Visibility matrix (who sees what, by role and level).

---

## 5. Support Model

- **First-line:** Steller runs support (e.g. independent of B2B, like Bede). Agents have tools (sub-agent list, stats, logs) to support their network; Steller remains the official support channel.
- **Escalation:** Steller handles escalations, critical issues, fraud. SLAs and escalation rules TBD.
- **Deliverable:** Support model (L1/L2) + escalation rules.

---

## 6. Admin Model

- **Views:** Tree (Steller → agents → sub-agents), per-partner health, filters by level/status.
- **Metrics:** Partners per level, GMV by partner/level, growth, churn, commission/payout volumes.
- **Risk:** Focus on money flow (unusual volumes, rapid cash-out, fraud patterns).
- **Controls:** Pause/activate partners; adjust commission rules; overrides.
- **Onboarding agents:** Approval flow, checks, capacity limits — to be specified.
- **Deliverable:** Admin dashboard spec (views, metrics, alerts, controls).

---

## 7. Dashboards and Financial Reporting

- **Agent dashboard:** Manage own account + all sub-agents; top up own and sub-agent wallets; **financial reporting** (own + sub-agents).
- **Sub-agent dashboard:** Order cards from dashboard; view **reports related to them** (own wallet, orders, commission). No API access.
- Both agent and sub-agent get cards from Steller via dashboard (no inventory held by agent).

---

## 8. Scope and Phases

- **B5 (backlog):** Scrape/remove referral module (schema, APIs, signup referral flow).
- **B6 (backlog):** Prepare for agency dashboards — backend/schema/APIs only: ParentPartnerId hierarchy, wallet financing, revenue share, financial reporting APIs. No UI.
- **B8–B11 (backlog, separate jobs):** Dashboards — (B8) Steller admin, (B9) Partner, (B10) Agent, (B11) Sub-agent. Each dashboard is a separate job; all consume APIs from B6.
- **MVP:** B6 first, then dashboard jobs as prioritized.

---

## 9. Open Questions / Decisions

- Commission formula and example (Steller cost, agent margin, sub-agent margin).
- SLA and escalation rules for support.
- Agent onboarding approval flow and capacity limits.
- Exact schema: ParentPartnerId vs repurposing ReferredByPartnerId; migration from referral to agency.

---

## 10. References

- **PRD:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`
- **Backlog:** `docs/BACKLOG_V2.md` (B5 remove referral; B6 prepare for dashboards; B8 Steller admin, B9 Partner, B10 Agent, B11 Sub-agent dashboards — separate jobs)
- **Dashboard prep:** `docs/architecture/blueprints/dashboards-prep.md`
- **ADR-008 (referral — to be superseded by agency):** `docs/architecture/decisions/adr-008-referral-program.md`
- **APIs (partner):** `docs/architecture/atlas/apis.yaml`
- **PLG plan:** `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md`
- **PLG execution:** `docs/PLG_EXECUTION_PLAN.md`
- **INDEX:** `docs/INDEX.yaml`
