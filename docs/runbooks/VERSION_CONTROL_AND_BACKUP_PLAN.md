# Version Control and Backup Plan — Steller v2

**Purpose:** Manage change (source control) and protect against loss (backup) for the Steller v2 system.  
**Last Updated:** 2026-02-22

---

## 1. Repos and Their Role

| Repo | Path | Remote | Contents |
|------|------|--------|----------|
| **steller-backend** | `/opt/steller-v2/` | github.com/manni17/steller-backend | API, EF, Hangfire, Bamboo, tests |
| **steller-v2-ops** | `/root/steller-v2-ops/` | github.com/manni17/steller-v2-ops | Deploy (docker-compose, scripts) + docs tree |

**Single source of truth:**
- App code → `steller-backend`
- Ops, docs, scripts, runbooks → `steller-v2-ops`

---

## 2. What Goes Where (Version Control)

### steller-backend
- `.NET` solution, projects, migrations
- Tests, `appsettings*.json` templates (no secrets)
- EF migrations, Hangfire, Bamboo integration

### steller-v2-ops
- `docs/` — architecture, qa, product, runbooks, incidents
- Scripts — `run_e2e_api_journey.sh`, deploy, migrations
- `docker-compose*.yml`, env templates (no secrets)
- Product assets — Dashboard template, READMEs

---

## 3. Single Source of Truth: Avoid Drift

**Problem:** Files in `/root/docs/` are not automatically in `steller-v2-ops`. Copy into ops and commit.

| Workspace path | Canonical (versioned) path |
|----------------|----------------------------|
| `/root/docs/...` | `steller-v2-ops/docs/...` |
| `/root/run_*.sh` | `steller-v2-ops/` or `steller-v2-ops/scripts/` |

**Rule:** Work in `/root/` if needed, but **commit from `steller-v2-ops`**. New docs/scripts → copy to ops → add → commit → push.

---

## 4. Backup Strategy (Protect Against Loss)

GitHub = remote backup. To protect against VPS loss:

| Action | Cadence | Command |
|--------|---------|---------|
| Push backend | After any code change | `cd /opt/steller-v2 && git push` |
| Push ops | After any doc/script change | `cd /root/steller-v2-ops && git push` |
| Verify remotes | Weekly | `git remote -v` and `git status` |

**What is backed up when pushed:**
- Full history (commits, branches)
- All tracked files in each repo

**What is NOT in Git (must back up separately if needed):**
- Secrets (`.env`, API keys, DB passwords)
- Database dumps
- Redis data
- Runtime logs

---

## 5. Commit Discipline (Manage Change)

| When | What to do |
|------|------------|
| Code change | Commit with clear message; push to `steller-backend` |
| Doc/script change | Copy to ops if created in `/root/`; commit; push to `steller-v2-ops` |
| New artifact | Add to ops; update `docs/INDEX.yaml` if it's a persistent reference |

**Commit message format:**  
`type: short description` (e.g. `docs: add E2E runbook`, `feat: add wallet credit endpoint`)

---

## 6. Recovery Procedure

**If VPS is lost:**

1. Clone `steller-backend` → restore app code
2. Clone `steller-v2-ops` → restore docs, scripts, compose files
3. Restore DB from separate backup (PostgreSQL dump)
4. Restore secrets from secure store
5. Run `docker compose` per ops docs

**If a file is deleted locally:**  
`git checkout -- <path>` or `git restore <path>`

**If unsure what’s uncommitted:**  
`git status` in both repos

---

## 7. Checklist: Add New Artifact to Backup

- [ ] Copy file to `steller-v2-ops` (correct subfolder: `docs/qa/`, `docs/product/`, etc.)
- [ ] `git add <path>`
- [ ] `git commit -m "docs: add <description>"`
- [ ] `git push`
- [ ] Update `docs/INDEX.yaml` if it's a persistent reference

---

## 8. Artifacts Pending Backup (as of 2026-02-22)

These exist in `/root/` but may not yet be in `steller-v2-ops`:

- [ ] `docs/product/steller-dashboard-template.html`
- [ ] `docs/product/STELLER_DASHBOARD_TEMPLATE_README.md`
- [ ] Any other new docs in `/root/docs/` not yet copied to ops
