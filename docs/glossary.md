# Glossary

**Purpose:** Definitions of terms used across Steller v2 architecture documentation.

**Last Updated:** 2026-02-18

---

## A

**API Key**
- Authentication credential for Partner API endpoints
- Stored as `KeyHash` (HMAC-SHA256) in `ApiClientSecrets` table
- Plain key only returned once when created via Admin API
- Never stored in plain text in database

**ApiClientSecrets**
- Database table storing API key hashes
- Columns: `PartnerId`, `KeyHash` (HMAC-SHA256)
- Do not query for plain key (does not exist)

**ApiKeyMiddleware**
- Middleware that validates `x-api-key` header
- Sets `context.User` with `PartnerId` claims
- Runs before `[Authorize]` attribute

---

## B

**Bamboo**
- External vendor API for placing gift card orders
- Integrated via Bamboo client library
- Circuit breaker pattern implemented for resilience

**Bamboo Client**
- .NET Standard library for integrating with Bamboo API
- Used by Hangfire jobs to place orders

---

## C

**Circuit Breaker**
- Pattern to prevent cascading failures
- Opens when vendor (Bamboo) is unavailable
- Order failures due to open circuit are expected behavior (not bugs)

**Container Port**
- Port inside Docker container
- Example: API container port 8080

**Context.User**
- ASP.NET Core user identity set by middleware
- Contains claims (`PartnerId`, `Role`)
- Used by `[Authorize]` attribute

---

## D

**Docker Compose**
- Tool for defining and running multi-container Docker applications
- Steller v2 uses: `/opt/steller-v2/docker-compose.yml`
- Project name: `steller-v2`

---

## H

**Hangfire**
- Background job processing library
- Runs embedded in API process (not separate container)
- Processes `PlaceBambooOrderJob` for order placement

**Host Port**
- Port exposed on VPS host
- Example: API host port 6091 (maps to container port 8080)

---

## I

**Idempotency**
- Property where duplicate requests produce same result
- Implemented via `referenceId` field in order requests
- Stored as `RequestId` in `Orders` table

---

## K

**KeyHash**
- HMAC-SHA256 hash of plain API key
- Stored in `ApiClientSecrets` table
- Used for API key validation (one-way hash)

---

## O

**Order Status**
- `Pending`: Order created, awaiting job processing
- `Completed`: Order successfully placed with vendor
- `Failed`: Order failed (vendor error, circuit breaker, etc.)

---

## P

**Partner API**
- API endpoints for partners (`/api/brand/*`, `/api/orders`)
- Authentication: `x-api-key` header
- Returns partner-specific catalog and processes orders

**PartnerId**
- Unique identifier for partner
- Stored in API key claims (set by ApiKeyMiddleware)
- Used to filter catalog and associate orders

**PlaceBambooOrderJob**
- Hangfire job that places orders with Bamboo vendor
- Runs asynchronously after order creation
- Updates order status based on vendor response

---

## R

**referenceId**
- Idempotency key sent in order request body
- Optional field in `POST /api/orders`
- Stored as `RequestId` in `Orders` table

**RequestId**
- Database column in `Orders` table
- Stores same value as `referenceId` from request
- Used for idempotency checks

---

## S

**Steller v2**
- New Steller platform (.NET 9 + PostgreSQL + Redis + Hangfire)
- Isolated Docker stack on shared VPS
- System ID: `steller-v2`

**Steller Legacy**
- Legacy Steller system running alongside v2
- System ID: `steller-legacy`
- Protected system (do not modify)

---

## X

**x-api-key**
- HTTP header for Partner API authentication
- Required for `/api/brand/*` and `/api/orders` endpoints
- Returns `401 Unauthorized` if missing or invalid

---

## Related Documentation

- Architecture: `docs/architecture/`
- API definitions: `docs/architecture/atlas/apis.yaml`
- Security: `docs/architecture/blueprints/security-auth.md`
