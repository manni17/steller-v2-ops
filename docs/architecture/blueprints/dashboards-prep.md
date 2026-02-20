# Dashboard Preparation Blueprint — Admin & Partner

**Purpose:** Single prep document for building Admin and Partner dashboards (v2). Defines scope, personas, APIs, auth, data needs, and backlog mapping.
**Last Updated:** 2026-02-18
**Owner:** TPM (prep); Dev (implementation)
**Reference:** `docs/BACKLOG_V2.md` §5 Tier 3, §6 Tier 4; `docs/architecture/atlas/apis.yaml`

---

## 1. Overview

| Dashboard | Audience | Auth | Purpose |
|-----------|----------|------|---------|
| **Admin Dashboard** | Internal admins | JWT (Role: Admin) | Partner management, orders, wallets, API keys, growth metrics (signups, activations, revenue) |
| **Partner Dashboard** | Partners (developers) | JWT (Role: Partner) or x-api-key | Self-serve: catalog, orders, order history, wallet, stats, webhook config |

**Legacy today:** Admin (port 8080), Consumer/Partner (port 8081) — Vue.js. v2 prep defines APIs and specs so dashboard UI can be built without ambiguity.

---

## 2. Admin Dashboard — Full Prep

### 2.1 Persona & Capabilities

- **Who:** Admin users (JWT, Role = Admin).
- **Capabilities:** Login; list/search partners (API to confirm/add); view partner detail (wallet, orders, API keys); create/revoke API keys; partner discounts; order actions (cancel, refund, mark-failed-and-refund); **growth metrics view** (signups, activations, revenue) via Admin growth metrics API (§2.3).

### 2.2 Existing Admin APIs (Today)

Create/List/Revoke API keys, Partner discounts, Order cancel/refund/mark-failed-and-refund — all exist under `/api/admin/*`, JWT Admin.

**Gaps:** (1) List partners: e.g. `GET /api/admin/partners` (paged) — confirm or add to backlog. (2) **Growth metrics:** `GET /api/admin/metrics/growth` — signups, activations, revenue. Backlog: **§4 item 14 — growth_metrics_dashboard**.

### 2.3 Admin Growth Metrics API (Prep Spec)

- **Endpoint (proposed):** `GET /api/admin/metrics/growth` (or `/api/admin/dashboard/metrics`).
- **Auth:** JWT, Role Admin.
- **Response (conceptual):** signupsCount, activationsCount, revenueTotal; optional by partner/date. Data: Partners, Orders, ApiRequestLogs. Dev to define exact schema.

### 2.4 Admin Backlog Mapping

- **Item 14 — growth_metrics_dashboard:** Implement Admin API for signups, activations, revenue.
- **Item 11 — developer_portal:** Landing, docs, signup CTA; can link to Admin dashboard.
- **Future:** Admin UI app (v2); port TBD (legacy 8080).

---

## 3. Partner Dashboard — Full Prep

### 3.1 Persona & Capabilities

- **Who:** Partner users (JWT Partner or x-api-key).
- **Capabilities:** Login; view catalog; place order; order history & detail; wallet balance; partner stats; webhook config; referral code/referrals.

### 3.2 Existing Partner APIs (Today)

Catalog, POST/GET orders, GET /api/partner/stats, GET/PUT /api/partner/webhook, GET /api/partner/referral-code, GET /api/partner/referrals — exist. Confirm GET /api/orders (list) and GET /api/wallet (or partner wallet).

### 3.3 Partner Backlog Mapping

- **Item 11 — developer_portal:** Landing + signup; Partner dashboard = logged-in experience.
- **Item 12 — sandbox_environment:** Sandbox mode for partners.
- **Item 18 — partner_stats_api:** Covered by GET /api/partner/stats; confirm scope.
- **Future:** Partner UI app (v2); port TBD (legacy 8081).

---

## 4. Auth Summary

| Dashboard | Primary Auth |
|-----------|--------------|
| Admin | JWT Bearer, Role Admin |
| Partner | JWT Bearer (Role Partner) or x-api-key |

---

## 5. Data & Endpoints Matrix

| Need | Admin | Partner | Endpoint / Source |
|------|--------|---------|-------------------|
| Login | Yes | Yes | POST /api/auth/login |
| Partners list | Yes | — | GET /api/admin/partners (confirm/add) |
| Growth metrics | Yes | — | GET /api/admin/metrics/growth (item 14) |
| Catalog | — | Yes | GET /api/brand/getCatalog |
| Orders | via Admin | Yes | GET /api/orders, GET /api/orders/{id} |
| Wallet | per partner | Yes | GET /api/wallet or partner wallet |
| Stats / Webhook / Referral | — | Yes | /api/partner/stats, webhook, referral-code, referrals |

---

## 6. Deployment & Ports

Legacy: Admin 8080, Partner 8081. v2: TBD; reserved 8080, 8081 (see `docs/architecture/containers.yaml`).

---

## 7. Definition of Done (Prep)

- [x] Blueprint documents Admin and Partner scope, personas, capabilities.
- [x] Existing APIs listed; gaps called out (admin list partners, admin growth metrics).
- [x] Admin growth metrics API specified; backlog item 14 = implementation.
- [x] Partner API matrix complete.
- [x] Dev: Implement GET /api/admin/metrics/growth (item 14); add GET /api/admin/partners if missing.
- [x] Dev: developer_portal (11), sandbox (12) per PLG plan.

---

## 8. References

- Backlog: `docs/BACKLOG_V2.md` (Tier 3 §5 items 11–14, Tier 4 §6 item 18)
- APIs: `docs/architecture/atlas/apis.yaml`
- Auth: `docs/architecture/blueprints/security-auth.md`
- Containers: `docs/architecture/containers.yaml`
- UI testing: `docs/qa/UI_TESTING_STRATEGY.md`
