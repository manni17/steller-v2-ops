# ADR-004: Hangfire Embedded in API Process

**Status:** Accepted  
**Date:** 2026-02-18  
**Deciders:** Architecture Team

---

## Context

Steller v2 needs background job processing for order placement with Bamboo vendor. Options:
- Separate container for job processor
- Embedded job processor in API container
- External job queue service (e.g., RabbitMQ)

**Requirements:**
- Process orders asynchronously
- Retry failed jobs
- Monitor job status
- Simple deployment (fewer containers)

---

## Decision

Run Hangfire job processor embedded in the API process:
- **No separate container** - Hangfire runs inside `steller-v2-api` container
- **Same process** - Jobs execute in API process (shared memory, same DB connection)
- **Dashboard** - Hangfire dashboard accessible via API (if exposed)
- **Database-backed** - Hangfire uses PostgreSQL for job storage

**Rationale:**
- Simpler deployment (one less container)
- Shared database connection pool
- Easier monitoring (same logs as API)
- Sufficient for current scale

---

## Consequences

### Positive

- **Simpler deployment** - One less container to manage
- **Shared resources** - API and jobs share database connections
- **Unified logging** - All logs in one place (`docker logs steller-v2-api`)
- **Easier debugging** - Can see API and job logs together
- **Lower resource usage** - No separate container overhead

### Negative

- **Coupling** - API and jobs are tightly coupled (restart API = restart jobs)
- **Scaling** - Cannot scale jobs independently from API
- **Resource contention** - Jobs and API compete for same process resources
- **Single point of failure** - If API crashes, jobs stop

### Neutral

- Jobs run synchronously within API process (acceptable for current load)
- Can migrate to separate container later if needed

---

## Alternatives Considered

### Alternative 1: Separate Hangfire Container

**Rejected because:**
- More complex deployment (additional container)
- More resource usage (separate process)
- Unnecessary for current scale
- Can add later if needed

### Alternative 2: External Queue (RabbitMQ)

**Rejected because:**
- More infrastructure to manage
- Legacy system already uses RabbitMQ (want to avoid)
- Hangfire is simpler for current needs
- Can migrate later if needed

### Alternative 3: Cloud Job Queue (AWS SQS, Azure Queue)

**Rejected because:**
- External dependency
- Additional cost
- Overkill for current scale
- Prefer self-contained solution

---

## Related Decisions

- ADR-001: Isolate v2 stack (Hangfire is part of v2 stack)
- Data flow blueprint: `docs/architecture/blueprints/data-flow.md`

---

## Notes

This decision prioritizes simplicity and ease of deployment. If job processing needs scale independently or if API and jobs need different resource profiles, can migrate to separate container later. For current scale, embedded approach is sufficient.
