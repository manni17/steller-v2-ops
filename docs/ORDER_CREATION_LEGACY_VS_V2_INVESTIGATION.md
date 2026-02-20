# Order Creation (Steller): Legacy vs Steller v2 — Investigation

**Goal:** Understand how legacy Steller creates orders (API → DB → Bamboo) vs how v2 does it (POST /api/orders → CreateOrderAsync → debit → enqueue), for test design and parity checks.

---

## 1. How legacy Steller creates and places orders

**Location:** `/opt/steller-apps/Steller/`

### Flow

1. **Order record creation** — Some flow (e.g. partner API or admin) creates an **Order** in the DB with **RequestId**, **OrderItems**, etc., and publishes **OrderCreated** (MassTransit) with OrderId = RequestId.
2. **OrderCreatedConsumer** receives **OrderCreated**; builds **OrderFromDataBaseDto** (RequestId, AccountId 1256); calls **OrderService.AddOrder(orderFromDataBaseDto)**.
3. **AddOrder**:
   - Loads order from DB by RequestId (no debit here; wallet may be debited elsewhere before message publish).
   - **GetOrderById** to check if order already exists in Bamboo (idempotency).
   - If not: **WaitForSlotAsync("bamboo_place_order")**; POST checkout with OrderDto; on 429 SetRetryAfter.
   - On success: **GetOrderById** again to fetch and update DB.
   - On failure: returns failure; consumer may call **MarkOrderAsFailed** (no wallet refund in AddOrder).
4. No single "create order + debit + place" transaction — order creation and wallet debit are separate from AddOrder.

### Summary (legacy)

| Aspect        | Legacy                                                                 |
|---------------|------------------------------------------------------------------------|
| Trigger       | Order already in DB; MassTransit OrderCreated → AddOrder               |
| Debit         | Not in AddOrder; assumed done before OrderCreated publish              |
| Idempotency   | GetOrderById before Place; if exists in Bamboo, return existing         |
| Place         | POST v1.0/orders/checkout (see Place Order doc)                        |

---

## 2. How Steller v2 creates orders

**Location:** `/opt/steller-v2/steller-backend/`

### Flow

1. **POST /api/orders** (OrdersController) with **CreateOrderRequest** (Sku, FaceValue, Quantity, ReferenceId); PartnerId from API key.
2. **OrderService.CreateOrderAsync(request, partnerId)** — single transaction:
   - **Idempotency:** If ReferenceId provided and order with that RequestId exists, return existing OrderDto.
   - **Product:** Load by Sku; **PricingCalculator.CalculatePriceAsync** (profit guard).
   - **Balance:** **HasSufficientFundsAsync**; if false return failure.
   - **Create Order** entity; add OrderItem; **SaveChangesAsync** (unique RequestId; on duplicate key return existing).
   - **Debit:** **DebitWalletAsync(partnerId, sellPrice, description, order.Id)**.
   - **Commit** transaction; **Enqueue PlaceBambooOrderJob(order.Id)**.
   - Return **OrderDto**.
3. **PlaceBambooOrderJob** runs asynchronously; on success enqueues **PollBambooOrderJob**.

### Summary (v2)

| Aspect        | V2                                                                      |
|---------------|-------------------------------------------------------------------------|
| Trigger       | POST /api/orders → CreateOrderAsync                                    |
| Debit         | In same transaction as order create; atomic                            |
| Idempotency   | By RequestId (ReferenceId); duplicate returns existing, no second debit|
| Place         | Enqueued PlaceBambooOrderJob after commit                              |

---

## 3. Root causes / divergence

- **Legacy:** Order create and debit separate from "place at Bamboo"; AddOrder only places.
- **V2:** Create + debit + enqueue in one API; idempotency by ReferenceId; atomic create+debit.

---

## 4. Side-by-side comparison

| Item              | Legacy                          | V2                                    |
|-------------------|---------------------------------|----------------------------------------|
| **Entry**         | Order in DB + OrderCreated msg  | POST /api/orders                       |
| **Debit**         | Separate (before message)       | In CreateOrderAsync transaction       |
| **Idempotency**   | Bamboo GetOrderById             | RequestId (ReferenceId)                |
| **Place**         | AddOrder POST checkout          | PlaceBambooOrderJob                    |

---

## 5. Recommendations

1. Treat v2 as the target — single API for create + debit + enqueue.
2. Parity checklist: Idempotency by ReferenceId; debit inside transaction; enqueue after commit; insufficient funds fail before create; profit guard.
3. Test design: P1_01, P1_02, P1_05, P1_06.

---

## 6. Conclusion

- **Legacy** separates order creation and debit from place at Bamboo.
- **V2** provides single Order Creation API: create order + debit wallet atomically, then enqueue PlaceBambooOrderJob; idempotency by ReferenceId.
- V2 is the reference for order creation and test coverage.
