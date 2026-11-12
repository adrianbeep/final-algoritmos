# Diccionario de datos

Este documento describe la base de datos `coworking`: sus seis tablas, sus dos
vistas, sus seis funciones almacenadas, sus siete procedimientos almacenados y
sus dos triggers. La fuente de verdad es el esquema realmente desplegado en
MariaDB 10.4.32 (`localhost:3306`), contrastado contra los scripts de
[`Scripts SQL/`](../Scripts%20SQL/).

Cada tabla se documenta en dos niveles: primero su **estructura** (tipo,
longitud, nulabilidad, valor por defecto, claves y restricciones), y después su
**significado** (qué representa el campo en el negocio y qué componente lo
escribe).

## Convenciones

| Convención | Significado |
|---|---|
| `Precision` | Longitud o precisión del tipo, cuando el tipo la define. Vacío cuando no aplica |
| `Nullable` | `Si` o `No`, según admita o no valores nulos |
| `Default` | Valor por defecto literal declarado en el script. Vacío cuando la columna no lo tiene |
| `PK`, `FK`, `UNIQUE` | `Si` o el nombre de la tabla referenciada. Vacío cuando la restricción no aplica |
| `CHECK` | Condición transcrita textualmente del script. Vacío cuando la columna no tiene restricción |
| `Si` / `No` en `PK` y `UNIQUE` | La columna forma parte de la clave primaria o tiene restricción de unicidad |

Las claves primarias `id` de `clientes`, `tipo_recursos`, `recursos`, `reservas`
y `auditoria_reservas`, y la clave primaria `id_recurso` de `recursos_detalle`,
son columnas `AUTO_INCREMENT`: la base les asigna un valor correlativo al
insertar la fila, por eso no declaran valor por defecto.

## Tabla `clientes`

Guarda los clientes del coworking. La escriben el script de carga inicial
`Coworking_db.sql` y los procedimientos `sp_alta_cliente` y
`sp_modificar_cliente`; la aplicación nunca inserta ni actualiza filas de esta
tabla de forma directa.

### Estructura

| Campo | Tipo | Precision | Nullable | Default | PK | FK | UNIQUE | CHECK |
|---|---|---|---|---|---|---|---|---|
| `id` | int | | No | | Si | | | |
| `nombre` | varchar(100) | 100 | No | | | | | |
| `apellido` | varchar(100) | 100 | No | | | | | |
| `dni` | int | | No | | | | Si | |
| `telefono` | varchar(30) | 30 | No | | | | | |
| `email` | varchar(50) | 50 | No | | | | Si | |

### Significado

| Campo | Descripcion funcional | Quien escribe el dato |
|---|---|---|
| `id` | Identificador interno del cliente. Lo asigna la base de forma correlativa y no se muestra al usuario | Seed inicial, sp_alta_cliente |
| `nombre` | Nombre de pila del cliente | Seed inicial, sp_alta_cliente, sp_modificar_cliente |
| `apellido` | Apellido del cliente | Seed inicial, sp_alta_cliente, sp_modificar_cliente |
| `dni` | Documento nacional de identidad. Es la clave de negocio para distinguir a un cliente de otro | Seed inicial, sp_alta_cliente, sp_modificar_cliente |
| `telefono` | Teléfono de contacto del cliente | Seed inicial, sp_alta_cliente, sp_modificar_cliente |
| `email` | Correo electrónico de contacto del cliente. Es la clave de negocio alternativa al `dni` | Seed inicial, sp_alta_cliente, sp_modificar_cliente |

## Tabla `tipo_recursos`

Guarda el catálogo de tipos de recurso (`Sala`, `PuestoTrabajo`,
`OficinaPrivada`). Es un catálogo fijo: la única escritura es la carga inicial
de `Coworking_db.sql`. Desde la aplicación es de solo lectura
(`TipoRecursoDAO.ObtenerTodos()`), y su `nombre_tipo_recurso` es un contrato de
código que comparan los procedimientos almacenados y `RecursoFactory`.

### Estructura

| Campo | Tipo | Precision | Nullable | Default | PK | FK | UNIQUE | CHECK |
|---|---|---|---|---|---|---|---|---|
| `id` | int | | No | | Si | | | |
| `nombre_tipo_recurso` | varchar(100) | 100 | No | | | | Si | |
| `descripcion` | text | | Si | | | | | |

### Significado

| Campo | Descripcion funcional | Quien escribe el dato |
|---|---|---|
| `id` | Identificador interno del tipo de recurso | Seed inicial |
| `nombre_tipo_recurso` | Nombre del tipo de recurso. Su valor exacta decide qué clase concreta se crea y qué datos son obligatorios | Seed inicial |
| `descripcion` | Descripción funcional del tipo de recurso, con las características que lo distinguen | Seed inicial |

## Tabla `recursos`

Guarda los recursos reservables del coworking (salas, puestos de trabajo y
oficinas privadas) con su estado, su ubicación y su precio. La escriben la
carga inicial y los procedimientos `sp_alta_recurso` y `sp_modificar_recurso`.

### Estructura

| Campo | Tipo | Precision | Nullable | Default | PK | FK | UNIQUE | CHECK |
|---|---|---|---|---|---|---|---|---|
| `id` | int | | No | | Si | | | |
| `id_tipo_recurso` | int | | No | | | tipo_recursos | | |
| `nombre` | varchar(100) | 100 | No | | | | | |
| `estado` | enum('Disponible', 'EnMantenimiento', 'Inhabilitado') | | Si | 'Disponible' | | | | |
| `ubicacion` | varchar(100) | 100 | No | | | | | |
| `costo` | decimal(10,2) | 10,2 | No | | | | | costo > 0 |
| `precio_dia` | decimal(10,2) | 10,2 | No | | | | | precio_dia > 0 |

### Significado

| Campo | Descripcion funcional | Quien escribe el dato |
|---|---|---|
| `id` | Identificador interno del recurso | Seed inicial, sp_alta_recurso |
| `id_tipo_recurso` | Tipo al que pertenece el recurso. `sp_modificar_recurso` no lo modifica: el tipo de un recurso es inmutable | Seed inicial, sp_alta_recurso |
| `nombre` | Nombre del recurso | Seed inicial, sp_alta_recurso, sp_modificar_recurso |
| `estado` | Situación operativa del recurso: `Disponible` (reservable), `EnMantenimiento` (fuera de servicio temporal) o `Inhabilitado` (retirado de la oferta) | Seed inicial, sp_alta_recurso, sp_modificar_recurso |
| `ubicacion` | Piso o zona del coworking donde se encuentra el recurso | Seed inicial, sp_alta_recurso, sp_modificar_recurso |
| `costo` | Costo base del recurso. Es el valor del que parte el cálculo del precio diario, que aplica el recargo propio de cada tipo | Seed inicial, sp_alta_recurso, sp_modificar_recurso |
| `precio_dia` | Precio por día del recurso, ya calculado con el recargo que corresponde a su tipo. Es el valor que multiplica la cantidad de días para obtener el total de una reserva | Seed inicial, sp_alta_recurso, sp_modificar_recurso |

## Tabla `recursos_detalle`

Guarda los atributos específicos del tipo de cada recurso: los que corresponden a
una `Sala`, a un `PuestoTrabajo` o a una `OficinaPrivada`. La relación es 1:1
con `recursos`: cada recurso tiene exactamente una fila de detalle y las
columnas que no aplican a su tipo quedan en `NULL`. La escriben la carga
inicial y los procedimientos `sp_alta_recurso` y `sp_modificar_recurso`, que la
actualizan dentro de una transacción junto con `recursos`.

### Estructura

| Campo | Tipo | Precision | Nullable | Default | PK | FK | UNIQUE | CHECK |
|---|---|---|---|---|---|---|---|---|
| `id_recurso` | int | | No | | Si | recursos | | |
| `capacidad_personas` | int | | Si | | | | | |
| `tiene_proyector` | bool | | Si | | | | | |
| `numero_monitor` | int | | Si | | | | | |
| `metros_cuadrados` | int | | Si | | | | | |

### Significado

| Campo | Descripcion funcional | Quien escribe el dato |
|---|---|---|
| `id_recurso` | Recurso al que corresponde la fila de detalle. Es a la vez clave primaria y clave foránea | Seed inicial, sp_alta_recurso |
| `capacidad_personas` | Cantidad de personas que admite el recurso. Solo se usa en recursos de tipo `Sala` | Seed inicial, sp_alta_recurso, sp_modificar_recurso |
| `tiene_proyector` | Indica si el recurso cuenta con proyector. Solo se usa en recursos de tipo `Sala` | Seed inicial, sp_alta_recurso, sp_modificar_recurso |
| `numero_monitor` | Cantidad de monitores disponibles. Solo se usa en recursos de tipo `PuestoTrabajo` | Seed inicial, sp_alta_recurso, sp_modificar_recurso |
| `metros_cuadrados` | Superficie del recurso en metros cuadrados. Solo se usa en recursos de tipo `OficinaPrivada` | Seed inicial, sp_alta_recurso, sp_modificar_recurso |

## Tabla `reservas`

Guarda las reservas de recursos: quién reservó, qué reservó, en qué período y a
qué precio. La escriben los procedimientos `sp_alta_reserva`,
`sp_modificar_reserva` y `sp_cancelar_reserva`; la carga inicial no inserta
reservas.

### Estructura

| Campo | Tipo | Precision | Nullable | Default | PK | FK | UNIQUE | CHECK |
|---|---|---|---|---|---|---|---|---|
| `id` | int | | No | | Si | | | |
| `id_cliente` | int | | No | | | clientes | | |
| `id_recurso` | int | | No | | | recursos | | |
| `fecha_inicio` | datetime | | No | | | | | DATE(fecha_inicio) <= DATE(fecha_fin) |
| `fecha_fin` | datetime | | No | | | | | DATE(fecha_fin) >= DATE(fecha_inicio) |
| `estado_reserva` | enum('Confirmada', 'Cancelada') | | Si | 'Confirmada' | | | | |
| `total_reserva` | decimal(10,2) | 10,2 | No | | | | | |

La tabla declara además el índice `ix_reservas_recurso_periodo` sobre
`(id_recurso, estado_reserva, fecha_inicio, fecha_fin)`, que es exactamente el
predicado que usa `fn_recurso_ocupado`.

### Significado

| Campo | Descripcion funcional | Quien escribe el dato |
|---|---|---|
| `id` | Identificador interno de la reserva | sp_alta_reserva |
| `id_cliente` | Cliente que realiza la reserva. `sp_modificar_reserva` no lo modifica: una reserva no cambia de cliente | sp_alta_reserva |
| `id_recurso` | Recurso reservado. Es el recurso que se libera al cancelar la reserva | sp_alta_reserva, sp_modificar_reserva |
| `fecha_inicio` | Primer día del período reservado. El sistema reserva por días, con conteo inclusivo | sp_alta_reserva, sp_modificar_reserva |
| `fecha_fin` | Último día del período reservado | sp_alta_reserva, sp_modificar_reserva |
| `estado_reserva` | Estado de la reserva: `Confirmada` (ocupa el recurso y lo bloquea) o `Cancelada` (libera el recurso) | sp_alta_reserva, sp_cancelar_reserva |
| `total_reserva` | Importe total de la reserva: precio diario del recurso por la cantidad de días reservados | sp_alta_reserva, sp_modificar_reserva |

## Tabla `auditoria_reservas`

Guarda el historial de altas y cancelaciones de reservas. La tabla se escribe
exclusivamente mediante el trigger de auditoría: la aplicación no inserta ni
actualiza filas de esta tabla.

### Estructura

| Campo | Tipo | Precision | Nullable | Default | PK | FK | UNIQUE | CHECK |
|---|---|---|---|---|---|---|---|---|
| `id` | int | | No | | Si | | | |
| `id_reserva` | int | | No | | | | | |
| `accion` | enum('INSERT', 'CANCEL') | | Si | | | | | |
| `fecha_accion` | datetime | | No | current_timestamp() | | | | |
| `detalle` | text | | Si | | | | | |

### Significado

| Campo | Descripcion funcional | Quien escribe el dato |
|---|---|---|
| `id` | Identificador correlativo de la entrada de auditoría | Trigger de auditoria |
| `id_reserva` | Reserva sobre la que se registra el evento. Es la relación lógica con `reservas` | Trigger de auditoria |
| `accion` | Tipo de evento auditado: `INSERT` por el alta de una reserva, `CANCEL` por su cancelación | Trigger de auditoria |
| `fecha_accion` | Momento en que se registró el evento. Lo completa el valor por defecto `current_timestamp()` | Trigger de auditoria |
| `detalle` | Descripción textual del evento, armada por el trigger con los datos de la reserva | Trigger de auditoria |

> **Nota sobre `id_reserva`.** Esta columna **no declara clave foránea** hacia
> `reservas`: es una relación lógica, y la mantiene el trigger, no el motor de
> la base. El motor no impide escribir en `auditoria_reservas` un `id_reserva`
> que no exista en `reservas`.

## Vistas

Las vistas no almacenan datos: derivan sus columnas de las tablas y solo se
leen. Ninguna de sus columnas se escribe.

### `vw_reservas_completas`

Reservas con los datos del cliente, del recurso y del tipo de recurso, en una
sola fila. La consume `ReservaDAO.ObtenerTodos()` para la grilla del
formulario de reservas (`FrmReservas`).

| Columna | Tipo | Como se obtiene | Quien escribe el dato | DAO que la consume | Pantalla |
|---|---|---|---|---|---|
| `id_reserva` | int | `reservas.id`, renombrado | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `fecha_inicio` | datetime | `reservas.fecha_inicio` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `fecha_fin` | datetime | `reservas.fecha_fin` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `estado_reserva` | enum('Confirmada', 'Cancelada') | `reservas.estado_reserva` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `total_reserva` | decimal(10,2) | `reservas.total_reserva` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `cliente_nombre` | varchar(201) | Concatenación de `clientes.nombre` y `clientes.apellido` separados por un espacio | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `dni` | int | `clientes.dni` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `recurso_nombre` | varchar(100) | `recursos.nombre` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `recurso_ubicacion` | varchar(100) | `recursos.ubicacion` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |
| `nombre_tipo_recurso` | varchar(100) | `tipo_recursos.nombre_tipo_recurso` | No se escribe: es de lectura | `ReservaDAO.ObtenerTodos()` | Formulario de reservas (`FrmReservas`), grilla de reservas |

### `vw_recursos_estado`

Recursos con su tipo y la cantidad de reservas activas que tienen. La consume
`RecursoDAO.ObtenerTodos()` para la grilla del formulario de recursos
(`FrmRecursos`). La consulta de la DAO solicita seis de las siete columnas: la
columna `reservas_activas` no se pide.

| Columna | Tipo | Como se obtiene | Quien escribe el dato | DAO que la consume | Pantalla |
|---|---|---|---|---|---|
| `id_recurso` | int | `recursos.id`, renombrado | No se escribe: es de lectura | `RecursoDAO.ObtenerTodos()` | Formulario de recursos (`FrmRecursos`), grilla de recursos |
| `recurso_nombre` | varchar(100) | `recursos.nombre` | No se escribe: es de lectura | `RecursoDAO.ObtenerTodos()` | Formulario de recursos (`FrmRecursos`), grilla de recursos |
| `recurso_ubicacion` | varchar(100) | `recursos.ubicacion` | No se escribe: es de lectura | `RecursoDAO.ObtenerTodos()` | Formulario de recursos (`FrmRecursos`), grilla de recursos |
| `estado` | enum('Disponible', 'EnMantenimiento', 'Inhabilitado') | `recursos.estado` | No se escribe: es de lectura | `RecursoDAO.ObtenerTodos()` | Formulario de recursos (`FrmRecursos`), grilla de recursos |
| `precio_dia` | decimal(10,2) | `recursos.precio_dia` | No se escribe: es de lectura | `RecursoDAO.ObtenerTodos()` | Formulario de recursos (`FrmRecursos`), grilla de recursos |
| `nombre_tipo_recurso` | varchar(100) | `tipo_recursos.nombre_tipo_recurso` | No se escribe: es de lectura | `RecursoDAO.ObtenerTodos()` | Formulario de recursos (`FrmRecursos`), grilla de recursos |
| `reservas_activas` | bigint | Cantidad de reservas del recurso que no están canceladas y cuya fecha de fin todavía no pasó (`fecha_fin >= NOW()`) | No se escribe: es de lectura | `RecursoDAO.ObtenerTodos()` (no solicita esta columna) | Formulario de recursos (`FrmRecursos`), grilla de recursos |

## Funciones almacenadas

Las seis funciones devuelven un valor escalar y no modifican datos.

| Funcion | Que devuelve | Donde se usa |
|---|---|---|
| `fn_total_clientes()` | `INT` con la cantidad total de clientes | `DashboardDAO.ObtenerTotalClientes()`, para el indicador de total de clientes del módulo de indicadores |
| `fn_reservas_por_finalizar()` | `INT` con la cantidad de reservas `Confirmada` que ya comenzaron y que todavía no finalizaron | `DashboardDAO.ObtenerReservasPorFinalizar()`, para el indicador de reservas por finalizar del módulo de indicadores |
| `fn_reservas_hoy()` | `INT` con la cantidad de reservas `Confirmada` que comienzan en la fecha actual | `DashboardDAO.ObtenerReservasHoy()`, para el indicador de reservas de hoy del módulo de indicadores |
| `fn_recursos_disponibles()` | `INT` con la cantidad de recursos en estado `Disponible` | `DashboardDAO.ObtenerRecursosDisponibles()`, para el indicador de recursos disponibles del módulo de indicadores |
| `fn_recursos_reservados_hoy()` | `INT` con la cantidad de recursos distintos que tienen una reserva `Confirmada` que comienza en la fecha actual | `DashboardDAO.ObtenerRecursosReservadosHoy()`, para el indicador de recursos reservados hoy del módulo de indicadores |
| `fn_recurso_ocupado(p_id_recurso int, p_fecha_inicio datetime, p_fecha_fin datetime, p_id_reserva_excluida int)` | `TINYINT`: `1` si el recurso ya tiene una reserva `Confirmada` que se superpone con el período indicado, `0` si está libre | `ReservaDAO.RecursoOcupado()`, que el modal de reservas consulta antes de guardar. Es la misma función que invocan internamente `sp_alta_reserva` y `sp_modificar_reserva` |

La regla de superposición de reservas está escrita una sola vez, en
`fn_recurso_ocupado`: la usan los dos procedimientos de reserva y también la
aplicación, de modo que la base y el cliente no pueden decidir distinto. El
parámetro `p_id_reserva_excluida` permite consultar la disponibilidad al editar
una reserva sin que la reserva se superponga consigo misma; en un alta se
envía `0`.

Las cinco primeras funciones se invocan desde `DashboardDAO` y alimentan las cuatro
tarjetas del tablero (`FrmDashboard`): la cuarta tarjeta, "Ocupación de hoy", combina
las dos últimas en un porcentaje. El tablero muestra un botón "Actualizar" que
vuelve a consultar los valores en la base.

## Procedimientos almacenados

Los siete procedimientos son la única vía de escritura de la aplicación sobre
los datos de negocio. Todos validan con `SIGNAL SQLSTATE '45000'`, y el mensaje
que se muestra al usuario es el `MESSAGE_TEXT` del `SIGNAL`.

| Procedimiento | Firma de parametros | Que valida | Que escribe |
|---|---|---|---|
| `sp_alta_cliente` | `IN p_nombre varchar(100)`, `IN p_apellido varchar(100)`, `IN p_dni int`, `IN p_telefono varchar(30)`, `IN p_email varchar(50)` | Que no exista otro cliente con el mismo `dni` (`Ya existe un cliente con ese dni`) y que no exista otro cliente con el mismo `email` (`Ya existe un cliente con ese email`) | Inserta una fila en `clientes` con `nombre`, `apellido`, `dni`, `telefono` y `email` |
| `sp_modificar_cliente` | `IN p_id int`, `IN p_nombre varchar(100)`, `IN p_apellido varchar(100)`, `IN p_dni int`, `IN p_telefono varchar(30)`, `IN p_email varchar(50)` | Que exista el cliente a modificar (`No existe el cliente a modificar`), que el `dni` no pertenezca a otro cliente (`Ya existe otro cliente con ese dni`) y que el `email` no pertenezca a otro cliente (`Ya existe otro cliente con ese email`) | Actualiza `clientes` (`nombre`, `apellido`, `dni`, `telefono`, `email`) del cliente cuyo `id` es `p_id` |
| `sp_alta_recurso` | `IN p_id_tipo_recurso int`, `IN p_nombre varchar(100)`, `IN p_estado enum('Disponible', 'EnMantenimiento', 'Inhabilitado')`, `IN p_ubicacion varchar(100)`, `IN p_costo decimal(10,2)`, `IN p_precio_dia decimal(10,2)`, `IN p_capacidad_personas int`, `IN p_tiene_proyector bool`, `IN p_numero_monitor int`, `IN p_metros_cuadrados int` | Que exista el tipo de recurso (`Tipo de recurso invalido`) y que se complete el dato obligatorio del tipo: capacidad de personas para `Sala` (`Sala requiere capacidad de personas`), cantidad de monitores para `PuestoTrabajo` (`PuestoTrabajo requiere numero de monitores`) y metros cuadrados para `OficinaPrivada` (`OficinaPrivada requiere metros cuadrados`) | Inserta una fila en `recursos` y una fila en `recursos_detalle` con el `id` del recurso recién insertado. Ambas inserciones van en una transacción: si alguna falla, se revierte y se re-lanza el error |
| `sp_modificar_recurso` | `IN p_id_recurso int`, `IN p_nombre varchar(100)`, `IN p_estado enum('Disponible', 'EnMantenimiento', 'Inhabilitado')`, `IN p_ubicacion varchar(100)`, `IN p_costo decimal(10,2)`, `IN p_precio_dia decimal(10,2)`, `IN p_capacidad_personas int`, `IN p_tiene_proyector bool`, `IN p_numero_monitor int`, `IN p_metros_cuadrados int` | Las mismas cuatro validaciones que `sp_alta_recurso`: `Tipo de recurso invalido`, `Sala requiere capacidad de personas`, `PuestoTrabajo requiere numero de monitores` y `OficinaPrivada requiere metros cuadrados` | Actualiza `recursos` (`nombre`, `estado`, `ubicacion`, `costo`, `precio_dia`) y `recursos_detalle` (`capacidad_personas`, `tiene_proyector`, `numero_monitor`, `metros_cuadrados`) del recurso indicado, en una transacción. No modifica `id_tipo_recurso` |
| `sp_alta_reserva` | `IN p_id_cliente int`, `IN p_id_recurso int`, `IN p_fecha_inicio datetime`, `IN p_fecha_fin datetime`, `IN p_total_reserva decimal(10,2)` | Que exista el cliente (`El cliente no existe`), que exista el recurso (`El recurso no existe`), que el recurso esté `Disponible` (`El recurso no esta disponible`), que el período sea coherente (`La fecha de inicio no puede ser posterior a la fecha de fin`) y que el recurso no tenga una reserva confirmada en ese período (`El recurso no esta disponible en la fecha ingresada`) | Inserta una fila en `reservas` con `estado_reserva` en `Confirmada`. El alta dispara el trigger de auditoría de inserción |
| `sp_modificar_reserva` | `IN p_id_reserva int`, `IN p_id_recurso int`, `IN p_fecha_inicio datetime`, `IN p_fecha_fin datetime`, `IN p_total_reserva decimal(10,2)` | Que exista la reserva y esté `Confirmada` (`La reserva no existe o no se puede modificar`), que exista el recurso (`El recurso no existe`), que el recurso esté `Disponible` (`El recurso no esta disponible`), que el período sea coherente (`La fecha de inicio no puede ser posterior a la fecha de fin`) y que el recurso no tenga otra reserva confirmada en ese período (`El recurso no esta disponible en la fecha ingresada`) | Actualiza `reservas` (`id_recurso`, `fecha_inicio`, `fecha_fin`, `total_reserva`). No modifica `id_cliente` ni `estado_reserva` |
| `sp_cancelar_reserva` | `IN p_id_reserva int` | Que exista la reserva y esté `Confirmada` (`La reserva no existe o ya esta cancelada`) | Actualiza `reservas.estado_reserva` a `Cancelada`. La cancelación dispara el trigger de auditoría de actualización |

## Triggers

Los dos triggers de auditoría escriben en `auditoria_reservas`. MySQL limita un
trigger a una sola acción, por eso el alta y la cancelación se auditan con
triggers separados.

| Trigger | Evento | Que registra | Con que valores |
|---|---|---|---|
| `trg_auditoria_reservas_insert` | `AFTER INSERT` sobre `reservas`, por cada fila | Inserta una fila en `auditoria_reservas` | `id_reserva` toma el `id` de la reserva insertada, `accion` toma el valor `INSERT` y `detalle` se arma con `CONCAT('Reserva creada. Cliente: ', id_cliente, ', Recurso: ', id_recurso, ', Inicio: ', fecha_inicio, ', Fin: ', fecha_fin, ', Total: ', total_reserva)`. `fecha_accion` queda con su valor por defecto `current_timestamp()` |
| `trg_auditoria_reservas_update` | `AFTER UPDATE` sobre `reservas`, por cada fila. Solo actúa cuando el estado pasa de `Confirmada` a `Cancelada` | Inserta una fila en `auditoria_reservas` | `id_reserva` toma el `id` de la reserva, `accion` toma el valor `CANCEL` y `detalle` se arma con `CONCAT('Reserva cancelada. Estado anterior: ', estado anterior, ', Estado nuevo: ', estado nuevo, ', Cliente: ', id_cliente, ', Recurso: ', id_recurso)`. `fecha_accion` queda con su valor por defecto `current_timestamp()` |
