# Security & Authentication Blueprint

**Purpose:** Describes authentication models, credential storage, and security guardrails for Steller v2.

**Last Updated:** 2026-02-18

---

## Overview

Steller v2 uses two authentication methods:
1. **x-api-key** for Partner API endpoints
2. **JWT Bearer tokens** for Admin API endpoints

API keys are stored as hashed values (HMAC-SHA256) - plain keys never stored in database.

---

## Authentication Models

### Partner API Authentication (x-api-key)

**Endpoints:**
- `GET /api/brand/getCatalog`
- `POST /api/orders`

**Method:** API Key in `x-api-key` header

**Flow:**
1. Client sends request with `x-api-key` header
2. `ApiKeyMiddleware` validates key against `ApiClientSecrets` table
3. Middleware sets `context.User` with claims (`PartnerId`, `Role = "Partner"`)
4. `[Authorize]` attribute checks for authenticated user
5. Controller uses `PartnerId` from claims

**Validation:**
- API key looked up in `ApiClientSecrets` table
- Key compared as `KeyHash` (HMAC-SHA256 of plain key)
- If valid, user authenticated; if invalid, returns `401 Unauthorized`

**Guardrails:**
- **NEVER** query for plain API key - it does not exist in database
- Plain API key only returned once when created via Admin API
- Use pre-provisioned key from secure store or obtain via Admin API
- Do not run: `SELECT "ApiKey" FROM "Partners"` - no such column exists

---

### Admin API Authentication (JWT)

**Endpoints:**
- `POST /api/auth/login` (no auth required)
- `POST /api/admin/*` (JWT required)

**Method:** JWT Bearer token in `Authorization` header

**Flow:**
1. Admin calls `POST /api/auth/login` with credentials
2. Server returns JWT access token
3. Client includes `Authorization: Bearer <token>` in subsequent requests
4. JWT middleware validates token and sets `context.User`
5. Admin endpoints require authenticated user with admin role

**Token Storage:**
- Client-side (not stored in database)
- Token expiration handled by JWT middleware

---

## Credential Storage

### API Keys

**Storage:**
- Table: `ApiClientSecrets`
- Column: `KeyHash` (HMAC-SHA256 hash of plain key)
- Column: `PartnerId` (foreign key to Partners table)

**Creation:**
- Admin calls `POST /api/admin/partners/{partnerId}/keys`
- Server generates plain API key
- Server stores `KeyHash` in database
- Server returns plain key **once** in response
- Plain key never stored in database

**Retrieval:**
- Plain key cannot be retrieved from database (only hash exists)
- If key lost, create new key via Admin API
- Old key hash remains in database (for validation of old keys)

**Guardrails:**
- **CRITICAL:** Plain API keys never stored in database
- **CRITICAL:** Do not attempt to query for plain key
- **CRITICAL:** Store returned plain key securely (only returned once)

---

### Database Credentials

**Storage:**
- Environment variables in `/opt/steller-v2/.env`
- Variables: `DB_NAME`, `DB_USERNAME`, `DB_PASSWORD`

**Access:**
- Used by API container to connect to PostgreSQL
- Not exposed via API endpoints

**Guardrails:**
- Do not expose `.env` contents in logs, chat, or code
- Do not modify `.env` without explicit user request
- Reading `.env` only allowed when user explicitly requests debugging

---

## Security Patterns

### Idempotency

**Mechanism:**
- `referenceId` field in order request body
- Stored as `RequestId` in `Orders` table
- Prevents duplicate orders if request retried

**Usage:**
- Client generates unique `referenceId` (e.g., `auto-qa-20260218-120000`)
- Server checks for existing order with same `RequestId`
- If exists, returns existing order; if not, creates new order

---

### API Key Validation

**Process:**
1. Extract `x-api-key` header from request
2. Compute HMAC-SHA256 hash of provided key
3. Query `ApiClientSecrets` for matching `KeyHash`
4. If match found, load `PartnerId` and authenticate user
5. If no match, return `401 Unauthorized`

**Performance:**
- Indexed lookup on `KeyHash` column
- Single database query per request

---

## Agent-Relevant

### CRITICAL: Security Guardrails

**DO NOT:**
- Query for plain API keys: `SELECT "ApiKey" FROM "Partners"` (no such column)
- Store plain API keys in logs or chat
- Expose `.env` contents (passwords, secrets)
- Modify `.env` or credential storage without explicit user request

**DO:**
- Use pre-provisioned API keys from secure store
- Obtain API keys via Admin API if needed
- Use unique `referenceId` for test orders
- Validate API key format before using (if known)

---

### Testing Authentication

**Partner API:**
```bash
# Valid request
curl -X GET "http://localhost:6091/api/brand/getCatalog" \
  -H "x-api-key: YOUR_API_KEY"

# Missing key (should return 401)
curl -X GET "http://localhost:6091/api/brand/getCatalog"
```

**Admin API:**
```bash
# Login
curl -X POST "http://localhost:6091/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"..."}'

# Use token
curl -X POST "http://localhost:6091/api/admin/partners/1/keys" \
  -H "Authorization: Bearer <token>"
```

---

## Quick Reference

| Endpoint Type | Auth Method | Header | Storage |
|---------------|-------------|--------|---------|
| Partner API | x-api-key | `x-api-key: <key>` | KeyHash in ApiClientSecrets |
| Admin API | JWT | `Authorization: Bearer <token>` | Token in client (not DB) |
| Health | None | N/A | N/A |

| Credential Type | Storage Location | Format |
|-----------------|------------------|--------|
| API Keys | ApiClientSecrets.KeyHash | HMAC-SHA256 hash |
| DB Credentials | .env file | Plain text (protected file) |
| JWT Tokens | Client-side | JWT token |

---

## Related Documentation

- API definitions: `docs/architecture/atlas/apis.yaml`
- Components: `docs/architecture/atlas/components.yaml`
- Data flow: `docs/architecture/blueprints/data-flow.md`
