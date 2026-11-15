---
title: Multi-divisa en España: cuentas 668 y 768 sin Excel
description: Guía educativa del PGC sobre diferencias de cambio, tolerancia de 0,01 € y las tres capas BCE (consulta, asiento, cierre trimestral 129).
author: Equipo Pennymatch
slug: multidivisa-espana-668-768-bce
lang: es
category: Contabilidad
published: 2026-05-02
tags:
  - 668
  - 768
  - bce
  - pgc
  - fx
---

Cobrar en USD o GBP y llevar el libro en EUR es el día a día de muchas tiendas y SaaS globales. El PGC, como estándar riguroso de partida doble, reserva la **668** para diferencias negativas de cambio y la **768** para positivas. El problema operativo es otro: quién calcula el tipo, cuándo y con qué rastro auditable.

## El céntimo que rompe el viernes

Un residual de **0,01 €** puede absorberse como redondeo FX. Por encima, es descuadre. Forzar el cuadre en Excel «porque el banco cuadra» es exactamente lo que un auditor no quiere ver.

## Tres capas BCE (sin pisarse)

1. **Consulta** — proyección analítica en el panel; **no** escribe Posted.  
2. **Asiento del día** — importe origen + tipo a 4 decimales en ese Draft/Ready/Posted.  
3. **Cierre de trimestre** — lote inmutable contra **668 / 768 / 129**.

Si el widget y el asiento discrepan, **gana el asiento**.

## De la pasarela al CSV

Stripe/Shopify/Wise aportan el hecho; Pennymatch asienta; Holded/Xero/QuickBooks reciben el CSV. No es software de facturación ni presentación de modelos: el impuesto de origen, si existe, mapea a **477**. Detalle técnico: [motor PGC](/docs/pgc-engine) y [FX BCE](/docs/fx-bce).

¿Listo para dejar de buscar e introducir manualmente tipos de cambio en hojas de cálculo? [Inicia la prueba](/).
