## Contexto

Issue #34 - [Etapa 2] Reservas: cancelacion, disponibilidad y contrato de errores.

Cierre del modulo de reservas en tres frentes que quedaban abiertos: el boton de cancelar no se habilitaba solo para reservas `Confirmada`, la regla de superposicion estaba escrita dos veces en la base y el modal no avisaba nada antes de guardar, y el lado de presentacion del refactor de errores del DAO habia quedado a medias.

## Qué incluye

**Cancelacion solo para reservas confirmadas**
- El boton `colCancelar` de la grilla aparece atenuado cuando la reserva no esta en estado `Confirmada`, y la accion no llega a la base en ese caso.
- La regla se resuelve con el enum `Reserva.EstadoReserva`, no con el string que muestra la grilla.
- `DataGridViewConfiguration` pinta el boton con el color de la celda, asi el form puede atenuarlo sin meterse en como se pinta.

**Disponibilidad: una sola definicion de la superposicion**
- Nueva funcion `fn_recurso_ocupado(p_id_recurso, p_fecha_inicio, p_fecha_fin, p_id_reserva_excluida)`: queda como la unica definicion de la regla de superposicion.
- `sp_alta_reserva` y `sp_modificar_reserva` la usan y ya no tienen el `IF EXISTS` duplicado.
- `ReservaDAO.RecursoOcupado` la consulta y el modal avisa antes de guardar: si el recurso esta ocupado, muestra el mensaje y no escribe.
- `p_id_reserva_excluida` evita que, al editar, la reserva se superponga consigo misma.
- Indice `ix_reservas_recurso_periodo` sobre `(id_recurso, estado_reserva, fecha_inicio, fecha_fin)`: la consulta era un recorrido completo de la tabla.
- Las fechas se comparan por dia y no por hora, en las columnas y en los procedimientos, porque el sistema reserva por dias.
- La funcion declara sus parametros sin la palabra `IN`: MariaDB la acepta en los procedimientos pero no en las funciones.

**El modelo decide la regla, no el formulario**
- `Reserva.ValidarReserva` reemplaza a `ValidarRecurso` y `ValidarFechas`, que reimplementaban la misma condicion.
- La regla la decide `Recurso.PuedeReservarse`, el metodo de la interfaz `IReservable`: el validador solo traduce el resultado al mensaje del usuario. Con eso, `IReservable.PuedeReservarse()` tiene un punto de llamada real.

**Contrato de errores entre el DAO y la presentacion**
- Los formularios y los modales capturan la excepcion del DAO con `try/catch` y la muestran con `ErroresDeBaseDeDatos.Mostrar`.
- Se corrigen los comentarios que describian un contrato inexistente: "el DAO ya mostro el error y devuelve false", "la escritura la hace el form" y "el modal entrega la entidad al form".
- El comentario de cabecera de los DAO deja de traer escapes literales pegados y dice lo que el contrato es: los metodos de escritura devuelven `void` y la excepcion es la que avisa que la operacion fallo.
- Este PR tambien recoge el commit pendiente de esa refactorizacion, que separa `ErroresDeBaseDeDatos` de `Datos/` y saca el `MessageBox` de la capa de datos: estaba commiteado pero nunca se abrio un PR.

**Documentacion**
- `Docs/Diagrama-clases.md`: las reglas de negocio y donde viven, el punto 6.1 de las advertencias para la defensa y el mensaje del `SIGNAL` de `sp_cancelar_reserva`.
- `Docs/DFD.md`: el proceso 3.0 y el flujo de control de superposicion.
- `Docs/Decisiones.md`: nueva decision D-007, y el pendiente de D-003 que queda resuelto.
- `README.md`: el orden de los cinco scripts de base y la configuracion externa de la conexion.
- Cuerpos de los issues 2.05 y 2.07, que describian el comportamiento anterior.

## Verificación

- `dotnet build SistemaCoworking\SistemaCoworking.csproj -t:Rebuild`: compilacion correcta, 0 errores y 7 advertencias preexistentes (4 en `FrmPrincipal.cs` y 3 en `TextControl.cs`).
- Cada commit de la rama compila por separado.
- El color de los botones se verifico midiendo los pixeles de la grilla dibujada: naranja (255,159,28) en las reservas `Confirmadas`, gris (224,224,224) en las canceladas, y el boton de editar azul (0,120,215) en todas las filas, pares e impares.
- Base real: los cinco scripts se ejecutaron en orden sobre MariaDB 10.4 y quedaron los 6 objetos creados (6 funciones y 7 procedimientos). La funcion se probo con datos reales: devuelve 1 cuando el recurso tiene una reserva confirmada que se superpone, 0 cuando no, 0 cuando la reserva editada es la misma que se excluye, y 0 si la reserva esta cancelada. `sp_alta_reserva` rechaza el alta superpuesta con `ERROR 1644 (45000)`, el error de negocio que la aplicacion sabe mostrar.

Closes #34
