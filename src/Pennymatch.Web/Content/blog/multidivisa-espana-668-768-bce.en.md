---
title: Multi-currency double-entry: PGC accounts 668 and 768 without Excel
description: Educational PGC guide on FX differences, 0.01 tolerance, and three ECB layers (query, day journal, quarterly close to 129).
author: Pennymatch team
slug: multidivisa-espana-668-768-bce
lang: en
category: Accounting
published: 2026-05-02
tags:
  - 668
  - 768
  - ecb
  - pgc
  - fx
---

Charging in USD or GBP while keeping books in EUR is daily life for global stores and SaaS teams. As a rigorous double-entry standard, the PGC reserves **668** for negative exchange differences and **768** for positive ones. The operational problem is elsewhere: who picks the rate, when, and with what auditable trail.

## The cent that ruins Friday

A residual of **0.01 EUR** may be absorbed as FX rounding. Above that, it is imbalance. Forcing a balance in Excel “because the bank matches” is exactly what an auditor does not want to see.

## Three ECB layers (they do not overlap)

1. **Query** — analytics projection on the dashboard; does **not** write Posted.  
2. **Day journal** — source amount + ECB rate to 4 decimals on that Draft/Ready/Posted.  
3. **Quarter close** — immutable batch against **668 / 768 / 129**.

If the widget and the journal disagree, **the journal wins**.

## From gateway to CSV

Stripe/Shopify/Wise supply the fact; Pennymatch posts; Xero/QuickBooks/Holded receive the CSV. This is not invoicing software or tax filing: source tax, when present, maps to **477**. Technical detail: [PGC engine](/docs/pgc-engine) and [ECB FX](/docs/fx-bce).

Ready to stop manually looking up exchange rates in spreadsheets? [Start the trial](/).
