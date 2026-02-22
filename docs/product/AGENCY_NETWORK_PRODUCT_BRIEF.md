# Agency Network — Product Brief

**Status:** Planning  
**Owner:** Product  
**Last Updated:** 2026-02-20  
**Reference:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`, `docs/BACKLOG_V2.md`, `docs/architecture/blueprints/dashboards-prep.md`

---

## 1. Problem / Opportunity

Steller needs a **B2B2C agency network**: partners (API) onboard agents who recruit and manage sub-agents; agents and sub-agents use a **Telegram bot** (no web dashboard, no API). Agents finance sub-agent wallets; everyone gets cards from Steller. This replaces the flat referral model with a hierarchy that supports revenue share, visibility, and financial reporting.

---

## 2. Agency Model

**Hierarchy:** Steller → Partner (B2B, API) → Agent (Telegram) → Sub-agent (Telegram).

- **Partner (B2B):** API access; integrates with Steller programmatically.
- **Agent (parent):** Telegram bot only. Manages own account and all sub-agents. Top-up is manual (offline → admin credits). Allocates balance to sub-agent wallets. Earns revenue share on sub-agent sales.
- **Sub-agent:** Telegram bot only. Orders cards from admin-configured list. Wallet funded by agent. Sees only reports related to themselves.

**Key rule:** Agent does not hold or dispense cards; agent **finances** sub-agent wallets. All cards come from Steller. Commission rates differ by level (agent vs sub-agent). Revenue share applies only to direct children (no cascading to grandchildren).

**Agent/sub-agent channel:** Telegram bot (not web dashboard). Agents and sub-agents interact via one bot with role-based menus.

---

## 3. Revenue Model

| Actor | Gets cards from | Who tops up their wallet | Commission |
|-------|-----------------|--------------------------|------------|
| Agent | Steller (via dashboard) | Agent (self) | Agent rate |
| Sub-agent | Steller (via dashboard) | **Agent** (parent) | Sub-agent rate (different) |

- **Flow:** Agent top-up is **manual**: agent sends money offline → admin verifies → admin credits agent’s account (no automated payment in bot). Agent can then allocate balance to own use or sub-agent wallets. Sub-agents order cards via Telegram using balance funded by agent.
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
- **Agent card list:** Admin configures a single list of cards (e.g. up to 10) that **all agents** see in the Telegram bot. Each card is either:
  - **Quantum:** fixed denominations (e.g. 10, 20, 50).
  - **Range:** user chooses amount in a range (e.g. 1–100).
- **Manual top-up:** Admin verifies offline payment from agent and credits the agent’s account (no automated payment in bot).
- **Deliverable:** Admin dashboard spec (views, metrics, alerts, controls, agent card list config, manual credit flow).

---

## 7. Agent/Sub-agent Channel: Telegram Bot

- **Auth (decided):**
  - **Agents:** Link code or magic link so only approved partners can become “agents” in the bot.
  - **Sub-agents:** Invite link from agent (e.g. `t.me/StellerBot?start=invite_xxx`); agent creates sub-agent in Steller (or admin does), Steller generates invite; sub-agent taps link and is bound to that identity.
- **Agent card list:** Admin-configured list (e.g. 10 cards) for all agents. Cards are either **quantum** (fixed denominations, e.g. 10, 20) or **range** (user picks amount in range, e.g. 1–100). Bot shows only this list for ordering.
- **Top-up:** Manual. Agent pays offline; admin verifies and credits agent’s account (in admin dashboard). No payment collection in the bot.
- **Agent in bot:** Manage own account and sub-agents; allocate balance to sub-agent wallets; view financial reporting (own + sub-agents).
- **Sub-agent in bot:** Order cards from the configured list; view own wallet, orders, commission. No API access.
- **Voucher delivery — same channel:** Clients receive the product in the **same channel they used to order**. Order in Telegram → voucher delivered in Telegram (no redirect to webpage). Agent/sub-agent gets the voucher in the bot chat and forwards or shares it with their client in the same way they communicate (e.g. same Telegram chat or same channel). No requirement for client to open a link.
- **Sensitive data (in-channel):** No full PIN in chat when possible; prefer masked (e.g. `****1234`) with “Reveal PIN” that sends the PIN in a separate message that **auto-deletes** after a short time (e.g. 60 seconds). Do not log PINs in bot logs.

---

## 8. Scope and Phases

- **B5 (backlog):** Scrape/remove referral module (schema, APIs, signup referral flow).
- **B6 (backlog):** Prepare for agency dashboards — backend/schema/APIs only: ParentPartnerId hierarchy, wallet financing, revenue share, financial reporting APIs. No UI.
- **B8–B11 (backlog, separate jobs):** (B8) Steller admin dashboard, (B9) Partner dashboard, (B10) Agent experience (Telegram bot), (B11) Sub-agent experience (Telegram bot). B10/B11 are the same bot with role-based flows; all consume APIs from B6.
- **MVP:** B6 first, then dashboard jobs as prioritized.

---

## 9. Open Questions / Decisions

- Commission formula and example (Steller cost, agent margin, sub-agent margin).
- SLA and escalation rules for support.
- Agent onboarding approval flow and capacity limits.
- Exact schema: ParentPartnerId vs repurposing ReferredByPartnerId; migration from referral to agency.
- PIN/sensitive-data: delivery in same channel (Telegram); masked PIN + auto-delete on reveal; no PIN in logs (see §7).

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
