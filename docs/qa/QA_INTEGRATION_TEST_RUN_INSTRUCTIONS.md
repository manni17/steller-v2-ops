# QA: Running Integration Tests (P1 & P2)

**Owner:** QA Agent  
**Reference:** `steller-backend/CONTRIBUTING.md` (Critical 4 + test run), `steller-backend/RUN_MIGRATIONS.md` (DB migrations protocol)  
**Shared with:** TPM / Dev

---

## Status (QA Agent → TPM / Dev)

| Item | Status | Notes |
|------|--------|--------|
| **P1 – OrderServiceTests** | ✅ Complete | 10 tests (P1_01–P1_06 + legacy T_01–T_03, Order_Creation). All passed. |
| **P2 – Order flow (8 tests)** | ✅ Complete | P2_01–P2_08: PlaceBambooOrderJob, PollBambooOrderJob, E2E. All passed. |
| **Full OrderServiceTests** | ✅ 18/18 passed | ~3 m 27 s. Connection: port 6432, user `steller_v2_user`, password from `../.env`. TPM owns `docs/BACKLOG_V2.md`; QA does not edit it. |
| **Tier 2 – PLG P0 gaps** | ✅ Resolved (Dev) | GAP-001–005 verified; report: `docs/PLG_P0_GAP_RESOLUTION_REPORT.md`. PLG Phase 1 unblocked. |
| **QA P3 – Bamboo integration (6 tests)** | ✅ Complete | 6/6 passed. Rate limit, 429, payload, OrderId. |
| **QA P4 – Operational jobs (6 tests)** | ✅ Complete | 6/6 passed. OrderQueueService, ReconciliationJob, WalletConsistencyJob. |
| **QA P5 – Auth (6 tests)** | ✅ Complete | 6/6 passed. API key middleware, catalog, orders, admin. |
| **Tier 2 (P3+P4+P5)** | ✅ 18/18 passed | ~1 m 48 s. See `docs/qa/QA_AGENT_STATUS.md` for test inventory. |

---

## Prerequisites

- Steller v2 stack running (`docker compose up -d` in `/opt/steller-v2`) **or** standalone Postgres for tests
- `.env` at `/opt/steller-v2/.env` (for Option A)
- **Schema:** If the repo has new EF migrations, apply them before tests: see `steller-backend/RUN_MIGRATIONS.md` (or CONTRIBUTING.md). Test DB may use EnsureCreated; for stack Postgres, migrations are usually applied by Dev.

---

## Run QA P1 (OrderServiceTests)

From `steller-backend`:

```bash
cd /opt/steller-v2/steller-backend
export TEST_DB_CONNECTION="Host=localhost;Port=6432;Database=StellerTestDB;Username=steller_v2_user;Password=$(grep DB_PASSWORD ../.env | cut -d= -f2);Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj --filter "FullyQualifiedName~OrderServiceTests" -c Release
```

If you prefer to paste the password manually: read `DB_PASSWORD` from `../.env` and use:

```bash
export TEST_DB_CONNECTION="Host=localhost;Port=6432;Database=StellerTestDB;Username=steller_v2_user;Password=YOUR_DB_PASSWORD;Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj --filter "FullyQualifiedName~OrderServiceTests" -c Release
```

---

## Run Critical 4 (Gate 1)

```bash
export TEST_DB_CONNECTION="Host=localhost;Port=6432;Database=StellerTestDB;Username=steller_v2_user;Password=$(grep DB_PASSWORD ../.env | cut -d= -f2);Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj \
  --filter "FullyQualifiedName~OrderServiceTests.T_01_Atomic_Rollback_Test|FullyQualifiedName~OrderServiceTests.T_02_Profit_Guard_Test|FullyQualifiedName~OrderServiceTests.T_03_Idempotency_Test|FullyQualifiedName~WalletServiceTests.T_04_Concurrency_Test" \
  -c Release
```

---

## Run QA P3, P4, P5 (Tier 2)

Bamboo integration (6), Operational jobs (6), Auth (6). Same `TEST_DB_CONNECTION` as above.

```bash
export TEST_DB_CONNECTION="Host=localhost;Port=6432;Database=StellerTestDB;Username=steller_v2_user;Password=$(grep DB_PASSWORD ../.env | cut -d= -f2);Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj --filter "FullyQualifiedName~BambooIntegrationTests|FullyQualifiedName~OperationalJobsTests|FullyQualifiedName~AuthTests" -c Release
```

Status and fix notes: `docs/qa/QA_AGENT_STATUS.md` (Tier 2 QA table).

---

## Full Integration Suite

```bash
export TEST_DB_CONNECTION="Host=localhost;Port=6432;Database=StellerTestDB;Username=steller_v2_user;Password=$(grep DB_PASSWORD ../.env | cut -d= -f2);Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj -c Release
```

---

## Alternative: Standalone Test Postgres

If steller-v2 stack is not running:

```bash
docker run -d --name steller-test-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=StellerTestDB -p 5433:5432 postgres:16-alpine
export TEST_DB_CONNECTION="Host=localhost;Port=5433;Database=StellerTestDB;Username=postgres;Password=postgres;Pooling=false;"
dotnet test Tests.Integration/Tests.Integration.csproj -c Release
docker stop steller-test-db && docker rm steller-test-db
```
