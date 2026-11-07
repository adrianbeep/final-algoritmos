## Descripcion

Vistas del sistema, 2 utilidades concretas que evitan JOINs y COUNTs en C#.

### `vw_reservas_completas`

JOIN de 4 tablas (`reservas`, `clientes`, `recursos`, `tipo_recursos`). Pensada para alimentar el DataGridView del form de Reservas con nombre del cliente, nombre del recurso, ubicacion y tipo, todo en una sola consulta.

### `vw_recursos_estado`

JOIN de `recursos` con `tipo_recursos` + subquery correlacionada que cuenta las reservas vigentes (estado = 'Confirmada' y fecha_fin no alcanzada). Pensada para el form de Recursos: muestra la cantidad de compromisos que tiene cada recurso sin que C# tenga que hacer N queries.

## Notas de diseno

- Las vistas usan `DROP IF EXISTS` antes del `CREATE` para que el script sea idempotente y se pueda correr varias veces
- La subquery de `reservas_activas` esta correlacionada por `id_recurso = r.id`, asi cada fila cuenta solo las reservas de su propio recurso
- Aliases explicitos en columnas (`id_reserva`, `cliente_nombre`, `recurso_nombre`) para que la grilla de C# muestre nombres claros, no ambiguos
- Cero filtros (`WHERE`) ni `ORDER BY` en las vistas: la vista es generica, el que la consulta decide que filtrar y como ordenar

Closes #4
