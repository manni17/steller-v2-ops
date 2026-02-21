# UI E2E Test Protocol — Omni-Channel Partner Provisioning & Ordering

**Purpose:** Strict step-by-step manual testing script for the human operator to execute the Omni-Channel E2E user journey via Web Dashboards. Mirrors the API automation scenario in `run_e2e_api_journey.sh`.

**Environment:** Steller v2 Production mode (Bamboo Sandbox)

**Scenario:** Admin provisions B2B Partner → funds wallet → assigns custom commissions to 5 SKUs → Partner logs in → views isolated catalog → places two orders (Qty 1, Qty 5) → verification of fulfillment and audit.

---

## Prerequisites

| Item | Value |
|------|-------|
| Admin Dashboard URL | e.g. `http://localhost:9000/admin` or production URL |
| Partner Dashboard URL | e.g. `http://localhost:9001/partner` or production URL |
| Admin credentials | admin@steller.com / Admin123! (or env-specific) |
| API running | Steller v2 API on port 6091 |
| Bamboo | Sandbox mode, catalog synced |

---

## Phase 1: Admin Dashboard — Provisioning

### Step 1.1: Login as Admin

| Action | Detail |
|--------|--------|
| Navigate | Open Admin Dashboard URL in browser |
| Input | Email: `admin@steller.com` |
| Input | Password: `Admin123!` |
| Action | Click **Sign In** / **Login** |

**Assertion:** Redirect to Admin Overview or Dashboard home. Admin avatar/name visible in header.

---

### Step 1.2: Create Partner

| Action | Detail |
|--------|--------|
| Navigate | **Partner Management** → **Create Partner** (or **Add Partner**) |
| Input | Company Name: `E2E Test Partner Inc` |
| Input | Email: `e2e-partner-{timestamp}@example.com` |
| Optional | Use Case / Notes: `E2E black-box test` |
| Action | Click **Create** / **Submit** |

**Assertion:** Partner created; success toast/message. Partner appears in Partners list. Note the **Partner ID** for subsequent steps.

---

### Step 1.3: Credit Partner Wallet

| Action | Detail |
|--------|--------|
| Navigate | **Wallet** → Select partner (or **Partner Management** → Partner detail → **Wallet** tab) |
| Action | Click **Credit** / **Add Funds** |
| Input | Amount: `1000` |
| Input | Description: `E2E test credit` |
| Optional | Reference ID: `e2e-credit-{timestamp}` |
| Action | Click **Submit** / **Credit** |

**Assertion:** Wallet balance updates immediately to **1000** (or previous balance + 1000). Credit transaction visible in ledger.

---

### Step 1.4: Assign Custom Margin to 5 SKUs

| Action | Detail |
|--------|--------|
| Navigate | **Catalog** / **Pricing** → Select partner (or **Partner Management** → Partner detail → **Pricing** tab) |
| Action | Filter or select **5 active products/SKUs** from the catalog |
| Action | For each of the 5 SKUs: Apply custom margin (e.g. 10% discount or fixed markup) |
| Action | Click **Apply** / **Save** for each or batch |

**Assertion:** Wallet balance remains correct. Margin rules display correctly in the pricing table. Partner-specific prices visible for the 5 SKUs.

---

## Phase 2: Partner Dashboard — Ordering

### Step 2.1: Login to Partner Dashboard

| Action | Detail |
|--------|--------|
| Navigate | Open Partner Dashboard URL (or switch to Partner context) |
| Input | Email: partner email from Step 1.2 |
| Input | Password: (use credentials created for partner; or API key if Partner Dashboard supports API-key auth) |
| Action | Click **Sign In** / **Login** |

**Assertion:** Redirect to Partner home/dashboard. Partner name/context visible. Wallet balance shown in header/sidebar.

---

### Step 2.2: View Catalog

| Action | Detail |
|--------|--------|
| Navigate | **Catalog** / **Products** |

**Assertion:** Only the **5 authorized SKUs** are visible (or prices reflect the exact margins assigned in Step 1.4). No unauthorized products shown.

---

### Step 2.3: Order 1 — Qty 1

| Action | Detail |
|--------|--------|
| Action | Select SKU #1 (first of the 5) |
| Action | Add **1 unit** to cart |
| Action | Proceed to **Checkout** |
| Action | Confirm payment (wallet debited) |

**Assertion:** Order placed. Order status: `Processing`. Wallet balance in header deducts the **exact expected total** for 1 unit of SKU #1.

---

### Step 2.4: Order 2 — Qty 5

| Action | Detail |
|--------|--------|
| Action | Select SKU #2 (second of the 5) |
| Action | Add **5 units** to cart |
| Action | Proceed to **Checkout** |
| Action | Confirm payment (wallet debited) |

**Assertion:** Order placed. Order status: `Processing`. Wallet balance deducts the **exact expected total** for 5 units of SKU #2. Total deduction = Order 1 + Order 2.

---

## Phase 3: Partner Dashboard — Fulfillment & Assets

### Step 3.1: Order History — Status Transition

| Action | Detail |
|--------|--------|
| Navigate | **Order History** / **Orders** |
| Action | Wait for Bamboo fulfillment (or manually trigger sync if in dev) |
| Action | Refresh page (or wait for WebSocket/real-time update) |

**Assertion:** Both orders transition from `Processing` → `Completed`. Status badges/indicators update accordingly. If WebSocket is used, no refresh required.

---

### Step 3.2: Vault / Asset View

| Action | Detail |
|--------|--------|
| Navigate | **Vault** / **Assets** / **Gift Cards** / **Downloads** |
| Action | View or expand orders to see PINs/Serials |

**Assertion:** Exactly **6 distinct PINs/Serials** are available for download or viewing (1 from Order 1 + 5 from Order 2). Each card has Serial and PIN (or equivalent identifiers).

---

## Phase 4: Admin Dashboard — Audit & Reconciliation

### Step 4.1: System Logs / Vendor Audit

| Action | Detail |
|--------|--------|
| Navigate | **System Logs** / **Vendor Audit** / **Vendor API Calls** |
| Action | Filter by partner or date range |

**Assertion:** Vendor API calls reflect **successful interactions with Bamboo**. Status codes 200/201 or equivalent success. Request/response payloads logged (or summary available).

---

### Step 4.2: Financials / Orders

| Action | Detail |
|--------|--------|
| Navigate | **Financials** / **Orders** / **Partner Orders** |
| Action | Select the E2E partner; view the two orders |

**Assertion:** Profit margins match the **exact configuration** assigned in Step 1.4. Order totals and partner prices align with catalog pricing.

---

## Verification Checklist

| # | Check | Pass / Fail |
|---|-------|-------------|
| 1 | Admin login successful | ☐ |
| 2 | Partner created; ID captured | ☐ |
| 3 | Wallet credited $1000 | ☐ |
| 4 | 5 SKUs with custom margin assigned | ☐ |
| 5 | Partner login successful | ☐ |
| 6 | Catalog shows only 5 SKUs (or correct prices) | ☐ |
| 7 | Order 1 (Qty 1) placed; wallet deducted | ☐ |
| 8 | Order 2 (Qty 5) placed; wallet deducted | ☐ |
| 9 | Orders transition to Completed | ☐ |
| 10 | 6 distinct PINs/Serials in Vault | ☐ |
| 11 | Vendor API calls show Bamboo success | ☐ |
| 12 | Financial margins match configuration | ☐ |

---

## Troubleshooting

| Issue | Possible Cause | Action |
|-------|----------------|--------|
| 401 on Admin login | Wrong credentials / JWT expiry | Verify Admin email/password; check env |
| Partner not in list | Creation failed | Check API logs; retry Create Partner |
| Catalog empty | Catalog not synced | Run `POST /api/brand/sync-catalog` |
| Order stuck Processing | Bamboo Sandbox delay / job not run | Wait; check Hangfire dashboard; verify PlaceBambooOrderJob / PollBambooOrderJob |
| Wallet balance wrong | Ledger inconsistency | Check WalletHistories; run WalletConsistencyJob |
| 6 PINs not visible | Orders not fulfilled | Poll `GET /api/orders/{id}`; ensure Bamboo returns cards |

---

## Related Artifacts

| Artifact | Description |
|----------|-------------|
| `run_e2e_api_journey.sh` | API automation script (curl + psql) for same scenario |
| `docs/qa/API_DRIVEN_E2E_UI_SPEC.md` | Playwright/Cypress E2E UI specs |
| `docs/qa/TESTING_INDEX.md` | QA entry point and run instructions |
| `docs/architecture/atlas/apis.yaml` | API contracts and endpoints |
