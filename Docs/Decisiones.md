# Decisiones de diseño

Registro cronológico de las decisiones de diseño tomadas durante el desarrollo del **Sistema de Gestión de Coworking y Reservas**.

El documento sigue el formato ADR (*Architecture Decision Record*): cada decisión deja por escrito **contexto**, **decisión**, **alternativas descartadas** y **consecuencias**. Se escribe en el momento en que se decide, no de forma retrospectiva: si más adelante una decisión se revierte, la entrada original se marca como *Reemplazada* y se agrega la entrada nueva que la cambia.

Este archivo es documentación técnica del proyecto: sirve para la entrega, para la defensa y para que cualquiera que tome el repositorio entienda **por qué** el sistema es como es.

---

## Índice

| ID | Decisión | Fecha | Estado |
|---|---|---|---|
| D-001 | [Descomponer el sistema en issues de GitHub](#d-001--descomponer-el-sistema-en-issues-de-github) | 02/11/2026 | Aceptada |
| D-002 | [Extender `recursos` con una tabla de detalle 1:1](#d-002--los-atributos-especificos-de-cada-tipo-viven-en-una-tabla-de-extensión-11-recursos_detalle) | 17/10/2026 | Aceptada |
| D-003 | [El precio diario es un valor derivado, no un dato guardado](#d-003--el-precio-diario-es-un-valor-derivado-no-un-dato-guardado) | 04/11/2026 | Aceptada |

---

## D-001 — Descomponer el sistema en issues de GitHub

**Fecha:** 02/11/2026
**Estado:** Aceptada
**Ámbito:** Proceso de trabajo (afecta a todo el proyecto)

### Contexto

El sistema completo es grande: base de datos (modelo relacional, procedimientos almacenados, vistas, funciones y trigger), POO con jerarquía de clases, interfaz Windows Forms con cuatro módulos, capa de acceso a datos, manejo de errores, intercambio JSON y configuración externa. Además son tres personas y no todas ven todo el sistema a la vez, y la entrega tiene etapas con requisitos claros de la cátedra.

Trabajar todo junto produce dos problemas:

- **Imposible de revisar.** Un diff de miles de líneas no se audita, ni por nosotros ni por la cátedra.
- **Sin trazabilidad.** Cuando algo se rompe dos meses después, no hay forma de saber *por qué* el código quedó como quedó, ni qué parte del sistema tenía que cambiar realmente.

### Decisión

El desarrollo se organiza como una **descomposición del sistema en issues de GitHub**, una unidad de trabajo por issue:

- **Un issue = una unidad de trabajo cohesiva y revisable.** Cada issue define objetivo, alcance, criterio de pase y archivos a crear o modificar. Nada de "y de paso agrego el módulo de reservas".
- **Los bodies de los issues se versionan en el repo** en `.github/issues/`, con el patrón `etapaN-XX-slug.md`. El issue de GitHub se crea a partir de ese body, así que el cuerpo de la tarea queda en el historial de Git y no solo en la plataforma.
- **Cada issue se cierra con un PR** cuyo body se versiona en `.github/prs/pr-N-body.md`, con contexto, qué incluye, verificación y pendientes.
- **El seguimiento visual queda en GitHub Projects**, como tablero Kanban del proyecto.
- **Las etapas del ROADMAP son agrupamientos de contenido, no una secuencia bloqueante.** Como los módulos del sistema no colisionan entre sí, se puede avanzar en paralelo sobre partes distintas (por ejemplo, el dashboard mientras se wirea el ABM de clientes). Lo que no se permite es cerrar una etapa con entregables a medias.

### Alternativas descartadas

- **Una única entrega monolítica (un branch largo, un PR final).** Se descartó porque elimina la revisión posible y oculta el orden real de resolución de problemas. Además, con la defensa oral de la cátedra, no queda material para mostrar el proceso de diseño.
- **Trabajar directo sobre `main` sin trazabilidad de tareas.** Se descartó porque las decisiones quedan solo en la cabeza del equipo y en los mensajes sueltos.
- **Usar el ROADMAP como lista secuencial estricta.** Se descartó como unidad de planificación: el ROADMAP define *qué* hay que entregar y en qué bloques, pero no encaja con el paralelismo entre módulos ni con el tamaño real de cada tarea.

### Consecuencias

**A favor:**

- Cada cambio entra por un PR chico y revisable, con su criterio de pase explícito.
- El historial de Git queda alineado con el historial de decisiones: se puede reconstruir *qué* se hizo y *por qué*.
- El trabajo se puede repartir entre los tres integrantes sin pisarse: cada issue tiene su alcance de archivos.
- La planificación deja de depender de una sola persona: el tablero muestra qué está abierto, qué está en revisión y qué está bloqueado.

**En contra (y cómo se mitigan):**

- **Overhead de gestión.** Cada issue, body versionado y PR agrega ceremonia. Se acepta porque el proceso ya está armado con templates y porque la mayor parte del esfuerzo es de implementación, no de gestión.
- **Riesgo de fragmentación.** Con muchas unidades chicas se puede perder de vista el diseño global. Se mitiga con este documento de decisiones y con el ROADMAP, que son los lugares donde se consolida la visión del conjunto.

### Referencias

- `.github/ROADMAP.md` — plan de trabajo por etapas
- `.github/issues/` — bodies versionados de los issues
- `.github/prs/` — bodies versionados de los PRs

---

## D-002 — Los atributos específicos de cada tipo viven en una tabla de extensión 1:1 (`recursos_detalle`)

**Fecha:** 17/10/2026 (decidida) · 04/11/2026 (documentada)
**Estado:** Aceptada
**Ámbito:** Base de datos — modelo relacional

### Contexto

El coworking tiene tres tipos de recurso, y cada uno tiene atributos propios que los demás no tienen:

| Tipo | Atributos exclusivos |
|---|---|
| Sala | `capacidad_personas`, `tiene_proyector` |
| PuestoTrabajo | `numero_monitor` |
| OficinaPrivada | `metros_cuadrados` |

Aparte de los atributos comunes (`nombre`, `ubicacion`, `estado`, `precio_dia`), hay un requisito del proyecto: el dominio tiene que estar modelado con POO — clase abstracta `Recurso` y tres clases derivadas — y **esa jerarquía tiene que quedar reflejada en la base**, no solo en el código C#. Los atributos propios de cada clase derivada necesitan un lugar donde persistirse.

El problema de diseño concreto era: ¿cómo se guardan los atributos específicos de cada tipo sin romper la integridad referencial de `reservas`, que necesita apuntar a un recurso único?

### Decisión

Los atributos comunes viven en `recursos`. Los atributos específicos de cada tipo se guardan en una **tabla de extensión `recursos_detalle`** con relación **1:1** con `recursos`:

- `recursos_detalle.id_recurso` es **PK y FK** a `recursos.id`. La PK garantiza que un recurso tiene como máximo un detalle.
- `recursos_detalle` tiene las cuatro columnas de los tres tipos, **todas NULLable**, y solo se completa la que corresponde al tipo del recurso:

| Tipo | Columnas que se completan | Columnas que quedan en NULL |
|---|---|---|
| Sala | `capacidad_personas`, `tiene_proyector` | `numero_monitor`, `metros_cuadrados` |
| PuestoTrabajo | `numero_monitor` | `capacidad_personas`, `tiene_proyector`, `metros_cuadrados` |
| OficinaPrivada | `metros_cuadrados` | `capacidad_personas`, `tiene_proyector`, `numero_monitor` |

- **La regla "este recurso es una Sala, entonces `capacidad_personas` es obligatoria" vive en los procedimientos almacenados**, no en el schema. `sp_alta_recurso` y `sp_modificar_recurso` leen `tipo_recursos.nombre_tipo_recurso` y levantan `SIGNAL SQLSTATE '45000'` con un mensaje específico cuando falta el campo obligatorio del tipo.
- **Toda alta o modificación de recurso escribe en dos tablas** (`recursos` + `recursos_detalle`). Por eso esos dos SP llevan `START TRANSACTION` / `COMMIT` y un `DECLARE EXIT HANDLER FOR SQLEXCEPTION` que ejecuta `ROLLBACK` y hace `RESIGNAL`: o se escribe el recurso completo, o no se escribe nada.

### Alternativas descartadas

- **Todas las columnas de detalle dentro de `recursos`** (tabla única con columnas NULLable). Se descartó porque agrega un `ALTER TABLE` sobre la tabla más consultada del sistema cada vez que aparece un tipo nuevo, mezcla atributos comunes con específicos y multiplica los NULLs en todas las filas.
- **Una tabla por tipo** (`salas`, `puestos_trabajo`, `oficinas_privadas`) con `recursos.id_tipo_recurso` como discriminador (*Table Per Type*). Se descartó porque obliga a que toda consulta que junta recurso con detalle sea un `LEFT JOIN` de tres tablas, y porque `reservas` y la validación de superposiciones pasarían a depender del discriminador. El modelo pedido por la cátedra se resuelve con una sola extensión.
- **Guardar los atributos específicos en una columna JSON**. Se descartó porque no es consultable ni indexable, y porque elTP pide modelado relacional.

### Consecuencias

**A favor:**

- La jerarquía POO queda reflejada en la base: `Recurso` abstracta ↔ `recursos` + `recursos_detalle`.
- La FK de `reservas` sigue apuntando a **una sola tabla**. Ni el control de superposiciones ni los listados necesitan saber el tipo del recurso.
- Un único `JOIN` (`recursos + recursos_detalle + tipo_recursos`) devuelve el recurso completo, y es el que usa el DAO para reconstruir la jerarquía en C#.
- Agregar un cuarto tipo es `INSERT` en `tipo_recursos` más columnas en `recursos_detalle`: **no toca `recursos`, ni `reservas`, ni el código de reservas**.

**En contra (y cómo se mitiga):**

- **`recursos_detalle` no está en 3NF estricta**: tiene columnas de los tres tipos, y **el schema no impide completar una columna que no corresponde al tipo** (por ejemplo, cargar `numero_monitor` en una Sala). Hoy lo evita el flujo de la aplicación, que solo envía los campos del tipo seleccionado. La validación de los SP cubre *"el campo obligatorio del tipo está"*, pero no *"los campos de los otros tipos están vacíos"*. → Mejora pendiente: un `SIGNAL` en el SP que rechace explícitamente los campos ajenos al tipo.
- **El 1:1 no lo garantiza el schema**: la PK sobre `id_recurso` asegura que un recurso tiene *como máximo* un detalle, pero nada obliga a que todo recurso *tenga* un detalle. El DER afirma "1:1 estricto, todo recurso tiene su detalle" y eso hoy lo garantiza el flujo de escritura (el SP siempre inserta la fila de detalle), no una restricción de la base.
- **El costo de la escritura en dos tablas** es la transaction obligatoria de los SP de recursos, que los SP de reservas no necesitan.

### Referencias

- `Scripts SQL/Coworking_db.sql` — creación de `recursos` y `recursos_detalle`
- `Scripts SQL/Procedimientos_Almacenados.sql` — `sp_alta_recurso`, `sp_modificar_recurso`
- `Docs/DER.md` — diagrama y tabla de relaciones

---

## D-003 — El precio diario es un valor derivado, no un dato guardado

**Fecha:** 04/11/2026
**Estado:** Aceptada
**Ámbito:** Modelo de dominio + capa de datos

### Contexto

El precio diario de un recurso depende de dos cosas: su **costo base** y los **atributos propios de su tipo** (si tiene proyector, cuántos monitores, cuántos metros cuadrados). La regla es distinta en cada tipo, así que la cátedra la pide como polimorfismo: `Recurso` abstracta con `CalcularPrecio()` y tres implementaciones (`Sala`, `PuestoTrabajo`, `OficinaPrivada`).

El problema era que esa regla estaba escrita **dos veces**:

- en el modelo, como `CalcularPrecio()`;
- en la base, como la columna `recursos.precio_dia`.

Dos representaciones del mismo valor, en dos lugares, sin ninguna garantía de que coincidieran. Y el desajuste se agravaba por un defecto en el procedimiento almacenado: `sp_modificar_recurso` **recibía el parámetro `p_costo` y nunca lo escribía**, así que el costo base quedaba congelado en el alta mientras `precio_dia` sí se actualizaba. Cualquier modificación dejaba el par desincronizado: la base decía un precio que el modelo no podía reproducir.

A esto se sumaba que `CalcularPrecio()` **no tenía un solo call site** en toda la aplicación: la regla de precio estaba declarada, pero no decidía nada.

### Decisión

- **`costo` es el dato de entrada**: el costo base del recurso. Se guarda y se modifica.
- **`precio_dia` es un valor derivado**: no se puede setear. En `Recurso`, `PrecioDia` es una propiedad calculada (`=> CalcularPrecio()`). Es imposible construir un `Recurso` con el precio desincronizado del costo base.
- **La regla de precio vive solo en C#.** No se replica en los procedimientos almacenados.
- La **columna `recursos.precio_dia` se mantiene**, porque `vw_recursos_estado` la expone al listado y el precio se consulta en cada fila. Es una desnormalización controlada: su valor es siempre `CalcularPrecio()`.
- Las **dos vías de escritura del DAO** (`Insertar` y `Modificar`) mandan `p_precio_dia = recurso.CalcularPrecio()`. La derivación es estructural, no una convención que hay que recordar.
- `sp_modificar_recurso` escribe `costo` junto con `precio_dia`.

### Alternativas descartadas

- **Duplicar la regla de precio dentro del procedimiento almacenado**, que calculase `precio_dia` con un `CASE` por tipo. Se descartó por dos razones concretas. La primera: la regla quedaría escrita en dos idiomas y divergiría con el tiempo. La segunda, y más importante: **el redondeo no es el mismo**. `Math.Round` de .NET usa *banker's rounding* (redondea al par) y `ROUND` de MySQL redondea hacia arriba; un valor como `1499.995` daría `1499.99` en C# y `1500.00` en SQL. Con la regla en un solo lado, ese caso no puede aparecer.
- **No guardar `precio_dia` y calcularlo en la vista `vw_recursos_estado`.** Se descartó por la misma razón que el punto anterior: la regla quedaría duplicada en SQL.
- **Dejar `precio_dia` como columna editable, independiente del costo base.** Se descartó porque vacía de sentido a `CalcularPrecio()`: el polimorfismo quedaría decorativo y el precio dejaría de estar relacionado con los atributos del recurso. La cátedra pide una jerarquía **con comportamiento propio**, no una etiqueta.
- **Mover el cálculo al procedimiento almacenado y dejar el modelo como una copia.** Se descartó porque la regla de negocio pertenece al modelo de dominio. El procedimiento almacenado es la capa que valida invariantes de datos (existencia, unicidad, no superposición de reservas), no la que define precios.

### Consecuencias

**A favor:**

- **Es imposible que `costo` y `precio_dia` queden desincronizados**: el precio no se puede setear, se calcula siempre a partir del costo base y de los atributos.
- `CalcularPrecio()` deja de ser código muerto y pasa a ser la **única fuente** del precio, usada en las dos vías de escritura.
- El polimorfismo exigido por la cátedra queda con un efecto **observable en el flujo real**, y no solo declarado en la jerarquía.
- `precio_dia` sigue disponible para el listado sin recalcular el precio fila por fila en C#.
- Los procedimientos almacenados quedan más simples: validan datos, no calculan precios.

**En contra (y cómo se mitiga):**

- **La columna `precio_dia` es redundante en la base.** Si alguien edita la fila a mano en Workbench, puede dejarla inconsistente con `costo`: la base no lo impide, porque el SP confía en el valor derivado que le manda el DAO. Se acepta: la única vía de escritura de la aplicación es el DAO.
- **El redondeo queda en un solo lado (C#)**, que es justamente lo que se buscaba, pero implica que el `precio_dia` guardado tiene el redondeo de .NET y no el de MySQL.
- **`RecursoDAO.Insertar` cambió de firma**: pasó de diez parámetros sueltos a `Insertar(Recurso recurso)`, simétrico con `Modificar`. No tenía callers (el formulario de Recursos todavía no existe), pero a partir de ahora quien construya el recurso tiene que pasar por el modelo y no por primitivos sueltos.

**Pendiente:** el total de una reserva todavía **no** sale de `CalcularPrecio()`, y `IReservable.PuedeReservarse()` sigue sin uso: la interfaz completa está sin uso: la validación de disponibilidad y de superposición vive hoy en los procedimientos almacenados. Eso se resuelve cuando se cablee el módulo de Reservas.

### Referencias

- `SistemaCoworking/Modelos/Recurso.cs` — `PrecioDia` derivado y `CalcularPrecio()` abstracto
- `SistemaCoworking/Modelos/Sala.cs`, `PuestoTrabajo.cs`, `OficinaPrivada.cs` — la regla de precio de cada tipo
- `SistemaCoworking/Datos/RecursoDAO.cs` — las dos vías de escritura
- `Scripts SQL/Procedimientos_Almacenados.sql` — `sp_alta_recurso`, `sp_modificar_recurso`
- `Scripts SQL/Coworking_db.sql` — columnas `costo` y `precio_dia`
