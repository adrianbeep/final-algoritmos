# [Etapa 1] Crear funciones almacenadas (UDFs) del dashboard

## Objetivo

Crear las 5 funciones definidas por el usuario que alimentan las 4 tarjetas del dashboard. Justifican la existencia de cada UDF con un uso real en la UI.

## Entregables

### UDFs a crear

- [ ] `fn_total_clientes() RETURNS INT` — `SELECT COUNT(*) FROM clientes`
- [ ] `fn_reservas_por_finalizar() RETURNS INT` — `SELECT COUNT(*) FROM reservas WHERE estado_reserva = 'Confirmada' AND fecha_fin >= NOW()`
- [ ] `fn_reservas_hoy() RETURNS INT` — `SELECT COUNT(*) FROM reservas WHERE DATE(fecha_inicio) = CURDATE() AND estado_reserva = 'Confirmada'`
- [ ] `fn_recursos_disponibles() RETURNS INT` — `SELECT COUNT(*) FROM recursos WHERE estado = 'Disponible'`
- [ ] `fn_recursos_reservados() RETURNS INT` — `SELECT COUNT(DISTINCT id_recurso) FROM reservas WHERE estado_reserva = 'Confirmada' AND NOW() BETWEEN fecha_inicio AND fecha_fin`

## Uso en la UI (Etapa 2)

- [ ] Tarjeta "Total clientes" → consume `fn_total_clientes`
- [ ] Tarjeta "Reservas por finalizar" → consume `fn_reservas_por_finalizar`
- [ ] Tarjeta "Reservas hoy" → consume `fn_reservas_hoy`
- [ ] Tarjeta "% ocupación actual" → consume `fn_recursos_disponibles` y `fn_recursos_reservados` (C# calcula el porcentaje)

## Criterio de pase

- [ ] Las 5 funciones devuelven valores consistentes con los datos de prueba
- [ ] Llamar cada función con `SELECT fn_nombre()` desde Workbench y verificar el resultado
- [ ] Verificar que los recuentos cambian cuando se insertan/cancelan reservas de prueba

## Archivos a modificar

- `Scripts SQL/Coworking_db.sql` — agregar las 5 UDFs al final del script
