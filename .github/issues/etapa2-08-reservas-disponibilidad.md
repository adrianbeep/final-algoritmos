# [Etapa 2] Reservas: cancelacion, disponibilidad y contrato de errores

## Objetivo

Cerrar el modulo de reservas en tres frentes que quedaron abiertos.

Primero, la cancelacion: el boton de la fila ya tenia su propio camino y ya llamaba a `sp_cancelar_reserva`, pero el spec del issue 2.05 pide que solo este habilitado si la reserva esta en estado `Confirmada`, y no lo estaba. Segundo, la disponibilidad: la regla de superposicion estaba escrita dos veces, como un `IF EXISTS` duplicado en `sp_alta_reserva` y `sp_modificar_reserva`, y el modal no avisaba nada antes de guardar. Tercero, el contrato de errores: el DAO lanza y la presentacion captura, pero el lado de la presentacion quedo a medias y los comentarios del codigo describian un contrato que ya no existia.

La consigna pide "al menos una funcion definida por el usuario que tenga una utilidad real" y da como ejemplo "determinar disponibilidad": todavia no habia ninguna funcion de ese tipo.

## Entregables

### El boton de cancelar solo para reservas confirmadas

- [ ] El boton `colCancelar` aparece atenuado cuando la reserva no esta en estado `Confirmada`
- [ ] La accion no llega a la base en ese caso: el modal avisa y sigue abierto
- [ ] La regla se resuelve con el enum `Reserva.EstadoReserva`, no con el string que muestra la grilla
- [ ] `DataGridViewConfiguration` pinta el boton con el color de la celda, para que el form pueda atenuarlo sin meterse en como se pinta

### Disponibilidad: una sola definicion de la superposicion

- [ ] La funcion `fn_recurso_ocupado(p_id_recurso, p_fecha_inicio, p_fecha_fin, p_id_reserva_excluida)` es la unica definicion de la regla
- [ ] `sp_alta_reserva` y `sp_modificar_reserva` la usan y ya no tienen el `IF EXISTS` propio
- [ ] `ReservaDAO.RecursoOcupado` la consulta y el modal avisa antes de guardar
- [ ] `p_id_reserva_excluida` evita que, al editar, la reserva se superponga consigo misma
- [ ] Indice `ix_reservas_recurso_periodo` sobre `(id_recurso, estado_reserva, fecha_inicio, fecha_fin)`: la consulta era un recorrido completo de la tabla
- [ ] Las fechas se comparan por dia y no por hora, en las columnas y en los procedimientos, porque el sistema reserva por dias

### El modelo decide la regla, no el formulario

- [ ] `Reserva.ValidarReserva` reemplaza a `ValidarRecurso` y `ValidarFechas`, que reimplementaban la misma condicion
- [ ] La regla la decide `Recurso.PuedeReservarse`, que es el metodo de la interfaz `IReservable`: el validador solo traduce el resultado al mensaje del usuario
- [ ] Con eso, `IReservable.PuedeReservarse()` tiene un punto de llamada real

### Contrato de errores entre el DAO y la presentacion

- [ ] Los formularios y los modales capturan la excepcion del DAO con `try/catch` y la muestran con `ErroresDeBaseDeDatos.Mostrar`
- [ ] Los comentarios de cabecera de los DAO dicen lo que el contrato es: los metodos de escritura devuelven `void` y la excepcion es la que avisa que la operacion fallo
- [ ] Los comentarios que decian "el DAO ya mostro el error y devuelve false" o "la escritura la hace el form" quedan corregidos

## Criterio de pase

- [ ] El proyecto compila con 0 errores
- [ ] Cancelar una reserva `Confirmada` la pasa a `Cancelada` y refresca la grilla
- [ ] El boton de cancelar esta atenuado en una reserva ya cancelada, y la accion no llega a la base
- [ ] Guardar una reserva sobre un recurso ya reservado en ese periodo muestra el mensaje y no escribe
- [ ] Editar una reserva no choca con el propio periodo de la reserva
- [ ] La regla de superposicion esta escrita una sola vez, en la base

## Fuera de alcance

- [ ] Busqueda y baja logica en las pantallas: siguen pendientes
- [ ] Una pantalla o modulo propio de consulta de disponibilidad: la consulta se hace desde el modal de reservas
- [ ] `IReservable` como tipo de parametro: el equipo no puede explicarlo en la defensa y la consigna no lo pide

## Archivos a crear o modificar

- `SistemaCoworking/Formularios/FrmReservas.cs`
- `SistemaCoworking/Formularios/FrmReservas.Designer.cs`
- `SistemaCoworking/Formularios/UserControls/ReservaModal.cs`
- `SistemaCoworking/Formularios/UserControls/ClienteModal.cs`
- `SistemaCoworking/Formularios/UserControls/RecursoModal.cs`
- `SistemaCoworking/Formularios/FrmClientes.cs`
- `SistemaCoworking/Formularios/FrmRecursos.cs`
- `SistemaCoworking/Herramientas/DataGridViewConfiguration.cs`
- `SistemaCoworking/Herramientas/ErroresDeBaseDeDatos.cs`
- `SistemaCoworking/Datos/ReservaDAO.cs`
- `SistemaCoworking/Datos/RecursoDAO.cs`
- `SistemaCoworking/Modelos/Reserva.cs`
- `Scripts SQL/Funciones.sql`
- `Scripts SQL/Procedimientos_Almacenados.sql`
- `Scripts SQL/Coworking_db.sql`
- `Docs/Diagrama-clases.md`, `Docs/DFD.md`, `Docs/Decisiones.md`

## Nota

Este issue cierra el modulo de reservas y, con el, la etapa 2. La consigna evalua "herencia, clase abstracta, interfaz y polimorfismo con sentido": la herencia la cubre la clase abstracta `Recurso` con sus tres derivadas, y la interfaz `IReservable` declara las dos capacidades de un recurso reservable, las dos en uso.

La regla de superposicion era la unica regla del sistema duplicada entre la base y la aplicacion. Ahora vive en la funcion `fn_recurso_ocupado`, que usan tanto los procedimientos almacenados como la aplicacion: la base y la interfaz no pueden decidir distinto.

Verificado contra una base real: los cinco scripts se corrieron en orden sobre MariaDB 10.4 y los objetos quedaron creados (6 funciones y 7 procedimientos). La funcion se probo con datos reales y devuelve 1 cuando el recurso tiene una reserva confirmada que se superpone, 0 cuando no se superpone, 0 cuando la reserva que se esta editando es la misma que se excluye, y 0 si la reserva esta cancelada. El procedimiento `sp_alta_reserva` rechaza el alta superpuesta con `ERROR 1644 (45000)`, que es el error de negocio que la aplicacion sabe mostrar.
