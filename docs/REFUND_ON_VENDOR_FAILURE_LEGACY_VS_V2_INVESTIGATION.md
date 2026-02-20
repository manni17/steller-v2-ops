# Refund-on-Vendor-Failure: Legacy Steller vs Steller v2 — Investigation

**Goal:** Understand how legacy Steller handles vendor failure (refund or not) vs how v2 does it (MarkFailedAndRefundAsync, double-refund guard), for test design and parity checks.

---

## 1. How legacy Steller handles vendor failure

**Location:** `/opt/steller-apps/Steller/`

### Flow

1. **OrderService.AddOrder** — On PlaceOrder (POST checkout) failure: returns **ServiceResponse.Fail**; **no wallet refund** in AddOrder.
2. **OrderCreatedConsumer** — On AddOrder failure: calls **MarkOrderAsFailed(orderId, message)** so the order status is set to failed and the order is not re-queued; **no refund** in consumer.
3. **MarkOrderAsFailed** — Updates order: Status = "failed", ErrorMessage; **no CreditWallet** or refund logic.
4. **GetOrderById** — On failure returns failed result; **no refund** in GetOrderById.

### Summary (legacy)

| Aspect        | Legacy                                                                 |
|---------------|------------------------------------------------------------------------|
| Place failure | MarkOrderAsFailed (status only); no wallet refund                     |
| Get failure   | Return failure; no refund                                              |
| Double-refund guard | N/A (no refund path)                                            |

---

## 2. How Steller v2 handles vendor failure (refund)

**Location:** `/opt/steller-v2/steller-backend/`

### Flow

1. **PlaceBambooOrderJob** — On **PlaceOrderAsync** failure (vendor reject, 429, etc.): calls **MarkFailedAndRefundAsync(order.Id, reason)** (single transaction: order → Failed, wallet credited).
2. **PollBambooOrderJob** — On **details.Status == "Failed"**: **ProcessFailureAsync** → **MarkFailedAndRefundAsync(order.Id, "Vendor Failed")**; then enqueue **SendWebhookJob**.
3. **OrderRepository.MarkFailedAndRefundAsync(orderId, reason)**:
   - Load order; **double-refund guard:** if **OrderStatusId == Failed** or **order.IsRefunded**, skip and return (no second credit).
   - **SetStatus(Failed)**; ErrorMessage = reason; **SyncItemStatuses**.
   - **CreditWalletAsync(partnerId, order.SaleTotal, "RECON REFUND: {reason}", order.Id)** (uses **SaleTotal** — amount debited, not face value).
   - On success set **order.IsRefunded = true**; SaveChanges; commit.
4. **OrderService.RefundOrderAsync** — Used for partial fulfillment or admin; **CreditWalletAsync** with given amount; no status change to Failed in this method (order may stay Completed with partial refund).
5. **ReconciliationJob** — For stuck orders that are Failed/NotFound at vendor: calls **RefundOrderAsync** then **SetStatus(Failed)** and SaveChanges (separate from MarkFailedAndRefundAsync; same financial effect).

### Summary (v2)

| Aspect        | V2                                                                      |
|---------------|-------------------------------------------------------------------------|
| Place failure | MarkFailedAndRefundAsync (Failed + CreditWallet SaleTotal)              |
| Poll failure  | ProcessFailureAsync → MarkFailedAndRefundAsync                          |
| Double-refund guard | Skip if already Failed or IsRefunded                             |
| Amount        | SaleTotal (amount debited)                                              |
| Audit         | WalletHistory credit with reason and order Id reference                |

---

## 3. Root causes / divergence

### 3.1 Refund on failure

| Legacy | V2 |
|--------|----|
| No wallet refund on Place or Get failure | MarkFailedAndRefundAsync credits wallet (SaleTotal) on vendor failure |

V2 ensures financial reconciliation; legacy leaves wallet debited when vendor fails.

### 3.2 Double-refund guard

| Legacy | V2 |
|--------|----|
| N/A | OrderRepository checks Failed / IsRefunded before crediting; prevents duplicate refund |

V2 avoids double credit if MarkFailedAndRefundAsync (or similar) is called twice.

### 3.3 Amount

V2 uses **SaleTotal** (amount actually debited from partner), not Total (face value), for refund amount.

---

## 4. Side-by-side comparison

| Item              | Legacy                          | V2                                    |
|-------------------|---------------------------------|----------------------------------------|
| **Place failure** | MarkOrderAsFailed (status only) | MarkFailedAndRefundAsync (Failed + refund) |
| **Poll failure**  | Return failure                  | MarkFailedAndRefundAsync + webhook     |
| **Refund amount** | —                               | SaleTotal                              |
| **Guard**         | —                               | IsRefunded / Failed → skip              |

---

## 5. Recommendations

1. **Treat v2 as the target** — Refund-on-failure and double-refund guard are required for financial correctness.
2. **Parity checklist:**
   - [ ] Place failure → MarkFailedAndRefundAsync (order Failed, wallet credited SaleTotal)
   - [ ] Poll Failed status → MarkFailedAndRefundAsync
   - [ ] Double-refund guard (second call no-ops)
   - [ ] CreditWalletAsync with reason and order reference
3. **Test design:** P1_03 (vendor reject → refund), P1_04 (double-refund guard), P2_03 (Place failure → MarkFailedAndRefund), P2_05 (Poll Failed → MarkFailedAndRefund).

---

## 6. Conclusion

- **Legacy** does not refund wallet on vendor failure; only marks order as failed.
- **V2** refunds wallet (SaleTotal) on vendor failure via MarkFailedAndRefundAsync and guards against double refund.
- V2 is the reference for refund-on-vendor-failure behavior and test coverage.
