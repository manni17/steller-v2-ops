# Partner onboarding runbook

**Purpose:** Single step-by-step guide for Steller admin + partner: from signup to first successful test order.  
**Audience:** GTM, support, and partner technical teams.  
**Reference:** `docs/qa/GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md`, `steller-backend/docs/integration/STELLER_INTEGRATION_GUIDE.md`

---

## 1. Overview

| Step | Who | Action |
|------|-----|--------|
| 1 | Partner | Sign up (public API or portal) |
| 2 | Admin | Fund partner wallet |
| 3 | Admin | Set partner discount % (and/or assign products with margin) |
| 4 | Admin | Assign products partner can sell (optional: “list of 10 cards”) |
| 5 | Admin | Generate API key and deliver to partner securely |
| 6 | Partner | Retrieve approved catalog, check wallet, place first test order |

---

## 2. Step-by-step

### Step 1 — Partner signs up

**Partner (or Steller on behalf of partner):**

- **Option A (self-service):** `POST /api/public/signup`  
  Body: `{ "companyName": "Acme Corp", "email": "partner@acme.com" }`  
  Response (201): Includes **API key** (shown once). Partner must store it securely.
- **Option B (admin-created):** Admin creates partner via dashboard or `POST /api/partner/createdPartner` (admin auth). No API key yet; admin will generate in Step 5.

**Outcome:** Partner record exists; PartnerId known. If Option A, partner already has an API key (skip Step 5 key generation; admin may still create a new key and revoke the signup key).

---

### Step 2 — Admin funds partner wallet

**Admin:**

1. Log in: `POST /api/auth/login` with admin credentials → obtain Bearer token.
2. Credit wallet: `POST /api/wallet/{partnerId}/credit`  
   Headers: `Authorization: Bearer {token}`  
   Body: `{ "amount": 500, "description": "Initial funding", "referenceId": "onboard-{date}" }`  
   Use the PartnerId from Step 1.
3. Verify: `GET /api/wallet/{partnerId}` → `availableBalance` should equal the credited amount.

**Outcome:** Partner has a positive wallet balance (e.g. 500.00).

---

### Step 3 — Admin sets partner discount %

**Admin:**

- **Per-vendor discount (used at order time):**  
  `POST /api/admin/partners/{partnerId}/discounts`  
  Body: `{ "vendorId": "BRAND", "discountPercent": 10 }`  
  (10 = 10% off; partner pays 90% of face value for that vendor.)
- List discounts: `GET /api/admin/partners/{partnerId}/discounts`.

**Outcome:** Partner’s order price is calculated with the set discount(s). If no discount is set, default (0%) is used; product cost and fixed fee still apply (see PricingCalculator).

---

### Step 4 — Admin assigns products partner can sell (“approved list”)

**Admin:**

- Assign specific products with margin (catalog price for this partner):  
  Use PartnerProductPricing (e.g. `AssignProductToPartnerWithMargin(productId, partnerId, marginPercentage)` or dashboard equivalent).  
  Repeat for each product in the “approved list” (e.g. 10 cards).
- **Note:** Today the catalog returns all products; those with PartnerProductPricing show partner-specific prices. To enforce “only these N products,” filter catalog by products that have PartnerProductPricing for this partner, or document that the approved list is implicit from assigned pricing.

**Outcome:** Partner sees custom prices for the assigned products when calling getCatalog; at order time, PricingCalculator still uses PartnerDiscount (Step 3) for the charge.

---

### Step 5 — Admin generates API key and delivers to partner

**Admin:**

1. Generate key: `POST /api/admin/partners/{partnerId}/keys`  
   Body: `{}` (optional: `allowedIpAddresses`).  
   Response: **API key** (plain text, shown once) and preview (e.g. `stlr_live_...abc`).
2. Deliver the key to the partner via a secure channel (not email plain text if possible; use secure link or vault).
3. Revoke old key if rotating: `DELETE /api/admin/partners/{partnerId}/keys/{keyId}` (list keys first to get keyId).

**Outcome:** Partner has a valid `x-api-key` to use in all API requests.

---

### Step 6 — Partner: first test order

**Partner (technical team):**

1. **Get catalog:**  
   `GET /api/brand/getCatalog`  
   Header: `x-api-key: {partner API key}`  
   → Verify products and partner-specific prices.
2. **Check wallet:**  
   `GET /api/wallet/me`  
   Header: `x-api-key: {partner API key}`  
   → Verify `availableBalance` (e.g. 500.00).
3. **Place order:**  
   `POST /api/orders`  
   Header: `x-api-key: {partner API key}`  
   Body: `{ "sku": "{SKU from catalog}", "faceValue": 50, "quantity": 1, "referenceId": "{new UUID}" }`  
   → Expect **202 Accepted** and order `id`.
4. **Poll until completed:**  
   `GET /api/orders/{orderId}` every 2–5 seconds until `status` is `Completed` or `Failed`.  
   When `Completed`, `cards` array contains serial, pin, expiry.
5. **Verify wallet deduction:**  
   `GET /api/wallet/me` → balance decreased by order `saleTotal`.  
   `GET /api/wallet/transactions` → one Debit row with description like `Order {orderNumber}`.

**Outcome:** Partner has completed first order and verified the money trail (partner X, wallet was Y, bought Z, deduction D). See `docs/qa/MONEY_MOVEMENT_TRAILS_EXAMPLE.md`.

---

## 3. Checklist (admin)

- [ ] Partner record created (signup or admin-created).
- [ ] Wallet credited; balance verified.
- [ ] Partner discount(s) set for relevant vendorId(s).
- [ ] Products assigned to partner (PartnerProductPricing) if using “approved list.”
- [ ] API key generated and securely delivered to partner.
- [ ] Partner has integration guide and base URL (production or staging).

---

## 4. Checklist (partner)

- [ ] API key received and stored securely.
- [ ] Called getCatalog; saw expected products and prices.
- [ ] Called wallet/me; saw expected balance.
- [ ] Placed test order with unique referenceId; received 202 and order id.
- [ ] Polled order until Completed; received card details.
- [ ] Verified wallet balance decreased and transactions show debit.

---

## 5. Troubleshooting

| Issue | Check |
|-------|--------|
| 401 on getCatalog/orders | API key missing, wrong, or revoked. Use valid `x-api-key` header. |
| 400 “Insufficient funds” | Wallet balance &lt; order saleTotal. Admin credits wallet (Step 2). |
| 400 “Product not found” | SKU not in catalog or typo. Use exact SKU from getCatalog. |
| Order stuck Processing | Vendor (Bamboo) may be slow or failing. Admin can cancel/refund or use mark-failed-and-refund. |
| Partner sees no/wrong prices | Confirm PartnerDiscount (Step 3) and/or PartnerProductPricing (Step 4) for this partner and product/vendor. |

---

*Runbook version: 2026-02. Align with `STELLER_INTEGRATION_GUIDE.md` and BACKLOG_V2 § GTM.*
