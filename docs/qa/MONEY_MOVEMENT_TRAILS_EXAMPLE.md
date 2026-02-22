# Money Movement Trails — Example (Partner, Wallet, Order, Deduction)

**Purpose:** Show how to read the **trail** of money movement: who (partner), wallet balance before/after, what was bought (order), and the deduction. Includes: where the catalog comes from (Bamboo), how partner price is calculated, and a full story from admin signup to order to trail.  
**Reference:** `Steller.Infrastructure/Services/Wallet/WalletService.cs`, `OrderService.cs`, `BrandService.cs`, `PricingCalculator.cs`, tables `Wallets`, `WalletHistories`, `Orders`, `Brands`, `Products`, `PartnerProductPricings`, `PartnerDiscounts`.

---

## 0. Catalog, pricing, and “did we run price?”

### 0.1 Are tables populated with real Bamboo catalog?

| Context | Catalog source | What’s in DB |
|--------|-----------------|--------------|
| **Production / staging** | Yes, when sync runs | Admin (or scheduled job) calls **Sync catalog** (e.g. `POST /api/brand/sync-catalog` or BrandBackgroundService). That uses Bamboo v2 catalog API and writes to **Brands** and **Products** (real SKUs, face values, vendor cost). So **Brands** and **Products** are populated from real Bamboo. |
| **Integration tests** | No | Tests use **MockBambooClient** and **TestDataFactory.EnsureProductExists(sku, …)**. So test DB has test products (e.g. TEST-SKU-UF4), not live Bamboo catalog. |

So: **Tables can be populated with real Bamboo catalog when sync runs**; in tests they are not.

### 0.2 How is partner price calculated?

- **At order time (what the partner is charged):**  
  **PricingCalculator** runs for every order:
  - **Partner discount:** `PartnerDiscounts` (per partner + vendor): `DiscountPercent` (e.g. 10 = 10% off).
  - **Sell price:** `sellPrice = faceValue × (1 - discountPercent)`.
  - **Cost:** `totalCost = bambooCost + FIXED_FEE` (e.g. 0.10). Bamboo cost comes from product (e.g. `faceValue × (VendorCostPercentage/100)`).
  - **Profit guard:** Order allowed only if `netProfit = sellPrice - totalCost >= 0`.
  - The **value used for the order** (and for the wallet deduction) is **PricingResult.SellPrice**. So “partner %” that affects what the partner pays is **PartnerDiscount.DiscountPercent** (and product cost).

- **In the catalog (what partner sees):**  
  - If admin has assigned products to the partner via **PartnerProductPricing**: catalog shows **PriceAllow** (min) and **Price** (max) for those products.
  - Otherwise: catalog uses the same **PricingCalculator** (same discount %) to show a price.

So: **Partner price is “run” on every order** via `PricingCalculator.CalculatePriceAsync`; the **value** is **SellPrice** (and that becomes Order.SaleTotal and the wallet debit).

### 0.3 Did we run price for that partner? What was the value?

- In **integration tests:** Yes. When a partner places an order, `OrderService.CreateOrderAsync` calls `_pricingCalculator.CalculatePriceAsync(partnerId, vendorId, totalFaceValue, totalBambooCost, request.ExpectedTotal)`. The returned **SellPrice** is what is debited. For test products (e.g. faceValue 50, VendorCostPercentage 98%), the exact value depends on PartnerDiscount (often 0 in tests) and FIXED_FEE; e.g. sellPrice ≈ 50, totalCost ≈ 49 + 0.10, so net profit ≈ 0.90 and order is allowed.
- So: **Price is run for that partner at order time**, and the **value** is stored in **Order.SaleTotal** and in **WalletHistories.Amount** for the debit row.

---

## 1. Full story (admin → partner → order → trail)

End-to-end flow as requested: admin signs up partner, sets his %, picks a list of cards (e.g. 10) that the partner can purchase, funds wallet, partner retrieves approved catalog, checks wallet, places order; then the money trail.

---

### Step 1 — Admin signs up partner

- Admin creates the **Partner** (e.g. Id = 1, BusinessName = "Acme Corp").
- **Tables:** `Partners` (and usually a wallet is created when first credited, or via onboarding).

---

### Step 2 — Admin sets partner’s %

- **Option A (order price):** Admin sets **PartnerDiscount** for that partner (and vendor): e.g. `POST /api/admin/partners/1/discounts` with `VendorId`, `DiscountPercent` (e.g. 10). This is the **%** used at **order time** by PricingCalculator: partner pays `faceValue × (1 - discountPercent)`.
- **Option B (catalog / margin):** Admin uses **PartnerProductPricingService.AssignProductToPartnerWithMargin(productId, partnerId, marginPercentage)** to assign specific products with a **margin %** of the profit buffer. That sets **PartnerProductPricing.Price** (and optionally PriceAllow) for **catalog display** for that partner. Order charge is still computed by PricingCalculator (Option A) unless the system is extended to use PPP at order time.

So: **“Set his %”** = PartnerDiscount.DiscountPercent (and/or margin % when assigning products).

---

### Step 3 — Admin picks list of cards (e.g. 10) that partner can purchase

- Admin assigns **products** to the partner. In the current implementation that is done by creating **PartnerProductPricing** rows (partnerId + productId + Price / PriceAllow / margin %). Those are the products for which the partner sees **custom prices** in the catalog.
- **“List of 10 cards”:** Conceptually = 10 products that have a **PartnerProductPricing** record for this partner. Today, **GetCatalog** returns **all** products; products with PartnerProductPricing show partner-specific prices; others show default (calculator) price. To enforce “only these 10”, you would either (a) filter catalog to only products that have PartnerProductPricing for this partner, or (b) introduce an explicit “allowed products” list. So: **the “approved” list is represented by the set of products that have PartnerProductPricing (and optionally by catalog filtering if added).**

---

### Step 4 — Partner wallet funded

- Admin credits the partner’s wallet: e.g. `POST /api/wallet/1/credit` with `amount`, `description`, `referenceId`.
- **WalletService.CreditWalletAsync** runs: **Wallets.AvailableBalance** increases; one **WalletHistories** row (TransactionTypeId = 1, Credit) with Amount, BalanceBefore, BalanceAfter, Description.
- Example: **Wallet was 0 → after credit 500.00.**

---

### Step 5 — Partner retrieves his approved catalog

- Partner calls **GET /api/brand/getCatalog** with **x-api-key** (or JWT).
- **BrandService.GetCatalogAsync(partnerId)** returns brands and products. For each product:
  - If **PartnerProductPricing** exists for (partnerId, productId): catalog shows **PriceAllow**, **Price** (the “approved” prices for those cards).
  - Else: price is computed via **PricingCalculator** (PartnerDiscount %).
- So partner **sees** the list of products; the ones “picked” for him show his custom prices.

---

### Step 6 — Partner checks wallet

- Partner calls **GET /api/wallet/me** (x-api-key).
- Response includes **availableBalance** (e.g. **500.00**). So: **Wallet was Y = 500.00.**

---

### Step 7 — Partner places order

- Partner calls **POST /api/orders** with e.g. `sku`, `faceValue`, `quantity`, `referenceId` (one of the “approved” products).
- **OrderService.CreateOrderAsync**:
  - Loads product; computes **bambooCost** (from product).
  - Runs **PricingCalculator.CalculatePriceAsync(partnerId, vendorId, totalFaceValue, totalBambooCost, …)** → **SellPrice** (this is the **value** we “run” for that partner).
  - Profit guard: rejects if net profit &lt; 0.
  - Checks wallet balance ≥ SellPrice.
  - Creates **Order** (OrderNumber, PartnerId, **SaleTotal = SellPrice**, Status = Processing).
  - **Debits wallet:** `DebitWalletAsync(partnerId, pricingResult.SellPrice, "Order {OrderNumber}", order.Id)`.
- So: **Z** = Order #1001 (or current OrderNumber), **D** = **SaleTotal** = **SellPrice** (e.g. **52.50**).

---

### Step 8 — Money trail (expanded and organized)

**Partner X, wallet was Y, bought Z, and deduction is D.**

| Trail element | Meaning | Example / Where |
|---------------|---------|------------------|
| **Partner X** | Partner identity | PartnerId = 1, BusinessName = "Acme Corp" |
| **Wallet was Y** | Balance before order | **500.00** (from Wallets.AvailableBalance or WalletHistories.BalanceBefore) |
| **Bought Z** | Order (what was bought) | Order #1001: 1× product (SKU, face value $50), SaleTotal = 52.50 |
| **Deduction D** | Amount taken from wallet | **52.50** = Order.SaleTotal = PricingResult.SellPrice |
| **Wallet after** | Balance after order | **447.50** (WalletHistories.BalanceAfter; Wallets.AvailableBalance) |

**Ledger (WalletHistories):**

| Field | Value |
|-------|--------|
| TransactionTypeId | 2 (Debit) |
| Amount | **52.50** |
| BalanceBefore | **500.00** |
| BalanceAfter | **447.50** |
| Description | `Order 1001` |
| OriginalTransactionId | Order.Id (links to Orders) |

So the **full trail** for this story: Admin signed up partner 1, set his % (discount/margin), picked his list of cards (PartnerProductPricing), funded wallet to 500. Partner retrieved catalog, saw his prices, checked wallet (500), placed order #1001; **deduction 52.50**, **wallet after 447.50**.

---

## 2. Trail in one sentence (reference)

**Partner X, wallet was Y, bought Z, and deduction is D.**

| Placeholder | Meaning | Where it lives |
|-------------|---------|----------------|
| **X** | Partner | `Partners`, `Wallets.PartnerId` |
| **Y** | Wallet balance **before** | `Wallets.AvailableBalance` or `WalletHistory.BalanceBefore` |
| **Z** | What was bought (order) | `Orders`: OrderNumber, SaleTotal, Status, OrderItems (SKU, face value, quantity) |
| **D** | Amount deducted | `WalletHistories.Amount`; for orders, D = Order.SaleTotal |

---

## 3. Concrete numeric example

**"Partner 1, wallet was 500.00, bought Order #1001 (1× $50 gift card), and deduction is 52.50."**

### 3.1 Before the order

| Entity | Field | Value |
|--------|--------|--------|
| Partner | Id | 1 |
| Wallet | AvailableBalance | **500.00** |

### 3.2 Order and wallet debit

| Entity | Field | Value |
|--------|--------|--------|
| Order | OrderNumber | 1001 |
| Order | SaleTotal | **52.50** |
| WalletHistories | Amount | **52.50** |
| WalletHistories | BalanceBefore | **500.00** |
| WalletHistories | BalanceAfter | **447.50** |
| WalletHistories | Description | `Order 1001` |
| WalletHistories | OriginalTransactionId | Order.Id |

### 3.3 After the order

| Entity | Field | Value |
|--------|--------|--------|
| Wallet | AvailableBalance | **447.50** |

---

## 4. Other wallet actions (credits)

| Action | Who | Description example | TransactionTypeId |
|--------|-----|---------------------|--------------------|
| Admin credits wallet | Admin | "Test credit" | 1 (Credit) |
| Refund (cancel order) | System | "Refund: User requested cancel" | 1 (Credit) |
| Order debit | System | "Order 1001" | 2 (Debit) |

---

## 5. How to read the trail (APIs and DB)

### Partner (own trail)

- **Balance:** `GET /api/wallet/me` (x-api-key) → `availableBalance`.
- **History:** `GET /api/wallet/transactions?page=1&pageSize=20` → list of TransactionDto: Id, Amount, Type ("Credit"/"Debit"), NewBalance, Description, CreatedAt. For an order, Description is like `"Order 1001"` and Amount is the deduction.

### Admin (any partner’s trail)

- **Balance:** `GET /api/wallet/{partnerId}` (Bearer).
- **Credit:** `POST /api/wallet/{partnerId}/credit` (body: amount, description, referenceId).

### Database (full trail)

- **Wallets:** current balance per partner.
- **WalletHistories:** immutable ledger. Link to orders via `OriginalTransactionId = Order.Id` (as string).
- **Orders:** OrderNumber, PartnerId, SaleTotal, Status; OrderItems for what was bought.

```sql
-- Ledger rows for partner 1 (newest first)
SELECT wh."Amount", wh."BalanceBefore", wh."BalanceAfter", wh."Description", wh."CreatedAt",
       tt."Name" AS Type
FROM "WalletHistories" wh
JOIN "Wallets" w ON w."Id" = wh."WalletId"
JOIN "TransactionTypes" tt ON tt."Id" = wh."TransactionTypeId"
WHERE w."PartnerId" = 1
ORDER BY wh."CreatedAt" DESC;
```

---

## 6. Summary

| Item | Meaning |
|------|--------|
| **Catalog** | Populated from **real Bamboo** when sync runs; tests use mock + test products. |
| **Partner price** | **Order:** PricingCalculator (PartnerDiscount %, fixed fee, profit guard); **Catalog:** PartnerProductPricing (PriceAllow/Price) or calculator. |
| **Price run for partner?** | Yes, at **order time**; **value** = PricingResult.SellPrice = Order.SaleTotal = wallet debit. |
| **Story** | Admin signs up partner → sets % (discount/margin) → picks list of cards (PartnerProductPricing) → funds wallet → partner gets catalog → partner checks wallet → partner places order → **trail:** Partner X, wallet was Y, bought Z, deduction D (and wallet after = Y − D). |

All money movements are in **WalletHistories** (immutable); **OriginalTransactionId** links to the order (or other reference).
