---
title: Why PGC middleware is not an ERP (or a MoR bridge)
description: How to separate Stripe/Shopify/Wise, immutable PGC journals, and CSV to Holded/Xero/QuickBooks without building an ERP or settling VAT.
author: Pennymatch team
slug: automatizar-partida-doble-ledger-api
lang: en
category: Product
published: 2026-03-12
tags:
  - pgc
  - middleware
  - erp
  - transparency
---

Spanish ecommerce and SaaS teams hit the same wall: Stripe payouts, Shopify orders, and Wise settlements arrive with fees and FX — then someone rebuilds them in Excel toward Holded or Xero. “Temporary” scripts last for years.

## What middleware must do (and must not)

Pennymatch is **not an ERP**: it does not invoice, replace Holded / Xero / QuickBooks, settle VAT, or file AEAT/SII. Its job is to:

1. Capture raw Stripe, Shopify, and Wise events (HMAC).
2. Materialize **immutable PGC journals** (double-entry, 0.01 EUR tolerance).
3. Export **universal CSVs** ready for your advisor’s ERP.

Lemon Squeezy (active) and Paddle Billing v2 (contingency) collect Pennymatch’s **SaaS fee** as Merchant of Record. They are not the product’s accounting destination.

## From raw event to Posted journal

The stable contract is not the provider JSON: it is the sealed journal (debit/credit accounts + SHA-256 correlation). Draft → Ready → **Posted**. Posted is never rewritten.

## Closing

Automate the fragile link — transcription into the PGC — and leave official books to the tools built for them. [Start a free trial](/) or read the [scope manual](/docs/scope-limits).
