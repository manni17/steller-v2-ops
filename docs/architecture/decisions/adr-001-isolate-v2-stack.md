# ADR-001: Isolate Steller v2 Stack from Legacy

**Status:** Accepted  
**Date:** 2026-02-18  
**Deciders:** Architecture Team

---

## Context

Steller v2 needed to be deployed on the same VPS as legacy Steller systems without interfering with production legacy services. Both systems needed to run simultaneously with complete isolation.

**Requirements:**
- Zero impact on legacy Steller systems
- Independent deployment and scaling
- Separate data storage (no cross-contamination)
- Independent upgrades and maintenance

---

## Decision

Deploy Steller v2 as a completely isolated Docker Compose stack:
- Separate Docker Compose project (`steller-v2`)
- Separate directory (`/opt/steller-v2/`)
- Separate Docker networks (no overlap with legacy)
- Separate PostgreSQL instance (port 6432 vs legacy 5432)
- Separate Redis instance (port 6379)
- Separate port ranges (6091 vs legacy 5091/5092)

**Isolation Strategy:**
- Physical separation: Different directories and compose files
- Network separation: Different Docker networks
- Data separation: Different databases and volumes
- Port separation: Different host ports

---

## Consequences

### Positive

- **Zero risk to legacy systems** - Complete isolation prevents accidental modifications
- **Independent operations** - Can restart, update, or modify v2 without affecting legacy
- **Clear boundaries** - Agents and operators know exactly what belongs to which system
- **Easy rollback** - Can stop/remove v2 stack without affecting legacy
- **Resource isolation** - Can monitor and manage resources separately

### Negative

- **Resource overhead** - Separate PostgreSQL instance uses additional RAM (~200-300 MB)
- **Port management** - Need to track and reserve ports for both systems
- **Deployment complexity** - Two separate stacks to manage
- **Documentation overhead** - Need to document both systems and their boundaries

### Neutral

- Both systems share the same VPS infrastructure (CPU, disk, network)
- Both systems can be accessed from the same host

---

## Alternatives Considered

### Alternative 1: Shared PostgreSQL

**Rejected because:**
- Risk of data cross-contamination
- Schema conflicts between legacy and v2
- Cannot upgrade databases independently
- Harder to rollback v2 without affecting legacy

### Alternative 2: Different VPS

**Rejected because:**
- Additional infrastructure cost
- More complex networking between systems
- Unnecessary for current scale

### Alternative 3: Namespace Isolation Only

**Rejected because:**
- Not sufficient isolation (shared volumes, networks could cause conflicts)
- Harder to enforce boundaries
- More risk of accidental modifications

---

## Related Decisions

- ADR-002: Port allocation strategy
- ADR-003: API key hashing approach

---

## Notes

This decision enables safe parallel operation of legacy and v2 systems, which is critical for gradual migration and zero-downtime deployments.
