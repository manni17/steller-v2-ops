# Steller v2 — Quality Management System (QMS) Plan

**Status:** Approved (de-bloated; Ledger Consistency as primary metric)  
**Last Updated:** 2026-02-18  

**Scope:** Development, testing, deployment, and operations for Steller v2. No process theater; focus on **one metric that matters** and executable gates.

---

## 1. Primary Quality Metric: Ledger Consistency

**The only quality metric that truly matters for correctness:**

```text
Sum(WalletHistory) == Wallet.AvailableBalance
```

**If this breaks, uptime does not matter.** Money is wrong; the system is broken.

### 1.1 How It Is Enforced

- **Runtime:** `WalletConsistencyJob` runs hourly (or as configured). On drift, it fires a critical alert via `AdminAlertService`.
- **Schema:** Ledger is append-only; `BEFORE UPDATE OR DELETE ON WalletHistories RAISE EXCEPTION` (PostgreSQL trigger).
- **Tests:** Integration tests (Critical 4 and wallet tests) exercise debit/credit/refund paths so that under test, ledger and balance stay consistent.

### 1.2 Response to Ledger Drift

1. **Alert** → On-call / admin notified.
2. **HALT** → No further financial operations until root cause is identified.
3. **Fix & reconcile** → Correct data and/or code; re-run consistency check.
4. **Post-mortem** → Document cause and preventive measure; no blame theater.

---

## 2. Quality Gates (Executable)

### 2.1 Every Commit

- **Gate 1 — Critical 4:** Atomic Rollback, Profit Guard, Idempotency, Concurrency. Run against real PostgreSQL (e.g. container). **Build rejected** on any failure.
- **Unit tests:** Light business-logic tests. Failures block commit/PR as per team policy.

### 2.2 Before Deploy

- **Gate 2 — E2E:** Autonomous QA Agent runs (Source of Truth audit, place order, observe logs/DB). **Deploy blocked** if E2E fails.
- **Build artifact:** Docker image built from the same commit that passed Gate 1 and Gate 2.

### 2.3 In Production

- **Ledger consistency:** Monitored by `WalletConsistencyJob`; alert on drift.
- **Health:** `GET /api/health` monitored; operational response as per runbooks.
- **Vendor/order failure:** Treated as vendor/connectivity issue when Steller correctly marks order Failed and refunds; see [STELLER_QA_AGENT_PROTOCOL_V2.md](STELLER_QA_AGENT_PROTOCOL_V2.md) §6.

---

## 3. Minimal Process (No Bloat)

### 3.1 Development

- **Code review:** Required for main/protected branches; reviewer checks that Critical 4 and related tests exist or are unchanged where relevant.
- **Tests:** Critical 4 run on every commit. New financial or wallet-touching code must be covered by integration tests.

### 3.2 Release

- **Pre-release:** Critical 4 pass; E2E pass; release notes / changelog updated for breaking changes.
- **Rollback:** Documented rollback path (e.g. previous image, DB backup policy) in runbooks; no formal matrix required.

### 3.3 Change and Incidents

- **Change:** Sensible impact assessment for financial/wallet changes; tests and Ledger Consistency check are the main mitigation.
- **Incidents:** Ledger drift or financial incorrectness is P0; fix and reconcile first, then post-mortem.

---

## 4. What We Explicitly Do Not Do

- **No ISO 25010 / risk assessment matrices** as deliverables; risk is mitigated by tests and ledger monitoring.
- **No vanity metrics** (e.g. “70% unit coverage” as a target); focus is **integration coverage** and **Critical 4 + Ledger**.
- **No heavy QMS documentation** for its own sake; this document and the referenced QA plan are sufficient unless compliance requirements change.

---

## 5. References

- **QA plan (pyramid, Critical 4, pipeline):** [STELLER_V2_QA_PLAN.md](STELLER_V2_QA_PLAN.md)
- **Critical path & pipeline:** [qa/QA_CRITICAL_PATH_AND_PIPELINE.md](qa/QA_CRITICAL_PATH_AND_PIPELINE.md)
- **Gray-box monitoring (wallet mutation, vendor dispatch, Hangfire, ledger):** Steller backend `docs/integration/PHASE_4_GRAYBOX_MONITORING.md`
- **Financial invariants (SDD):** Steller backend `docs/SOFTWARE_DESIGN_DOCUMENT.md` §4
