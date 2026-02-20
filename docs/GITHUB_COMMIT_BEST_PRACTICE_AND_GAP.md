# GitHub Commit: Best Practice, Current State, Gap, and How to Close It

**Purpose:** Get to a safe, reviewable workflow: code on GitHub with clear commits and (optionally) PRs.

---

## 1. Best practice (what we should do)

| Practice | Why |
|----------|-----|
| **One logical change per commit** | Easy to revert, bisect, and review. |
| **Clear commit message** | First line = short summary (≤72 chars); body = what and why. Example: `feat(catalog): use Bamboo v2 catalog with pagination` then body. |
| **Commit often** | Small commits reduce merge pain and preserve history. |
| **Branch per feature/fix** | `main` stays deployable; work happens on `feature/...` or `fix/...`. |
| **Push to GitHub** | Backup, CI (e.g. Critical 4 on push), and collaboration. |
| **Open PR for non-trivial changes** | Review before merging to `main`; CI runs on the branch. |
| **Run Critical 4 before push** | Per `steller-backend/CONTRIBUTING.md`; keeps main green. |

**Suggested flow:**  
`git checkout -b feature/bamboo-catalog-v2` → make changes → `git add` → `git commit -m "feat(catalog): ..."` → run Critical 4 → `git push origin feature/bamboo-catalog-v2` → open PR → merge after review.

---

## 2. What we’re doing today

- **Backend:** In Git at `/opt/steller-v2/steller-backend`; remote **https://github.com/manni17/steller-backend**. Pushed; main has Bamboo catalog v2, referrals, PLG Phase 3-4.
- **Deploy + docs:** In Git at `/root/steller-v2-ops`; remote **https://github.com/manni17/steller-v2-ops**. Contains docker-compose, scripts, and full docs tree (BACKLOG_V2, INDEX, qa, architecture, product). Pushed.
- **Single place for latest version:** See `docs/INDEX.yaml` -> `repos:` for the canonical list (backend + ops URLs and descriptions).

---

## 3. The gap

| Best practice | Current state | Gap |
|---------------|---------------|-----|
| Code in Git | No repo | No history, no backup |
| Push to GitHub | No remote | Can’t push; no central copy |
| One change per commit | N/A | Can’t commit |
| PR before merge | No branches | No review flow |
| CI on push/PR | No GitHub repo | Critical 4 not run from GitHub |

**In short:** We’re not “visiting GitHub” at all yet—there’s no link from this machine to a GitHub repo, and no commits to push.

---

## 4. How to close the gap

### Step 1: Create or get the GitHub repo

- **If the repo already exists on GitHub:** Note its URL (e.g. `https://github.com/Org/steller-v2.git` or `git@github.com:Org/steller-v2.git`).
- **If it doesn’t:** On GitHub, create a new repository (e.g. `steller-v2`). Don’t initialize with a README if you’re going to push an existing folder.

### Step 2: Initialize Git and make the first commit (if no repo yet)

From the machine where the code lives (e.g. VPS or your laptop with the code):

```bash
cd /opt/steller-v2
git init
git add .
# Add a .gitignore if missing (e.g. .env, bin/, obj/, *.user, node_modules/)
echo ".env" >> .gitignore
echo "**/bin/" >> .gitignore
echo "**/obj/" >> .gitignore
git add .gitignore
git status   # review what will be committed
git commit -m "chore: initial commit — Steller v2 backend, Bamboo catalog v2, docs"
```

If you prefer to group recent work into one clear commit, you can do a single commit as above. Later you can adopt “one logical change per commit” for new work.

### Step 3: Add GitHub as remote and push

```bash
git remote add origin https://github.com/YOUR_ORG/steller-v2.git
# Or SSH: git remote add origin git@github.com:YOUR_ORG/steller-v2.git
git branch -M main
git push -u origin main
```

Use the real org/repo name and the same URL you use on GitHub (HTTPS or SSH). If the repo already had a `main` and history, use instead:

```bash
git remote add origin <URL>
git fetch origin
git branch -u origin/main main   # or merge/rebase as needed
git push -u origin main
```

### Step 4: (Optional) Use branches and PRs for future work

- Create a branch: `git checkout -b feature/your-feature`.
- Commit and push: `git push -u origin feature/your-feature`.
- On GitHub, open a Pull Request from that branch to `main`.
- After review and CI (e.g. Critical 4), merge. Then pull on the server: `git pull origin main`.

### Step 5: Docs in a separate repo (optional)

If docs live under `/root/docs` and you want them on GitHub too:

- Either **include docs in the same repo** (e.g. copy or move `docs` under `/opt/steller-v2` and commit), **or**
- Create a **separate repo** (e.g. `steller-docs`), `git init` in `/root/docs`, add remote, commit, and push.

---

## 5. Quick checklist to “visit GitHub” and close the gap

- [ ] GitHub repo exists (create or get URL).
- [ ] `cd /opt/steller-v2` and `git init` (if no repo).
- [ ] `.gitignore` in place (e.g. `.env`, `bin/`, `obj/`).
- [ ] `git add` and `git commit` with a clear message.
- [ ] `git remote add origin <GitHub URL>`.
- [ ] `git push -u origin main`.
- [ ] From here on: branch → commit → push → open PR → merge (optional but recommended).
- [ ] Run Critical 4 before pushing (see `steller-backend/CONTRIBUTING.md`).

Once this is done, “visiting GitHub” means: push commits and open/merge PRs. The gap is closed when the codebase is in a GitHub repo and you’re pushing commits (and optionally using PRs) consistently.
