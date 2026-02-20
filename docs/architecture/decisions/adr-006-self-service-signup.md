# ADR-006: Self-Service Partner Signup

**Status:** Accepted  
**Date:** 2026-02-20  
**Deciders:** Dev Agent (PLG Phase 1)

---

## Context

PLG requires self-service partner onboarding so developers can register without Admin intervention. Manual flow today: Admin creates partner and key via POST /api/admin/partners/{id}/keys.

---

## Decision

- **Endpoint:** `POST /api/public/signup` (no auth). ApiKeyMiddleware skips /api/public.
- **Request:** companyName (required), email (required), useCase (optional).
- **Flow:** Create Partner → link to sandbox with fake USD only (no real money on signup) → IApiKeyService.GenerateKeyAsync → return partnerId, apiKey, walletBalance, currencyCode, message. Real money only via explicit top-up request.
- **Duplicate email:** 409 Conflict. API key returned once only (same pattern as Admin).
- **Config:** Sandbox balance (fake USD) only on signup; real-money top-up is request-based only.

---

## Consequences

- **Positive:** Developers can sign up and get an API key without Admin. Same key storage (KeyHash) and validation as Admin-created keys.
- **Negative:** Public endpoint; consider rate limiting or captcha for production if abuse appears.
- **Security:** No plain API key stored; key shown once. Email uniqueness enforced.

---

## Related

- apis.yaml: steller-v2-public-api
- PLG plan: self_service_signup task
- GAP-001–005 resolution (email, Partners, Wallet, Redis, Wallet API)
