---
title: Why PGC middleware is not an ERP (or a MoR connector)
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

Global ecommerce and SaaS teams hit the same wall: multi-currency Stripe payouts, Shopify orders, and Wise settlements arrive with fees and FX — then someone rebuilds them in Excel toward Xero or QuickBooks. “Temporary” scripts last for years.

## What middleware must do (and must not)

Pennymatch is **not an ERP and not invoicing software**: it does not issue invoices, file tax returns, or replace Xero / QuickBooks / Holded. Its job is to:

1. Capture raw Stripe, Shopify, and Wise events (HMAC).
2. Materialize **immutable PGC double-entry journals** (0.01 tolerance, no duplicate entries).
3. Export **universal CSVs** ready for your advisor’s ERP.

Lemon Squeezy (active) and Paddle Billing v2 (contingency) collect Pennymatch’s **SaaS fee** as Merchant of Record. They are not the product’s accounting destination.

## From raw payment event to Posted journal

The stable contract is not the provider JSON: it is the sealed journal (debit/credit accounts + SHA-256 correlation). Draft → Ready → **Posted**. Posted is never rewritten.

## Closing

Automate transcription into rigorous double-entry — the operational step that accumulates the most errors — and leave official books to the tools built for them. [Start a free trial](/) or read the [scope manual](/docs/scope-limits).
