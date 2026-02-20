# User-Flow Integration Test Report

**Date:** 2026-02-20  
**Plan:** `docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md`  
**Backlog:** B7 (docs/BACKLOG_V2.md)

---

## 1. Summary

| Item | Status |
|------|--------|
| Backlog entry (B7) | ✅ Added |
| Test plan document | ✅ Exists |
| Implementation | ✅ Complete (8 tests) |
| Build | ✅ Succeeded |
| Test execution | ❌ Failed (environment: DB auth) |

---

## 2. What Was Done

### 2.1 Backlog

- **B7** added to §7 Backlog: *User-flow integration tests — Multi-step API journeys (Partner + Admin). Plan: docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md. UF-P1–P5, UF-A1–A3.*
- References section updated with link to user-flow plan.

### 2.2 Implementation

- **`Tests.Integration/UserFlowIntegrationTests.cs`** — 8 tests:
  - **UF-P1** `UF_P1_Partner_CreateOrder_ReceivesGiftCard` — POST order → run jobs → GET order (Completed, cards with serial/PIN) → GET wallet (balance decreased).
  - **UF-P2** `UF_P2_Partner_WalletDeductionAfterOrder` — POST order → run jobs → GET wallet + GET transactions (debit present).
  - **UF-P3** `UF_P3_Partner_Idempotency_NoDoubleCharge` — POST same referenceId twice → same orderId, wallet charged once.
  - **UF-P4** `UF_P4_Partner_CatalogToOrder_FullJourney` — GET catalog → POST order → run jobs → GET order (Completed, cards).
  - **UF-P5** `UF_P5_Partner_InsufficientFunds_OrderRejected` — POST order with low balance → 400, wallet unchanged.
  - **UF-A1** `UF_A1_Admin_Login_CreditPartnerWallet` — Admin login → GET wallet → POST credit → GET wallet (balance increased).
  - **UF-A2** `UF_A2_Admin_CancelOrder_PartnerRefunded` — Create order → Admin cancel → Partner GET wallet (refunded).
  - **UF-A3** `UF_A3_Admin_CreateApiKey_PartnerCanUse` — Admin POST keys → Partner GET catalog with new key.

- **`CustomWebApplicationFactory`** — Admin user seeding: after reset/init, an Admin user is created (`admin@steller.com` / `Admin123!`) so admin flows can log in. Uses `Steller.Core.Entities.User` and `IPasswordHasher`.

- **No changes** to Steller.Api or Steller.Infrastructure (test-only code).

### 2.3 Build

```text
dotnet build Tests.Integration/Tests.Integration.csproj
# Result: 0 Error(s), 37 Warning(s) (existing warnings in other test classes).
```

---

## 3. Test Run Results

**Command run:**

```bash
dotnet test Tests.Integration/Tests.Integration.csproj \
  --filter "FullyQualifiedName~UserFlowIntegrationTests" --no-build
```

**Result:** All 8 tests **failed** during initialization.

**Root cause:** PostgreSQL authentication failure:

```text
Npgsql.PostgresException (0x80004005): 28P01: password authentication failed for user "postgres"
```

- Tests use default connection: `Host=localhost;Port=5432;Database=StellerTestDB;Username=postgres;Password=password`.
- Either no PostgreSQL is running on localhost:5432, or the password for user `postgres` is not `password`.

**Failed tests (all due to same DB error in InitializeAsync):**

| Test | Outcome | Cause |
|------|---------|--------|
| UF_P1_Partner_CreateOrder_ReceivesGiftCard | Failed | DB auth |
| UF_P2_Partner_WalletDeductionAfterOrder | Failed | DB auth |
| UF_P3_Partner_Idempotency_NoDoubleCharge | Failed | DB auth |
| UF_P4_Partner_CatalogToOrder_FullJourney | Failed | DB auth |
| UF_P5_Partner_InsufficientFunds_OrderRejected | Failed | DB auth |
| UF_A1_Admin_Login_CreditPartnerWallet | Failed | DB auth |
| UF_A2_Admin_CancelOrder_PartnerRefunded | Failed | DB auth |
| UF_A3_Admin_CreateApiKey_PartnerCanUse | Failed | DB auth |

---

## 4. How to Run When DB Is Available

1. **Start PostgreSQL** with a database and user the tests can use (e.g. database `StellerTestDB`, user `postgres`).

2. **Optional:** set connection string:
   ```bash
   export TEST_DB_CONNECTION="Host=localhost;Port=5432;Database=StellerTestDB;Username=postgres;Password=YOUR_PASSWORD;Pooling=false;"
   ```

3. **Build and run user-flow tests:**
   ```bash
   cd /opt/steller-v2/steller-backend
   dotnet build Tests.Integration/Tests.Integration.csproj
   dotnet test Tests.Integration/Tests.Integration.csproj \
     --filter "FullyQualifiedName~UserFlowIntegrationTests" --no-build
   ```

4. **Run all integration tests** (includes user-flow):
   ```bash
   dotnet test Tests.Integration/Tests.Integration.csproj
   ```

---

## 5. Conclusion

- **Backlog:** B7 added; plan and report linked.
- **Implementation:** 8 user-flow tests implemented and building; no app code changed.
- **Execution:** Blocked by test environment (PostgreSQL auth). Once a valid `TEST_DB_CONNECTION` (or default postgres/password) is available, re-run the command above and record pass/fail in this report or a follow-up run report.

---

## 6. References

- Plan: `docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md`
- Backlog: `docs/BACKLOG_V2.md` (§7 B7, §10)
- Run instructions: `docs/qa/QA_INTEGRATION_TEST_RUN_INSTRUCTIONS.md` (if present)
