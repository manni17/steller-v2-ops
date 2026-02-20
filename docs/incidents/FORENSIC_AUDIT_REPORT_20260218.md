# Steller Forensic Audit Report

**Date:** 2026-02-18  
**Agent:** TPM Stabilization Agent  
**Duration:** ~60 minutes  
**Protocol:** `docs/TPM_STABILIZATION_AUDIT_PROTOCOL.md`

---

## Phase 1: Core Reality Check

### 1.1 Schema Audit — **PASS**

| Table   | Exists | Key Columns                                                                 |
|---------|--------|-----------------------------------------------------------------------------|
| Wallets | ✅     | Id, PartnerId, AvailableBalance, CurrencyId, WalletNumber, WalletTypeId, CreatedAt, UpdatedAt |

- No `Version`/`RowVersion` for optimistic locking; balance updates use atomic SQL.
- `Wallets` table is present with balance and partner linkage.

### 1.2 Logic Audit — **PASS**

| Component      | Location                                                | Evidence                                                                 |
|----------------|---------------------------------------------------------|---------------------------------------------------------------------------|
| IWalletService | `Steller.Core/Interfaces/IWalletService.cs`             | `DebitWalletAsync`, `CreditWalletAsync` defined                           |
| WalletService  | `Steller.Infrastructure/Services/Wallet/WalletService.cs` | Implements debit/credit with atomic SQL                                  |
| OrderService   | `Steller.Infrastructure/Services/Orders/OrderService.cs` | Line 150: `_walletService.DebitWalletAsync(partnerId, pricingResult.SellPrice, ...)` |
| Refund path    | `PlaceBambooOrderJob`, `OrderRepository`                | `CreditWalletAsync` on failure/refund                                     |

**Flow:** `POST /api/orders` → OrderService.CreateOrderAsync → HasSufficientFundsAsync → Create Order → **DebitWalletAsync** → Commit → Enqueue PlaceBambooOrderJob. Balance is actually debited before job runs.

### 1.3 Inventory Check — **PASS**

| Port | Service              | Purpose                          | Steller? |
|------|----------------------|----------------------------------|----------|
| 3001 | OpenClaw Gateway     | LLM gateway (separate product)   | No       |
| 6091 | Steller v2 API       | Partner/Admin API                | Yes      |
| 6432 | Steller v2 PostgreSQL| steller_v2 DB                    | Yes      |
| 6379 | Steller v2 Redis     | Cache                            | Yes      |

---

## Phase 2: Infrastructure Interrogation

### 2.1 Restart Cause — **Summary**

- **API container:** Started 2026-02-18T20:47:31Z (~22 minutes before audit).
- **Logs (30m):** No crash/OOMKilled; found `Polly.Timeout.TimeoutRejectedException` for Bamboo GetCatalog (2 min timeout). Vendor timeout, not app crash.
- **Conclusion:** Restart likely due to deploy/health check or manual intervention, not crash. Bamboo catalog sync hits timeouts.

### 2.2 Port Identity

| Port | Service          | Response  | Purpose                               |
|------|------------------|-----------|----------------------------------------|
| 3001 | OpenClaw Gateway | 200 OK    | LLM gateway (HTML "OpenClaw Control")  |
| 6091 | Steller v2 API   | 200 OK    | Health, Partner API, Admin API         |

### 2.3 Container Health Snapshot

| Container                   | Status  | Uptime     |
|-----------------------------|---------|------------|
| steller-v2-api              | Healthy | ~22 min    |
| steller-v2-postgres         | Healthy | ~17 hours  |
| steller-v2-redis            | Healthy | ~17 hours  |
| steller-admin-dashboard     | Up      | 5 days     |
| steller-consumer-dashboard  | Up      | 5 days     |
| shared_postgres_db          | Up      | 5 days     |
| shared_rabbitmq             | Up      | 5 days     |
| source-openclaw-gateway-1   | Up      | 12 hours   |

---

## Phase 3: Smoke Test

### 3.1 Setup — **PARTIAL**

- **Partner 1:** Exists.
- **Wallet:** PartnerId 1, AvailableBalance 4825.00.
- **Products:** MOCK-ITUNES-25, MOCK-GOOGLE-50, MOCK-AMAZON-100 present.
- **API key:** Not obtained — admin login failed (invalid credentials). API keys stored as KeyHash; plain key only via Admin API.

### 3.2 Auth Verification — **PASS**

- `POST /api/orders` without `x-api-key`: **401** — "API Key missing."
- `GET /api/brand/getCatalog` without `x-api-key`: **401** — "API Key missing."

### 3.3 Full Order Flow — **BLOCKED**

- Cannot run full smoke test without valid API key.
- Code path traced: Order → Debit → Hangfire job → Bamboo → success/fail → optional refund.
- **Recommendation:** Provide pre-provisioned API key or valid admin credentials to complete smoke test.

---

## Verdict

| Item           | Result  |
|----------------|---------|
| **FINANCIAL CORE** | **REAL** |
| **Critical blockers** | None |
| **Schema**     | Wallets exists; balance/partner columns present |
| **Logic**      | Order flow debits wallet via `DebitWalletAsync` |
| **Refund path**| CreditWalletAsync on failure in PlaceBambooOrderJob and OrderRepository |

### Summary

- Golden Path (Money In → Product Out) is implemented.
- `POST /api/orders` checks balance, creates order, debits wallet, then enqueues Bamboo job.
- Refunds on failure use `CreditWalletAsync`.
- No `Version` column; WalletService uses atomic SQL for balance updates.

### Recommended Next Steps

1. **Smoke test:** Obtain API key via Admin API (or pre-provisioned key) and run full POST /orders → verify order row and wallet debit.
2. **Bamboo timeouts:** Review Bamboo GetCatalog timeout (2 min); consider retry or monitoring.
3. **Admin credentials:** Confirm admin login for API key generation (README: no seeded admin; create manually).
