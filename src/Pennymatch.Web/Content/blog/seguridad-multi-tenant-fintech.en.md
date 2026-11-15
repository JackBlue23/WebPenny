---
title: Multi-tenant security in PGC financial middleware
description: Tenant isolation, RBAC, encryption, Native AOT, and SHA-256 chains when posting Stripe/Shopify/Wise — with no OpenAI in the legal notice.
author: Pennymatch team
slug: seguridad-multi-tenant-fintech
lang: en
category: Security
published: 2026-04-09
tags:
  - security
  - multi-tenant
  - rbac
  - sha-256
---

When several B2B companies share the same middleware into the PGC, an isolation bug is not an edge case: it is leaking journals across customers. Three layers reinforce each other: **TenantId at every boundary**, **RBAC on every action**, and **secrets that never live in clear text**.

## TenantId as the frontier

It must cross authentication, authorization, persistence, and queues (`HasQueryFilter`). Tests must read another tenant’s resource and expect `404`.

## Hot-path RBAC

Admin publishes Ready → Posted; Bookkeeper maps accounts; ReadOnly audits. Rotating an HMAC secret is not the same as connecting Xero OAuth.

## Fingerprints, AOT, and subprocessors

SHA-256 idempotency keys bound to the tenant; append-only audit chain. Native AOT client. Legal notice lists PostgreSQL, Redis/Garnet, and **DeepSeek** for Penny (latent). **Zero OpenAI**. Lemon Squeezy / Paddle are MoRs for the SaaS fee, not for the ledger.

## Clear limits

Isolating tenants does not turn Pennymatch into an ERP. Holded, Xero, and QuickBooks remain the official destination; here we protect event → PGC → CSV. More in [security and billing](/docs/security-billing).
