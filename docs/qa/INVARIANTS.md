# Steller v2 — Formal Invariants

**Purpose:** Single spec for financial and wallet invariants. Each invariant is linked to the tests that enforce it.  
**Reference:** [SPEC_INDEX.yaml](SPEC_INDEX.yaml), [USER_FLOW_INTEGRATION_TEST_PLAN.md](USER_FLOW_INTEGRATION_TEST_PLAN.md), OrderServiceTests, WalletServiceTests.  
**Last Updated:** 2026-02-20

---

## 1. Wallet Invariants

### INV-WALLET-BALANCE

**Statement:** At any point in time, for a given wallet:

`AvailableBalance = initial_balance + sum(credits) - sum(debits)`

Credits and debits are immutable ledger entries. No partial credits or debits.

| Enforcing tests |
|-----------------|
| OrderServiceTests.P1_01_CreateOrder_DebitsWalletAtomically |
| OrderServiceTests.P1_03_VendorReject_OrderFailedAndWalletRefunded |
| OrderServiceTests.P2_08_E2E_HappyPath_OrderCompletedWithCards |
| WalletServiceTests.T_04_Concurrency_Test |
| UserFlowIntegrationTests.UF_P1_Partner_CreateOrder_ReceivesGiftCard |
| UserFlowIntegrationTests.UF_P2_Partner_WalletDeductionAfterOrder |
| UserFlowIntegrationTests.UF_A1_Admin_Login_CreditPartnerWallet |
| UserFlowIntegrationTests.UF_A2_Admin_CancelOrder_PartnerRefunded |

---

### INV-DEBIT-ATOMICITY

**Statement:** A debit is atomic. Either the full debit succeeds and is persisted, or none of it is applied. No partial debits on insufficient funds; no orphan debits without corresponding order state.

| Enforcing tests |
|-----------------|
| OrderServiceTests.P1_01_CreateOrder_DebitsWalletAtomically |
| OrderServiceTests.T_01_Atomic_Rollback_Test |
| OrderServiceTests.P2_08_E2E_HappyPath_OrderCompletedWithCards |
| WalletServiceTests.T_04_Concurrency_Test |
| UserFlowIntegrationTests.UF_P1_Partner_CreateOrder_ReceivesGiftCard |
| UserFlowIntegrationTests.UF_P2_Partner_WalletDeductionAfterOrder |

---

### INV-CREDIT-ATOMICITY

**Statement:** A credit is atomic. Either the full credit succeeds and is persisted, or none of it is applied.

| Enforcing tests |
|-----------------|
| WalletServiceTests.T_04_Concurrency_Test |
| UserFlowIntegrationTests.UF_A1_Admin_Login_CreditPartnerWallet |

---

## 2. Refund Invariants

### INV-REFUND-ATOMICITY

**Statement:** On order failure (vendor reject, timeout), the refund is atomic. The full sale amount is credited back; no partial refund.

| Enforcing tests |
|-----------------|
| OrderServiceTests.P1_03_VendorReject_OrderFailedAndWalletRefunded |
| OrderServiceTests.P2_03_PlaceBambooOrderJob_Failure_MarkFailedAndRefund |
| OrderServiceTests.P2_05_PollBambooOrderJob_Failed_MarkFailedAndRefund |
| OrderServiceTests.P2_07_PollBambooOrderJob_PartialFulfillment_Refunds |
| UserFlowIntegrationTests.UF_A2_Admin_CancelOrder_PartnerRefunded |

---

### INV-NO-DOUBLE-REFUND

**Statement:** A given order cannot be refunded more than once. Second refund attempt is a no-op (guard).

| Enforcing tests |
|-----------------|
| OrderServiceTests.P1_04_MarkFailedAndRefund_DoubleRefundGuard |

---

## 3. Order Invariants

### INV-IDEMPOTENCY

**Statement:** For a given `referenceId` (stored as `RequestId`), duplicate order creation requests return the same order and perform a single debit.

| Enforcing tests |
|-----------------|
| OrderServiceTests.P1_05_Idempotency_DuplicateReferenceId_ReturnsSameOrder |
| OrderServiceTests.P1_06_ConcurrentDuplicate_SingleDebit |
| OrderServiceTests.T_03_Idempotency_Test |
| UserFlowIntegrationTests.UF_P3_Partner_Idempotency_NoDoubleCharge |

---

### INV-NO-PARTIAL-DEBIT

**Statement:** If the wallet has insufficient balance, no debit occurs. The order is rejected; wallet balance is unchanged.

| Enforcing tests |
|-----------------|
| OrderServiceTests.P1_02_InsufficientFunds_Rejects |
| OrderServiceTests.P1_06_ConcurrentDuplicate_SingleDebit |
| OrderServiceTests.T_01_Atomic_Rollback_Test |
| UserFlowIntegrationTests.UF_P5_Partner_InsufficientFunds_OrderRejected |

---

## 4. Business Invariants

### INV-PROFIT-GUARD

**Statement:** Order creation rejects if the calculated sale total would result in negative margin (selling below cost). Profit guard is enforced before debit.

| Enforcing tests |
|-----------------|
| OrderServiceTests.T_02_Profit_Guard_Test |

---

## 5. Spec-Driven Workflow

**Before new feature:**

1. Add or update spec (flow in USER_FLOW_INTEGRATION_TEST_PLAN; invariants here).
2. Add or update tests that match spec IDs.
3. Update [SPEC_INDEX.yaml](SPEC_INDEX.yaml) with traceability.

**Reference:** [Spec-Driven Deterministic Steller Dev plan](../../.cursor/plans/spec-driven_deterministic_steller_dev_3277b8d0.plan.md).
