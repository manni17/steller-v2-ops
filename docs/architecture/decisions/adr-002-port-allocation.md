# ADR-002: Port Allocation Strategy

**Status:** Accepted  
**Date:** 2026-02-18  
**Deciders:** Architecture Team

---

## Context

Steller v2 needed ports that don't conflict with legacy Steller systems already running on the VPS. Port allocation needed to be:
- Non-conflicting with legacy ports
- Memorable and consistent
- Documented for agent awareness
- Easy to verify availability

**Legacy Ports Already Reserved:**
- 5091, 5092: Legacy APIs
- 5432: Legacy PostgreSQL
- 5672, 15672: Legacy RabbitMQ
- 8080, 8081: Legacy Dashboards

---

## Decision

Allocate ports for Steller v2 using offset pattern:
- **API:** 6091 (legacy API is 5091, offset by 1000)
- **PostgreSQL:** 6432 (legacy PostgreSQL is 5432, offset by 1000)
- **Redis:** 6379 (standard Redis port, not used by legacy)

**Pattern:**
- v2 ports = legacy ports + 1000 (where applicable)
- Standard ports used when not conflicting (Redis 6379)
- All ports exposed on host for direct access

**Documentation:**
- All ports documented in `docs/architecture/containers.yaml`
- Reserved ports listed in VPS navigation rules
- Agents check port availability before use

---

## Consequences

### Positive

- **Easy to remember** - Offset pattern makes ports predictable
- **Clear separation** - Easy to identify which system uses which port
- **No conflicts** - Verified no overlap with legacy ports
- **Agent-friendly** - Clear documentation enables agents to avoid conflicts

### Negative

- **Port range usage** - Uses ports in 6000+ range (not standard)
- **Documentation overhead** - Need to maintain port lists in multiple places

### Neutral

- Ports are arbitrary - functionality doesn't depend on specific numbers
- Can be changed if needed (with documentation updates)

---

## Alternatives Considered

### Alternative 1: Use Standard Ports with Different Hosts

**Rejected because:**
- Single VPS, no multiple hosts
- Would require reverse proxy configuration for every service

### Alternative 2: Random High Ports (30000+)

**Rejected because:**
- Harder to remember
- Less consistent
- No clear relationship to legacy ports

### Alternative 3: Internal-Only Ports (No Host Exposure)

**Rejected because:**
- Need host access for debugging and monitoring
- Agents need direct access for testing
- More complex networking setup

---

## Related Decisions

- ADR-001: Isolate v2 stack (ports are part of isolation strategy)

---

## Notes

Port allocation is part of the isolation strategy. The offset pattern provides consistency while maintaining clear separation from legacy systems.
