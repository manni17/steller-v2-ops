# PLG P0 Gap Resolution Report (GAP-001 – GAP-005)

**Owner:** Dev Agent  
**Date:** 2026-02-20  
**Reference:** Tier 2 — `docs/BACKLOG_V2.md` (TPM-owned); `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md`

---

## Summary

| Gap ID | Category | Status | Blocker for |
|--------|----------|--------|-------------|
| GAP-001 | Email service | ✅ VERIFIED | Self-service signup, activation emails, re-engagement |
| GAP-002 | Partners schema | ✅ VERIFIED | Onboarding columns, signup source |
| GAP-003 | Wallet schema | ✅ VERIFIED | Wallet init (sandbox fake USD), top-up flow |
| GAP-004 | Redis queue | ✅ VERIFIED | Analytics async logging |
| GAP-005 | Wallet API | ✅ VERIFIED | Wallet balance display (sandbox vs real) |

**Conclusion:** All P0 gaps are verified. PLG Phase 1 implementation can proceed (self-service signup, api_logging, analytics_infrastructure, api_documentation).

---

## GAP-001: Email service existence and configuration

**Verification:** Codebase and `.env` (steller-backend and `/opt/steller-v2/.env`).

| Item | Result |
|------|--------|
| **Interface** | `Steller.Api/Interfaces/IEmailService.cs` — exists |
| **Implementation** | `Steller.Api/Services/EmailService.cs` — uses `EmailSettingsDto` (SmtpServer, SmtpPort) |
| **Send path** | `Steller.Infrastructure/Services/Notifications/SendGridEmailSender.cs` — implements `IEmailSender` |
| **Registration** | `Program.cs`: `IEmailService` → `EmailService`; when `SENDGRID_API_KEY` set → `SendGridEmailSender`, else `MockEmailSender` |
| **Config (.env)** | `EMAIL_SMTP_SERVER`, `EMAIL_SMTP_PORT`, `SENDGRID_API_KEY` (optional) |

**Config in Program.cs:** `EmailSettings:SmtpServer` ← `EMAIL_SMTP_SERVER`, `EmailSettings:SmtpPort` ← `EMAIL_SMTP_PORT`.

**Status:** VERIFIED. Email service exists; production uses SendGrid when key is set; SMTP settings available for fallback. PLG can use `IEmailService` / `IEmailSender` for welcome and activation emails.

---

## GAP-002: Partners table schema

**Verification:** EF model `Steller.Core/Models/Partner.cs` and migration `Steller.EF/Migrations/20260213002616_Initial_v2.cs`.

**Table:** `Partners`

| Column | Type | Nullable | Notes |
|--------|------|----------|--------|
| Id | integer | NO | PK, IdentityByDefaultColumn |
| BusinessName | text | NO | |
| RegistrationNumber | text | NO | |
| Logo | text | NO | |
| Address | text | NO | |
| Phone | text | NO | |
| Email | text | NO | |
| RevenueSharePercentage | numeric(5,2) | NO | |
| ClientSecret | text | YES | |
| WebhookUrl | text | YES | |
| WebhookSecret | text | YES | |
| IsActive | boolean | NO | (BaseEntity) |
| CreatedAt | timestamp with time zone | NO | |
| UpdatedAt | timestamp with time zone | NO | |
| CreatedBy | integer | YES | |
| UpdatedBy | integer | YES | |

**Not present (to add for PLG):** `OnboardingState`, `SignupSource`, `ReferralCode`, `ReferredByPartnerId`. These require a new migration when implementing self-service signup.

**Status:** VERIFIED. Schema known; PLG can add onboarding/signup columns via EF migration.

---

## GAP-003: Wallet system schema and credit/debit

**Verification:** EF model `Wallet.cs`, `WalletHistory.cs`, migration, and `WalletService.cs`.

**Table:** `Wallets`

| Column | Type | Nullable | Notes |
|--------|------|----------|--------|
| Id | uuid | NO | PK |
| PartnerId | integer | NO | FK → Partners.Id |
| AvailableBalance | numeric(18,4) | NO | Balance field |
| CurrencyId | integer | NO | FK → Currencies |
| WalletNumber | text | YES | |
| WalletTypeId | integer | NO | FK → WalletTypes |
| CreatedAt, UpdatedAt, CreatedBy, UpdatedBy | (standard) | | |

**Credit/debit:** `IWalletService.CreditWalletAsync(partnerId, amount, description, referenceId)` and `DebitWalletAsync(...)`. Implemented in `WalletService` with atomic SQL update on `Wallets.AvailableBalance` and append to `WalletHistories`. Wallet created on first credit if missing (WalletService fallback).

**Status:** VERIFIED. Sandbox/fake USD for signup can use `CreditWalletAsync`; balance is `AvailableBalance`. Real money only via top-up request.

---

## GAP-004: Redis queue operations support

**Verification:** Codebase uses `StackExchange.Redis`; `RedisRateLimiterService` uses `IDatabase`, SortedSet, and Lua.

| Item | Result |
|------|--------|
| **Package** | `StackExchange.Redis` (via Program.cs, RedisRateLimiterService) |
| **Current use** | Rate limiting: SortedSet + Lua script (sliding window) |
| **Redis capability** | Redis supports LIST (LPUSH/RPUSH/LPOP), SortedSet, Pub/Sub. Queue pattern: producer LPUSH, consumer BRPOP (or Hangfire job reading from list). |

**Status:** VERIFIED. Redis is in use and supports list/queue operations. Analytics middleware can push to a Redis list and a Hangfire job can pop and write to PostgreSQL (async pattern).

---

## GAP-005: Wallet API response format

**Verification:** `WalletController`, `IWalletService.GetWalletAsync`, `WalletDto`.

**Endpoints:**
- **Partner (self):** `GET /api/wallet/me` — auth: Partner (JWT with PartnerId). Returns `WalletDto`.
- **Admin:** `GET /api/wallet/{partnerId}` — auth: Admin. Returns `WalletDto`.

**Response body (WalletDto):**

```json
{
  "id": "uuid",
  "partnerId": 1,
  "availableBalance": 0.0000,
  "currencyCode": "USD"
}
```

**Type:** `Steller.Core.DTOs.Wallet.WalletDto` — `(Guid Id, int PartnerId, decimal AvailableBalance, string CurrencyCode)`.

**Status:** VERIFIED. Documented for PLG signup response (e.g. `walletBalance` from `result.Value.AvailableBalance`, `currencyCode` from `result.Value.CurrencyCode`).

---

## Implementation Status (2026-02-17)

- **PLG Phase 1:** ✅ COMPLETED — self-service signup, api_logging, analytics_infrastructure, api_documentation
- **PLG Phase 2:** ✅ COMPLETED — developer_portal, sandbox_environment, activation_tracking, growth_metrics_dashboard
- **Execution plan:** See `docs/PLG_EXECUTION_PLAN.md` for Phase 3 next steps (multi-user workspaces, partner log explorer, IP allowlisting, webhooks)

---

## Next steps (Dev Agent)

1. **PLG Phase 3** next: multi-user workspaces, partner_log_explorer, ip_allowlisting, webhooks (per `docs/PLG_EXECUTION_PLAN.md`).
2. **Self-service signup:** Add migration for `Partners` (OnboardingState, SignupSource, etc.); use `IWalletService.CreditWalletAsync` for sandbox fake USD only (real money via top-up request only); use `IEmailService`/`IEmailSender` for welcome email; return `WalletDto`-derived balance in signup response.
3. **Analytics:** Use Redis LIST or Hangfire fire-and-forget for request logging; do not write to PostgreSQL synchronously in middleware.

---

## References

- **PLG plan:** `.cursor/plans/steller_product_lead_growth_strategy_e28a2ede.plan.md` (Gap Analysis section)
- **PRD:** `docs/product/PRD_STELLER_V2_STABILIZATION_AND_GROWTH.md`
- **Backlog:** `docs/BACKLOG_V2.md` (TPM-owned; read-only for Dev)
