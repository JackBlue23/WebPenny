---
title: Seguridad multi-tenant en middleware financiero PGC
description: Aislamiento por tenant, RBAC, cifrado, Native AOT y cadena SHA-256 cuando asientas Stripe/Shopify/Wise sin OpenAI en el aviso legal.
author: Equipo Pennymatch
slug: seguridad-multi-tenant-fintech
lang: es
category: Seguridad
published: 2026-04-09
tags:
  - seguridad
  - multi-tenant
  - rbac
  - sha-256
---

Cuando varias empresas B2B comparten el mismo middleware hacia el PGC, un bug de aislamiento no es un edge case: es filtrar asientos entre clientes. Tres capas se refuerzan: **TenantId en cada límite**, **RBAC en cada acción** y **secretos que nunca viven en claro**.

## TenantId como frontera

Debe atravesar autenticación, autorización, persistencia y colas (`HasQueryFilter`). Los tests deben leer el recurso de otro tenant y esperar `404`.

## RBAC del camino caliente

Admin publica Ready → Posted; Bookkeeper mapea cuentas; ReadOnly audita. Rotar un secreto HMAC no es lo mismo que conectar OAuth de Xero.

## Huellas, AOT y subencargados

Claves de idempotencia SHA-256 ligadas al tenant; cadena de auditoría append-only. Cliente Native AOT. En el aviso legal: PostgreSQL, Redis/Garnet y **DeepSeek** para Penny (latente). **Cero OpenAI**. Lemon Squeezy / Paddle son MoR de la cuota SaaS, no del libro.

## Límites claros

Aislar tenants no convierte a Pennymatch en un ERP. Holded, Xero y QuickBooks siguen siendo el destino oficial; aquí protegemos el puente evento → PGC → CSV. Más en [seguridad y facturación](/docs/security-billing).
