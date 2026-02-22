# GitHub Push Runbook — VPS Configuration

**Purpose:** How to push Steller v2 code from this VPS to GitHub. Reference after SSH key setup or when push fails.

---

## 1. Repos and Remotes

| Repo | Path | Remote | Push command |
|------|------|--------|--------------|
| **steller-backend** | `/opt/steller-v2/steller-backend` | `origin` → `git@github.com:manni17/steller-backend.git` | `git push origin main` |
| **steller-v2-ops** | `/root/steller-v2-ops` | `origin` → `github.com-steller-ops:manni17/steller-v2-ops.git` | `git push origin main` |

---

## 2. SSH Key (Required for steller-backend)

Backend uses **SSH** (not HTTPS). Key must be loaded for push to work.

### Key location
- `~/.ssh/id_ed25519` (private)
- `~/.ssh/id_ed25519.pub` (public)

### Add public key to GitHub
1. `cat ~/.ssh/id_ed25519.pub`
2. Copy output
3. GitHub → **Settings → SSH and GPG keys → New SSH key**
4. Paste and save

### Test authentication
```bash
ssh -T git@github.com
```
Expected: `Hi manni17! You've successfully authenticated...`

### If passphrase forgotten
1. Generate new key: `ssh-keygen -t ed25519 -f ~/.ssh/id_ed25519_new -C "your@email.com"`
2. Add `id_ed25519_new.pub` to GitHub (Settings → SSH keys)
3. Replace old key: `mv ~/.ssh/id_ed25519 ~/.ssh/id_ed25519.old && mv ~/.ssh/id_ed25519_new ~/.ssh/id_ed25519 && mv ~/.ssh/id_ed25519_new.pub ~/.ssh/id_ed25519.pub`
4. Test: `ssh -T git@github.com`

---

## 3. Push Commands

### steller-backend
```bash
cd /opt/steller-v2/steller-backend
git add .
git status
git commit -m "feat(scope): description"
git push origin main
```

### steller-v2-ops (docs, backlog, runbooks)
```bash
cd /root/steller-v2-ops
git add docs/
git status
git commit -m "docs: update backlog"
git push origin main
```

---

## 4. Troubleshooting

| Symptom | Fix |
|---------|-----|
| `Permission denied (publickey)` | SSH key not in GitHub or not loaded. Run `ssh -T git@github.com`; add key if missing. |
| `could not read Username for 'https://github.com'` | Remote is HTTPS. Switch to SSH: `git remote set-url origin git@github.com:manni17/steller-backend.git` |
| `Enter passphrase for key` (and forgot it) | Generate new key (see §2 above) and add to GitHub. |

---

## 5. Reference

- Commit practice: `docs/GITHUB_COMMIT_BEST_PRACTICE_AND_GAP.md`
- Repo list: `docs/INDEX.yaml` → `repos:`
