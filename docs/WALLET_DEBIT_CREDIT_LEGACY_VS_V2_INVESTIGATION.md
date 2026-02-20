# Wallet Debit/Credit: Legacy Steller vs Steller v2

**Goal:** Compare legacy vs v2 wallet debit/credit for test design and parity.

## 1. Legacy

Debit is not in AddOrder; assumed before OrderCreated. No refund-on-vendor-failure (see Refund doc). Atomicity implementation-dependent.

## 2. V2

**Debit:** In CreateOrderAsync transaction; DebitWalletAsync (atomic SQL + WalletHistory). **Credit:** MarkFailedAndRefundAsync, RefundOrderAsync; CreditWalletAsync; double-refund guard (IsRefunded/Failed). **Consistency:** WalletConsistencyJob (AvailableBalance vs ledger).

## 3. Divergence

Legacy: debit/credit separate from place; no refund-on-failure. V2: debit with order create; credit with guard; consistency job.

## 4. Comparison

| Item       | Legacy   | V2                           |
|------------|----------|------------------------------|
| Debit      | Separate | In CreateOrderAsync tx       |
| Credit     | None on failure | MarkFailedAndRefundAsync, RefundOrderAsync |
| Guard      | —        | IsRefunded / Failed          |
| Consistency| —        | WalletConsistencyJob         |

## 5. Recommendations

Treat v2 as target. Tests: P1_01, P1_04, P4_06.

## 6. Conclusion

V2 is the reference for wallet behavior.
