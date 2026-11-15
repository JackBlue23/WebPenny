---
title: Transparencia: lo que Pennymatch no hace (ERP, facturación, OpenAI)
description: Checklist claro para CFOs y gestores: middleware PGC y CSV sí; ERP, software de facturación y OpenAI no. Lemon Squeezy solo cobra el SaaS.
author: Equipo Pennymatch
slug: pennymatch-no-es-erp-ni-sii
lang: es
category: Producto
published: 2026-05-18
tags:
  - transparencia
  - alcance
  - erp
  - deepseek
  - mor
---

El peor coste de un middleware financiero no es el precio mensual: es el malentendido con el gestor. Esta checklist existe para que nadie trate el CSV de Pennymatch como si fuera su software de facturación o su libro oficial.

## Sí

- Ingesta HMAC de Stripe, Shopify y Wise.  
- Asientos PGC inmutables (668/768/129/477, 0,01 €, BCE en 3 capas).  
- CSV / OAuth hacia Holded, Xero y QuickBooks.  
- Velocidad de banco: O(1), Native AOT, idempotencia, SHA-256.  
- Penny con **DeepSeek** como subencargado latente.

## Qué NO incluye

- Sustituir el ERP.  
- Ser **software de facturación**: no emite facturas a clientes finales.  
- Presentar modelos fiscales por usted.  
- Declarar **OpenAI** como subencargado.  
- Presentar Lemon Squeezy / Paddle como «el destino del libro» — son el MoR de la **cuota SaaS** (Lemon activo; Paddle en contingencia).

## Planes con techo

Growth 1.000 · Pro 5.000 · Enterprise 20.000 tx/mes. Sin «ilimitado» opaco.

Manual completo: [qué es y qué no es](/docs/scope-limits). Si encaja, [prueba gratuita](/).
