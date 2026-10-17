# [Etapa 1] Crear procedimientos almacenados simples

## Objetivo

Crear los procedimientos almacenados de alta y modificación para clientes y recursos. No tocan tablas múltiples, así que no requieren transaction ni handler.

## Entregables

### SPs a crear

- [ ] `sp_alta_cliente(p_nombre, p_apellido, p_dni, p_telefono, p_email)`
  - Inserta en `clientes`
  - Validar DNI único antes de insertar
  - Devuelve el ID del cliente insertado
- [ ] `sp_alta_recurso(p_id_tipo_recurso, p_nombre, p_capacidad, p_estado, p_ubicacion, p_precio_dia)`
  - Inserta en `recursos`
  - Validar que `id_tipo_recurso` exista
  - Devuelve el ID del recurso insertado
- [ ] `sp_modificar_recurso(p_id, p_nombre, p_capacidad, p_estado, p_ubicacion, p_precio_dia)`
  - Actualiza `recursos` por ID
  - Validar que el recurso exista

## Criterio de pase

- [ ] Los 3 SPs probados en Workbench con datos válidos
- [ ] Probada la rejection de DNI duplicado
- [ ] Probada la rejection de `id_tipo_recurso` inexistente
- [ ] Probada la rejection de ID inexistente en modificación

## Archivos a modificar

- `Scripts SQL/Coworking_db.sql` — agregar los 3 SPs al final del script
