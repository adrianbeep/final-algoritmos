## Descripcion

Stored procedures de reservas. 3 SPs separados por responsabilidad, todos contra una sola tabla (sin transaction).

### `sp_alta_reserva`

Crea una reserva en estado `Confirmada`. Valida que el cliente exista, que el recurso exista y este Disponible, que las fechas sean validas y que no haya superposicion con otras reservas confirmadas del mismo recurso.

### `sp_modificar_reserva`

Actualiza recurso, fechas y total de una reserva existente. Valida que la reserva exista y este Confirmada, que el nuevo recurso este Disponible, que las fechas sean validas y que no haya superposicion con OTRAS reservas (excluye la actual con `id != p_id_reserva`).

No se puede cambiar el `id_cliente` ni el `estado_reserva` desde aca.

### `sp_cancelar_reserva`

Cambia el estado de una reserva Confirmada a `Cancelada`. No permite cancelar una reserva ya cancelada.

## Notas de diseno

- Ninguno usa transaction ni handler porque los 3 SPs tocan una sola tabla (`reservas`). El autocommit de MySQL alcanza para una operacion simple
- Las validaciones van en orden fail-fast: existencia de la reserva → existencia del recurso → estado del recurso → integridad de fechas → regla de superposicion
- En `sp_alta_reserva` el `estado_reserva` se setea explicito a `'Confirmada'` en el INSERT para no depender del DEFAULT de la tabla
- En `sp_modificar_reserva` se usa `AND id != p_id_reserva` en la validacion de superposicion para excluir la reserva que se esta modificando
- La deteccion de superposicion usa la regla `fecha_inicio <= p_fecha_fin AND fecha_fin >= p_fecha_inicio`, que cubre todos los casos (envolventes, exactos, parciales)
- En `sp_cancelar_reserva` el check `estado_reserva = 'Confirmada'` evita que cancelar dos veces la misma reserva pase silenciosamente

Closes #5
