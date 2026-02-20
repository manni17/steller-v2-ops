# Steller v2 — UI Testing Strategy

**Status:** Approved (P1 Priority)  
**Last Updated:** 2026-02-18  
**Priority:** **P1** (Important, not critical). API testing (P0) takes precedence.  
**Scope:** Admin Dashboard (port 8080) and Consumer Dashboard (port 8081) — Vue.js applications.

---

## 1. UI Testing Philosophy

**Priority:** **P1** (Important, not critical). API testing (P0) takes precedence.

**Principle:** Test **critical user journeys** that touch financial operations, not visual regressions or pixel-perfect layouts.

**Why:** In a financial platform, UI bugs that affect order placement, wallet viewing, or API key management can cause real business impact. Visual bugs (spacing, colors) are lower priority.

**Approach:** **Critical path only** — focus on workflows that matter to business operations.

**Note:** UI testing is **not** a blocker for API-only deployments. If UI code is unchanged, Gate 3 is skipped and deployment proceeds after Gate 2 (API E2E) passes.

---

## 2. UI Components

| Component | Port | Purpose | Critical Flows |
|-----------|------|---------|----------------|
| **Admin Dashboard** | 8080 | Admin operations, partner management, metrics | Login, view orders, view wallet, create API key, view metrics |
| **Consumer Dashboard** | 8081 | Partner-facing operations, order placement | Login, view catalog, place order, view order status, view wallet |

**Note:** These are **separate** from the Steller v2 API stack. They consume the API (`http://localhost:6091`) but are deployed independently.

---

## 3. Testing Strategy

### 3.1 Test Pyramid (UI Layer)

```
        /\
       /E2E\         20% - Full user journeys (login → order → verify)
      /------\
     /Component\     30% - Vue component unit tests (if applicable)
    /------------\
   /  Visual      \  50% - Visual regression (optional, low priority)
  /----------------\
```

**Reality:** Focus on **E2E user journeys** (80%) and skip visual regression unless explicitly required.

### 3.2 Critical User Journeys (Must Test)

#### Admin Dashboard

1. **Login Flow**
   - Admin logs in → sees dashboard
   - Invalid credentials → error shown
   - Session persists → refresh page → still logged in

2. **Order Management**
   - View orders list → filter by status → view order details
   - Order details show correct data (order number, status, cards)

3. **Wallet Management**
   - View partner wallet balance
   - Balance matches API response (`GET /api/wallet`)

4. **API Key Management**
   - Create API key for partner → key displayed once
   - List API keys → see previews (not plain keys)
   - Revoke API key → key marked inactive

5. **Metrics Dashboard**
   - View metrics summary → data loads
   - Filter by date range → data updates

#### Consumer/Partner Dashboard

1. **Login Flow**
   - Partner logs in (JWT or API key) → sees dashboard
   - Invalid credentials → error shown

2. **Catalog View**
   - View catalog → products displayed
   - Catalog matches API (`GET /api/brand/getCatalog`)
   - Partner-specific pricing shown

3. **Order Placement**
   - Select product → enter face value → place order
   - Order created → order ID shown
   - Order status updates (Pending → Processing → Completed/Failed)

4. **Order History**
   - View orders → filter by status
   - Order details show cards (serial, PIN) when completed

5. **Wallet Balance**
   - View wallet balance → matches API
   - Balance updates after order placement

---

## 4. Tooling Recommendations

### 4.1 Playwright (Recommended)

**Why:** Fast, reliable, cross-browser, good API integration.

**Setup:**
```bash
npm init -y
npm install --save-dev @playwright/test
npx playwright install
```

**Example Test:**
```typescript
import { test, expect } from '@playwright/test';

test('Admin: Login and view orders', async ({ page }) => {
  await page.goto('http://localhost:8080');
  await page.fill('[name="email"]', 'admin@steller.com');
  await page.fill('[name="password"]', 'password');
  await page.click('button[type="submit"]');
  
  await expect(page).toHaveURL(/.*dashboard/);
  await expect(page.locator('text=Orders')).toBeVisible();
  
  // Verify orders API call was made
  const ordersResponse = await page.waitForResponse(
    response => response.url().includes('/api/orders') && response.status() === 200
  );
  expect(ordersResponse.ok()).toBeTruthy();
});
```

### 4.2 Cypress (Alternative)

**Why:** Good developer experience, time-travel debugging.

**Setup:**
```bash
npm install --save-dev cypress
npx cypress open
```

---

## 5. Test Data for UI Tests

**Strategy:** Use **real API** (staging/sandbox) or **API mocks** (WireMock/Mock Service Worker).

**Option 1: Real API (Recommended)**
- UI tests hit `http://localhost:6091` (Steller v2 API)
- Use test data from `TestDataFactory` or seed data
- **Pros:** Tests real integration
- **Cons:** Requires API to be running

**Option 2: API Mocks**
- Mock API responses using WireMock or MSW
- **Pros:** Faster, no API dependency
- **Cons:** May miss API contract changes

**Recommendation:** Use **real API** for critical flows (order placement, wallet); use **mocks** for non-critical flows (metrics, catalog).

---

## 6. Gate 3: UI Testing Gate (P1 Priority)

### 6.1 When It Runs

- **Priority:** **P1** (Important, not critical). API testing (P0) takes precedence.
- **Conditional:** Only if UI code changed (detected via git diff or CI path filters).
- **Trigger:** After Gate 2 (API E2E) passes, before deploy.
- **Scope:** Critical user journeys only (see §3.2).

### 6.2 Failure Handling

- **UI changes:** Gate 3 failure **blocks deploy** (UI changes only).
- **API-only changes:** Gate 3 skipped (not blocking). Deploy proceeds after Gate 2 (API E2E) passes.
- **Backend-only changes:** Gate 3 skipped (not blocking). Deploy proceeds after Gate 2 (API E2E) passes.

**Priority Enforcement:** P0 gates (Gate 1, Gate 2) **always** run and block. P1 gate (Gate 3) is conditional and can be deferred if UI unchanged.

### 6.3 CI Integration

**Example GitHub Actions:**

```yaml
- name: Gate 3 - UI Tests (if UI changed)
  if: contains(github.event.head_commit.modified, 'dashboard/') || contains(github.event.head_commit.modified, 'vue')
  run: |
    npm ci
    npm run test:ui
  env:
    API_BASE_URL: http://localhost:6091
    ADMIN_DASHBOARD_URL: http://localhost:8080
    CONSUMER_DASHBOARD_URL: http://localhost:8081
```

---

## 7. Test Organization

### 7.1 Structure

```
Tests.UI/
├── admin/
│   ├── login.spec.ts
│   ├── orders.spec.ts
│   ├── wallet.spec.ts
│   ├── api-keys.spec.ts
│   └── metrics.spec.ts
├── consumer/
│   ├── login.spec.ts
│   ├── catalog.spec.ts
│   ├── order-placement.spec.ts
│   ├── order-history.spec.ts
│   └── wallet.spec.ts
└── shared/
    ├── test-helpers.ts
    └── api-helpers.ts
```

### 7.2 Test Helpers

**API Helpers:** Reuse API calls from E2E protocol (get API key, place order).

**Page Objects:** Encapsulate UI interactions (e.g. `AdminLoginPage`, `OrderListPage`).

---

## 8. Non-Critical (Skip)

**Do not test:**
- Visual regressions (spacing, colors, fonts) — unless explicitly required
- Animation timing
- Responsive design (unless critical for mobile partners)
- Accessibility (unless compliance requirement)

**Focus:** Functional correctness of financial operations.

---

## 9. Implementation Roadmap

### Phase 1: Foundation (Week 1)
- Set up Playwright/Cypress
- Create test structure
- Implement login flow tests (Admin + Consumer)

### Phase 2: Critical Flows (Week 2)
- Order placement (Consumer)
- Order viewing (Admin)
- Wallet balance (both)

### Phase 3: Integration (Week 3)
- Wire Gate 3 into CI
- Add conditional execution (only if UI changed)
- Document in CONTRIBUTING.md

---

## 10. References

- **QA Plan:** [../STELLER_V2_QA_PLAN.md](../STELLER_V2_QA_PLAN.md)
- **E2E Protocol (API):** [../STELLER_QA_AGENT_PROTOCOL_V2.md](../STELLER_QA_AGENT_PROTOCOL_V2.md)
- **Test Data Factory:** [TEST_DATA_FACTORY_SPEC.md](TEST_DATA_FACTORY_SPEC.md)
- **Playwright Docs:** https://playwright.dev
- **Cypress Docs:** https://docs.cypress.io
