# Roadmap — TP Final: Sistema de Gestión de Coworking y Reservas

> **Carrera:** Analista de Sistemas de Información · Algoritmos II  
> **Stack:** C# · Windows Forms · MySQL  
> **Año:** 2026

---

## Estado al 11/11/2026

Lo que falta, en una línea cada cosa:

| Pendiente | Por qué |
|---|---|
| **Búsqueda por módulo** | *Nice to have*, fuera del alcance de la etapa 2 |
| **DAO base** | Cada DAO repite el bloque de abrir conexión y ejecutar. Es una consolidación, no un faltante de la consigna |
| **Documentación técnica breve** | La están escribiendo los integrantes del equipo |

Todo lo demás está implementado y verificado contra la base real.

---

## Descripción

Roadmap de trabajo del proyecto integrador. Cada etapa corresponde a un hito del desarrollo, con entregables concretos que deben cumplirse antes de pasar a la siguiente.

---

## Etapa 1 — Base de datos

### Modelo relacional
- [x] Definir entidades: `clientes`, `recursos`, `tipo_recursos`, `reservas`, `recursos_detalle` y `auditoria_reservas`
- [x] Tabla `tipo_recursos` con clave primaria y datos suficientes
- [x] Relación `recursos` → `tipo_recursos` por clave foránea
- [x] Claves primarias y foráneas en todas las tablas
- [x] Restricciones de integridad (NOT NULL, UNIQUE, CHECK, ENUM)
- [x] Datos iniciales suficientes para pruebas (incluye tipos precargados: Sala, PuestoTrabajo, OficinaPrivada)

> **Nota:** `auditoria_reservas.id_reserva` **no** declara clave foránea hacia `reservas`. La relación es lógica y la mantienen los triggers de auditoría, no el motor. Ver `Docs/DER.md`.

### Tabla `recursos_detalle` (campos específicos por tipo)
- [x] Tabla `recursos_detalle` con FK a `recursos` (relación 1:1)
- [x] Campos para Sala: `capacidad_personas`, `tiene_proyector`
- [x] Campos para PuestoTrabajo: `numero_monitor`
- [x] Campos para OficinaPrivada: `metros_cuadrados`
- [x] Todos los campos son NULLable (solo se completa el que corresponde al tipo)

### Procedimientos almacenados
- [x] `sp_alta_cliente`
- [x] `sp_alta_recurso`
- [x] `sp_modificar_recurso`
- [x] `sp_alta_reserva`
- [x] `sp_modificar_reserva`
- [x] `sp_cancelar_reserva`

> **Nota:** `tipo_recursos` no tiene SP porque sus valores son fijos y se cargan en el script de inicialización. La catedra no exige CRUD sobre tipos.

### Transactions y Handlers en SP de recursos
- [x] `sp_alta_recurso` con `START TRANSACTION` y `COMMIT`
- [x] `sp_alta_recurso` con `DECLARE EXIT HANDLER FOR SQLEXCEPTION` que ejecuta `ROLLBACK` y `RESIGNAL`
- [x] `sp_modificar_recurso` con `START TRANSACTION` y `COMMIT`
- [x] `sp_modificar_recurso` con `DECLARE EXIT HANDLER FOR SQLEXCEPTION` que ejecuta `ROLLBACK` y `RESIGNAL`
- [x] Justificación: el alta y la modificación de recurso escriben en dos tablas (`recursos` + `recursos_detalle`); si algo falla a mitad, no debe quedar inconsistente

> **Nota:** los SP de reservas (`sp_alta_reserva`, `sp_modificar_reserva`, `sp_cancelar_reserva`) no llevan transaction porque escriben en una sola tabla. Sus validaciones de negocio igual levantan `SIGNAL SQLSTATE '45000'`.

### Vistas
- [x] `vw_reservas_completas` — JOIN de `reservas + clientes + recursos + tipo_recursos`. Usada en el DataGridView de Reservas para mostrar nombre del cliente, nombre del recurso y tipo en una sola consulta
- [x] `vw_recursos_estado` — Recursos con cantidad de reservas activas y estado actual (Disponible / En Mantenimiento / Inhabilitado). Usada en el listado principal de Recursos

### Funciones almacenadas
- [x] `fn_total_clientes()` — total de clientes registrados
- [x] `fn_reservas_por_finalizar()` — reservas que aún no terminaron
- [x] `fn_reservas_hoy()` — reservas del día actual
- [x] `fn_recursos_disponibles()` — recursos en estado `Disponible` (inventario bookable)
- [x] `fn_recursos_reservados_hoy()` — recursos distintos con al menos una reserva hoy
- [x] `fn_recurso_ocupado()` — si un recurso ya tiene una reserva confirmada que se superpone con el período. La usan `sp_alta_reserva` y `sp_modificar_reserva` para rechazar, y el modal de reservas para avisar antes de guardar

### Trigger
- [x] Trigger de auditoría sobre una operación importante (alta o cancelación de reserva)
- [x] `trg_auditoria_reservas_insert` y `trg_auditoria_reservas_update`

### Pruebas y entrega
- [x] Probar todos los SP, vistas, funciones y triggers en Workbench
- [x] Guardar script SQL completo de creación de la base

> **Nota:** los cinco scripts se ejecutan en orden y cada uno incluye `USE coworking;`, así que la base se crea desde cero sin seleccionar el esquema a mano.

---

## Etapa 2 — POO + Interfaz Windows Forms

### Diseño de clases
- [x] Clase `TipoRecurso` con atributos básicos (Id, Nombre, Descripción) — usada solo como modelo de lectura para cargar ComboBox
- [x] Clase abstracta `Recurso` con atributos y comportamiento común
- [x] Clases derivadas: `Sala`, `PuestoTrabajo`, `OficinaPrivada`
- [x] Cada clase derivada tiene atributos propios mapeados a `recursos_detalle`:
  - `Sala`: `CapacidadPersonas`, `TieneProyector`
  - `PuestoTrabajo`: `NumeroMonitor`
  - `OficinaPrivada`: `MetrosCuadrados`
- [x] Cada clase derivada implementa `CalcularPrecio()` con regla propia
- [x] Herencia funcional con polimorfismo (referencia base → objetos derivados)
- [x] `RecursoFactory` reconstruye la jerarquía a partir de `nombre_tipo_recurso` al mapear el JOIN `recursos + recursos_detalle + tipo_recursos`
- [x] Interfaz `IReservable` con `PuedeReservarse(DateTime, DateTime)` y `CalcularPrecio()`

### Estructura de la UI
- [x] Formulario principal con sidebar de navegación + panel central
- [x] Módulos accesibles desde el sidebar: Clientes, Recursos, Reservas y Dashboard

### Patrón de formulario (Listado + Modal)
- [x] Clientes: grid a la izquierda y modal de detalle con campos del cliente y botones Guardar/Cancelar
- [x] Recursos: grid a la izquierda y modal de detalle. ComboBox de tipo arriba; al cambiar tipo, se muestran/ocultan los campos específicos de `Sala` / `PuestoTrabajo` / `OficinaPrivada`. El ComboBox de tipo se carga desde la tabla `tipo_recursos` (valores fijos)
- [x] Reservas: grid a la izquierda y modal de detalle con campos de la reserva y botones Guardar/Cancelar

> **Nota:** el detalle va en un modal y no en un panel embebido a la derecha, para que el formulario noMezcle la lista con la edición. Ver la decisión D-005 en `Docs/Decisiones.md`.

### Controles utilizados
- [x] DataGridView para listados
- [x] ComboBox con descripción legible e ID interno (buscables, con filtro por `Contains`)
- [x] DateTimePicker para fechas
- [ ] NumericUpDown cuando corresponda — no se usa: ningún campo numérico del dominio se edita en la interfaz
- [x] Botones de acción (Nuevo en el formulario, Guardar/Cancelar en el modal)
- [ ] Campo de búsqueda por módulo — *nice to have: fuera del alcance de la etapa 2. Queda para una entrega posterior si sobra tiempo.*

### Dashboard principal
- [x] Tarjeta: Total clientes (consume `fn_total_clientes`)
- [x] Tarjeta: Reservas por finalizar (consume `fn_reservas_por_finalizar`)
- [x] Tarjeta: Reservas hoy (consume `fn_reservas_hoy`)
- [x] Tarjeta: % de ocupación actual (consume `fn_recursos_disponibles` y `fn_recursos_reservados_hoy`)
- [ ] Accesos directos a las acciones más comunes (nueva reserva, alta cliente, alta recurso)

> **Nota:** las cuatro tarjetas están en `FrmDashboard` y se actualizan con el botón "Actualizar" (PR #46). Lo que queda pendiente es la quinta: los accesos directos a las acciones más comunes.

---

## Etapa 3 — Integración C# + MySQL

- [x] Clase centralizada `ConexionBD` con la cadena de conexión
- [x] Ningún formulario repite la cadena de conexión
- [x] Capa DAO en `Datos/` que encapsula todo el acceso a datos
- [x] Consumo de los 7 procedimientos almacenados desde los DAO
- [x] Consumo de las 2 vistas en los listados
- [x] Consumo de las funciones de indicadores desde el dashboard — las cinco de indicadores se muestran en las tarjetas de `FrmDashboard` (PR #46)
- [x] Parámetros bindeados correctamente (sin concatenación SQL)
- [x] Cero SQL embebido fuera de `Datos/`
- [x] Flujo: `Form → lógica → DAO → ConexionBD → MySQL`

---

## Etapa 4 — Manejo de errores

- [x] Captura de `MySqlException` en operaciones críticas
- [x] Diferenciar errores del proveedor vs errores personalizados
- [x] Regla de negocio validada desde MySQL con `SIGNAL SQLSTATE '45000'`
- [x] Captura en C# del SQLSTATE 45000 con mensaje específico al usuario
- [x] Catch genérico `Exception` para errores no específicos de MySQL
- [x] Ningún bloque `catch` vacío

> **Nota:** la captura es explícita en cada punto, sin un helper que la esconda. Ver la decisión D-010 en `Docs/Decisiones.md`.

---

## Etapa 5 — Organización de la capa de acceso a datos

> La capa DAO se crea en la Etapa 3. Esta etapa la consolida y la reutiliza.

- [ ] Extraer un DAO base con el comportamiento común (abrir conexión, ejecutar, mapear, dispose)
- [ ] Unificar el manejo de conexión y recursos en un solo lugar
- [x] Evaluar un factory o pool de conexiones si el volumen lo justifica — evaluado: no hace falta un pool propio, `MySqlConnector` poola las conexiones y el `using` las devuelve
- [x] `TipoRecursoDAO` mantiene `ObtenerTodos()` de solo lectura para el ComboBox de Recursos
- [x] Lógica de dominio separada de la interfaz
- [x] Flujo final: `Form → lógica → DAO → ConexionBD → MySQL`

> **Nota:** el `using` con `ConexionBD.CrearConexion()` ya unifica el cierre de la conexión en todos los DAO. Lo que queda es extraer la apertura y el mapeo a una clase base, que es una consolidación interna.

---

## Etapa 6 — Intercambio y configuración

### JSON
- [x] Al menos una operación de importación o exportación en JSON
- [x] Usar objetos reales del sistema: la exportación serializa las entidades `Cliente` del modelo, no las filas de la grilla
- [x] Archivo JSON de ejemplo en la entrega (`Docs/catalogo-clientes.json`)

### Configuración externa
- [x] Retirar la cadena de conexión del código fuente
- [x] Leer la conexión desde `appsettings.json` o equivalente
- [x] `ConexionBD` ya no contiene datos de conexión hardcodeados

---

## Entregables finales

- [x] Solución completa de Visual Studio
- [x] Script SQL de creación completa de la base
- [x] Diagrama del modelo de datos (`Docs/DER.md`)
- [x] Diagrama de clases simplificado (`Docs/Diagrama-clases.md`)
- [ ] Documentación técnica breve — la están escribiendo los integrantes del equipo
- [x] Archivo JSON de ejemplo (`Docs/catalogo-clientes.json`)
- [x] Instrucciones para ejecutar el proyecto (`README.md`)

---

## Criterios de evaluación

| Área | Ponderación |
|---|---|
| Base de datos (modelo, SP, vistas, funciones, trigger) | 25% |
| POO (clases, herencia, abstracta, interfaz, polimorfismo) | 20% |
| Integración C# + MySQL | 20% |
| Windows Forms (interfaz, controles, navegación) | 15% |
| Evolución del diseño (DAO, JSON, configuración) | 10% |
| Documentación y defensa | 10% |

---

## Dependencias entre etapas

Las etapas son **agrupamientos de contenido**, no una secuencia bloqueante. Los módulos del
sistema no colisionan entre sí, asi que se puede avanzar en paralelo sobre distintas partes
(por ejemplo, el dashboard mientras se wirea el ABM de clientes). Lo que no se puede es
cerrar una etapa dejando sus entregables a medias: si algo quedo pendiente de una etapa
anterior, se cierra en la siguiente y queda explicitado en su checklist.

```
Etapa 1 (Base de datos)
        ↓
Etapa 2 (POO + Interfaz)
        ↓
Etapa 3 (Integración: ConexionBD + capa DAO)
        ↓
Etapa 4 (Manejo de errores + wiring pendiente)
        ↓
Etapa 5 (Consolidación de la capa de acceso a datos)
        ↓
Etapa 6 (JSON + Config)
```
