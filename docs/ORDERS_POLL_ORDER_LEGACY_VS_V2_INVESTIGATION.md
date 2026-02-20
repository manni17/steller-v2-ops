# Poll Order / Get Order Details (Bamboo): Legacy Steller vs Steller v2 — Investigation

**Goal:** Understand how legacy Steller gets order details from Bamboo (poll/status) vs how v2 does it, and identify divergences for test design and parity checks.

---

## 1. How legacy Steller gets order details from Bamboo

**Location:** `/opt/steller-apps/Steller/`

### Flow

1. **OrderService.AddOrder** (after placing order): calls **GetOrderById(orderFromDataBaseDto.RequestId.ToString())** once to fetch and update DB with full order details.
2. **GetOrderById(string orderId)**:
   - **WaitForSlotAsync("bamboo_get_order")** before calling Bamboo.
   - GET `{_baseUrl}/orders/{orderId}` via **ExternalApiService.GetApiResponse&lt;OrderBambo&gt;** (Basic ClientId:ClientSecret).
   - On success: **UpdateOrder(orderId, result.Data)** — updates local order (OrderId, Status, CreateDate, Total, ErrorMessage, OrderType, Currency, OrderItems).
   - On 429: **ExtractRetrySeconds** + **SetRetryAfter("bamboo_get_order")**.
   - Returns ServiceResponse&lt;OrderBambo&gt; (no retry loop; caller gets result once).
3. **UpdateOrder** — persists Bamboo response to DB (order + order items); no gift-card persistence in this path (legacy may store cards elsewhere or in Items).

### Configuration (legacy .env)

Same as Place Order: `EXTERNAL_API_BASE_URL_V1`, `EXTERNAL_API_CLIENT_ID`, `EXTERNAL_API_CLIENT_SECRET`.

### Summary (legacy)

| Aspect        | Legacy                                                                 |
|---------------|------------------------------------------------------------------------|
| Trigger       | Single call from AddOrder immediately after PlaceOrder                 |
| Endpoint      | GET `BaseUrlV1 + /orders/{orderId}` → v1.0/orders/{requestId}          |
| Auth          | Basic ClientId:ClientSecret (ExternalApiService)                       |
| Rate limit    | WaitForSlotAsync("bamboo_get_order"); SetRetryAfter on 429            |
| Retry         | No job retry; one GET per PlaceOrder                                   |
| On success    | UpdateOrder (DB status, items); no dedicated gift-card table path     |
| On failure    | Returns failed result; no wallet refund in GetOrderById                |

---

## 2. How Steller v2 gets order details (Poll)

**Location:** `/opt/steller-v2/steller-backend/`

### Flow

1. **PlaceBambooOrderJob** on success enqueues **PollBambooOrderJob(order.Id)** (Hangfire).
2. **PollBambooOrderJob.ExecuteAsync(orderId)**:
   - Loads order with **_orderRepo.GetByIdAsync(orderId)**.
   - **WaitForSlotAsync("bamboo_get_order")**.
   - **GetOrderDetailsAsync(order.RequestId.ToString())** via **IVendorAdapter** (BambooVendorAdapter).
   - **BambooVendorAdapter.GetOrderDetailsAsync**: calls **BambooApiClient.GetOrderAsync(referenceId)**; maps response to **VendorOrderDetails** (ReferenceId, Status, Cards); on 429 **SetRetryAfter("bamboo_get_order")**; persists **VendorApiCall** audit.
   - **BambooApiClient.GetOrderAsync**: GET `/api/integration/v1.0/orders/{requestId}`, Basic Username:Password; returns **BambooOrderDetails** (RequestId, Status, Items with Cards).
3. **Status handling:**
   - **Succeeded:** **ProcessSuccessAsync** — save **GiftCard** entities, **MarkCompletedAsync**, enqueue **SendGiftCardEmailJob** and **SendWebhookJob**; partial fulfillment → **RefundOrderAsync** for difference.
   - **Failed:** **ProcessFailureAsync** — **MarkFailedAndRefundAsync**, enqueue **SendWebhookJob**.
   - **Pending/other:** throw so Hangfire retries later.
4. **VendorApiCall** audit record persisted for each GetOrderDetails call (RequestId, Status, payloads).

### Configuration (v2 .env)

Same as Place Order: `BAMBOO_BASE_URL`, `BAMBOO_USERNAME`, `BAMBOO_PASSWORD`.

### Summary (v2)

| Aspect        | V2                                                                      |
|---------------|-------------------------------------------------------------------------|
| Trigger       | Hangfire PollBambooOrderJob (enqueued after PlaceBambooOrderJob success)|
| Endpoint      | GET `/api/integration/v1.0/orders/{requestId}`                          |
| Auth          | Basic Username:Password (BambooApiClient)                               |
| Rate limit    | WaitForSlotAsync("bamboo_get_order"); SetRetryAfter on 429             |
| Retry         | Job throws on Pending → Hangfire retries                                |
| On success    | SaveGiftCardsAsync, MarkCompletedAsync, email, webhook; partial refund   |
| On failure    | MarkFailedAndRefundAsync, webhook                                       |
| Audit         | VendorApiCall persisted                                                 |

---

## 3. Root causes / divergence

### 3.1 Orchestration

| Legacy | V2 |
|--------|----|
| Single GET immediately after Place (no separate poll job) | Dedicated PollBambooOrderJob; retries until Succeeded/Failed |

Legacy does not poll repeatedly; v2 uses a job that can retry on Pending.

### 3.2 Auth

| Legacy | V2 |
|--------|----|
| Basic ClientId:ClientSecret (ExternalApiService) | Basic Username:Password (BambooApiClient) |

Same as Place Order; credential sets may differ by environment.

### 3.3 Endpoint

Both use GET orders by request ID. Legacy URL: `_baseUrl/orders/{orderId}` (v1.0). V2: `/api/integration/v1.0/orders/{requestId}`. Same logical endpoint.

### 3.4 Success path

| Legacy | V2 |
|--------|----|
| UpdateOrder (status, items, totals); no explicit gift-card table | SaveGiftCardsAsync (GiftCard entity), MarkCompletedAsync, email, webhook, partial-fulfillment refund |

V2 has explicit gift-card persistence and completion workflow; legacy updates order/items only.

### 3.5 Failure path

| Legacy | V2 |
|--------|----|
| GetOrderById returns failure; no wallet refund in this method | ProcessFailureAsync → MarkFailedAndRefundAsync (wallet refund) + webhook |

V2 reconciles wallet on vendor-reported failure; legacy does not refund in GetOrderById.

---

## 4. Side-by-side comparison

| Item              | Legacy                          | V2                                    |
|-------------------|---------------------------------|----------------------------------------|
| **Endpoint**      | GET v1.0/orders/{orderId}       | GET v1.0/orders/{requestId}            |
| **Auth**          | ClientId:ClientSecret           | Username:Password                      |
| **Rate limit**    | bamboo_get_order                | bamboo_get_order                       |
| **429 handling**  | SetRetryAfter                   | SetRetryAfter                          |
| **When called**   | Once after Place               | Job after Place; retries on Pending    |
| **On success**    | UpdateOrder (status/items)      | GiftCard save, MarkCompleted, email, webhook, partial refund |
| **On failure**    | Return failure                  | MarkFailedAndRefundAsync, webhook      |
| **Audit**         | None                            | VendorApiCall                          |

---

## 5. Recommendations

1. **Treat v2 as the target** — PollBambooOrderJob, GetOrderDetailsAsync, rate limit, 429, and refund-on-failure align with Phase 4 and P2 tests.
2. **Parity checklist for Poll Order:**
   - [ ] Rate limit: bamboo_get_order
   - [ ] 429: ExtractRetrySeconds + SetRetryAfter
   - [ ] GET v1.0/orders/{requestId}
   - [ ] Succeeded → SaveGiftCardsAsync, MarkCompletedAsync, partial fulfillment refund
   - [ ] Failed → MarkFailedAndRefundAsync
   - [ ] Pending → throw for Hangfire retry
3. **Test design:** P2_04–P2_08 cover PollBambooOrderJob Succeeded/Failed/Pending/partial and E2E; P3_02, P3_04 cover WaitForSlotAsync and 429 SetRetryAfter for GetOrderDetails.

---

## 6. Conclusion

- **Legacy** calls GetOrderById once after Place, uses ClientId:ClientSecret, updates order/items only, no wallet refund in this path.
- **V2** uses a dedicated PollBambooOrderJob with retries, Username:Password, VendorApiCall audit, gift-card persistence, MarkCompleted/MarkFailedAndRefund, and partial-fulfillment refund.
- V2 is the reference for poll/order-details behavior and test coverage.
