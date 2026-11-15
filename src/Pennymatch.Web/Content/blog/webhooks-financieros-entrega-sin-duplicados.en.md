---
title: Financial webhooks into the PGC without duplicates
description: Stripe, Shopify, and Wise webhook design: HMAC, circuit breaker, SHA-256 idempotency, and Ready journals — without posting twice.
author: Pennymatch team
slug: webhooks-financieros-entrega-sin-duplicados
lang: en
category: Integrations
published: 2026-03-28
tags:
  - webhooks
  - idempotency
  - hmac
  - pgc
---

A webhook is not a “pretty callback”: it is how the ledger learns that money changed state. Design it poorly and you post the same charge twice. Design it well and retries are resilience.

## At-least-once on the wire, once in the book

Stripe, Shopify, and Wise resend. Your duty is **idempotency**: the SHA-256 fingerprint of the normalized event is the key, not the HTTP attempt count.

## Useful anatomy

Payload with `id`, `type`, timestamp, canonical amounts, and HMAC signature. Pennymatch verifies the signature, materializes the PGC draft, then leaves the card in Draft/Ready. After **3 consecutive imbalances** (&gt; 0.01 EUR) the circuit breaker pauses ingest.

## O(1) hot cache

Respond quickly to the sender, resolve Growth/Pro/Enterprise limits from a hot cache, and avoid materializing waterfalls on the hot path.

## What not to put here

Do not mix invoice issuance or tax-return logic into the receiver. Shopify tax, when present on the source, maps to account **477**. Xero/QuickBooks/Holded get the CSV or OAuth publish — another domain. Guide: [HMAC ingest](/docs/webhooks-hmac).
