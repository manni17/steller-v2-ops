# ADR-007: Webhook Infrastructure for Order Status Notifications

**Status:** Accepted  
**Date:** 2026-02-20  
**Deciders:** Dev Agent (PLG Phase 3)

---

## Context

Partners need to receive order status updates (e.g. Succeeded, Failed) without polling. Delivery and retry already exist (SendWebhookJob, WebhookService, Partner.WebhookUrl/WebhookSecret). Missing: partner-facing API to register or update webhook URL and secret.

---

## Decision

- **Storage:** Partner table columns WebhookUrl and WebhookSecret (existing). No separate Webhooks table for now.
- **Registration:** Partner-facing GET /api/partner/webhook (returns URL and hasSecret; secret never returned) and PUT /api/partner/webhook (body: webhookUrl, webhookSecret, both optional). Auth: x-api-key or JWT (Partner/Admin).
- **Delivery:** Existing SendWebhookJob (Hangfire, queue "webhooks") enqueued from PollBambooOrderJob on order Succeeded/Failed. Payload includes requestId, status, orderId, partnerId, total, currency, createdAt, updatedAt. Signature: X-Steller-Signature with HMAC-SHA256 of JSON body using partner WebhookSecret.
- **Retry:** Hangfire AutomaticRetry(Attempts = 5) on SendWebhookJob.

---

## Consequences

- **Positive:** Partners can self-serve webhook config; same delivery path as before; credential-only switch (mock/sandbox/production) unchanged.
- **Negative:** Single URL per partner; no per-event subscription (future enhancement if needed).
- **Security:** Secret never returned in GET; HTTPS recommended for webhook URLs in production.

---

## Related

- apis.yaml: GET/PUT /api/partner/webhook
- components.yaml: steller-v2-webhook-service
- SendWebhookJob, WebhookService, PartnerController
