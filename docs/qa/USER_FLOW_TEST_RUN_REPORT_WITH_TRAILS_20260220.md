# User-Flow Integration Test Run — Report with Trails

**Date:** 2026-02-20  
**Plan:** `docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md`  
**Dev fixes:** ServiceResponse unwrap, wallet balance reading, auth switching (partner vs admin).

---

## 1. Dev Report Read and Instructions Used

**Summary of what was fixed (from dev):**

- **QA env:** Test DB uses `.env` plus `TEST_DB_HOST` / `TEST_DB_PORT`; Redis disabled in Testing; schema reset with EnsureDeleted + EnsureCreated.
- **User-flow fixes:** All API responses are wrapped in `{ "Status", "Data": { ... } }`. Tests now unwrap `Data` and read order/wallet JSON correctly; wallet balance is read from the unwrapped payload; when switching between partner (API key) and admin (Bearer), the client clears the other auth so the correct identity is used; admin login response supports both `Data`/`data` and `AccessToken`/`accessToken`.

**Run instructions used:**

```bash
cd /opt/steller-v2/steller-backend
export TEST_DB_HOST=localhost TEST_DB_PORT=6432
dotnet test Tests.Integration/Tests.Integration.csproj -c Release --filter "FullyQualifiedName~UserFlowIntegrationTests"
```

`.env` at repo root or under steller-backend supplies `DB_USERNAME` and `DB_PASSWORD` for the factory.

---

## 2. Test Run Result

**Command:** As above (Release build, filter UserFlowIntegrationTests).  
**Result:** **Test Run Successful.**  
**Total tests:** 8  
**Passed:** 8  
**Total time:** ~2.9 minutes  

---

## 3. Trails (What Happened — User and Money Story)

Trails describe each flow in plain language: who acted, what they saw, and how money moved. No variable names or technical identifiers.

---

### Trail 1 — Partner places order and receives gift card

A **partner** started with a **wallet balance of five hundred**. They placed an order for **one gift card** at a **fifty** face value. The system accepted the order and showed it as **processing**. After the order was fulfilled, the partner looked up the order and saw it **completed** with **one gift card** that had a **serial number and a PIN**. The partner then checked their wallet: the balance had **gone down by the order amount** (the sale total). So: **money left the wallet when the order was placed**, and the partner **received a usable gift card** when the order completed.

---

### Trail 2 — Partner checks wallet and transaction history after an order

A **partner** had **five hundred** in their wallet. They placed an order (one card at **twenty-five** face value). The order was fulfilled. The partner checked their wallet again: the balance was **five hundred minus the order amount**. They then opened their **transaction history** and saw a **debit** entry tied to that order. So: **the wallet balance decreased by the order amount**, and the **ledger shows the debit** for the order.

---

### Trail 3 — Partner sends the same order twice (idempotency)

A **partner** had **five hundred** in their wallet. They placed an order with a **unique reference** and the system accepted it; their wallet went down by the **order amount**. They then **sent the exact same request again** (same reference). The system again responded successfully but **returned the same order** and **did not take money a second time**. The partner’s wallet balance **stayed the same** after the second request. So: **no double charge** when the partner retries with the same reference.

---

### Trail 4 — Partner browses catalog, then places order and gets the card

A **partner** first called the **catalog** and saw products, including one with a known product code. They then placed an order for that product (one card at **fifty** face value). The order was fulfilled. The partner fetched the order and saw it **completed** with **at least one gift card**. So: **catalog → order → completed order with card** works end to end for the partner.

---

### Trail 5 — Partner with low balance cannot place order

A **partner** had only **twenty** in their wallet. They tried to place an order for a **fifty** face-value card. The system **rejected the order** and returned an error about **insufficient funds**. The partner checked their wallet again: the balance was still **twenty**. So: **no order was created and no money was taken** when the balance was too low.

---

### Trail 6 — Admin logs in and credits a partner’s wallet

An **administrator** logged in with email and password. Once logged in, they looked up a **partner’s wallet** and saw its current balance. The admin then **credited** that partner’s wallet with a **fixed amount**. After the credit, the admin looked up the same wallet again: the balance had **increased by that amount**. So: **admin can add funds** to a partner’s wallet and the new balance is correct.

---

### Trail 7 — Admin cancels an order; partner gets refund

A **partner** had **five hundred** in their wallet. They placed an order (one card at **twenty-five**); the system accepted it and **reduced the partner’s balance** by the order amount. An **administrator** then logged in and **cancelled that order** (with a reason). After the cancel, the **partner** checked their wallet: the balance was back to **five hundred**. So: **admin cancel leads to a full refund** to the partner’s wallet.

---

### Trail 8 — Admin creates API key; partner uses it to call catalog

An **administrator** logged in and **created a new API key** for a partner. The system returned the new key (shown once). That key was then used **as the partner** to call the **catalog**. The catalog request **succeeded**. So: **admin-issued key** can be used by the partner to access the API (e.g. catalog).

---

## 4. Summary Table (Outcome Only)

| # | Flow | Outcome |
|---|------|--------|
| 1 | Partner: order → gift card | Order completed; card with serial and PIN; wallet decreased by order amount. |
| 2 | Partner: order → wallet and transactions | Wallet decreased by order amount; transaction history shows debit. |
| 3 | Partner: duplicate reference | Same order returned; no second charge; wallet unchanged. |
| 4 | Partner: catalog → order → card | Catalog visible; order completed with card. |
| 5 | Partner: insufficient funds | Order rejected; wallet unchanged. |
| 6 | Admin: credit partner wallet | Partner wallet balance increased by credit amount. |
| 7 | Admin: cancel order | Partner wallet refunded to pre-order balance. |
| 8 | Admin: create API key | New key works for partner (e.g. catalog). |

---

## 5. References

- Plan: `docs/qa/USER_FLOW_INTEGRATION_TEST_PLAN.md`
- Previous run (DB auth failure): `docs/qa/USER_FLOW_INTEGRATION_TEST_REPORT_20260220.md`
- Backlog: `docs/BACKLOG_V2.md` (B7)
