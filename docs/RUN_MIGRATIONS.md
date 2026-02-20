# Steller v2 Database Migrations — Runbook

**Purpose:** Canonical instructions for running EF migrations. **All agents and operators** should follow this protocol.

---

## Primary method: Docker (recommended)

**Steller v2 Postgres** (`steller-v2-postgres`) is on a Docker network. The hostname resolves **only inside the Docker network**. Running `dotnet ef database update` from the host fails with `Resource temporarily unavailable` or `Name or service not known` because `steller-v2-postgres` does not resolve on the host.

**Always use Docker to run migrations:**

```bash
cd /opt/steller-v2
docker compose --profile tools run --rm steller-migrations
```

This runs migrations inside a container on the same network as Postgres. No config changes or host overrides needed.

---

## Fallback: Host (only if Docker unavailable)

If you **cannot** use Docker (e.g. migrations must run from CI on a host without compose):

```bash
cd /opt/steller-v2/steller-backend
DB_HOST=localhost DB_PORT=6432 dotnet ef database update --project Steller.EF --startup-project Steller.Api
```

Requires Postgres port 6432 mapped to host (127.0.0.1:6432). Do **not** change `.env` — use the env override for this command only.

---

## Why Docker first?

- **Same VPS:** Steller v1, v2, and OpenClaw share the VPS. Docker migrations don't touch other services.
- **Config safety:** `.env` stays `DB_HOST=steller-v2-postgres` for the API. No risk of breaking the running app.
- **Consistency:** Migrations run in the same network context as the app — if the app connects, migrations connect.

---

## References

- **Backend:** `steller-backend/CONTRIBUTING.md` — Database migrations section
- **Backend:** `steller-backend/RUN_MIGRATIONS.md` — Full protocol
- **Compose:** `docker-compose.yml` — `steller-migrations` service under `profiles: tools`
