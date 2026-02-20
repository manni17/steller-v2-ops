# QA Workflow Documentation Index

**Owner:** QA Agent  

**Purpose:** Expand QA by documenting all workflows in the same Legacy vs V2 format as `docs/CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md`.

**Scope (QA Agent owns):**
- This index and all workflow investigation docs
- Create and maintain each workflow doc (Legacy vs V2 format)
- Keep status updated in the index as docs are completed

**Why QA owns this:** The index is a QA backlog for test design, regression, and parity checks; aligns with Phase 4; Legacy vs V2 format supports understanding and verifying behavior; guides test design and coverage decisions.

**Handoffs:**
| Role | Responsibility |
|------|----------------|
| QA Agent | Own index, write each investigation doc, update status |
| Dev Agent | Provide Legacy code context and technical details when needed |
| TPM | Set priorities, track completion |

**Reference:** `docs/CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md` — template structure.

---

**Template structure (per workflow):**
1. Goal
2. How Legacy does it (flow, configuration, summary table)
3. How V2 does it (flow, configuration, summary table)
4. Root causes / divergence
5. Side-by-side comparison
6. Recommendations
7. Conclusion
8. (Optional) Lessons learned / AI failure analysis

---

## Workflows to Document

| # | Workflow | Doc | Status | Notes |
|---|----------|-----|--------|-------|
| 1 | Catalog Sync | `CATALOG_SYNC_LEGACY_VS_V2_INVESTIGATION.md` | Done | Reference template |
| 2 | Categories | Same as catalog or separate | Done | Covered in catalog doc |
| 3 | Place Order (Bamboo) | `ORDERS_PLACE_ORDER_LEGACY_VS_V2_INVESTIGATION.md` | Done | PlaceBambooOrderJob, BambooVendorAdapter, rate limit, 429 |
| 4 | Poll Order / Get Order Details (Bamboo) | `ORDERS_POLL_ORDER_LEGACY_VS_V2_INVESTIGATION.md` | Done | PollBambooOrderJob, GetOrderDetailsAsync |
| 5 | Order Creation (Steller) | `ORDER_CREATION_LEGACY_VS_V2_INVESTIGATION.md` | Done | POST /api/orders → CreateOrderAsync → DebitWallet → Enqueue |
| 6 | Refund-on-Vendor-Failure | `REFUND_ON_VENDOR_FAILURE_LEGACY_VS_V2_INVESTIGATION.md` | Done | MarkFailedAndRefundAsync, CreditWalletAsync |
| 7 | Backfill (Order Queue) | `BACKFILL_ORDER_QUEUE_LEGACY_VS_V2_INVESTIGATION.md` | Done | OrderQueueService, QueueOldOrdersFromLast24Hours, QueueOrdersByDateRange, QueuePendingOrders |
| 8 | Wallet Debit/Credit | `WALLET_DEBIT_CREDIT_LEGACY_VS_V2_INVESTIGATION.md` | Done | DebitWalletAsync, CreditWalletAsync (if Legacy differs) |
| 9 | Partner Auth / API Key | `PARTNER_AUTH_LEGACY_VS_V2_INVESTIGATION.md` | Done | x-api-key, Admin key creation, ApiKeyMiddleware |
| 10 | Catalog Retrieval (Partner) | `CATALOG_RETRIEVAL_LEGACY_VS_V2_INVESTIGATION.md` | Done | GET /api/brand/getCatalog, partner filtering |
| 11 | Pricing (B4) | `PRICING_LEGACY_VS_V2_INVESTIGATION.md` | Done | PricingCalculator, FIXED_FEE, profit guard, CreateOrderAsync |

---

## Priority Order (suggested)

1. **Place Order** — Core vendor path; rate limit, 429, Bamboo parity
2. **Poll Order** — Status updates, Bamboo parity
3. **Order Creation** — Primary API path, idempotency, debit
4. **Refund-on-Vendor-Failure** — Financial reconciliation (recent fix)
5. **Backfill** — Secondary path; Order.Id vs RequestId (recent fix)
6. **Wallet Debit/Credit** — If Legacy has different behavior
7. **Partner Auth** — Security, key validation
8. **Catalog Retrieval** — Partner-facing read path

---

## User-Flow Integration Tests

Multi-step API journey tests from the user perspective (partner and admin). See **`docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md`** for the full plan, flow specs, and phased rollout.

- Partner flows: Create order → receive gift card (serial/PIN), wallet deduction, idempotency, insufficient funds, catalog→order
- Admin flows: Login → credit wallet, cancel order → refund, create API key

---

## Related

- **Running API tests:** `docs/RUNNING_API_TESTS.md` — How QA and AI agents run API tests; where credentials come from (DevCredentialsSeeder, `.dev-credentials.json`); example curl commands.

---

## Notes

- Start with **Place Order** and **Poll Order** — they mirror the catalog investigation (Bamboo URLs, auth, rate limit, 429).
- **Order Creation**, **Refund-on-Vendor-Failure**, **Backfill** are Steller-internal; Legacy comparison may be simpler or N/A for some.
- Each doc should be self-contained; cross-reference others where needed.
