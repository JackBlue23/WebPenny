---
title: Transparencia: lo que Pennymatch no hace (ERP, IVA, SII, OpenAI)
description: Checklist claro para CFOs y gestores: middleware PGC y CSV sí; ERP, liquidación IVA, AEAT/SII y OpenAI no. Lemon Squeezy solo cobra el SaaS.
author: Equipo Pennymatch
slug: pennymatch-no-es-erp-ni-sii
lang: es
category: Producto
published: 2026-05-18
tags:
  - transparencia
  - sii
  - iva
  - deepseek
  - mor
---

El peor coste de un middleware financiero no es el precio mensual: es el malentendido con el gestor. Esta checklist existe para que nadie diga en una inspección «el CSV de Pennymatch es el SII».

## Sí

- Ingesta HMAC de Stripe, Shopify y Wise.  
- Asientos PGC inmutables (668/768, 0,01 €, BCE en 3 capas).  
- CSV / OAuth hacia Holded, Xero y QuickBooks.  
- Velocidad de banco: O(1), Native AOT, idempotencia, SHA-256.  
- Penny con **DeepSeek** como subencargado latente.

## No

- Sustituir el ERP.  
- Emitir facturas con efectos AEAT / Veri*factu.  
- Liquidar IVA, presentar SII, OSS o Sociedades.  
- Declarar **OpenAI** como subencargado.  
- Presentar Lemon Squeezy / Paddle como «el destino del libro» — son el MoR de la **cuota SaaS** (LS activo; Paddle en contingencia).

## Planes con techo

Growth 1.000 · Pro 5.000 · Enterprise 20.000 tx/mes. Sin «ilimitado» opaco.

Manual completo: [qué es y qué no es](/docs/scope-limits). Si encaja, [prueba gratuita](/).
