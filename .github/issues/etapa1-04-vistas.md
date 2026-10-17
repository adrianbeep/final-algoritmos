# [Etapa 1] Crear vistas

## Objetivo

Crear las 2 vistas que alimentan los listados principales de la aplicación. Permiten traer datos ya joineados desde MySQL en lugar de hacer 3-4 queries desde C#.

## Entregables

### Vista `vw_reservas_completas`

- [ ] JOIN de `reservas + clientes + recursos + tipo_recursos`
- [ ] Columnas expuestas:
  - `id_reserva`
  - `fecha_inicio`
  - `fecha_fin`
  - `estado_reserva`
  - `total_reserva`
  - `cliente_nombre` (concatenación de `nombre + ' ' + apellido`)
  - `cliente_dni`
  - `recurso_nombre`
  - `recurso_ubicacion`
  - `tipo_recurso` (nombre del tipo)
- [ ] Usada en el DataGridView de Reservas (Etapa 3)

### Vista `vw_recursos_estado`

- [ ] JOIN de `recursos + tipo_recursos + LEFT JOIN con subquery de reservas activas`
- [ ] Columnas expuestas:
  - `id_recurso`
  - `nombre`
  - `ubicacion`
  - `estado`
  - `precio_dia`
  - `tipo_recurso`
  - `reservas_activas` (COUNT de reservas no canceladas con `fecha_fin >= NOW()`)
- [ ] Usada en el listado principal de Recursos (Etapa 3)

## Criterio de pase

- [ ] Ambas vistas devuelven datos correctos con los datos de prueba del sub-issue #1
- [ ] `vw_reservas_completas` muestra al menos 1 reserva con todos los datos joineados visibles
- [ ] `vw_recursos_estado` muestra al menos 1 recurso con su conteo de reservas activas correcto

## Archivos a modificar

- `Scripts SQL/Coworking_db.sql` — agregar las 2 vistas al final del script
