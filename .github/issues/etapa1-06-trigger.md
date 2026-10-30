# [Etapa 1] Crear trigger de auditoría

## Objetivo

Crear una tabla de auditoria y los triggers que registran automaticamente las operaciones importantes sobre `reservas`. Justifica el uso de triggers con un proposito concreto, no como adorno.

## Entregables

### Tabla `auditoria_reservas`

- [ ] `id` INT auto_increment PK
- [ ] `id_reserva` INT NOT NULL, FK a `reservas(id)`
- [ ] `accion` ENUM('INSERT', 'CANCEL') — tipo de operacion auditada
- [ ] `fecha_accion` DATETIME DEFAULT CURRENT_TIMESTAMP
- [ ] `detalle` TEXT — info adicional (estado anterior, fechas, etc.)

### Triggers

MySQL no permite un unico trigger que cubra multiples eventos. Hay que crear uno por evento:

- [ ] `trg_auditoria_reservas_insert` — AFTER INSERT sobre `reservas`
  - Inserta en `auditoria_reservas` con `accion = 'INSERT'` y detalle con info de la reserva creada
- [ ] `trg_auditoria_reservas_update` — AFTER UPDATE sobre `reservas`
  - Inserta en `auditoria_reservas` solo si `OLD.estado_reserva = 'Confirmada'` y `NEW.estado_reserva = 'Cancelada'`
  - Con `accion = 'CANCEL'` y detalle con el cambio de estado

## Por que dos triggers separados

MySQL sintaxis `CREATE TRIGGER ... AFTER INSERT OR UPDATE` no existe. Cada trigger es para UN evento especifico. Si quisieras cubrir ambos eventos con un solo bloque, tendrias que usar un stored procedure desde la aplicacion, pero eso rompe la automatizacion.

## Justificacion

Los triggers automatizan la auditoria sin que la aplicacion tenga que recordar hacerlo. Si en el futuro se agrega otra forma de insertar o cancelar reservas (importacion masiva, otro SP, SQL directo en Workbench), la auditoria sigue funcionando. Es transparente para el codigo de la aplicacion.

## Criterio de pase

- [ ] Al hacer un INSERT en `reservas` (directo o via `sp_alta_reserva`), aparece una fila en `auditoria_reservas` con `accion='INSERT'`
- [ ] Al hacer UPDATE `estado_reserva = 'Cancelada'` (directo o via `sp_cancelar_reserva`), aparece una fila con `accion='CANCEL'`
- [ ] Al hacer UPDATE de otro campo (ej: fechas), NO aparece fila en `auditoria_reservas` (solo auditamos cancelaciones, no todo update)
- [ ] El trigger no interfiere con las transacciones de los SPs de reservas

## Archivos a crear

- `Scripts SQL/Triggers.sql` — la tabla `auditoria_reservas` y los 2 triggers. Se ejecuta despues de `Coworking_db.sql` (la tabla `reservas` tiene que existir).
