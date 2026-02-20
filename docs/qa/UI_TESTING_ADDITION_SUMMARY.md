# UI Testing Addition — Summary

**Date:** 2026-02-18  
**Status:** ✅ Added to QA Plan

---

## What Was Added

### 1. UI Testing Strategy Document

**File:** `docs/qa/UI_TESTING_STRATEGY.md`

**Contents:**
- **Philosophy:** Test critical user journeys (financial operations), not visual regressions
- **Components:** Admin Dashboard (port 8080), Consumer Dashboard (port 8081)
- **Critical flows:** Login, order placement, wallet viewing, API key management
- **Tooling:** Playwright (recommended) or Cypress
- **Gate 3:** Conditional UI testing gate (only if UI code changed)

### 2. Updated QA Plan

**File:** `docs/STELLER_V2_QA_PLAN.md`

**Changes:**
- Added §3.4 "UI Testing (Separate Layer)"
- Added Gate 3 to quality gates table
- Updated references section

### 3. Updated Pipeline

**File:** `docs/qa/QA_CRITICAL_PATH_AND_PIPELINE.md`

**Changes:**
- Updated mermaid diagram to include Gate 3 (UI)
- Added Gate 3 execution details (§3.4)
- Gate 3 is conditional (only if UI changed)

---

## UI Testing Approach

### Philosophy

**Focus:** Critical user journeys that touch financial operations.

**Skip:** Visual regressions, pixel-perfect layouts, animations (unless explicitly required).

### Critical Flows (Must Test)

**Admin Dashboard:**
- Login → Dashboard
- View orders → Filter → Details
- View wallet balance
- Create API key → Display → Revoke
- View metrics

**Consumer Dashboard:**
- Login → Dashboard
- View catalog → Partner-specific pricing
- Place order → Order ID → Status updates
- View order history → Cards (serial, PIN)
- View wallet balance

### Gate 3 Behavior

- **Conditional:** Only runs if UI code changed (git diff or CI path filters)
- **Blocking:** Deploys blocked if UI tests fail (UI changes only)
- **Skipped:** For API-only or backend-only changes

---

## Implementation Status

**Current:** Strategy documented, not yet implemented.

**Next Steps:**
1. Set up Playwright/Cypress in UI repos
2. Create test structure (`Tests.UI/admin/`, `Tests.UI/consumer/`)
3. Implement critical flow tests (login, orders, wallet)
4. Wire Gate 3 into CI (conditional execution)

---

## References

- **Full UI Strategy:** [UI_TESTING_STRATEGY.md](UI_TESTING_STRATEGY.md)
- **QA Plan:** [../STELLER_V2_QA_PLAN.md](../STELLER_V2_QA_PLAN.md)
- **Pipeline:** [QA_CRITICAL_PATH_AND_PIPELINE.md](QA_CRITICAL_PATH_AND_PIPELINE.md)
