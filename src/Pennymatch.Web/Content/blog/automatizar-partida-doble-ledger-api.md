---
title: Por qué el middleware PGC no es un ERP (ni un puente al MoR)
description: Cómo separar Stripe/Shopify/Wise, asientos PGC inmutables y CSV a Holded/Xero/QuickBooks sin construir un ERP ni liquidar IVA.
author: Equipo Pennymatch
slug: automatizar-partida-doble-ledger-api
lang: es
category: Producto
published: 2026-03-12
tags:
  - pgc
  - middleware
  - erp
  - transparencia
---

Los equipos de ecommerce y SaaS en España tropiezan con el mismo muro: payouts de Stripe, pedidos Shopify y liquidaciones Wise llegan con comisiones y FX… y alguien los reconstruye en Excel hacia Holded o Xero. Los scripts «temporales» duran años.

## Qué debe hacer el middleware (y qué no)

Pennymatch **no es un ERP ni software de facturación**: no emite facturas, no presenta modelos fiscales y no sustituye Holded / Xero / QuickBooks. Su trabajo es:

1. Capturar eventos crudos de Stripe, Shopify y Wise (HMAC).
2. Materializar **asientos PGC inmutables** (partida doble, tolerancia 0,01 €).
3. Exportar **CSV universales** listos para el ERP del gestor.

Lemon Squeezy (activo) y Paddle Billing v2 (contingencia) cobran la **cuota SaaS** de Pennymatch como Merchant of Record. No son el destino contable del producto.

## De evento crudo a asiento Posted

El contrato estable no es el JSON del proveedor: es el asiento sellado (cuentas Debe/Haber + correlación SHA-256). Draft → Ready → **Posted**. Posted no se reescribe.

## Conclusión

Automatiza el eslabón frágil — la transcripción al PGC — y deja la contabilidad oficial a las herramientas pensadas para ello. [Prueba gratuita](/) o lee el [manual de alcance](/docs/scope-limits).
