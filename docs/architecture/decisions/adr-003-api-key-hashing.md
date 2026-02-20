# ADR-003: API Key Hashing (HMAC-SHA256)

**Status:** Accepted  
**Date:** 2026-02-18  
**Deciders:** Architecture Team

---

## Context

Steller v2 needs to authenticate partners via API keys. Security requirements:
- API keys must not be stored in plain text
- Keys must be validated efficiently
- Keys should be one-way hashed (cannot be retrieved)
- Plain key only returned once when created

**Security Considerations:**
- Database compromise should not expose plain API keys
- Keys should be validated quickly (low latency)
- Key generation should be secure

---

## Decision

Store API keys as HMAC-SHA256 hashes in `ApiClientSecrets` table:
- **Storage:** `KeyHash` column (HMAC-SHA256 of plain key)
- **Table:** `ApiClientSecrets` (not `Partners` table)
- **Validation:** Compute hash of provided key, compare with stored hash
- **Creation:** Admin API generates plain key, stores hash, returns plain key once

**Implementation:**
- Plain API key generated when created via Admin API
- HMAC-SHA256 hash computed and stored in `KeyHash` column
- Plain key returned in response (client must store securely)
- Plain key never stored in database
- Validation: hash provided key, compare with stored hash

---

## Consequences

### Positive

- **Security** - Plain keys never stored, cannot be retrieved from database
- **One-way** - Even if database compromised, plain keys not exposed
- **Efficient** - Single hash computation per validation (fast)
- **Standard** - HMAC-SHA256 is industry-standard approach

### Negative

- **Key recovery impossible** - If plain key lost, must create new key
- **Hash comparison required** - Must compute hash for every validation (acceptable overhead)
- **Documentation critical** - Agents must understand keys are hashed (not plain text)

### Neutral

- Slightly more complex than plain text storage (but much more secure)
- Requires careful documentation so agents don't try to query for plain keys

---

## Alternatives Considered

### Alternative 1: Plain Text Storage

**Rejected because:**
- Security risk - database compromise exposes all keys
- Violates security best practices
- Not acceptable for production systems

### Alternative 2: Encrypted Storage (AES)

**Rejected because:**
- Requires key management (encryption key storage)
- Can be decrypted (defeats purpose if encryption key compromised)
- More complex than hashing
- HMAC-SHA256 is sufficient for one-way storage

### Alternative 3: bcrypt/Argon2 (Password Hashing)

**Rejected because:**
- Designed for passwords (slow by design)
- API key validation needs to be fast
- HMAC-SHA256 is faster and appropriate for API keys

---

## Related Decisions

- ADR-001: Isolate v2 stack (security is part of isolation)
- Security blueprint: `docs/architecture/blueprints/security-auth.md`

---

## Guardrails for Agents

**CRITICAL:**
- Do not query for plain API keys - they do not exist in database
- Do not run: `SELECT "ApiKey" FROM "Partners"` - no such column exists
- Keys are in `ApiClientSecrets` table as `KeyHash` (HMAC-SHA256)
- Plain key only returned once when created via Admin API
- Store returned plain key securely (only returned once)

---

## Notes

This decision ensures API keys are stored securely while maintaining fast validation. The one-way hash approach is standard for API key storage and provides strong security guarantees.
