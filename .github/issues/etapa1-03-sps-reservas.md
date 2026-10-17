# [Etapa 1] Crear SPs de reservas con transaction y handler

## Objetivo

Crear los procedimientos almacenados de alta y cancelación/modificación de reserva. Como tocan múltiples tablas (reservas + recursos), requieren `START TRANSACTION`, `COMMIT` y `DECLARE EXIT HANDLER FOR SQLEXCEPTION` para garantizar atomicidad.

## Entregables

### SP `sp_alta_reserva`

- [ ] Parámetros: `p_id_cliente`, `p_id_recurso`, `p_fecha_inicio`, `p_fecha_fin`
- [ ] Validar que el cliente exista
- [ ] Validar que el recurso exista y esté en estado `Disponible`
- [ ] Validar que no haya superposición de reservas para el mismo recurso en el mismo período
- [ ] Calcular `total_reserva` (en base a días * `precio_dia` del recurso, o un cálculo equivalente)
- [ ] Insertar en `reservas` con `estado_reserva = 'Confirmada'`
- [ ] **Transaction y handler:**
  - `DECLARE EXIT HANDLER FOR SQLEXCEPTION` que ejecuta `ROLLBACK` + `RESIGNAL`
  - `START TRANSACTION` antes de los INSERTs
  - `COMMIT` al final si todo salió bien
- [ ] Devolver el ID de la reserva creada

### SP `sp_cancelar_modificar_reserva`

- [ ] Parámetros: `p_id_reserva`, `p_accion` ('cancelar' o 'modificar'), `p_nueva_fecha_inicio`, `p_nueva_fecha_fin`
- [ ] Si la acción es `cancelar`: actualizar `estado_reserva = 'Cancelada'`
- [ ] Si la acción es `modificar`: validar nueva disponibilidad (sin superposición) y actualizar fechas
- [ ] Validar que la reserva exista
- [ ] **Transaction y handler:**
  - `DECLARE EXIT HANDLER FOR SQLEXCEPTION` que ejecuta `ROLLBACK` + `RESIGNAL`
  - `START TRANSACTION` antes de los UPDATEs
  - `COMMIT` al final si todo salió bien

## Justificación de transaction + handler

Si la reserva se inserta pero falla la validación posterior (o viceversa), sin transaction la base queda inconsistente. El `DECLARE EXIT HANDLER FOR SQLEXCEPTION` captura cualquier error, hace `ROLLBACK` automático y propaga la excepción con `RESIGNAL` para que C# la capture en la Etapa 4.

## Criterio de pase

- [ ] Alta de reserva exitosa con datos válidos
- [ ] Alta rechazada si el cliente no existe
- [ ] Alta rechazada si el recurso no existe o no está Disponible
- [ ] Alta rechazada si hay superposición con otra reserva
- [ ] Cancelación de reserva exitosa
- [ ] Modificación de reserva exitosa
- [ ] Si se fuerza un error a mitad del SP (ej: violar un CHECK), se hace ROLLBACK y no queda nada insertado

## Archivos a modificar

- `Scripts SQL/Coworking_db.sql` — agregar los 2 SPs al final del script
