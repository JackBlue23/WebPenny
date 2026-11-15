---
title: Transparency: what Pennymatch does not do (ERP, invoicing, OpenAI)
description: Clear CFO checklist: PGC middleware and CSV yes; ERP, invoicing software, and OpenAI no. Lemon Squeezy only bills the SaaS fee.
author: Pennymatch team
slug: pennymatch-no-es-erp-ni-sii
lang: en
category: Product
published: 2026-05-18
tags:
  - transparency
  - scope
  - erp
  - deepseek
  - mor
---

The worst cost of financial middleware is not the monthly fee: it is the misunderstanding with your advisor. This checklist exists so nobody treats the Pennymatch CSV as invoicing software or as the official books.

## Yes

- HMAC ingest from Stripe, Shopify, and Wise.  
- Immutable PGC double-entry journals (668/768/129/477, 0.01 tolerance, three-layer ECB rates).  
- CSV / OAuth toward Xero, QuickBooks, and Holded.  
- Bank-grade speed: O(1), Native AOT, idempotency (no duplicate entries), SHA-256.  
- Penny with **DeepSeek** as a latent subprocessor.

## What it does not include

- Replacing the ERP.  
- Being **invoicing software**: it does not issue invoices to end customers.  
- Filing tax returns for you.  
- Listing **OpenAI** as a subprocessor.  
- Presenting Lemon Squeezy / Paddle as “the book’s destination” — they are the MoR for the **SaaS fee** (Lemon active; Paddle contingency).

## Plans with a ceiling

Growth 1,000 · Pro 5,000 · Enterprise 20,000 tx/month. No opaque “unlimited.”

Full manual: [what it is and is not](/docs/scope-limits). If it fits, [start a free trial](/).
