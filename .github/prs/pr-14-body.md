## Descripcion

Funciones almacenadas del sistema. 5 UDFs en total, cada una con un uso real y visible en el dashboard de la aplicacion.

### UDFs creadas

- `fn_total_clientes()` — total de clientes registrados
- `fn_reservas_por_finalizar()` — reservas confirmadas actualmente en curso (entre fecha_inicio y fecha_fin, no terminadas)
- `fn_reservas_hoy()` — reservas que empiezan hoy
- `fn_recursos_disponibles()` — recursos en estado `Disponible` (inventario bookable)
- `fn_recursos_reservados_hoy()` — recursos DISTINTOS con al menos una reserva hoy

## Uso en el dashboard (Etapa 2)

- Tarjeta "Total clientes" consume `fn_total_clientes`
- Tarjeta "Reservas por finalizar" consume `fn_reservas_por_finalizar`
- Tarjeta "Reservas hoy" consume `fn_reservas_hoy`
- Tarjeta "% ocupacion hoy" consume `fn_recursos_disponibles` y `fn_recursos_reservados_hoy`. El calculo del porcentaje se hace en C#: `reservados_hoy / disponibles * 100`

## Notas de diseno

- Todas son `NOT DETERMINISTIC` porque dependen del estado actual de la BD (cambian al insertar o cancelar reservas)
- Todas son `READS SQL DATA` porque solo hacen SELECT, ninguna modifica datos
- `fn_reservas_por_finalizar` usa `fecha_inicio <= now() AND fecha_fin >= now()` para contar solo las reservas actualmente en curso, no las futuras. Asi "por finalizar" significa "en proceso de finalizarse", no "pendiente"
- `fn_recursos_reservados_hoy` usa `COUNT(DISTINCT id_recurso)` para que un recurso con varias reservas hoy cuente una sola vez
- El archivo usa `DELIMITER //` y `DELIMITER ;` para que el `;` interno de cada UDF no se confunda con el delimitador

## Orden de ejecucion

Se ejecuta despues de `Coworking_db.sql` (necesita las tablas creadas) y antes de cualquier SP que pueda usarlas.

Closes #6
