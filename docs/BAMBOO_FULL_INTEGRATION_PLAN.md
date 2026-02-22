# Plan: Full Bamboo Integration

**Purpose:** Single plan to reach full integration with the Bamboo Client API (Bamboo Client Integration V2). Use this doc to track what is done and what remains; update "Status" and "Done" as work completes.

**Reference:** Bamboo Client Integration (Documentation) V2 — Production host `https://api.bamboocardportal.com`; Basic Auth (ClientId/ClientSecret or Username/Password). Sandbox = same host, different credentials.

---

## 1. Current state (what we have)

| Bamboo API | Status | Implementation |
|------------|--------|----------------|
| **Get Catalog v2** | Done | `IBambooApiClient.GetCatalogAsync()` — v2 paginated, 1 req/sec; `BambooCatalogV2Response`; BrandService/BrandController use BaseUrlV2. |
| **Get Categories** | Done | `IBambooApiClient.GetCategoriesAsync()` — v2.0 then v1.0 fallback; used by sync. |
| **Place Order** | Done | `IBambooApiClient.PlaceOrderAsync()` — POST v2.0/orders/checkout; PlaceBambooOrderJob, rate limit 2 req/sec. |
| **Get Order** | Done | `IBambooApiClient.GetOrderAsync()` — GET v1.0/orders/{requestId}; PollBambooOrderJob, ReconciliationJob, rate limit 120/min. |

**Rate limits (already configured):** `bamboo_catalog`, `bamboo_categories`, `bamboo_place_order`, `bamboo_get_order`, `bamboo_exchange_rate`, `bamboo_transactions`, `bamboo_other`.

---

## 2. Bamboo API surface (from official doc)

| API | Method | Path (pattern) | Bamboo limit | Notes |
|-----|--------|----------------|--------------|-------|
| Get Catalog v2 | GET | `/api/integration/v2.0/catalog` | 1 req/sec | Pagination, filtering. |
| Get Categories | GET | v2.0/categories | Other (1/sec) | — |
| Get Accounts | GET | (doc: Get Accounts) | — | Account/balance info. |
| Place Order | POST | v2.0/orders/checkout | 2 req/sec | — |
| Get Order | GET | v1.0/orders/{id} | 120/min | By requestId. |
| Exchange rates | GET | v1.0/exchange-rates?baseCurrency=&currency= | 20/min | Updated hourly. |
| Transactions | GET | v1/transactions?startDate=&endDate= | 4/hour | Date range. |
| Orders (list) | GET | v1/orders?startDate=&endDate= | Other (1/sec) | Date range. |
| Notification | POST | v1/notification | Other | Set callback URL + secretKey. |
| Get Notification | GET | v1/notification | Other | Read current config. |

---

## 3. Gap (not yet integrated)

| API | Priority | Use case |
|-----|----------|----------|
| **Exchange rates** | P1 | Pricing in multiple currencies; display/conversion in partner/admin. |
| **Get Accounts** | P1 | Balance/account visibility; reconciliation; low-balance alerts. |
| **Orders (list)** | P2 | Order history by date range; reporting; reconciliation. |
| **Transactions** | P2 | Transaction history from Bamboo; reconciliation, audit. |
| **Notification (POST/GET)** | P2 | Webhook callback when order reaches Succeeded/Failed/PartialFailed; reduce polling. |

---

## 4. Phased plan

### Phase A — Exchange rates (P1)

- **Endpoint:** `GET /api/integration/v1.0/exchange-rates?baseCurrency=USD&currency=EUR` (optional params; no params = all rates vs USD).
- **Rate limit:** 20/min (already in `bamboo_exchange_rate`).
- **DTOs:** Add `BambooExchangeRatesResponse` (e.g. `baseCurrencyCode`, `rates[]` with `currencyCode`, `value`).
- **IBambooApiClient:** `Task<BambooExchangeRatesResponse> GetExchangeRatesAsync(string? baseCurrency = null, string? currency = null)`.
- **Use in:** Pricing service (optional multi-currency), admin/partner UI, or a small ExchangeRatesController for read-only exposure.
- **Acceptance:** Unit/integration test; 200 with valid response; rate limiter slot used.

### Phase B — Get Accounts (P1)

- **Endpoint:** GET (doc: Get Accounts) — confirm exact path from Bamboo doc (e.g. `/api/integration/v1.0/accounts` or similar).
- **Rate limit:** Likely "other" (1/sec) unless doc states otherwise.
- **DTOs:** Add response DTO for account/balance as per Bamboo response shape.
- **IBambooApiClient:** `Task<BambooAccountsResponse> GetAccountsAsync()` (or single account if that’s what the API returns).
- **Use in:** Admin balance visibility, reconciliation job, or low-balance alerting.
- **Acceptance:** Call returns 200; parse balance/account id; document in BAMBOO_INTEGRATION_COMPATIBILITY.

### Phase C — Orders list (P2)

- **Endpoint:** `GET /api/Integration/v1/orders?startDate=YYYY-MM-DD&endDate=YYYY-MM-DD`.
- **Rate limit:** Other (1/sec).
- **DTOs:** Add DTO for list response (array of order summary per doc).
- **IBambooApiClient:** `Task<List<BambooOrderSummary>> GetOrdersAsync(DateTime startDate, DateTime endDate)`.
- **Use in:** Reconciliation, reporting, or “orders in range” API for admin/partner.
- **Acceptance:** 200 with list; optional sync or cache policy documented.

### Phase D — Transactions (P2)

- **Endpoint:** `GET /api/Integration/v1/transactions?startDate=...&endDate=...`.
- **Rate limit:** 4/hour (already `bamboo_transactions`).
- **DTOs:** Add DTO matching Bamboo response (startDate, endDate, clients[].transactions[]).
- **IBambooApiClient:** `Task<BambooTransactionsResponse> GetTransactionsAsync(DateTime startDate, DateTime endDate)`.
- **Use in:** Reconciliation, audit, financial reporting.
- **Acceptance:** 200; parse; respect 4/hour in job or UI.

### Phase E — Notification config (P2)

- **Endpoints:** POST and GET `/api/Integration/v1/notification`. POST body: `notificationUrl`, `notificationUrlSandbox`, `secretKey`. GET returns current config.
- **Rate limit:** Other (1/sec).
- **DTOs:** Request/response DTOs per Bamboo doc.
- **IBambooApiClient:** `Task SetNotificationConfigAsync(string? notificationUrl, string? notificationUrlSandbox, string? secretKey)`, `Task<BambooNotificationConfig> GetNotificationConfigAsync()`.
- **Use in:** Configure webhook URL so Bamboo calls us on order completion; optionally reduce polling or add instant webhook handler.
- **Backend:** Webhook endpoint (e.g. POST /api/webhooks/bamboo) to receive callback; verify secretKey; then fetch order details via GetOrder and process.
- **Acceptance:** Set config returns 200; GET returns config; webhook handler receives callback and processes (document in runbook).

---

## 5. Implementation order and dependencies

1. **Exchange rates (A)** — No dependency; add client + DTOs; optional use in pricing/UI.
2. **Get Accounts (B)** — No dependency; add client + DTOs; use in balance/reconciliation.
3. **Orders list (C)** — No dependency; add client + DTOs; use in reporting/reconciliation.
4. **Transactions (D)** — No dependency; add client + DTOs; use in reconciliation/audit.
5. **Notification (E)** — After (optional) webhook route exists: add client for POST/GET; then configure URL and implement webhook handler.

**Cross-cutting:** For each new API, add or reuse rate-limit key in `RedisRateLimiterService` / `InMemoryRateLimiterService`; use `WaitForSlotAsync` before calling; handle 429 with `SetRetryAfter` where applicable; log and document in BAMBOO_INTEGRATION_COMPATIBILITY.md.

---

## 6. Checklist (update as done)

- [ ] **A** Exchange rates: DTOs, IBambooApiClient method, rate limit, test.
- [ ] **B** Get Accounts: confirm path in Bamboo doc, DTOs, client method, rate limit, test.
- [ ] **C** Orders list: DTOs, client method, rate limit, test.
- [ ] **D** Transactions: DTOs, client method, rate limit (4/hour), test.
- [ ] **E** Notification: DTOs, POST/GET client methods; webhook endpoint; handler; doc and test.
- [ ] Update BAMBOO_INTEGRATION_COMPATIBILITY.md with all new endpoints and request/response notes.
- [ ] Update BAMBOO_CATALOG_WHY_IT_FAILS.md (or a single “Bamboo limits” doc) if adding new rate-limit notes.

---

## 7. References

- **Bamboo doc:** User-provided Bamboo Client Integration (Documentation) V2 (Get catalog, Get Accounts, Place Order, Get order, Exchange rates, Transactions, Orders, Notification).
- **Current client:** `Steller.Infrastructure/Integrations/Bamboo/BambooApiClient.cs`, `IBambooApiClient.cs`.
- **Rate limits:** `Steller.Api/Services/RedisRateLimiterService.cs`, `RateLimiterService.cs`.
- **Compatibility:** `docs/BAMBOO_INTEGRATION_COMPATIBILITY.md`, `docs/BAMBOO_CATALOG_WHY_IT_FAILS.md`.
