# .env Backup Checklist — Steller v2

**Purpose:** Ensure you have a secure backup of all credentials before disaster. Store in a password manager or encrypted file. **Never commit to Git.**

---

## Required Variables

| Variable | Description |
|----------|-------------|
| `DB_NAME` | PostgreSQL database name (e.g. `steller_v2`) |
| `DB_USERNAME` | PostgreSQL user |
| `DB_PASSWORD` | PostgreSQL password |
| `DB_HOST` | For Docker app: `steller-v2-postgres` |
| `REDIS_PASSWORD` | Redis auth (can be empty) |
| `BAMBOO_USERNAME` | Bamboo API client ID |
| `BAMBOO_PASSWORD` | Bamboo API secret |
| `BAMBOO_ACCOUNT_ID` | Bamboo Account ID (string) |
| `BAMBOO_ACCOUNT_ID_INT` | Bamboo Account ID (numeric) |
| `EXTERNAL_API_CLIENT_ID` | Same as BAMBOO_USERNAME typically |
| `EXTERNAL_API_CLIENT_SECRET` | Same as BAMBOO_PASSWORD typically |
| `API_KEY_HMAC_SECRET` | Min 32 chars for API key hashing |
| `JWT_KEY` | JWT signing key (min 32 chars) |
| `JWT_ISSUER` | JWT issuer (e.g. "Steller Dashboard") |
| `JWT_AUDIENCE` | JWT audience |
| `PIN_ENCRYPTION_KEY` | Base64 32-byte key for gift card PINs |

---

## Optional (if used)

| Variable | Description |
|----------|-------------|
| `EMAIL_SMTP_SERVER`, `EMAIL_SMTP_PORT` | SMTP |
| `EMAIL_SENDER_EMAIL`, `EMAIL_SENDER_PASSWORD` | SMTP credentials |
| `SENDGRID_API_KEY` | If using SendGrid |
| `SLACK_WEBHOOK_URL` | Alerting |

---

## Backup Actions

- [ ] Export `.env` from `/opt/steller-v2/.env` to secure store
- [ ] Update backup after any credential change
- [ ] Test restore on a disposable VM periodically
