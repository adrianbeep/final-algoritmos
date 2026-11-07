## Descripcion

Cierra el #7. Incluye el trigger de auditoria y un cambio transversal para hacer todos los scripts SQL idempotentes.

### `Scripts SQL/Triggers.sql` (nuevo)

Tabla `auditoria_reservas`... no, espera, la tabla quedo en `Coworking_db.sql`. Este archivo solo tiene los 2 triggers:

- `trg_auditoria_reservas_insert` — AFTER INSERT sobre `reservas`. Inserta en `auditoria_reservas` con `accion='INSERT'` y detalle de la reserva creada.
- `trg_auditoria_reservas_update` — AFTER UPDATE sobre `reservas`. Inserta en `auditoria_reservas` solo cuando `OLD.estado_reserva = 'Confirmada'` y `NEW.estado_reserva = 'Cancelada'`, con `accion='CANCEL'`.

### `Scripts SQL/Coworking_db.sql` (modificado)

- Se agrego la tabla `auditoria_reservas` (id, id_reserva, accion, fecha_accion, detalle)
- Se agregaron `DROP DATABASE IF EXISTS` y `DROP TABLE IF EXISTS` antes de cada CREATE para hacer el script idempotente

### `Scripts SQL/Funciones.sql` (modificado)

- Se agrego `DROP FUNCTION IF EXISTS` antes de cada UDF

### `Scripts SQL/Procedimientos_Almacenados.sql` (modificado)

- Se agrego `DROP PROCEDURE IF EXISTS` antes de cada SP

### `Scripts SQL/Vistas.sql` (modificado)

- Se agrego `DROP VIEW IF EXISTS` antes de cada vista

## Notas de diseno

- **Dos triggers en vez de uno**: MySQL no permite un solo trigger que cubra multiples eventos. La consigna original lo planteaba como uno, pero la sintaxis MySQL obliga a separar.
- **Trigger de UPDATE solo cancelaciones**: si actualizas las fechas de una reserva (sin cancelar), el trigger no se dispara. Solo audita lo que nos importa: la cancelacion.
- **Idempotencia**: ahora todos los scripts se pueden correr multiples veces sin error. Util en desarrollo para iterar rapido, y en la entrega para que se pueda recrear la base desde cero.
- **Orden de ejecucion**: `Coworking_db.sql` (tablas + datos) → `Funciones.sql` → `Vistas.sql` → `Procedimientos_Almacenados.sql` → `Triggers.sql`. Los triggers van al final porque dependen de que la tabla `auditoria_reservas` exista.

Closes #7
