# Pricing: Legacy Steller vs Steller v2 — Investigation

**Goal:** Compare how legacy vs v2 compute partner-facing price (sell price, vendor cost, fee, profit) for test design and parity.

---

## 1. Legacy

**Location:** `/opt/steller-apps/Steller/` — **PartnerProductPricingService**, **BrandService**, **Product** / **PartnerProductPricing** models.

Legacy uses **PartnerProductPricing**, **ProductPricing**, **Price**, **PartnerProductPricingValue** and related services to derive partner-specific prices (e.g. per-product or per-partner discounts). Exact formula (sell = face - discount, fee, profit) and where it runs (order create vs catalog) are implementation-specific.

---

## 2. V2

**Location:** `/opt/steller-v2/steller-backend/` — **Steller.Core/Services/Pricing/PricingCalculator**, **IPricingCalculator**.

### Flow

- **CalculatePriceAsync(partnerId, vendorId, faceValue, bambooCost, maxTotal)**:
  - **GetDiscountAsync(partnerId, vendorId)** → discount percent (default 0).
  - **sellPrice = faceValue * (1 - discountPercent)**.
  - **totalCost = bambooCost + FIXED_FEE** (0.10m from PRD).
  - **netProfit = sellPrice - totalCost**.
  - **IsAllowed = (netProfit >= 0) and (maxTotal null or sellPrice <= maxTotal)**; else RejectionReason set.
- Used in **OrderService.CreateOrderAsync** (pricing result drives SaleTotal, VendorCost, FeeCost, NetProfit; profit guard rejects if negative margin).

### Summary (v2)

| Aspect   | V2                                                                 |
|----------|--------------------------------------------------------------------|
| Formula  | sellPrice = faceValue * (1 - discountPercent); totalCost = bambooCost + FIXED_FEE; netProfit = sellPrice - totalCost |
| Fee      | FIXED_FEE = 0.10m                                                 |
| Guard    | netProfit >= 0; sellPrice <= maxTotal when provided               |
| Usage    | CreateOrderAsync (order create path)                              |

---

## 3. Divergence

- **Legacy:** Partner/product pricing models and services; formula and fee may differ; may be used in catalog and/or order.
- **V2:** Single PricingCalculator; explicit FIXED_FEE; profit guard and maxTotal check in CreateOrderAsync.

---

## 4. Side-by-side comparison

| Item      | Legacy        | V2                                    |
|-----------|---------------|----------------------------------------|
| Service   | PartnerProductPricingService, etc. | PricingCalculator (IPricingCalculator) |
| Fee       | Implementation-specific | FIXED_FEE 0.10m                 |
| Profit guard | —         | netProfit >= 0; maxTotal check         |
| Discount  | Partner/product tables             | GetDiscountAsync(partnerId, vendorId)  |

---

## 5. Recommendations

1. Treat v2 formula and profit guard as target for order-create path.
2. Parity checklist: sellPrice = faceValue * (1 - discountPercent); totalCost = bambooCost + fee; netProfit >= 0; maxTotal when provided.
3. Test design: P1_02 (insufficient funds); profit guard covered in Order Creation / CreateOrderAsync tests.

---

## 6. Conclusion

- **Legacy** pricing is spread across partner/product pricing services and models.
- **V2** uses a single PricingCalculator with FIXED_FEE, discount per partner/vendor, and profit guard in CreateOrderAsync.
- V2 is the reference for pricing behavior in the order-create path; add to workflow index if further workflow docs are needed for pricing-only flows.
