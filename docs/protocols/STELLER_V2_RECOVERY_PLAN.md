# Steller v2 Disaster Recovery Plan

**Purpose:** Rebuild Steller v2 on a new VPS if laptop and/or current VPS are lost. Code is on GitHub; credentials and DB must be restored from secure backups.

---

## 1. What You Need (Backup Before Disaster)

| Item | Where to Store | How Often |
|------|----------------|-----------|
| **`.env` file** | Password manager or encrypted file (e.g. 1Password, Bitwarden, VeraCrypt volume). Do **not** put in Git. | After any credential change |
| **PostgreSQL dump** | External storage (S3, Backblaze B2, encrypted backup, or separate backup server) | Daily recommended |
| **SSH key for GitHub** | Password manager (public + private) or new key generated on new machine | One-time or when rotated |

See **§4 Backup Script** for automated DB backup.

---

## 2. New VPS Prerequisites

- Linux (Ubuntu 22.04+ or similar)
- Docker and Docker Compose
- Git
- SSH access (port 22)

```bash
# Ubuntu example
sudo apt update && sudo apt install -y docker.io docker-compose-plugin git
sudo usermod -aG docker $USER
# Log out and back in for docker group
```

---

## 3. Recovery Steps (New VPS)

### Step 1: Clone Repositories

```bash
# Create structure (adjust paths to match your setup)
sudo mkdir -p /opt/steller-v2
sudo chown $USER:$USER /opt/steller-v2
cd /opt/steller-v2

# Clone backend (requires SSH key added to GitHub)
git clone git@github.com:manni17/steller-backend.git steller-backend

# Clone ops (deploy, docs)
git clone git@github.com:manni17/steller-v2-ops.git /root/steller-v2-ops
# If using deploy from ops as subdir:
cp -r /root/steller-v2-ops/deploy/* /opt/steller-v2/  # or symlink as needed
```

**Alternative:** If steller-v2-ops contains a full deploy layout, follow its README. Current layout: `steller-v2/` has docker-compose at root; `steller-backend` is a sibling/subdir. See `docs/INDEX.yaml` → repos.

### Step 2: Restore `.env`

1. Retrieve `.env` from your secure backup (password manager, encrypted store).
2. Place at `/opt/steller-v2/.env`.

**Required variables (see `steller-backend/.env.example`):**

| Variable | Purpose |
|----------|---------|
| `DB_NAME`, `DB_USERNAME`, `DB_PASSWORD` | PostgreSQL |
| `DB_HOST=steller-v2-postgres` | For app inside Docker |
| `REDIS_PASSWORD` | Redis (optional, can be empty) |
| `BAMBOO_USERNAME`, `BAMBOO_PASSWORD`, `BAMBOO_ACCOUNT_ID`, `BAMBOO_ACCOUNT_ID_INT` | Bamboo API |
| `EXTERNAL_API_CLIENT_ID`, `EXTERNAL_API_CLIENT_SECRET` | Bamboo (same as BAMBOO_* for most setups) |
| `API_KEY_HMAC_SECRET` | API key hashing (min 32 chars) |
| `JWT_KEY`, `JWT_ISSUER`, `JWT_AUDIENCE` | Dashboard auth |
| `PIN_ENCRYPTION_KEY` | Gift card PIN encryption |

### Step 3: Restore Database (or Start Fresh)

**Option A: Restore from backup**

```bash
cd /opt/steller-v2
docker compose up -d postgres
# Wait for Postgres healthy
docker exec -i steller-v2-postgres pg_restore -U steller_v2_user -d steller_v2 --clean --if-exists < /path/to/your/backup.dump
# Or if using SQL format:
docker exec -i steller-v2-postgres psql -U steller_v2_user -d steller_v2 < /path/to/your/backup.sql
```

**Option B: Fresh start**

```bash
cd /opt/steller-v2
docker compose --profile tools run --rm steller-migrations
```

### Step 4: Start the Stack

```bash
cd /opt/steller-v2
docker compose up -d
```

### Step 5: Verify

- Health: `curl http://localhost:6091/api/health`
- Hangfire: `http://YOUR_VPS_IP:6091/hangfire` (if enabled)
- Catalog sync: Check logs for `BrandBackgroundService`

---

## 4. Backup Script

Run regularly (e.g. cron daily) to create PostgreSQL dumps.

**Script:** `scripts/backup-steller-v2-db.sh` (in this repo)

```bash
./scripts/backup-steller-v2-db.sh /root/backups/steller-v2
```

**Cron example (daily at 2am):**

```
0 2 * * * /path/to/steller-v2-ops/scripts/backup-steller-v2-db.sh /root/backups/steller-v2
```

---

## 5. Credential Backup Checklist

See `docs/protocols/ENV_BACKUP_CHECKLIST.md`. Store `.env` in password manager or encrypted backup. **Never** commit to Git.

---

## 6. References

- **Migrations:** `docs/RUN_MIGRATIONS.md`
- **GitHub push:** `docs/protocols/GITHUB_PUSH_RUNBOOK.md`
- **Repos:** `docs/INDEX.yaml` → `repos:`
- **VPS navigation:** `.cursor/rules/claw-vps-navigation.md`
