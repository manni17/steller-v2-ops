# Go-to-Market Readiness: B2B Partner Experience & System Readiness

**Owner:** GTM / QA Lead / Product  
**Viewpoint:** Go-to-market (GTM) expert  
**Purpose:** Assess whether current tests and system are enough for B2B partners; describe the partner experience and what’s missing for business and technical teams.  
**References:** `USER_FLOW_INTEGRATION_TEST_PLAN.md`, `MONEY_MOVEMENT_TRAILS_EXAMPLE.md`, `BUSINESS_PLATFORM_GAP_ANALYSIS.md`, `STELLER_INTEGRATION_GUIDE.md`  
**Last Updated:** 2026-02-20

---

## How to Use

**Quick guide:** Load this doc for GTM readiness. Actionable items live in `docs/BACKLOG_V2.md` §7a. Tests: GTM-T1–T5; Docs: GTM-D1–D3; Platform: GTM-P1–P7. Use as checklist for B2B launch readiness.

### Top 10 Common Prompts

| # | Prompt |
|---|--------|
| 1 | What GTM tests and platform items are done vs pending? → BACKLOG_V2 §7a |
| 2 | Is the system ready for controlled B2B launch? → Readiness checklist §2 |
| 3 | Implement GTM-T1 (signup→first order E2E); add to UserFlowIntegrationTests |
| 4 | Add GTM-T2 webhook delivery test; assert POST with HMAC |
| 5 | Implement GTM-P1 (audit log); update AdminAuditController, GET /api/admin/audit-log |
| 6 | What is missing for partners (business vs technical)? → §4 |
| 7 | Describe B2B partner journey (signup → onboard → integrate → operate) |
| 8 | Run partner onboarding runbook (GTM-D1); verify signup → funding → key → first order |
| 9 | Add GTM-P2 (self-service key rotation) or GTM-P3 (usage summary API) |
| 10 | Check integration guide has rate limits and webhook docs (GTM-D2, GTM-D3) |

---

## 1. Are these tests enough?

### What exists today

| Test type | Coverage | Enough for GTM? |
|-----------|----------|-----------------|
| **User-flow integration (8 tests)** | Partner: catalog → order → cards, wallet deduction, idempotency, insufficient funds. Admin: login, credit wallet, cancel/refund, create API key. | **Core journey: yes.** Covers “partner gets catalog, places order, gets cards and correct wallet trail” and “admin provisions and supports partner.” |
| **Financial invariants (Critical 4)** | Atomic rollback, profit guard, idempotency, concurrency. | **Yes** for money correctness. |
| **Bamboo integration** | Place order, get order, catalog sync (with mocks in tests). | **Partial.** Real Bamboo E2E is not in automated tests; production readiness depends on staging/manual runs. |
| **Money trails** | Documented (partner X, wallet Y, bought Z, deduction D) and traceable in DB/API. | **Yes** for understanding and support. |

### Gaps from a GTM perspective

| Gap | Why it matters |
|-----|-----------------|
| **No partner signup → first order E2E test** | User flows use TestDataFactory (pre-created partner + product). Real path: `POST /api/public/signup` → admin funds wallet → partner gets catalog → places order. That path is not automated. |
| **No webhook delivery test** | Partners rely on webhooks (order completed/failed, low balance). No test asserts “webhook is sent and signed” in the integration suite. |
| **No catalog-filtering test** | “Partner sees only approved products” (e.g. 10 cards) is not enforced in code; no test for “partner A sees 10, partner B sees 5.” |
| **No load/rate-limit tests** | B2B partners may burst; Bamboo has strict rate limits. No automated check that Steller respects limits and returns clear errors under load. |
| **No sandbox E2E** | Sandbox (Bamboo + Steller) is documented but not part of CI; partners need “try before production” with real-like behavior. |

**Verdict:** Tests are **enough to ship an MVP** and to prove core partner and admin flows and financial correctness. They are **not enough** for a mature GTM without adding: (1) public signup → first order flow, (2) webhook assertion, (3) optional sandbox E2E and load/rate-limit checks.

---

## 2. Is the system ready?

### Readiness checklist (GTM lens)

| Dimension | Ready? | Notes |
|-----------|--------|-------|
| **Partner can integrate** | ✅ Yes | API key auth, catalog, orders, wallet, idempotency, polling, integration guide. |
| **Partner can operate** | ✅ Yes | Balance, transactions, export CSV, order status, webhooks (URL + secret). |
| **Admin can onboard** | ✅ Yes | Create partner, generate API key, set discounts, credit wallet, assign products (margin). |
| **Admin can support** | ✅ Yes | Cancel/refund/mark-failed-and-refund, view wallet, view orders. |
| **Money is correct** | ✅ Yes | Atomic debit, immutable ledger, profit guard, reconciliation, consistency job. |
| **Partner trust** | ⚠️ Partial | Ledger export ✅; self-service key rotation ❌; audit log for admin actions ❌ (planned). |
| **Vendor (Bamboo)** | ✅ Yes | Catalog v2, place order, get order, rate limits; exchange rates, accounts, orders list, transactions, notification (client added). |
| **Observability** | ⚠️ Partial | Health, metrics, alerts to admin; partner request logs (developer role). No partner-facing status page or SLA visibility. |
| **Compliance / audit** | ❌ Not yet | Audit log for admin actions + query API planned, not done. |

**Verdict:** System is **ready for controlled B2B launch** (known partners, manual onboarding, prepaid wallet, API + docs). It is **not fully ready** for scale or high-trust enterprise without: audit log, optional self-service key rotation, and clearer “catalog visibility” (who sees which products).

---

## 3. B2B partner experience using Steller

### 3.1 Journey (business view)

| Stage | What happens | Steller today |
|-------|----------------|----------------|
| **Discover** | Partner learns about Steller (sales, marketing). | Out of scope (no portal). |
| **Sign up** | Partner registers. | `POST /api/public/signup` (company name, email). Returns API key once; optional trial credit. |
| **Onboard** | Steller sets partner up. | Admin: create/configure partner, set discount %, assign products, fund wallet, share API key + docs. |
| **Integrate** | Partner’s tech team builds to API. | Catalog (GET), orders (POST + poll), wallet (GET), transactions, webhooks. Integration guide + idempotency + errors. |
| **Go live** | Partner switches to production. | Same API; production base URL and keys. |
| **Operate** | Partner runs day to day. | Check balance, place orders, receive webhooks, export transactions, view order status. |
| **Support** | Issues or questions. | Admin can cancel/refund, credit wallet, revoke/issue keys. No partner-facing status page or SLA. |

### 3.2 Experience by role

**Business (partner ops / finance):**

- **Get catalog** → See only products and prices they’re allowed (partner-specific pricing).
- **Check wallet** → Current balance (GET /api/wallet/me).
- **See spend** → Transaction history + CSV export (ledger).
- **Understand charges** → Money trail: “Partner X, wallet was Y, bought Z, deduction D” (documented; support can trace in DB/API).
- **Low balance** → Webhook + email (if configured); no in-app dashboard unless partner builds one.

**Technical (partner dev/integration):**

- **Auth** → One API key per partner (`x-api-key`); key shown once at creation.
- **Catalog** → GET /api/brand/getCatalog → list of products and partner-specific prices; use SKU in orders.
- **Orders** → POST /api/orders (sku, faceValue, quantity, referenceId); 202 Accepted; poll GET /api/orders/{id} until Completed/Failed; cards in response when Completed.
- **Wallet** → GET /api/wallet/me, GET /api/wallet/transactions (and export CSV).
- **Webhooks** → Configure URL (and secret); receive order completed/failed and low_balance (HMAC-signed).
- **Logs** → Partner request history (developer role) for debugging.
- **Sandbox** → Documented (Bamboo sandbox + Steller); not self-serve in CI.

So: **B2B partner experience is “API-first, prepaid wallet, partner-specific catalog and pricing, with webhooks and ledger export.”** There is no Steller-built partner portal; partners use the API (and optionally their own UI).

---

## 4. What’s missing for B2B partners

### 4.1 Business team (partner’s ops / finance / management)

| Missing | Why it matters |
|---------|----------------|
| **Partner-facing usage/revenue summary** | No single “last 30 days: orders, spend, top products” API or report. They have to aggregate from orders + transactions. |
| **Forecast / balance alerts** | Low balance alert exists; no “projected runout” or recommended top-up. |
| **Clear “approved catalog”** | Which products they can sell is implicit (pricing/discounts). No explicit “your product list” or “10 cards” contract view. |
| **Statement / invoice** | Prepaid only; no monthly statement or invoice document. Acceptable for MVP; needed if billing model changes. |
| **SLA / status visibility** | No partner-facing status page or SLA (e.g. uptime, incident history). Trust is via support and docs. |

### 4.2 Technical team (partner’s developers / DevOps)

| Missing | Why it matters |
|---------|----------------|
| **Self-service API key rotation** | Key rotation requires Steller admin. Slows response to compromise or key leak. |
| **Webhook/secret self-service** | Partner can set WebhookUrl/Secret via API (GET/PUT partner webhook); confirm in docs and that it’s enough for “self-service.” |
| **Sandbox in CI / try-before-prod** | Sandbox exists in docs but no automated “sandbox E2E” or public sandbox environment; harder to validate before go-live. |
| **Rate limits and retries (documented)** | Bamboo limits (and any Steller throttling) should be in integration guide: when to back off, retry-after, idempotency. |
| **Webhook delivery guarantee / replay** | No “webhook replay” or “last N events” API; if partner misses an event, they must poll orders. |
| **API versioning** | No version in path or header; changes are backward-compatible but no formal version policy for partners. |
| **OpenAPI / Postman** | Integration guide is the contract; machine-readable spec (OpenAPI) or Postman collection would speed integration. |

### 4.3 Both (business + technical)

| Missing | Why it matters |
|---------|----------------|
| **Audit trail for admin actions** | “Who credited this wallet? Who cancelled this order?” — needed for disputes and compliance; planned, not yet done. |
| **Catalog visibility control** | “Only these 10 cards” is via pricing assignment; no explicit “visible” flag. Business and tech both need a clear, consistent rule. |
| **Partner onboarding runbook** | Step-by-step (admin + partner): signup → funding → discount → product assignment → key delivery → first test order. Exists in pieces; one runbook would help GTM and support. |

---

## 5. Summary: GTM verdict

| Question | Answer |
|----------|--------|
| **Are these tests enough?** | **For MVP: yes.** Core partner and admin flows and financial correctness are covered. For a stronger GTM: add signup→first-order flow, webhook test, optional sandbox E2E and rate/load checks. |
| **Is the system ready?** | **Yes for controlled B2B launch:** API-first, prepaid wallet, partner catalog/pricing, admin support, Bamboo integrated, money trails clear. **Not yet** for scale/enterprise without audit log, optional self-service key rotation, and clearer catalog visibility. |
| **What is the B2B partner experience?** | **Sign up (public) → admin onboarding (funding, discount, products, API key) → integrate (catalog, orders, wallet, webhooks) → operate (balance, transactions, export, order status).** No Steller-built partner portal; experience is API + docs + support. |
| **What’s missing for partners?** | **Business:** Usage/revenue summary, forecast/alerts, explicit “approved catalog” view, statement/invoice (if needed), SLA/status. **Technical:** Self-service key rotation, sandbox E2E, rate-limit/retry docs, webhook replay (optional), API versioning, OpenAPI/Postman. **Both:** Admin audit log, catalog visibility rule, single onboarding runbook. |

---

## 6. Actionable items (for backlog & execution)

**Source:** Sections 1–4 above. Each row is a discrete task for PRD/backlog; owners and status live in `docs/BACKLOG_V2.md` § GTM.

### 6.1 Tests (QA / Dev)

| ID | Action | Owner | Backlog ref |
|----|--------|-------|-------------|
| GTM-T1 | Add E2E test: public signup → admin funds wallet → partner getCatalog → place order → completed | QA Agent | BACKLOG_V2 § GTM |
| GTM-T2 | Add integration test: webhook delivery (order completed/failed) — assert POST to partner URL with HMAC | QA Agent | BACKLOG_V2 § GTM |
| GTM-T3 | Optional: test “partner A sees N products, partner B sees M” (catalog filtering by PartnerProductPricing) | QA Agent | BACKLOG_V2 § GTM |
| GTM-T4 | Optional: document or add sandbox E2E in CI (Bamboo sandbox + Steller) | Dev Agent | BACKLOG_V2 § GTM |
| GTM-T5 | Optional: rate-limit / load test (Steller respects Bamboo limits; clear 429/retry-after) | Dev/QA | BACKLOG_V2 § GTM |

### 6.2 Documentation (execute first)

| ID | Action | Owner | Status |
|----|--------|-------|--------|
| GTM-D1 | **Partner onboarding runbook:** Single doc: signup → admin funding → discount → product assignment → key delivery → first test order | QA/TPM | ✅ Executed |
| GTM-D2 | **Integration guide:** Rate limits & retries (Bamboo limits, back off, retry-after, idempotency) | Dev Agent | ✅ Executed |
| GTM-D3 | **Integration guide:** Webhook self-service (GET/PUT /api/partner/webhook) and HMAC | Dev Agent | ✅ Executed |

### 6.3 Product / platform (backlog)

| ID | Action | Owner | Backlog ref |
|----|--------|-------|-------------|
| GTM-P1 | Audit log for admin actions + GET /api/admin/audit-log (who credited, who cancelled, when) | Dev Agent | BACKLOG_V2 § GTM; GAP_CLOSURE_PLAN |
| GTM-P2 | Partner self-service API key rotation (partner can rotate own key without admin) | Dev Agent | BACKLOG_V2 § GTM |
| GTM-P3 | Partner-facing usage/revenue summary API (e.g. last 30 days: orders count, spend, top products) | Dev Agent | BACKLOG_V2 § GTM |
| GTM-P4 | Catalog visibility rule: document or implement “approved product list” per partner (e.g. only products with PartnerProductPricing) | Product/Dev | BACKLOG_V2 § GTM |
| GTM-P5 | OpenAPI spec or Postman collection for partner API | Dev Agent | BACKLOG_V2 § GTM |
| GTM-P6 | Optional: Webhook replay or “last N events” API for missed events | Dev Agent | BACKLOG_V2 § GTM |
| GTM-P7 | Optional: Partner-facing status page or SLA visibility | Product/Ops | BACKLOG_V2 § GTM |

### 6.4 Execution summary

- **Done in this pass:** GTM-D1 (runbook), GTM-D2 (rate limits in integration guide), GTM-D3 (webhook self-service in guide); PRD and BACKLOG_V2 updated with GTM section.
- **Next:** GTM-T1, GTM-T2 (tests); GTM-P1 (audit log) per GAP_CLOSURE_PLAN; then P2–P5 as capacity allows.

---

*This assessment is based on the codebase and docs as of 2026-02. Update as features and tests are added.*
