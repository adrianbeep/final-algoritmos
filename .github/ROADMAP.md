# Roadmap — TP Final: Sistema de Gestión de Coworking y Reservas

> **Carrera:** Analista de Sistemas de Información · Algoritmos II  
> **Stack:** C# · Windows Forms · MySQL  
> **Año:** 2026

---

## Descripción

Roadmap de trabajo del proyecto integrador. Cada etapa corresponde a un hito del desarrollo, con entregables concretos que deben cumplirse antes de pasar a la siguiente.

---

## Etapa 1 — Base de datos

### Modelo relacional
- [ ] Definir entidades: Clientes, Recursos, TiposRecurso, Reservas
- [ ] Tabla `TiposRecurso` con clave primaria y datos suficientes
- [ ] Relación Recursos → TiposRecurso por clave foránea
- [ ] Definir claves primarias y foráneas en todas las tablas
- [ ] Definir restricciones de integridad (NOT NULL, UNIQUE, CHECK)
- [ ] Cargar datos iniciales suficientes para pruebas (incluye tipos precargados: Sala, PuestoTrabajo, OficinaPrivada)

### Tabla `recursos_detalle` (campos específicos por tipo)
- [ ] Tabla `recursos_detalle` con FK a `recursos` (relación 1:1)
- [ ] Campos para Sala: `capacidad_personas`, `tiene_proyector`
- [ ] Campos para PuestoTrabajo: `numero_monitor`
- [ ] Campos para OficinaPrivada: `metros_cuadrados`
- [ ] Todos los campos son NULLable (solo se completa el que corresponde al tipo)

### Procedimientos almacenados
- [ ] `sp_alta_cliente`
- [ ] `sp_alta_recurso`
- [ ] `sp_modificar_recurso`
- [ ] `sp_alta_reserva`
- [ ] `sp_cancelar_modificar_reserva`

> **Nota:** `tipo_recursos` no tiene SP porque sus valores son fijos y se cargan en el script de inicialización. La catedra no exige CRUD sobre tipos.

### Transactions y Handlers en SP de reservas
- [ ] `sp_alta_reserva` con `START TRANSACTION` y `COMMIT`
- [ ] `sp_alta_reserva` con `DECLARE EXIT HANDLER FOR SQLEXCEPTION` que ejecuta `ROLLBACK` y `RESIGNAL`
- [ ] `sp_cancelar_modificar_reserva` con `START TRANSACTION` y `COMMIT`
- [ ] `sp_cancelar_modificar_reserva` con `DECLARE EXIT HANDLER FOR SQLEXCEPTION` que ejecuta `ROLLBACK` y `RESIGNAL`
- [ ] Justificación: alta y cancelación de reserva tocan varias tablas; si algo falla a mitad, no debe quedar inconsistente

### Vistas
- [ ] `vw_reservas_completas` — JOIN de `reservas + clientes + recursos + tipo_recursos`. Usada en el DataGridView de Reservas para mostrar nombre del cliente, nombre del recurso y tipo en una sola consulta
- [ ] `vw_recursos_estado` — Recursos con cantidad de reservas activas y estado actual (Disponible / En Mantenimiento / Inhabilitado). Usada en el listado principal de Recursos

### Función almacenada
- [ ] `fn_total_clientes()` — total de clientes registrados
- [ ] `fn_reservas_por_finalizar()` — reservas que aún no terminaron
- [ ] `fn_reservas_hoy()` — reservas del día actual
- [ ] `fn_recursos_disponibles()` — recursos libres en este momento
- [ ] `fn_recursos_reservados()` — recursos con reserva activa en este momento

### Trigger
- [ ] Un trigger de auditoría sobre una operación importante (ej: alta o cancelación de reserva)

### Pruebas y entrega
- [ ] Probar todos los SP, vistas, función y trigger en Workbench
- [ ] Guardar script SQL completo de creación de la base

---

## Etapa 2 — POO + Interfaz Windows Forms

### Diseño de clases
- [ ] Clase `TipoRecurso` con atributos básicos (Id, Nombre, Descripción) — usada solo como modelo de lectura para cargar ComboBox
- [ ] Clase abstracta `Recurso` con atributos y comportamiento común
- [ ] Clases derivadas: `Sala`, `PuestoTrabajo`, `OficinaPrivada`
- [ ] Cada clase derivada tiene atributos propios mapeados a `recursos_detalle`:
  - `Sala`: `CapacidadPersonas`, `TieneProyector`
  - `PuestoTrabajo`: `NumeroMonitor`
  - `OficinaPrivada`: `MetrosCuadrados`
- [ ] Cada clase derivada implementa `CalcularCosto(int dias)` con regla propia
- [ ] Herencia funcional con polimorfismo (referencia base → objetos derivados)
- [ ] DAO reconstruye la jerarquía con switch sobre `nombre_tipo_recurso` al mapear el JOIN `recursos + recursos_detalle + tipo_recursos`
- [ ] Interfaz `IReservable` con `PuedeReservarse(DateTime fecha)` y `CalcularCosto(int dias)`

### Estructura de la UI
- [ ] Formulario principal con sidebar de navegación + panel central
- [ ] Módulos accesibles desde el sidebar: Dashboard, Clientes, Recursos, Reservas

### Patrón de formulario (Listado + Detalle)
- [ ] Clientes: grid a la izquierda, detalle a la derecha con campos del cliente y botones Nuevo/Guardar/Eliminar/Cancelar
- [ ] Recursos: grid a la izquierda, detalle a la derecha con campos del recurso y botones Nuevo/Guardar/Eliminar/Cancelar. ComboBox de tipo arriba; al cambiar tipo, se muestran/ocultan los campos específicos de `Sala` / `PuestoTrabajo` / `OficinaPrivada`. El ComboBox de tipo se carga desde la tabla `tipo_recursos` (valores fijos)
- [ ] Reservas: grid a la izquierda, detalle a la derecha con campos de la reserva y botones Nuevo/Guardar/Eliminar/Cancelar

### Controles utilizados
- [ ] DataGridView para listados
- [ ] ComboBox con descripción legible e ID interno
- [ ] DateTimePicker para fechas
- [ ] NumericUpDown cuando corresponda
- [ ] Botones de acción (Nuevo, Guardar, Eliminar, Cancelar)
- [ ] Campo de búsqueda por módulo

### Dashboard principal
- [ ] Tarjeta: Total clientes (consume `fn_total_clientes`)
- [ ] Tarjeta: Reservas por finalizar (consume `fn_reservas_por_finalizar`)
- [ ] Tarjeta: Reservas hoy (consume `fn_reservas_hoy`)
- [ ] Tarjeta: % de ocupación actual (consume `fn_recursos_disponibles` y `fn_recursos_reservados`)
- [ ] Accesos directos a las acciones más comunes (nueva reserva, alta cliente, alta recurso)

---

## Etapa 3 — Integración C# + MySQL

- [ ] Clase centralizada `ConexionBD` con la cadena de conexión
- [ ] Ningún formulario repite la cadena de conexión
- [ ] Consumo de los 5 procedimientos almacenados desde los formularios
- [ ] Consumo de las 2 vistas en los listados
- [ ] Consumo de las 5 funciones almacenadas desde el dashboard
- [ ] Parámetros bindeados correctamente (sin concatenación SQL)
- [ ] Flujo: `Form → lógica → ConexionBD → MySQL`

---

## Etapa 4 — Manejo de errores

- [ ] Captura de `MySqlException` en operaciones críticas
- [ ] Diferenciar errores del proveedor vs errores personalizados
- [ ] Regla de negocio validada desde MySQL con `SIGNAL SQLSTATE '45000'`
- [ ] Captura en C# del SQLSTATE 45000 con mensaje específico al usuario
- [ ] Catch genérico `Exception` para errores no específicos de MySQL
- [ ] Ningún bloque `catch` vacío

---

## Etapa 5 — Organización del acceso a datos (DAO)

- [ ] Crear `ClienteDAO`
- [ ] Crear `RecursoDAO`
- [ ] Crear `ReservaDAO`
- [ ] Crear `TipoRecursoDAO` con método de solo lectura (`ObtenerTodos()`) para cargar el ComboBox en el form de Recursos
- [ ] Retirar de los formularios el código directo de acceso a MySQL
- [ ] Lógica de dominio separada de la interfaz
- [ ] Flujo final: `Form → lógica → DAO → ConexionBD → MySQL`

---

## Etapa 6 — Intercambio y configuración

### JSON
- [ ] Al menos una operación de importación o exportación en JSON
- [ ] Usar objetos reales del sistema (ej: catálogo de recursos, reservas)
- [ ] Archivo JSON de ejemplo en la entrega

### Configuración externa
- [ ] Retirar la cadena de conexión del código fuente
- [ ] Leer la conexión desde `appsettings.json` o equivalente
- [ ] `ConexionBD` ya no contiene datos de conexión hardcodeados

---

## Entregables finales

- [ ] Solución completa de Visual Studio
- [ ] Script SQL de creación completa de la base
- [ ] Diagrama del modelo de datos
- [ ] Diagrama de clases simplificado
- [ ] Documentación técnica breve
- [ ] Archivo JSON de ejemplo
- [ ] Instrucciones para ejecutar el proyecto

---

## Criterios de evaluación

| Área | Ponderación |
|---|---|
| Base de datos (modelo, SP, vistas, función, trigger) | 25% |
| POO (clases, herencia, abstracta, interfaz, polimorfismo) | 20% |
| Integración C# + MySQL | 20% |
| Windows Forms (interfaz, controles, navegación) | 15% |
| Evolución del diseño (DAO, JSON, configuración) | 10% |
| Documentación y defensa | 10% |

---

## Dependencias entre etapas

```
Etapa 1 (Base de datos)
        ↓
Etapa 2 (POO + Interfaz)
        ↓
Etapa 3 (Integración)
        ↓
Etapa 4 (Manejo de errores)
        ↓
Etapa 5 (DAO)
        ↓
Etapa 6 (JSON + Config)
```
