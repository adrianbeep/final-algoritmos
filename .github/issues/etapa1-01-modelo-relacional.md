# [Etapa 1] Crear modelo relacional: tablas, FKs, restricciones y datos iniciales

## Objetivo

Definir y crear todas las tablas de la base de datos `coworking`, con sus claves primarias, claves foráneas, restricciones de integridad y datos iniciales suficientes para probar el sistema.

## Entregables

### Tablas a crear

- [ ] `clientes` (id, nombre, apellido, dni UNIQUE, telefono, email UNIQUE)
- [ ] `tipo_recursos` (id, nombre_tipo_recurso, descripcion) — carga inicial: Sala, PuestoTrabajo, OficinaPrivada
- [ ] `recursos` (id, id_tipo_recurso FK, nombre, capacidad, estado ENUM, ubicacion, precio_dia)
- [ ] `recursos_detalle` (id_recurso PK + FK, capacidad_personas NULL, tiene_proyector NULL, numero_monitor NULL, metros_cuadrados NULL)
- [ ] `reservas` (id, id_cliente FK, id_recurso FK, fecha_inicio, fecha_fin, estado_reserva ENUM, total_reserva)

### Restricciones de integridad

- [ ] Claves primarias en todas las tablas
- [ ] Claves foráneas: `recursos.id_tipo_recurso → tipo_recursos.id`, `reservas.id_cliente → clientes.id`, `reservas.id_recurso → recursos.id`, `recursos_detalle.id_recurso → recursos.id`
- [ ] UNIQUE en `clientes.dni` y `clientes.email`
- [ ] NOT NULL en campos obligatorios
- [ ] CHECK en `recursos.capacidad >= 0` y `recursos.precio_dia > 0`
- [ ] CHECK en `reservas.fecha_inicio < fecha_fin`

### Datos iniciales

- [ ] Insertar 3 tipos de recurso: Sala, PuestoTrabajo, OficinaPrivada
- [ ] Insertar al menos 3 clientes de prueba
- [ ] Insertar al menos 3 recursos de prueba (uno de cada tipo) con su `recursos_detalle` correspondiente

## Criterio de pase

- [ ] El script SQL corre limpio en MySQL Workbench desde cero (`DROP DATABASE` + `CREATE DATABASE` + tablas + datos)
- [ ] Todas las FKs validadas (no se puede insertar un recurso con `id_tipo_recurso` inexistente)
- [ ] Las restricciones CHECK y UNIQUE rechazan inserciones inválidas
- [ ] Los datos de prueba permiten probar el sistema completo en etapas siguientes

## Archivos a modificar

- `Scripts SQL/Coworking_db.sql` — agregar las tablas nuevas y los datos iniciales
