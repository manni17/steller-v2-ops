# Deployment Blueprint

**Purpose:** Describes how Steller v2 is deployed, including Docker stack, ports, networks, and volumes.

**Last Updated:** 2026-02-18

**Canonical Sources:**
- System boundaries: `docs/architecture/systems.yaml`
- Containers and ports: `docs/architecture/containers.yaml`

---

## Overview

Steller v2 runs as an isolated Docker Compose stack on a shared VPS, alongside legacy Steller systems. Complete isolation is maintained through separate networks, ports, databases, and volumes.

---

## Deployment Stack

### Docker Compose

- **Compose File:** `/opt/steller-v2/docker-compose.yml`
- **Project Name:** `steller-v2`
- **Project Directory:** `/opt/steller-v2/`

### Containers

| Container Name | Service Name | Type | Status |
|----------------|--------------|------|--------|
| steller-v2-api | api | API + Hangfire | Running |
| steller-v2-postgres | postgres | Database | Running |
| steller-v2-redis | redis | Cache | Running |

---

## Port Map

| Service | Host Port | Container Port | Protocol | Purpose |
|---------|-----------|----------------|----------|---------|
| API | 6091 | 8080 | HTTP | Main API endpoint |
| PostgreSQL | 6432 | 5432 | TCP | Database access |
| Redis | 6379 | 6379 | TCP | Cache access |

**Note:** All ports are exposed on host for direct access. Internal Docker networking also available.

---

## Networks

- **Network Name:** `steller-v2-network` (or similar, defined in compose file)
- **Type:** Bridge network (isolated from legacy systems)
- **Purpose:** Container-to-container communication

**Isolation:** No overlap with legacy Docker networks.

---

## Volumes

| Volume Name | Container | Purpose |
|------------|-----------|---------|
| steller_v2_postgres_data | steller-v2-postgres | PostgreSQL data persistence |
| steller_v2_redis_data | steller-v2-redis | Redis data persistence |

**Location:** Managed by Docker (typically `/var/lib/docker/volumes/`)

---

## Environment Configuration

- **Config File:** `/opt/steller-v2/.env`
- **Key Variables:**
  - `DB_NAME`: steller_v2
  - `DB_USERNAME`: steller_v2_user
  - `DB_PASSWORD`: (from secure store)
  - Port mappings and other service configs

**Guardrails:**
- Do not modify `.env` without explicit user request
- Do not expose `.env` contents (passwords, API keys) in logs or chat
- Reading `.env` only allowed when user explicitly requests debugging

---

## Deployment Directory Structure

```
/opt/steller-v2/
├── docker-compose.yml      # Compose configuration
├── .env                    # Environment variables (protected)
└── src/                    # Application source code (if present)
```

---

## Agent-Relevant

### CRITICAL: Protected Deployment

- **DO NOT** restart containers: `docker restart steller-v2-api`
- **DO NOT** stop containers: `docker stop steller-v2-api`
- **DO NOT** modify compose file: `/opt/steller-v2/docker-compose.yml`
- **DO NOT** modify `.env` file without explicit user request

### Safe Operations (Read-Only)

- View logs: `docker logs steller-v2-api`
- Check status: `docker ps --filter name=steller-v2`
- Inspect containers: `docker inspect steller-v2-api`
- Health check: `curl http://localhost:6091/api/health`

### Port Conflicts

Before using any port, verify availability:
```bash
sudo netstat -tuln | grep <PORT>
```

**Reserved Ports (DO NOT USE):**
- Legacy: 5091, 5092, 5432, 5672, 8080, 8081, 15672
- Steller v2: 6091, 6432, 6379

**Safe Port Ranges:** 8000+, 9000+, 30000+

---

## Quick Reference

| Item | Value |
|------|-------|
| Compose file | `/opt/steller-v2/docker-compose.yml` |
| API endpoint | `http://localhost:6091` |
| DB host | `localhost:6432` |
| DB name | `steller_v2` |
| DB user | `steller_v2_user` |
| Redis host | `localhost:6379` |
| Check containers | `docker ps --filter name=steller-v2` |
| View API logs | `docker logs steller-v2-api` |

---

## Database Migrations

- **Protocol:** Persistent execution (not one-time). When and how often: see backend repo **CONTRIBUTING.md**.
- **Quick reference:** Backend repo **RUN_MIGRATIONS.md** (command, `DB_HOST_MIGRATIONS` override, troubleshooting).
- **Also:** README Operational Notes, QUICK_START.md (step 4), PRODUCTION_DEPLOYMENT.md; `.env.example` documents DB/override options.

---

## Related Documentation

- System boundaries: `docs/architecture/systems.yaml`
- Container details: `docs/architecture/containers.yaml`
- Data flow: `docs/architecture/blueprints/data-flow.md`
- DB migration protocol: backend repo `CONTRIBUTING.md`, `RUN_MIGRATIONS.md`
