---
title: Transparency: what Pennymatch does not do (ERP, VAT, SII, OpenAI)
description: Clear CFO checklist: PGC middleware and CSV yes; ERP, VAT settlement, AEAT/SII, and OpenAI no. Lemon Squeezy only bills the SaaS fee.
author: Pennymatch team
slug: pennymatch-no-es-erp-ni-sii
lang: en
category: Product
published: 2026-05-18
tags:
  - transparency
  - sii
  - vat
  - deepseek
  - mor
---

The worst cost of financial middleware is not the monthly fee: it is the misunderstanding with your advisor. This checklist exists so nobody tells an inspector “the Pennymatch CSV is the SII filing.”

## Yes

- HMAC ingest from Stripe, Shopify, and Wise.  
- Immutable PGC journals (668/768, 0.01 EUR, three-layer BCE FX).  
- CSV / OAuth toward Holded, Xero, and QuickBooks.  
- Bank-grade speed: O(1), Native AOT, idempotency, SHA-256.  
- Penny with **DeepSeek** as a latent subprocessor.

## No

- Replacing the ERP.  
- Issuing invoices with AEAT / Veri*factu effect.  
- Settling VAT or filing SII, OSS, or corporate tax.  
- Listing **OpenAI** as a subprocessor.  
- Presenting Lemon Squeezy / Paddle as “the book’s destination” — they are the MoR for the **SaaS fee** (Lemon active; Paddle contingency).

## Plans with a ceiling

Growth 1,000 · Pro 5,000 · Enterprise 20,000 tx/month. No opaque “unlimited.”

Full manual: [what it is and is not](/docs/scope-limits). If it fits, [start a free trial](/).
