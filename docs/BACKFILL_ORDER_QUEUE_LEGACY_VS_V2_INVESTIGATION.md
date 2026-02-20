# Backfill Order Queue: Legacy vs v2

**Goal:** Compare legacy vs v2 backfill of orders for test design.

## Legacy

No dedicated OrderQueueService. Placement via OrderCreated message only.

## V2

OrderQueueService: QueueOldOrdersFromLast24Hours, QueueOrdersByDateRange, QueuePendingOrders. Criteria: OrderId eq 0, CreateDate/Status. Enqueues ProcessOrderAsync(order.Id) then PlaceBambooOrderJob. Uses order.Id (PK).

## Comparison

Legacy: message-driven. V2: backfill API; job arg order.Id. Tests: P4_01 to P4_03.

## Conclusion

V2 is the reference for backfill.
