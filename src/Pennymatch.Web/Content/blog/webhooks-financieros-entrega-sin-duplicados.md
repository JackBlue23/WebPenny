---
title: Webhooks financieros: entrega confiable sin duplicados
description: Diseño de webhooks para eventos de pago y ledger: reintentos, firmas, idempotencia y colas para no cobrar ni contabilizar dos veces.
author: Equipo Pennymatch
slug: webhooks-financieros-entrega-sin-duplicados
category: Integraciones
published: 2026-03-28
tags:
  - webhooks
  - idempotencia
  - pagos
  - eventos
---

En fintech, un webhook no es un “callback bonito”: es el canal por el que tu sistema se entera de que el dinero cambió de estado. Si lo diseñas mal, cobras dos veces, dejas asientos huérfanos o pierdes liquidaciones enteras. Si lo diseñas bien, los reintentos dejan de ser un riesgo y se convierten en resiliencia.

## El contrato: al menos una vez, nunca “exactamente una” en la red

La red falla. Los balanceadores reintentan. Tu receptor se cae 30 segundos. Por eso los emisores serios (PSPs, ledgers, bancos open banking) adoptan **at-least-once delivery**: pueden enviar el mismo evento más de una vez.

Tu obligación como receptor es hacer el procesamiento **idempotente**. “Exactamente una vez” se logra en la aplicación, no en el cable.

## Anatomía de un webhook financiero sólido

Un payload útil suele incluir:

- `id` — identificador único del evento (no del recurso de negocio).
- `type` — p. ej. `payment.succeeded`, `ledger.entry.posted`.
- `created_at` — tiempo del emisor.
- `data` — objeto de negocio.
- `signature` / cabecera firmada — prueba de origen.

Ejemplo simplificado:

```json
{
  "id": "evt_9f2c",
  "type": "payment.succeeded",
  "created_at": "2026-03-28T10:14:02Z",
  "data": {
    "payment_id": "pay_441",
    "amount": 1299,
    "currency": "EUR",
    "tenant_id": "tn_acme"
  }
}
```

## Firma HMAC: verifica antes de confiar

Antes de tocar base de datos:

1. Lee el cuerpo crudo (raw body).
2. Calcula HMAC-SHA256 con el secreto del endpoint.
3. Compara en tiempo constante con la firma de la cabecera.
4. Rechaza con `401` si no cuadra.

Rota secretos con solapamiento: acepta el secreto anterior durante una ventana corta para no romper entregas en vuelo.

## Idempotencia en el receptor

Guarda `event.id` (o un hash de `type + resource_id + version`) en una tabla con **unique constraint**. Flujo:

1. Inserta el id del evento.
2. Si la inserción falla por duplicado → responde `200` sin reprocesar.
3. Si es nuevo → procesa en la misma transacción o vía outbox.

Responder `200` a un duplicado es correcto: le dices al emisor “ya lo tengo”, no “inténtalo otra vez”.

## Reintentos con backoff y DLQ

Configura el emisor (o tu capa de reenvío) con:

- Backoff exponencial (p. ej. 1m, 5m, 30m, 2h).
- Tope de intentos y cola de dead-letter para inspección.
- Alertas cuando la tasa de `4xx/5xx` sube.

En el receptor, distingue errores:

- **4xx de validación** → no reintentar a ciegas; corrige el contrato.
- **5xx / timeouts** → reintento legítimo.

## Orden y consistencia eventual

Los eventos pueden llegar desordenados (`payment.failed` antes que un `payment.succeeded` retrasado). Usa versiones o timestamps del recurso y aplica **last-write-wins con reglas de negocio**, no el orden de llegada del webhook.

Para el ledger: traduce cada evento de pago en un asiento solo cuando el estado canónico del pago lo permita, usando la misma clave de idempotencia que el `event.id`.

## Checklist operativo

- [ ] Verificación de firma en el edge
- [ ] Unique index sobre `event_id`
- [ ] Timeouts cortos + procesamiento async
- [ ] Métricas: lag de entrega, duplicados absorbidos, fallos de firma
- [ ] Runbook para reinyectar desde DLQ

Los webhooks financieros bien hechos no eliminan los duplicados: los **hacen inofensivos**. Esa es la diferencia entre un fin de semana de incendios y un sistema que duerme tranquilo.
