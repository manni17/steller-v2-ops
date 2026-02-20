# QA Agent Internal Status

**Owner:** QA Agent  
**Read-only for:** TPM (this doc is QA internal; shared doc for TPM handoff is `QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md`)

**Rule:** QA does NOT edit `docs/BACKLOG_V2.md` — TPM owns it.

---

## Dev / environment reference (QA use)

| Topic | Location | Notes |
|-------|----------|--------|
| **Critical 4 + test run** | `steller-backend/CONTRIBUTING.md` | Full protocol; TEST_DB_CONNECTION options (stack vs standalone). |
| **Database migrations** | `steller-backend/RUN_MIGRATIONS.md` | Persistent protocol (when/how). Quick command: `DB_HOST_MIGRATIONS=localhost dotnet ef database update --project Steller.EF --startup-project Steller.Api`. |
| **When to run migrations** | Same | After new EF migrations; if integration tests fail with schema/table errors, ensure migrations have been applied to the test DB (or use EnsureCreated in test factory). |

Integration tests use `TEST_DB_CONNECTION` and `CustomWebApplicationFactory` (EnsureCreated/Reset); for a **fresh or schema-changed** stack, Dev may have run migrations first — see README, QUICK_START, PRODUCTION_DEPLOYMENT for full flow.

---

## Tier 1 Stabilization (QA P1 + P2)

| Item | Status | Completed | Notes |
|------|--------|-----------|-------|
| **QA P1 – Financial (7 tests)** | Done | 2026-02-19 | P1_01–P1_06 + T_01–T_03, Order_Creation. OrderServiceTests. |
| **QA P2 – Order flow (8 tests)** | Done | 2026-02-19 | P2_01–P2_08: PlaceBambooOrderJob, PollBambooOrderJob, E2E. |
| **Gate** | Passed | 2026-02-19 | P1 and P2 both pass → Tier 1 gate met. TPM updates BACKLOG_V2. |

---

## Test Inventory (OrderServiceTests)

| Test | Description |
|------|-------------|
| P1_01 | CreateOrder debits wallet atomically |
| P1_02 | Insufficient funds rejects |
| P1_03 | Vendor reject → PlaceBambooOrderJob → order Failed, wallet refunded |
| P1_04 | MarkFailedAndRefund double-refund guard |
| P1_05 | Idempotency (duplicate referenceId returns same order) |
| P1_06 | Concurrent duplicate single debit |
| P2_01 | PlaceBambooOrderJob order-not-found skips |
| P2_02 | PlaceBambooOrderJob success enqueues Poll |
| P2_03 | PlaceBambooOrderJob failure → MarkFailedAndRefund |
| P2_04 | PollBambooOrderJob Succeeded → saves cards, marks completed |
| P2_05 | PollBambooOrderJob Failed → MarkFailedAndRefund |
| P2_06 | PollBambooOrderJob Pending → throws for retry |
| P2_07 | PollBambooOrderJob partial fulfillment → refunds |
| P2_08 | E2E happy path (Create → Place → Poll → Completed) |

---

## Run Command

```bash
cd /opt/steller-v2/steller-backend
export TEST_DB_CONNECTION="Host=localhost;Port=6432;Database=StellerTestDB;Username=steller_v2_user;Password=$(grep DB_PASSWORD ../.env | cut -d= -f2);Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj --filter "FullyQualifiedName~OrderServiceTests" -c Release
```

---

## Tier 2 QA (P3, P4, P5)

| Item | Status | Notes |
|------|--------|-------|
| **QA P3 – Bamboo integration (6 tests)** | ✅ 6/6 passed | Rate limit WaitForSlot (Place/Poll), 429 SetRetryAfter (Place/GetOrderDetails), VendorApiCall RequestPayload, OrderId. ~1 m 48 s with P4+P5. |
| **QA P4 – Operational jobs (6 tests)** | ✅ 6/6 passed | OrderQueueService (24h / date range / pending), ReconciliationJob (stuck orders, refund), WalletConsistencyJob (drift + alert). Assertions: queuedCount matches enqueued job count; when queuedCount ≥ 1, specific order IDs asserted. |
| **QA P5 – Auth (6 tests)** | ✅ 6/6 passed | ApiKey missing→401, invalid→403, valid→pass, catalog/orders require key, admin 401 or 404. |

**Test inventory (P3):** P3_01 PlaceBambooOrderJob WaitForSlotAsync, P3_02 PollBambooOrderJob WaitForSlotAsync, P3_03 429 PlaceOrder SetRetryAfter, P3_04 429 GetOrderDetails SetRetryAfter, P3_05 VendorApiCall RequestPayload, P3_06 VendorApiCall OrderId.  
**Test inventory (P4):** P4_01–P4_03 OrderQueueService (QueueOldOrdersFromLast24Hours, QueueOrdersByDateRange, QueuePendingOrders), P4_04–P4_05 ReconciliationJob (stuck orders, refund), P4_06 WalletConsistencyJob (drift + alert).  
**Test inventory (P5):** P5_01–P5_06 ApiKey middleware (missing/invalid/valid), catalog/orders require key, admin bypass (401 or 404).

**Run P3+P4+P5:**
```bash
cd /opt/steller-v2/steller-backend
export TEST_DB_CONNECTION="Host=localhost;Port=6432;Database=StellerTestDB;Username=steller_v2_user;Password=$(grep DB_PASSWORD ../.env | cut -d= -f2);Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj --filter "FullyQualifiedName~BambooIntegrationTests|FullyQualifiedName~OperationalJobsTests|FullyQualifiedName~AuthTests" -c Release
```

**Tier 2 gate:** 18/18 P3+P4+P5 passed (Duration ~1 m 48 s).

---

## Tier 3 / Backlog QA (Item 15, B2, B4)

| Item | Status | Notes |
|------|--------|-------|
| **15 – QA workflow docs (Tier 3)** | Done | Poll Order, Order Creation, Refund-on-Vendor-Failure, Backfill, Wallet Debit/Credit, Partner Auth, Catalog Retrieval. Index: `docs/QA_WORKFLOW_DOCUMENTATION_INDEX.md`. |
| **B2 – QA workflow docs (full set)** | Done | 8 workflows (Place Order was done; 4–10 completed). All rows in index marked Done. |
| **B4 – PRICING_LEGACY_VS_V2 investigation** | Done | `docs/PRICING_LEGACY_VS_V2_INVESTIGATION.md`; added to workflow index as #11. |

---

## Next (QA)

- No unblocked QA-only items; TPM owns BACKLOG_V2 status updates.

Per `docs/BACKLOG_V2.md` (TPM-owned).
