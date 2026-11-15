---
title: Webhooks financieros hacia el PGC sin duplicados
description: Diseño de webhooks Stripe, Shopify y Wise: HMAC, circuit breaker, idempotencia SHA-256 y asientos Ready — sin cobrar ni asentar dos veces.
author: Equipo Pennymatch
slug: webhooks-financieros-entrega-sin-duplicados
lang: es
category: Integraciones
published: 2026-03-28
tags:
  - webhooks
  - idempotencia
  - hmac
  - pgc
---

Un webhook no es un «callback bonito»: es el canal por el que el libro se entera de que el dinero cambió. Si lo diseñas mal, asientas dos veces el mismo cargo. Si lo diseñas bien, los reintentos son resiliencia.

## Al menos una vez en la red, una vez en el libro

Stripe, Shopify y Wise reenvían. La obligación es **idempotencia**: la huella SHA-256 del evento normalizado es la clave, no el número de intentos HTTP.

## Anatomía útil

Payload con `id`, `type`, timestamp, importes canónicos y firma HMAC. Pennymatch verifica la firma, materializa el borrador PGC y solo entonces deja la ficha en Draft/Ready. Tras **3 descuadres** consecutivos (&gt; 0,01 €) el circuit breaker pausa la ingesta.

## Hot cache O(1)

Responde rápido al emisor, consulta límites Growth/Pro/Enterprise en caché caliente y evita materializar waterfalls en el camino caliente.

## Qué no meter aquí

No mezcles liquidación de IVA ni SII en el receptor. El IVA de Shopify, si viene en origen, se mapea a cuenta **477**. Holded/Xero/QuickBooks reciben el CSV o el publish OAuth — otro dominio. Guía: [ingesta HMAC](/docs/webhooks-hmac).
