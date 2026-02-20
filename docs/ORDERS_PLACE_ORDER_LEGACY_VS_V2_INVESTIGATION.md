# Place Order (Bamboo): Legacy Steller vs Steller v2 — Investigation

**Goal:** Understand how legacy Steller places orders with Bamboo vs how v2 does it, and identify divergences for test design and parity checks.

---

## 1. How legacy Steller places orders with Bamboo

**Location:** `/opt/steller-apps/Steller/`

### Flow

1. **OrderCreatedConsumer** (MassTransit) consumes `OrderCreated` message with `OrderId` (RequestId).
2. Calls **OrderService.AddOrder(OrderFromDataBaseDto)** with `RequestId`, `AccountId` (hardcoded 1256).
3. **OrderService.AddOrder**:
   - Loads order from DB by RequestId.
   - Calls **GetOrderById** to check if order already exists in Bamboo (idempotency).
   - If not: **PostApiResponse** to `{_baseUrl}/orders/checkout` with `OrderDto` (RequestId, AccountId, Products).
   - **WaitForSlotAsync("bamboo_place_order")** before POST.
   - On 429: **ExtractRetrySeconds** + **SetRetryAfter("bamboo_place_order")**.
   - On failure: **MarkOrderAsFailed** (order status → failed; no wallet refund in this path).
4. **ExternalApiService.PostApiResponse** — Basic auth `ClientId:ClientSecret`, POST JSON.

### Configuration (legacy .env)

```env
EXTERNAL_API_BASE_URL_V1=https://api.bamboocardportal.com/api/integration/v1.0
EXTERNAL_API_CLIENT_ID=STELLER-TECHNOLOGY-FOR-COMMUNICATIONS-INFORMATION---CLIENT-SANDBOX
EXTERNAL_API_CLIENT_SECRET=...
```

### Payload (legacy OrderDto)

| Field    | Type | Notes                                  |
|----------|------|----------------------------------------|
| RequestId| Guid | Idempotency key                        |
| AccountId| int  | 1256 (hardcoded in OrderCreatedConsumer) |
| Products | array| Each: ProductId (int), Quantity, Value (decimal) |

### Summary (legacy)

| Aspect        | Legacy                                                                 |
|---------------|------------------------------------------------------------------------|
| Trigger       | MassTransit OrderCreatedConsumer → OrderService.AddOrder               |
| Endpoint      | `BaseUrlV1 + /orders/checkout` → v1.0/orders/checkout                  |
| Auth          | Basic ClientId:ClientSecret (ExternalApiService)                       |
| Rate limit    | WaitForSlotAsync("bamboo_place_order"); SetRetryAfter on 429           |
| Product ident | ProductId (int)                                                        |
| Value type    | decimal                                                                |
| On failure    | MarkOrderAsFailed; no wallet refund                                    |

---

## 2. How Steller v2 places orders with Bamboo

**Location:** `/opt/steller-v2/steller-backend/`

### Flow

1. **OrderService.CreateOrderAsync** creates order, debits wallet (atomic), enqueues **PlaceBambooOrderJob(order.Id)** via Hangfire.
2. **PlaceBambooOrderJob.ExecuteAsync(orderId)**:
   - Loads order with `GetByIdIgnoreFiltersAsync`.
   - **WaitForSlotAsync("bamboo_place_order")**.
   - Builds **VendorOrderRequest** (ReferenceId=order.RequestId, ProductCode=item.Product.Sku, Quantity, Amount, OrderId).
   - Calls **_vendorAdapter.PlaceOrderAsync(vendorRequest)**.
   - On success: enqueues **PollBambooOrderJob(order.Id)**.
   - On failure: **MarkFailedAndRefundAsync** (order → Failed, wallet refunded).
3. **BambooVendorAdapter.PlaceOrderAsync** → **BambooApiClient.PlaceOrderAsync(requestId, sku, quantity, value)**.
4. **BambooApiClient** POST ` /api/integration/v2.0/orders/checkout` with **BambooOrderRequest** (RequestId, AccountId int, Products: Sku, Quantity, Value int).
5. Auth: Basic Username:Password (`BAMBOO_USERNAME`, `BAMBOO_PASSWORD`).
6. On BambooApiException (including 429): returns Success=false; **SetRetryAfter("bamboo_place_order")** on 429.
7. **VendorApiCall** audit record persisted (OrderId, RequestId, Status, payloads).

### Configuration (v2 .env)

```env
BAMBOO_BASE_URL=https://api.bamboocardportal.com
BAMBOO_USERNAME=STELLER-...
BAMBOO_PASSWORD=...
BAMBOO_ACCOUNT_ID=0
BAMBOO_ACCOUNT_ID_INT=1256
```

### Payload (v2 BambooOrderRequest)

| Field    | Type | Notes                                      |
|----------|------|--------------------------------------------|
| RequestId| string | order.RequestId.ToString()               |
| AccountId| int  | BAMBOO_ACCOUNT_ID_INT or parsed BAMBOO_ACCOUNT_ID |
| Products | array| Each: Sku (string), Quantity (int), Value (int)  |
| Cache-Control | header | "public"                              |

### Summary (v2)

| Aspect        | V2                                                                      |
|---------------|-------------------------------------------------------------------------|
| Trigger       | Hangfire PlaceBambooOrderJob (enqueued after CreateOrderAsync)          |
| Endpoint      | POST `/api/integration/v2.0/orders/checkout`                            |
| Auth          | Basic Username:Password (BAMBOO_USERNAME, BAMBOO_PASSWORD)              |
| Rate limit    | WaitForSlotAsync("bamboo_place_order"); SetRetryAfter on 429            |
| Product ident | Sku (string)                                                            |
| Value type    | int (rounded)                                                           |
| On failure    | MarkFailedAndRefundAsync (order Failed, wallet refunded)                |
| Audit         | VendorApiCall persisted                                                 |

---

## 3. Root causes / divergence

### 3.1 API version and endpoint

| Legacy | V2 |
|--------|----|
| v1.0: `.../api/integration/v1.0/orders/checkout` | v2.0: `.../api/integration/v2.0/orders/checkout` |

Bamboo v1.0 and v2.0 may have different contracts (e.g. ProductId vs Sku). V2 uses the documented v2.0 PlaceOrder contract (Sku, Value int, Cache-Control).

### 3.2 Product identifier

| Legacy | V2 |
|--------|----|
| ProductId (int) — internal product ID | Sku (string) — product code (e.g. MOCK-ITUNES-25) |

Legacy maps OrderItem.ProductId; v2 maps item.Product.Sku. Bamboo v1.0 may accept ProductId; v2.0 expects Sku.

### 3.3 Auth

| Legacy | V2 |
|--------|----|
| Basic ClientId:ClientSecret (ExternalApiService) | Basic Username:Password (BambooApiClient) |

Different credential sets. Both may be valid for Bamboo depending on account setup.

### 3.4 Trigger mechanism

| Legacy | V2 |
|--------|----|
| MassTransit OrderCreatedConsumer (message queue) | Hangfire PlaceBambooOrderJob (background job) |

Different orchestration; same logical flow: create order → place with Bamboo.

### 3.5 Failure handling

| Legacy | V2 |
|--------|----|
| MarkOrderAsFailed; no wallet refund | MarkFailedAndRefundAsync; wallet refunded |

V2 adds financial reconciliation on vendor failure; legacy does not.

---

## 4. Side-by-side comparison

| Item              | Legacy                          | V2                                    |
|-------------------|---------------------------------|----------------------------------------|
| **Endpoint**      | v1.0/orders/checkout            | v2.0/orders/checkout                   |
| **Auth**          | ClientId:ClientSecret           | Username:Password                      |
| **Product ID**    | ProductId (int)                 | Sku (string)                           |
| **Value**         | decimal                         | int                                    |
| **Rate limit**    | bamboo_place_order              | bamboo_place_order                     |
| **429 handling**  | SetRetryAfter                   | SetRetryAfter                          |
| **Idempotency**   | Check GetOrderById first        | RequestId in payload                   |
| **On failure**    | MarkOrderAsFailed               | MarkFailedAndRefundAsync               |
| **Audit**         | None                            | VendorApiCall                          |

---

## 5. Recommendations

1. **Treat v2 as the target** — v2 uses Bamboo v2.0 PlaceOrder contract (Sku, Value int, Cache-Control). Legacy v1.0 may be deprecated or differ.
2. **Parity checklist for Place Order:**
   - [ ] Rate limit: bamboo_place_order, 2 req/sec
   - [ ] 429: ExtractRetrySeconds + SetRetryAfter
   - [ ] AccountId: int (BAMBOO_ACCOUNT_ID_INT)
   - [ ] Products: Sku, Quantity, Value (int)
   - [ ] On failure: MarkFailedAndRefundAsync (v2 behavior)
3. **Test design:** Cover PlaceBambooOrderJob success, vendor rejection, 429, and MarkFailedAndRefundAsync path (Phase 4 confirmed).
4. **Legacy migration:** If migrating legacy consumers, ensure they enqueue PlaceBambooOrderJob (or equivalent) instead of MassTransit OrderCreated.

---

## 6. Conclusion

- **Legacy** uses Bamboo v1.0 (ProductId, ClientId:ClientSecret), MassTransit, and does not refund wallet on PlaceOrder failure.
- **V2** uses Bamboo v2.0 (Sku, Username:Password), Hangfire, VendorApiCall audit, and refunds wallet on PlaceOrder failure.
- V2 aligns with the documented Bamboo PlaceOrder contract. Test coverage should focus on PlaceBambooOrderJob, rate limiting, 429 handling, and the refund-on-failure path.
