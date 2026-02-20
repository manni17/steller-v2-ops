# Architecture Documentation

**Purpose:** AI-agent-first architecture documentation for Steller v2 system.

**Last Updated:** 2026-02-18

---

## Quick Start for Agents

1. **Start here:** Read `docs/INDEX.yaml` to find docs for your role/task
2. **System boundaries:** `systems.yaml` and `containers.yaml`
3. **Component catalog:** `atlas/INDEX.yaml` → load needed atlas files
4. **Deployment:** `blueprints/deployment.md`
5. **Decisions:** `decisions/adr-*.md`

---

## Directory Structure

```
docs/architecture/
├── README.md                    # This file
├── SCHEMAS.md                   # Schema definitions for all YAML files
├── systems.yaml                  # System boundaries and protection status
├── containers.yaml              # Container definitions and ports
├── atlas/                        # Component catalog
│   ├── INDEX.yaml               # Atlas entry point
│   ├── components.yaml          # Component catalog
│   ├── apis.yaml                # API surface definitions
│   └── dependencies.yaml        # Component dependencies
├── blueprints/                   # Deployment and design docs
│   ├── deployment.md            # Deployment architecture
│   ├── data-flow.md             # Data flow diagrams
│   └── security-auth.md         # Security and authentication
├── decisions/                    # Architecture Decision Records
│   ├── adr-001-isolate-v2-stack.md
│   ├── adr-002-port-allocation.md
│   ├── adr-003-api-key-hashing.md
│   └── adr-004-hangfire-embedded.md
└── diagrams/                     # Optional: Mermaid diagrams
```

---

## File Types

### YAML Files (Machine-Parseable)

- **systems.yaml**: Systems, boundaries, protected status
- **containers.yaml**: Containers, ports, compose projects
- **atlas/*.yaml**: Component catalog, APIs, dependencies

**Schema:** See `SCHEMAS.md` for expected structure.

### Markdown Files (Human-Readable)

- **blueprints/*.md**: Deployment, data flow, security
- **decisions/adr-*.md**: Architecture Decision Records
- **README.md**: This file

**Conventions:** See `SCHEMAS.md` for section naming conventions.

---

## Key Concepts

### Single Source of Truth

- **Systems and containers:** `systems.yaml`, `containers.yaml`
- **APIs:** `atlas/apis.yaml`
- **Components:** `atlas/components.yaml`
- **Dependencies:** `atlas/dependencies.yaml`

**Do not duplicate facts** - reference canonical files instead.

### Protected Systems

Systems marked `protected: true` in `systems.yaml`:
- **DO NOT MODIFY** without explicit user request
- Containers marked `protected: true` in `containers.yaml`
- See guardrails in each file

### PLG Execution

- **Execution plan:** `docs/PLG_EXECUTION_PLAN.md` — phased PLG strategy execution (Phase 1–3, next steps)
- **Backlog:** `docs/BACKLOG_V2.md`

### Agent Protocols

Agent-specific protocols reference canonical sources:
- QA Protocol: `docs/STELLER_QA_AGENT_PROTOCOL_V2.md`
- VPS Navigation: `.cursor/rules/claw-vps-navigation.md`

---

## Usage Examples

### "What systems exist?"

```yaml
# Read: docs/architecture/systems.yaml
systems:
  - system_id: steller-v2
    protected: true
  - system_id: steller-legacy
    protected: true
```

### "What ports are reserved?"

```yaml
# Read: docs/architecture/containers.yaml
# Check host_port values for all containers
```

### "How do I call the Partner API?"

```yaml
# Read: docs/architecture/atlas/apis.yaml
# Find api_id: steller-v2-partner-api
# Check endpoints, auth_method, guardrails
```

### "Why was port 6091 chosen?"

```markdown
# Read: docs/architecture/decisions/adr-002-port-allocation.md
# See Context, Decision, Consequences
```

---

## Maintenance

- **Update frequency:** When systems/containers/APIs change
- **Validation:** YAML files must be valid (check with `yamllint`)
- **Cross-references:** Ensure `system_id` and `component_id` references exist
- **Guardrails:** Keep "Do not" sections updated with current constraints

---

## Related Documentation

- **Documentation index:** `docs/INDEX.yaml`
- **Glossary:** `docs/glossary.md`
- **Schemas:** `docs/architecture/SCHEMAS.md`

---

## For Humans

This documentation is designed **AI-agent-first** but remains human-readable. YAML files provide machine-parseable facts; Markdown files provide context and explanations. All files are version-controlled and should be updated when the system changes.
