# [Etapa 1] Crear trigger de auditoría

## Objetivo

Crear un trigger que registre automáticamente en una tabla de auditoría las operaciones importantes del sistema. Justifica la inclusión de triggers en el proyecto con un uso real.

## Entregables

### Tabla `auditoria_reservas`

- [ ] `id` INT auto_increment PK
- [ ] `id_reserva` INT
- [ ] `accion` ENUM('INSERT', 'UPDATE', 'CANCEL') — tipo de operación
- [ ] `fecha_accion` DATETIME DEFAULT CURRENT_TIMESTAMP
- [ ] `detalle` TEXT — campo libre para guardar info adicional (estado anterior, usuario, etc.)

### Trigger

- [ ] Trigger `trg_auditoria_reservas` sobre la tabla `reservas`
- [ ] Se dispara AFTER INSERT y AFTER UPDATE
- [ ] Inserta en `auditoria_reservas` con:
  - `id_reserva` = NEW.id
  - `accion` = 'INSERT' si fue INSERT, 'UPDATE' si el campo `estado_reserva` cambió a 'Cancelada', sino solo INSERT
  - `detalle` = concat de info relevante (estado anterior, fechas, etc.)

## Justificación

El trigger automatiza la auditoría sin que la aplicación tenga que recordar hacerlo. Si en el futuro se agrega otra forma de insertar/cancelar reservas, la auditoría sigue funcionando. Justifica el uso de triggers con un propósito concreto, no como adorno.

## Criterio de pase

- [ ] Al hacer un INSERT en `reservas`, aparece una fila en `auditoria_reservas` con accion='INSERT'
- [ ] Al cancelar una reserva (UPDATE estado='Cancelada'), aparece una fila con accion='UPDATE' y el detalle del cambio
- [ ] El trigger no interfiere con las transacciones de `sp_alta_reserva` y `sp_cancelar_modificar_reserva`

## Archivos a modificar

- `Scripts SQL/Coworking_db.sql` — agregar la tabla `auditoria_reservas` y el trigger al final del script
