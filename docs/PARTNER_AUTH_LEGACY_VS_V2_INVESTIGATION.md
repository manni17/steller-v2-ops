# Partner Auth / API Key: Legacy Steller vs Steller v2

**Goal:** Compare legacy vs v2 partner auth (API key) for test design and parity.

## 1. Legacy

Auth mechanism (JWT or API key) and skip paths are implementation-specific.

## 2. V2

**ApiKeyMiddleware:** Requires x-api-key for /api except /api/auth, /api/admin, /api/health, /api/public, /api/brand/sync-catalog. JWT Admin bypass. Missing key: 401. Invalid/inactive: 403. **ApiKeyService:** ValidateKeyAsync (HMAC-SHA256 hash, ApiClientSecrets); GenerateKeyAsync (single active key per partner). Sets PartnerId in Claims and Items.

## 3. Divergence

Legacy: implementation-specific. V2: explicit x-api-key, skip list, Admin bypass, hash-only storage.

## 4. Comparison

| Item   | Legacy        | V2                |
|--------|---------------|-------------------|
| Header | Various       | x-api-key         |
| Skip   | —             | /api/admin, public, etc. |
| Admin  | —             | JWT Admin bypass  |

## 5. Recommendations

Treat v2 as target. Tests: P5_01–P5_06.

## 6. Conclusion

V2 is the reference for partner auth.
