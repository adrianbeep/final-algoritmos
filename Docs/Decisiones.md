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
| D-004 | [`tipo_recursos` es un catálogo fijo, de solo lectura](#d-004--tipo_recursos-es-un-catálogo-fijo-de-solo-lectura-desde-la-aplicación) | 17/10/2026 | Aceptada |
| D-005 | [La fábrica de recursos vive en el dominio](#d-005--la-fábrica-de-recursos-vive-en-el-dominio-no-en-la-capa-de-datos) | 04/11/2026 | Aceptada |
| D-006 | [Cada recurso declara sus propios datos de tipo](#d-006--cada-recurso-declara-sus-propios-datos-de-tipo) | 04/11/2026 | Aceptada |
| D-007 | [La regla de superposicion de reservas vive en la base, en una funcion](#d-007--la-regla-de-superposicion-de-reservas-vive-en-la-base-en-una-funcion) | 10/11/2026 | Aceptada |

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
- **Guardar los atributos específicos en una columna JSON**. Se descartó porque no es consultable ni indexable, y porque el TP pide modelado relacional.

### Consecuencias

**A favor:**

- La jerarquía POO queda reflejada en la base: `Recurso` abstracta ↔ `recursos` + `recursos_detalle`.
- La FK de `reservas` sigue apuntando a **una sola tabla**. Ni el control de superposiciones ni los listados necesitan saber el tipo del recurso.
- Un único `JOIN` (`recursos + recursos_detalle + tipo_recursos`) devuelve el recurso completo, y es el que usa la capa de datos para reconstruir la jerarquía en C# (D-005).
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

**Pendiente (resuelto en D-007, 10/11/2026):** al escribir esta decisión, el total de una reserva todavía **no** salía de `CalcularPrecio()`, y `IReservable.PuedeReservarse()` seguía sin uso: la interfaz completa estaba sin uso y la validación de disponibilidad y de superposición vivía en los procedimientos almacenados. Con el módulo de Reservas cableado, el total sale de `CalcularPrecio()` (`Reserva.CalcularTotal`) y `PuedeReservarse()` decide la disponibilidad: la interfaz tiene uso real, y la regla de superposición vive en la base, en una función.

### Referencias

- `SistemaCoworking/Modelos/Recurso.cs` — `PrecioDia` derivado y `CalcularPrecio()` abstracto
- `SistemaCoworking/Modelos/Sala.cs`, `PuestoTrabajo.cs`, `OficinaPrivada.cs` — la regla de precio de cada tipo
- `SistemaCoworking/Datos/RecursoDAO.cs` — las dos vías de escritura
- `Scripts SQL/Procedimientos_Almacenados.sql` — `sp_alta_recurso`, `sp_modificar_recurso`
- `Scripts SQL/Coworking_db.sql` — columnas `costo` y `precio_dia`

---

## D-004 — `tipo_recursos` es un catálogo fijo, de solo lectura desde la aplicación

**Fecha:** 17/10/2026 (decidida) · 04/11/2026 (documentada)
**Estado:** Aceptada
**Ámbito:** Base de datos — modelo relacional

### Contexto

El dominio tiene tres tipos de recurso, y cada uno arrastra **comportamiento**, no solo datos:

| Tipo | Clase derivada | Columnas propias | Regla de precio |
|---|---|---|---|
| Sala | `Sala` | `capacidad_personas`, `tiene_proyector` | +25% con proyector |
| PuestoTrabajo | `PuestoTrabajo` | `numero_monitor` | +10% por monitor adicional |
| OficinaPrivada | `OficinaPrivada` | `metros_cuadrados` | +20% si supera los 20 m² |

Eso obliga a decidir una cosa que parece de datos pero no lo es: **¿`tipo_recursos` es una tabla administrable, con su propio CRUD, o es un catálogo fijo?** La diferencia importa: si el operador pudiera inventar un tipo desde la pantalla, el sistema no sabría qué clase instanciar, qué columnas mostrar ni con qué regla calcular el precio.

La cátedra tampoco exige CRUD sobre tipos: el ROADMAP lo deja asentado de forma explícita ("`tipo_recursos` no tiene SP porque sus valores son fijos y se cargan en el script de inicialización. La cátedra no exige CRUD sobre tipos").

### Decisión

`tipo_recursos` es un **catálogo fijo, sembrado en el script de inicialización y de solo lectura desde la aplicación**:

- La tabla tiene tres columnas: `id` (PK autoincremental), `nombre_tipo_recurso` (not null) y `descripcion` (informativa).
- Los valores se cargan en `Coworking_db.sql`: `Sala`, `PuestoTrabajo`, `OficinaPrivada`.
- **No hay procedimientos almacenados para esta tabla**: ni alta, ni modificación, ni baja.
- `TipoRecursoDAO` expone **una sola operación**, `ObtenerTodos()`, que sirve para cargar el ComboBox de tipo del formulario de Recursos.
- La FK `recursos.id_tipo_recurso` conserva la integridad referencial: ningún recurso puede apuntar a un tipo inexistente.
- `nombre_tipo_recurso` es el **contrato** que usan tanto los procedimientos almacenados (para validar los campos obligatorios del tipo) como `RecursoFactory` (para reconstruir la jerarquía de clases, D-005).

### Alternativas descartadas

- **CRUD completo sobre `tipo_recursos` desde la aplicación.** Se descartó porque un tipo no es un dato de negocio que el operador pueda inventar: es una clase derivada, un juego de columnas en `recursos_detalle` y una regla de precio. Un tipo creado desde la UI no tendría clase, ni columnas, ni regla, y el sistema no sabría ni cómo mostrarlo ni cuánto cobrar. El catálogo lo determina el diseño, no los datos.
- **Guardar el tipo como ENUM en `recursos.id_tipo_recurso`**, en lugar de una tabla. Se descartó por tres razones. La primera: el proyecto **ya usa ENUM donde corresponde** (`recursos.estado`, `reservas.estado_reserva`) — valores de estado, que no cargan comportamiento. El tipo de recurso sí lo carga, y esa es exactamente la diferencia. La segunda: un ENUM exige un `ALTER TABLE` para agregar un tipo y no tiene dónde guardar la descripción. La tercera: la tabla da integridad referencial de verdad.
- **Eliminar la tabla y guardar el nombre del tipo directamente en `recursos`.** Se descartó porque se pierde la integridad referencial (el nombre se repetiría en cada recurso y en cada JOIN) y se pierde la descripción.

### Consecuencias

**A favor:**

- El catálogo y la jerarquía de clases quedan alineados: cada fila tiene su clase derivada, sus columnas y su regla de precio.
- `TipoRecursoDAO` tiene una sola responsabilidad —leer— y su API lo refleja: un único método.
- **No existe ningún camino de escritura** que pueda dejar la base desalineada del código.
- La integridad referencial se mantiene: `recursos.id_tipo_recurso` no puede apuntar a un tipo inexistente.
- Los procedimientos almacenados pueden ramificar sobre `nombre_tipo_recurso` para validar los campos obligatorios de cada tipo.

**En contra (y cómo se mitiga):**

- **Agregar un tipo nuevo no es una operación de datos, es un cambio de diseño**: nueva clase derivada, columnas nuevas en `recursos_detalle`, una rama más en `RecursoFactory.Crear` y las validaciones del SP. Es el costo directo de que el tipo sea comportamiento.
- **El `nombre_tipo_recurso` es un contrato de código, y la base lo restringe.** Ese nombre se compara en los procedimientos almacenados (`IF v_tipo = 'Sala'`) y en `RecursoFactory.Crear`, así que la tabla declara `nombre_tipo_recurso varchar(100) not null unique`: la base no puede dejar dos filas con el mismo tipo. Con esa restricción, un seed duplicado hace fallar el `INSERT` en vez de dejar que la fábrica tome la primera fila sin avisar y el SP valide mal.
- **El despacho por tipo es sobre string.** `RecursoFactory.Crear` compara contra `Sala`, `PuestoTrabajo` y `OficinaPrivada`, y el `default` lanza `InvalidOperationException`. Un error de tipeo en el seed no se detecta al compilar, sino en runtime.

### Referencias

- `Scripts SQL/Coworking_db.sql` — creación y seed de `tipo_recursos`
- `SistemaCoworking/Datos/TipoRecursoDAO.cs` — la única operación de lectura
- `SistemaCoworking/Modelos/TipoRecurso.cs`
- `Scripts SQL/Procedimientos_Almacenados.sql` — validación por tipo en `sp_alta_recurso` y `sp_modificar_recurso`
- `.github/ROADMAP.md` — nota de que `tipo_recursos` no tiene procedimientos almacenados

---

## D-005 — La fábrica de recursos vive en el dominio, no en la capa de datos

**Fecha:** 04/11/2026
**Estado:** Aceptada
**Ámbito:** Modelo de dominio + capa de datos

### Contexto

Hay **dos caminos** para obtener un `Recurso` concreto, y los dos toman exactamente la misma decisión: *qué clase corresponde a qué tipo*.

| Camino | Origen | Destino |
|---|---|---|
| Desde la base | el DAO lee una fila | `Sala`, `PuestoTrabajo` u `OficinaPrivada` |
| Desde el formulario | el `RecursoModalControl` toma lo que tipeó el usuario | las mismas tres clases |

Ese conocimiento tiene que estar en **un solo lugar**. Si está duplicado, un cuarto tipo obliga a tocar los dos, y es cuestión de tiempo que uno se olvide.

La pregunta es **dónde** vive ese lugar. La primera implementación lo puso en `RecursoDAO.Construir`, con un `switch` sobre `nombre_tipo_recurso`. Eso dejaba la fábrica **en la capa de datos**, y el segundo camino se quedaba sin acceso: el modal no podía usarla sin depender del DAO para armar el recurso, que es justo lo que la decisión ya tomada para Clientes evita —*"para armar el cliente, el modal no necesita el DAO"*.

El síntoma es concreto: **el `Recurso` que entrega el modal no tenía origen**. El formulario de Recursos no podía armar el objeto sin escribir su propio `switch`, es decir, duplicando el conocimiento en la capa de interfaz, que es el peor lugar posible para que viva.

### Decisión

**La fábrica vive en el dominio**, en `SistemaCoworking/Modelos/RecursoFactory.cs`, como clase estática con un único método `Crear(...)`:

- Recibe el nombre del tipo, los **datos comunes** (id, idTipoRecurso, nombre, ubicación, costo, estado) y los **datos específicos del tipo como opcionales** (`capacidadPersonas`, `tieneProyector`, `numeroMonitor`, `metrosCuadrados`).
- **Es el único lugar del sistema que sabe qué tipo se construye con qué clase.**
- **Devuelve `null` cuando falta el dato obligatorio del tipo.** El formulario construye el recurso mientras el usuario escribe, y un formulario incompleto no es un error: es el estado normal. La excepción queda reservada para el **tipo desconocido**, que es un error de programación (un seed corrupto), no del usuario. *(Contrato revisado el 06/11/2026: antes devolvía `null` en los dos casos; ver la nota de abajo.)*
- **No conoce la regla de precio**: esa vive en `CalcularPrecio()`, que es polimorfismo del modelo (D-003). La fábrica solo decide **qué clase** se construye.
- `RecursoDAO` dejó de instanciar clases concretas: ahora llama a `RecursoFactory.Crear(...)`.
- En toda la cadena se respeta la diferencia entre **"no aplica"** (null) y **"vale cero"**: los helpers de lectura del DAO devuelven `null` en vez de `0` cuando la columna viene en NULL, porque la fábrica necesita distinguir las dos cosas.

### Alternativas descartadas

- **Dejar la fábrica dentro del DAO.** Se descartó porque rompe la simetría de la arquitectura: el formulario queda obligado a depender de la capa de datos para construir un objeto de dominio, que es exactamente lo que el modal de Clientes evita. Y habilita el camino peor —un `switch` propio en el modal— que duplica el conocimiento.
- **Método estático de fábrica sobre la clase abstracta `Recurso`** (`Recurso.Crear(...)`). Se descartó porque mezcla la construcción con la clase que describe el comportamiento: `Recurso` quedaría con la responsabilidad de saber instanciar sus propias subclases. Es una alternativa válida, pero la clase aparte deja el propósito explícito y el archivo se lee solo.
- **Que cada clase concreta se construya a sí misma**, con un registro de factories por tipo. Se descartó por complejidad desproporcionada para tres tipos fijos: un diccionario agrega indirección y un punto de falla más (registrar el factory) a cambio de nada.
- **Que el modal arme la clase concreta con su propio `switch`.** Se descartó: duplicaría el conocimiento de "qué tipo es qué clase" en la capa de interfaz.

### Consecuencias

**A favor:**

- Los dos caminos de creación usan la **misma** fábrica: un cuarto tipo se agrega en un solo lugar.
- La capa de datos **no instancia** clases concretas. `RecursoDAO` dejó de hacer `new Sala(...)`, `new PuestoTrabajo(...)` y `new OficinaPrivada(...)`.
- El modal de Recursos puede armar el `Recurso` **sin tocar la capa de datos**.
- **Aclaración posterior**: los modales reciben el DAO por constructor, pero solo para **escribir** la entidad cuando el usuario guarda. Armar el objeto de dominio no pasa por el DAO: eso lo hace la fábrica, que vive en el dominio. Que el modal escriba no mueve la fábrica de lugar.
- La invariante "una `Sala` siempre tiene capacidad" se verifica **al construir el objeto**, no recién cuando se escribe en la base.
- La regla de precio sigue en un solo lugar: `CalcularPrecio()`.
- **El modal puede construir el recurso mientras el usuario escribe**, sin excepciones en el flujo normal: la fábrica devuelve `null` hasta que el formulario está completo, y el precio se recalcula en vivo sobre ese mismo objeto.

**En contra (y cómo se mitiga):**

- **La fábrica recibe once parámetros**, de los cuales cuatro son opcionales. Es el precio de que los datos específicos de cada tipo no compartan forma. Se acepta: los tipos son fijos y el conjunto de datos es estable.
- **El despacho sigue siendo sobre el string del tipo.** `RecursoFactory.Crear("Sala", ...)` depende de que el string coincida exactamente: un error de tipeo se detecta en runtime, con `InvalidOperationException`, no al compilar. Despachar por `id` no es mejor —los ids son mágicos y el nombre es el que se muestra y se documenta.
- **Queda un acoplamiento residual del DAO con las clases concretas.** `Insertar` y `Modificar` tienen que extraer del objeto los datos específicos del tipo para mandarlos al procedimiento almacenado, y lo hacen con un `switch` sobre el tipo concreto. Es un acoplamiento de otro nivel que el de la fábrica: la capa de datos mapea el objeto a los parámetros de un SP cuya firma la define la estructura de la tabla, que ya replica la jerarquía de tipos (D-002).
  - **Resuelto en D-006**: cada clase expone sus datos específicos en una forma común y el DAO dejó de conocer las clases concretas.
- **La validación del dato obligatorio del tipo vive en tres capas**: la UI (mensaje al usuario), la fábrica (al construir el objeto) y el procedimiento almacenado (al escribir, con `SIGNAL SQLSTATE '45000'`). No es duplicación: cada capa protege su frontera, y las tres son exigidas por el diseño en capas que el proyecto ya manifiesta.

### Nota — revisión del contrato (06/11/2026)

La fábrica nació **lanzando** `InvalidOperationException` cuando faltaba el dato obligatorio del tipo. Eso se vio al construir el modal de Recursos: el modal tiene que armar el recurso **en vivo**, con el formulario a medio completar, para poder mostrar el precio mientras el usuario escribe. Con el contrato viejo, cada tecla que dejaba el formulario incompleto lanzaba una excepción.

El contrato quedó así:

- **Falta el dato obligatorio del tipo** → la fábrica devuelve `null`. Es el estado normal de un formulario incompleto, no un error.
- **El tipo no existe** → sigue lanzando `InvalidOperationException`. Eso es un error de programación (o un seed corrupto) y tiene que ser ruidoso.

El mensaje al usuario no lo da la fábrica: lo dan los validadores del modelo (`Recurso.ValidarCosto`, `OficinaPrivada.ValidarMetrosCuadrados`), que son los que la interfaz muestra. La fábrica deja de ser la capa que informa y queda como la que construye.

Consecuencia de fondo: **el objeto que calcula el precio y el objeto que se guarda son el mismo**. El modal mantiene un borrador, que se rehace en cada cambio, y el precio sale de ese borrador. No hay dos objetos ni dos caminos que puedan desincronizarse.

### Referencias

- `SistemaCoworking/Modelos/RecursoFactory.cs` — la fábrica
- `SistemaCoworking/Datos/RecursoDAO.cs` — la capa de datos la usa y ya no crea clases concretas
- `SistemaCoworking/Formularios/UserControls/RecursoModalControl.cs` — el formulario la usa para armar el recurso
- `SistemaCoworking/Modelos/Recurso.cs`, `Sala.cs`, `PuestoTrabajo.cs`, `OficinaPrivada.cs` — la jerarquía y la regla de precio

---

## D-006 — Cada recurso declara sus propios datos de tipo

**Fecha:** 04/11/2026
**Estado:** Aceptada
**Ámbito:** Modelo de dominio + capa de datos

### Contexto

Con la fábrica en el dominio (D-005), `RecursoDAO` ya no creaba clases concretas. Pero le quedaba un **segundo acoplamiento**: `Insertar` y `Modificar` tenían que extraer del recurso los datos específicos del tipo para mandarlos como parámetros al procedimiento almacenado, y lo hacían con un `switch` sobre la clase concreta:

```csharp
switch (recurso)
{
    case Sala sala:             capacidadPersonas = sala.CapacidadPersonas; ...
    case PuestoTrabajo puesto:  numeroMonitor = puesto.NumeroMonitor; ...
    case OficinaPrivada of:     metrosCuadrados = Convert.ToInt32(of.MetrosCuadrados); ...
}
```

El efecto era concreto: **agregar un cuarto tipo obligaba a tocar la capa de datos**. El conocimiento de "qué datos tiene cada tipo" estaba duplicado —en la clase que los tiene y en el DAO que los busca— y el DAO es el peor lugar para esa copia: su trabajo es hablar SQL, no conocer la jerarquía de clases.

Ese `switch` además **escondía un problema de precisión**. `OficinaPrivada.MetrosCuadrados` es `decimal` en el modelo y la columna `metros_cuadrados` es `int`. El DAO lo resolvía con un `Convert.ToInt32` que **truncaba en silencio**: quien cargaba 25,7 m² terminaba con 25 guardados, sin ningún aviso.

### Decisión

**Cada recurso declara sus propios datos de tipo**, con una proyección:

- `DetalleTipo` es un contenedor con los cuatro datos específicos en forma opcional: `CapacidadPersonas`, `TieneProyector`, `NumeroMonitor`, `MetrosCuadrados`. Las columnas que no aplican al tipo van en `null`.
- `Recurso` expone `public virtual DetalleTipo Detalle => new DetalleTipo();`: por defecto no hay datos específicos.
- Cada clase derivada **overridea** `Detalle` y devuelve solo los suyos:

| Clase | `Detalle` devuelve |
|---|---|
| `Sala` | capacidad de personas y tiene proyector |
| `PuestoTrabajo` | cantidad de monitores |
| `OficinaPrivada` | metros cuadrados |

- El DAO llama a `AgregarParametrosDetalle(comando, recurso.Detalle)` y **no nombra ninguna clase concreta**. La conversión de `decimal` a `int` de los metros queda en el DAO, que es la capa que conoce la forma de la columna.
- **Los metros con decimales se rechazan en vez de truncar**: `OficinaPrivada.ValidarMetrosCuadrados` lo valida para la interfaz, y `RecursoFactory` devuelve `null` si el valor no es entero.

### Alternativas descartadas

- **Truncar los metros en silencio**, que es lo que hacía el `Convert.ToInt32`. Se descartó porque es pérdida de datos sin aviso: el usuario carga 25,7 m², el sistema guarda 25 y nadie se entera. La diferencia entre el modelo (`decimal`) y la columna (`int`) se resuelve rechazando el valor, no degradándolo.
- **Mover el `switch` al dominio**, por ejemplo a un `RecursoDetalle.De(recurso)`. Se descartó porque solo mueve el conocimiento: el DAO dejaría de conocer las clases, pero el dominio tendría un `switch` que contradice el polimorfismo que ya se usó para el precio (D-003). La opción virtual no tiene `switch`: cada clase declara lo suyo.
- **Que cada clase escriba su propio procedimiento almacenado** (Active Record). Se descartó porque invierte la dependencia: el dominio pasaría a depender de `MySql.Data`, y se rompería el flujo `Form → lógica → DAO → ConexionBD → MySQL`.
- **Mandar el detalle como JSON al procedimiento almacenado.** Se descartó porque pierde el tipado de los parámetros, y la cátedra pide procedimientos con parámetros declarados.
- **Cambiar la columna `metros_cuadrados` a `decimal`.** Se descartó como alternativa cerrada: la columna es parte del modelo que definieron las consignas y el DER. Si alguna vez hace falta precisión decimal, es un cambio de modelo y no un arreglo de código.

### Consecuencias

**A favor:**

- **La capa de datos no nombra ninguna clase concreta.** `RecursoDAO` no importa `Sala`, `PuestoTrabajo` ni `OficinaPrivada`.
- **Agregar un tipo nuevo toca dos archivos de código** —la clase nueva con su `Detalle` y la fábrica— y **ninguno de la capa de datos**.
- Cada clase es dueña de sus datos: "qué tiene una Sala" se responde en `Sala`.
- Los datos del tipo dejan de duplicarse: la proyección se calcula al vuelo y no se guarda.
- Los metros con decimales ya no se pierden en silencio.

**En contra (y cómo se mitiga):**

- **Suma un tipo al dominio** (`DetalleTipo`) que hay que explicar en la defensa. Es el precio de que la capa de datos no conozca la jerarquía.
- **El dominio replica la forma ancha de la tabla**: `DetalleTipo` tiene cuatro campos porque `recursos_detalle` tiene cuatro columnas. No es un problema nuevo —la forma ancha ya se aceptó en D-002— pero ahora está replicada en dos lugares.
- **`Detalle` es una proyección duplicada**: `Sala.CapacidadPersonas` sigue siendo la fuente y `Detalle` la copia. Si alguien modificara el `Detalle` esperando que cambie el recurso, no va a pasar. Por eso `Detalle` es de solo lectura y se calcula en cada acceso.
- **La diferencia de precisión entre el modelo y la columna no se resuelve**: el dominio maneja `decimal` y la columna `int`. Se rechaza el valor fraccionario, pero el modelo sigue siendo más preciso que la base.

### Referencias

- `SistemaCoworking/Modelos/DetalleTipo.cs` — la proyección
- `SistemaCoworking/Modelos/Recurso.cs` — `Detalle` virtual
- `SistemaCoworking/Modelos/Sala.cs`, `PuestoTrabajo.cs`, `OficinaPrivada.cs` — cada una declara los suyos
- `SistemaCoworking/Datos/RecursoDAO.cs` — `AgregarParametrosDetalle`
- `SistemaCoworking/Modelos/RecursoFactory.cs` — el rechazo de los metros fraccionarios

---

## D-007 — La regla de superposicion de reservas vive en la base, en una funcion

**Fecha:** 10/11/2026
**Estado:** Aceptada
**Ámbito:** Base de datos y capa de presentacion (modal de reservas)

### Contexto

La consigna pide "al menos una funcion definida por el usuario que tenga una utilidad real" y da como ejemplo "determinar disponibilidad". El sistema no tenia ninguna funcion de disponibilidad: las cinco que existian (`fn_total_clientes`, `fn_reservas_por_finalizar`, `fn_reservas_hoy`, `fn_recursos_disponibles`, `fn_recursos_reservados_hoy`) son todas contadores del dashboard, sin parametros.

La regla que impedia superponer reservas estaba escrita dos veces, como un `IF EXISTS (SELECT ... FROM reservas ...)` duplicado dentro de `sp_alta_reserva` y `sp_modificar_reserva`. Era la unica regla del sistema duplicada entre la base y la aplicacion.

Mientras tanto, el modal de reservas no daba ninguna senal antes de guardar: el usuario completaba el formulario, presionaba Guardar y recien ahi la base rechazaba la operacion con `SIGNAL 'El recurso no esta disponible en la fecha ingresada'`.

Ademas, el modelo tenia la misma regla escrita en dos lugares: `Reserva.ValidarRecurso` miraba el estado del recurso y `Reserva.ValidarFechas` miraba el orden de las fechas, que es exactamente lo que hace `Recurso.PuedeReservarse`.

### Decisión

Se agrego la funcion `fn_recurso_ocupado(p_id_recurso, p_fecha_inicio, p_fecha_fin, p_id_reserva_excluida)`, que devuelve 1 si el recurso ya tiene una reserva confirmada que se superpone con el periodo, y 0 si no.

- `sp_alta_reserva` y `sp_modificar_reserva` dejan de tener el `IF EXISTS` propio y llaman a la funcion. La regla queda escrita **una sola vez**, en la base.
- `ReservaDAO.RecursoOcupado(...)` llama a la misma funcion, y el modal la consulta antes de guardar: si el recurso esta ocupado, avisa y no llega a escribir.
- Se agrego el indice `ix_reservas_recurso_periodo` sobre `(id_recurso, estado_reserva, fecha_inicio, fecha_fin)`, porque la consulta de superposicion recorria la tabla entera.
- El parametro `p_id_reserva_excluida` existe para que, al editar, la reserva no se superponga consigo misma.
- `Reserva.ValidarRecurso` y `Reserva.ValidarFechas` se reemplazan por `Reserva.ValidarReserva`, que llama a `Recurso.PuedeReservarse` y traduce el resultado al mensaje del usuario. La regla de disponibilidad del recurso queda escrita una sola vez, en el recurso.

### Alternativas descartadas

- **Escribir el predicado de superposicion en C# y dejar los SP como estaban.** Se descarto porque duplicaba la regla en dos lugares: si uno cambiaba, la aplicacion y la base decidirian distinto. Es el mismo problema que la desnormalizacion de `precio_dia` (ver D-003), pero con la diferencia de que aca el duplicado estaria en el camino de la escritura.
- **Dejar la superposicion solo en los SP y no avisar antes.** Se descarto porque el usuario descubria el conflicto recien despues de completar el formulario. La preconsulta no cambia quien decide: la base sigue validando al guardar.
- **Una funcion por procedimiento** (`fn_recurso_ocupado_para_alta` y `..._para_modificar`). Se descarto porque el parametro `p_id_reserva_excluida` ya resuelve la diferencia con un solo parametro.
- **Reescribir la regla en la aplicacion y sacar la funcion de la base.** Se descarto porque la consigna pide una funcion con utilidad real, y porque la base es la que tiene que decidir igual.

### Consecuencias

**A favor:**

- La regla de superposicion existe en un solo lugar.
- El modal avisa antes de escribir, y el error le llega al usuario en la etiqueta correspondiente y no como un MessageBox.
- La consigna pide una funcion con utilidad real: esta la tiene y ademas la usa la propia base.
- El indice evita el recorrido completo de la tabla en la consulta de disponibilidad.

**En contra (y cómo se mitigan):**

- **Un round trip mas por guardado.** El modal consulta la disponibilidad y despues escribe. Es aceptable: el volumen es bajo y la base sigue decidiendo.
- **La consulta y el guardado no son atomicos.** Entre la consulta y el `INSERT` otra sesion podria ocupar el recurso. Por eso el SP sigue validando: la preconsulta es una comodidad, no la garantia.

### Referencias

- `Scripts SQL/Funciones.sql` — `fn_recurso_ocupado`
- `Scripts SQL/Procedimientos_Almacenados.sql` — `sp_alta_reserva`, `sp_modificar_reserva`
- `Scripts SQL/Coworking_db.sql` — indice `ix_reservas_recurso_periodo`
- `SistemaCoworking/Datos/ReservaDAO.cs` — `RecursoOcupado`
- `SistemaCoworking/Formularios/UserControls/ReservaModal.cs` — la preconsulta
- `SistemaCoworking/Modelos/Reserva.cs` — `ValidarReserva`
