# Architecture Documentation Schemas & Conventions

**Purpose:** This document defines the structure and conventions for all architecture documentation files. Agents can reference this to understand expected formats.

**Last Updated:** 2026-02-18

---

## YAML File Schemas

### systems.yaml

```yaml
# Schema: List of systems with boundaries and protection status
systems:
  - system_id: string          # Unique identifier (e.g., "steller-v2", "steller-legacy")
    name: string               # Human-readable name
    status: string             # "production" | "development" | "deprecated"
    protected: boolean         # true = DO NOT MODIFY without explicit user request
    description: string        # Brief description
    boundaries:
      external_actors: []      # List of external systems/users that interact
      integrations: []        # External services (e.g., "Bamboo")
    location: string           # File system path (e.g., "/opt/steller-v2/")
    compose_file: string       # Path to docker-compose.yml if applicable
```

### containers.yaml

```yaml
# Schema: Container definitions with ports and compose project info
systems:
  - system_id: string          # Matches systems.yaml
    containers:
      - container_name: string # Docker container name
        service_name: string   # Docker Compose service name
        type: string          # "api" | "database" | "cache" | "queue" | "dashboard"
        host_port: integer     # Host port (null if internal-only)
        container_port: integer # Container port
        protocol: string       # "http" | "tcp" | "udp"
        compose_file: string   # Path to docker-compose.yml
        compose_project: string # Docker Compose project name
        protected: boolean     # true = DO NOT MODIFY without explicit user request
```

### atlas/components.yaml

```yaml
# Schema: Component catalog
components:
  - component_id: string      # Unique identifier
    name: string               # Human-readable name
    system_id: string          # Owning system (matches systems.yaml)
    type: string              # "api" | "database" | "cache" | "job-processor" | "client"
    description: string        # Brief description
    doc_path: string           # Path to detailed documentation (optional)
    code_path: string          # Path to source code (optional)
    technology: string         # e.g., ".NET 9", "PostgreSQL 15", "Redis 7"
```

### atlas/apis.yaml

```yaml
# Schema: API surface definitions
apis:
  - api_id: string            # Unique identifier
    name: string              # Human-readable name
    system_id: string         # Owning system
    base_path: string         # e.g., "/api"
    auth_method: string        # "x-api-key" | "jwt" | "none" | "basic"
    endpoints:
      - path: string          # e.g., "/api/orders"
        method: string        # "GET" | "POST" | "PUT" | "DELETE"
        auth_required: boolean
        description: string
    guardrails: []            # List of "do not" rules (e.g., "API keys are hashed; never query for plain key")
    openapi_ref: string       # Path to OpenAPI spec (optional)
```

### atlas/dependencies.yaml

```yaml
# Schema: Component dependencies
dependencies:
  - from_component_id: string  # Component that depends on
    to_component_id: string    # Component being depended upon
    dependency_type: string   # "database" | "api" | "cache" | "queue" | "external-service"
    description: string        # Brief description of the dependency
```

---

## Markdown File Conventions

### Blueprint Files (deployment.md, data-flow.md, security-auth.md)

**Required Sections:**
- `## Overview` - High-level description
- `## [Topic]` - Main content sections (e.g., `## Deployment`, `## Port Map`, `## Auth Model`)
- `## Agent-Relevant` or `## Do Not` - Guardrails and constraints for agents
- `## Quick Reference` - Table format for key facts

**Table Format:**
- Use pipe-separated tables with headers
- First column: Key/Item name
- Second column: Value/Description

### ADR Files (adr-XXX-*.md)

**Required Headers:**
- `## Context` - Why this decision was necessary
- `## Decision` - What was decided
- `## Consequences` - Positive and negative impacts

**Optional Headers:**
- `## Alternatives` - Options considered and why rejected
- `## Status` - "Accepted" | "Superseded by ADR-XXX" | "Deprecated"

**File Naming:** `adr-XXX-short-description.md` (e.g., `adr-001-isolate-v2-stack.md`)

---

## Index File Format

### docs/INDEX.yaml

```yaml
# Schema: Role/task → list of documentation paths
roles:
  qa:
    - docs/STELLER_QA_AGENT_PROTOCOL_V2.md
    - docs/features-master-v1.md  # Master features doc (if exists)
  architecture:
    - docs/architecture/systems.yaml
    - docs/architecture/containers.yaml
    - docs/architecture/atlas/*.yaml
  deploy:
    - docs/architecture/blueprints/deployment.md
    - docs/architecture/blueprints/data-flow.md
  security:
    - docs/architecture/blueprints/security-auth.md
  decisions:
    - docs/architecture/decisions/adr-*.md
```

---

## Glossary Format

### docs/glossary.md

**Format:** Term → Definition pairs, organized alphabetically or by category.

```markdown
## [Category]

| Term | Definition |
|------|------------|
| referenceId | Idempotency key sent in API request body |
| RequestId | Database column storing the same value as referenceId |
```

---

## Agent Protocol Pattern

All agent protocols follow this structure:

1. **Identity & Mission** - Who the agent is and what it does
2. **CRITICAL / NEVER / MUST** - Explicit guardrails
3. **Execution Protocol** - Step-by-step procedures
4. **Quick Reference** - Table format for key facts
5. **Canonical Sources** - Links to architecture YAML files

---

## Validation Rules

- YAML files must be valid YAML (can be validated with `yamllint` or similar)
- All `system_id` references must exist in `systems.yaml`
- All `component_id` references in dependencies must exist in `components.yaml`
- Port numbers must be unique across all systems (except internal-only ports)
- Protected systems/containers must be clearly marked
