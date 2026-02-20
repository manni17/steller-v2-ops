# ADR-005: API Request Logging (Async Queue)

**Status:** Accepted  
**Date:** 2026-02-20  
**Deciders:** Dev Agent (PLG Phase 1)

---

## Context

PLG Phase 1 requires tracking API calls for analytics (activation, usage, growth metrics). Logging must not block or slow API responses. Writing synchronously to PostgreSQL in middleware would add latency and lock risk.

---

## Decision

- **Middleware:** `AnalyticsMiddleware` runs after the pipeline; captures PartnerId (from claims), Path, Method, StatusCode, ElapsedMs, TimestampUtc.
- **Queue:** Log entries are enqueued to a **Redis list** (`analytics:request_logs`) via `IApiRequestLogQueue`. Implementation: `RedisApiRequestLogQueue` when Redis is configured; `NoOpApiRequestLogQueue` when Redis is not (e.g. tests).
- **Persistence:** A Hangfire job (`ProcessAnalyticsQueueJob`, analytics_infrastructure) reads from the Redis list and batch-inserts into PostgreSQL `ApiRequestLogs` table. No synchronous write from middleware to PostgreSQL.

**Rationale:**
- API response latency is unaffected (fire-and-forget enqueue).
- Redis list is fast and supports multiple consumers if needed.
- Batch writes in the job reduce DB load.

---

## Consequences

- **Positive:** Non-blocking; analytics decoupled from request path; Redis already in use for rate limiting.
- **Negative:** If Redis is down, logs are dropped (no-op or best-effort); eventual consistency for analytics.
- **Guardrail:** MUST NOT write to PostgreSQL synchronously in middleware (per PRD and PLG plan).

---

## Related

- PLG plan: `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md`
- GAP-004 resolution: Redis supports list/queue operations
- Data flow: `docs/architecture/blueprints/data-flow.md` (to be updated with analytics flow)
