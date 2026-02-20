# Steller V2 — Ops & Docs

Deploy assets and canonical docs for Steller V2 (backend lives in [manni17/steller-backend](https://github.com/manni17/steller-backend)).

## Contents

- **deploy/** — Docker Compose and scripts to run the v2 stack (API, Postgres, Redis, migrations).
- **docs/** — Backlog, INDEX, QA reports, architecture, product briefs, ADRs, and integration docs.

## Deploy

From this repo root, with [steller-backend](https://github.com/manni17/steller-backend) cloned as a sibling or path set in compose:

```bash
cd deploy
# Copy .env from your environment or create from backend .env.example
docker compose up -d
```

See `docs/architecture/blueprints/deployment.md` and backend `CONTRIBUTING.md` for migrations and full flow.

## Docs

- **BACKLOG_V2.md** — Canonical backlog.
- **INDEX.yaml** — Doc index.
- **qa/** — QA protocol, test reports, workflow docs.
- **architecture/** — Systems, containers, APIs, ADRs, blueprints.
- **product/** — PRD, agency brief, catalog/order investigations.
