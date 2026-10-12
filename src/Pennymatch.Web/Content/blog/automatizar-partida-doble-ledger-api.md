---
title: Cómo automatizar la partida doble con una Ledger API
description: Guía práctica para modelar asientos contables, balances y conciliación en tiempo real usando una API de ledger de partida doble.
author: Equipo Pennymatch
slug: automatizar-partida-doble-ledger-api
category: Ledger
published: 2026-03-12
tags:
  - ledger
  - partida-doble
  - api
  - contabilidad
---

La partida doble no es un detalle contable: es el contrato que garantiza que cada movimiento de dinero deja el sistema en equilibrio. Cuando tu producto mueve fondos —pagos, reembolsos, comisiones, liquidaciones— automatizar ese contrato en una **Ledger API** evita cuadres manuales, reportes inconsistentes y sorpresas en auditoría.

## Qué debe hacer una Ledger API (y qué no)

Una ledger de producto no sustituye tu ERP. Su trabajo es registrar **hechos financieros inmutables** con la misma disciplina que un libro mayor:

- Cada asiento tiene al menos un **débito** y un **crédito** que suman cero.
- Las cuentas tienen un tipo (activo, pasivo, ingreso, gasto, patrimonio) y un saldo derivado, no inventado.
- Los movimientos son **append-only**: se corrigen con asientos de ajuste, no reescribiendo el pasado.

Lo que no debería hacer: mezclar UI de reporting con la fuente de verdad, ni permitir “saldos sueltos” sin contrapartida.

## Modelo mínimo: cuentas, asientos y líneas

En Pennymatch el núcleo se expresa así:

1. **Cuenta** — identificador estable, moneda, tipo y `TenantId`.
2. **Asiento (journal entry)** — metadata (referencia externa, timestamp, idempotency key).
3. **Líneas** — cada una apunta a una cuenta con `debit` o `credit` y un importe.

Ejemplo conceptual al cobrar una comisión:

- Débito: `cash` (activo aumenta)
- Crédito: `revenue_fees` (ingreso aumenta)

Si el cobro falla después de haberse provisionado, no borras el asiento: publicas un **reversal** que invierte las líneas. Así el historial permanece auditable.

## Idempotencia: el detalle que salva liquidaciones

Los pagos llegan dos veces. Los workers se reintentan. Sin idempotencia, duplicas ingresos y rompes el balance.

Patrón recomendado:

- El cliente envía `Idempotency-Key` (UUID o hash del evento de origen).
- La Ledger API rechaza o reutiliza el mismo asiento si la clave ya existe.
- El resultado es el mismo `entryId` y el mismo estado de cuentas.

Esto es especialmente crítico cuando la fuente es un webhook de un PSP: el mismo `payment.succeeded` puede reenviarse horas después.

## Conciliación en tiempo casi real

Automatizar partida doble brilla cuando conectas tres flujos:

1. **Ingesta** — eventos de pago o transferencia.
2. **Asiento** — reglas de mapeo cuenta ↔ evento.
3. **Vista** — saldos por cuenta, por tenant y por periodo.

Con saldos derivados del ledger (no de tablas auxiliares), la conciliación deja de ser un Excel semanal y pasa a ser una consulta: “¿el `cash` del tenant X cuadra con el extracto del banco a las 18:00?”.

## Errores habituales al integrar

- **Un solo “saldo de usuario” sin contrapartida.** Parece simple; rompe auditoría.
- **Mutar asientos.** Si hay un bug de importe, ajusta con un nuevo entry.
- **Ignorar multi-moneda.** Cada línea debe declarar moneda; las conversiones FX merecen asientos propios.
- **Acoplar el ledger al ciclo de request HTTP.** Prefiere outbox + worker: el pago se confirma, luego se contabiliza de forma fiable.

## Checklist de adopción

- [ ] Cuentas tipadas por tenant y moneda
- [ ] Asientos con clave de idempotencia
- [ ] Reversiones explícitas
- [ ] Exportación para auditoría (CSV/JSON inmutable)
- [ ] Alertas cuando un batch de eventos no cuadra a cero

Automatizar la partida doble no es “poner débitos y créditos en un JSON”. Es convertir tu producto en un sistema que **no puede mentir sobre el dinero**. Una Ledger API bien diseñada convierte esa promesa en infraestructura reutilizable.
