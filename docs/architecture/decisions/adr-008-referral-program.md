# ADR-008: Referral Program

**Status:** Accepted  
**Date:** 2026-02-20  
**Deciders:** Dev Agent (PLG Phase 4)

---

## Context

PLG requires referral tracking: partners can share a code; when a new partner signs up with that code, the referrer is recorded. Rewards (wallet credit, etc.) can be added later.

---

## Decision

- **Partner fields:** `ReferralCode` (unique, nullable; generated on first use as REF-{PartnerId}) and `ReferredByPartnerId` (nullable; set when signup uses a referral code).
- **Referrals table:** ReferrerPartnerId, ReferredPartnerId, Status (e.g. SignedUp, Activated, Rewarded), RewardAmount (nullable), CreatedAt.
- **Signup:** POST /api/public/signup accepts optional `referralCode`. If valid, set partner.ReferredByPartnerId and insert Referral row (Status = SignedUp).
- **Partner API:** GET /api/partner/referral-code (returns or generates code), GET /api/partner/referrals (list referrals made by current partner).
- **Rewards:** Not implemented; RewardAmount and status Rewarded left for a future iteration.

---

## Consequences

- **Positive:** Lightweight referral tracking without external dependency; signup and partner APIs are self-contained.
- **Negative:** Referral code format REF-{id} is predictable; could add random suffix for production if needed.
- **Security:** Only the authenticated partner can read their own referral code and list; no PII in referral list beyond ReferredPartnerId and dates.

---

## Related

- apis.yaml: GET /api/partner/referral-code, GET /api/partner/referrals; POST /api/public/signup optional referralCode
- components.yaml: steller-v2-referral-service
- Migration: AddReferralsAndPartnerReferralFields
