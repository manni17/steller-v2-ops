# Steller Development Index

**Owner:** Dev Agent  
**Purpose:** Single entry point for backend development. Load this first when adding endpoints, migrations, jobs, or porting features from Legacy.  
**Last Updated:** 2026-02-20

---

## How to Use

**Quick guide:** Load this index as Dev Agent. Pick role `developer` from `docs/INDEX.yaml`. Canonical sources: `apis.yaml`, backend `AGENTS.md`, `ARCHITECTURE_RULES.md`. Before code: update apis.yaml; run migrations via Docker; never edit applied migrations.

### Top 10 Common Prompts

| # | Prompt |
|---|--------|
| 1 | Add a new API endpoint for X; update apis.yaml first, then controller and DTO |
| 2 | Add an EF migration for entity Y; use Docker: `docker compose --profile tools run --rm steller-migrations` |
| 3 | Add a Hangfire background job for Z; follow data-flow.md and background job rules |
| 4 | Port feature from Legacy to V2; extract contract first, follow BAMBOO_MIGRATION_CONTRACT |
| 5 | Where is the API contract defined? → docs/architecture/atlas/apis.yaml |
| 6 | What are the golden flow and invariants? → steller-backend/ARCHITECTURE_RULES.md |
| 7 | Build and run integration tests; use QA_INTEGRATION_TEST_RUN_INSTRUCTIONS |
| 8 | Add new endpoint to OpenAPI/Swagger; ensure apis.yaml is in sync |
| 9 | Resolve PLG gap or GTM platform item; check BACKLOG_V2 §7a |
| 10 | Update CHANGELOG and ARCHITECTURE_RULES after behavior change |

---

## 1. Start Here

| Task | Load first | Then |
|------|------------|------|
| **Backend code** | [steller-backend/AGENTS.md](/opt/steller-v2/steller-backend/AGENTS.md) | TREE.md, ARCHITECTURE_RULES.md |
| **API contract** | [docs/architecture/atlas/apis.yaml](../architecture/atlas/apis.yaml) | apis.yaml is source of truth |
| **Migrations** | [docs/RUN_MIGRATIONS.md](../RUN_MIGRATIONS.md) | Always Docker; add-only migrations |
| **Integration tests** | [docs/qa/TESTING_INDEX.md](../qa/TESTING_INDEX.md) | QA_INTEGRATION_TEST_RUN_INSTRUCTIONS |
| **Bamboo / vendor** | [docs/integration/BAMBOO_MIGRATION_CONTRACT.md](../integration/BAMBOO_MIGRATION_CONTRACT.md) | Legacy → V2 parity |
| **Backlog** | [docs/BACKLOG_V2.md](../BACKLOG_V2.md) | GTM-P1–P7, B5–B12 |

---

## 2. Canonical Sources

- **API contracts:** `docs/architecture/atlas/apis.yaml`
- **Architecture rules:** `steller-backend/ARCHITECTURE_RULES.md`
- **Data flow / jobs:** `docs/architecture/blueprints/data-flow.md`
- **Auth model:** `docs/architecture/blueprints/security-auth.md`
- **Systems / ports:** `docs/architecture/systems.yaml`, `containers.yaml`

---

## 3. Key Rules (Do Not Violate)

| Rule | Why |
|------|-----|
| PartnerId from identity only | Never from URL, query, or body; tenant isolation |
| Add-only migrations | Never edit applied migration files; checksum errors |
| Migrations via Docker | `steller-v2-postgres` resolves only inside Docker network |
| Update docs before code | apis.yaml, ARCHITECTURE_RULES, CHANGELOG on behavior change |
| No plain API key in DB | Keys are KeyHash; obtain via Admin API or pre-provisioned secret |

---

## 4. Related

- **INDEX.yaml:** Role `developer` loads this index and linked docs.
- **QA spec-driven:** `docs/qa/TESTING_INDEX.md`, SPEC_INDEX.yaml, INVARIANTS.md
- **TPM:** `docs/TPM_AGENT_PROTOCOL.md` — doc alignment, anti-drift, phase tracking
- **PLG:** `docs/PLG_EXECUTION_PLAN.md` — phased execution
- **GTM:** `docs/qa/GO_TO_MARKET_READINESS_B2B_PARTNER_EXPERIENCE.md` — GTM-T/P items
