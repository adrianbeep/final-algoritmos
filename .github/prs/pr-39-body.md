## Contexto

Issue #38 - Fix: la funcion `fn_recurso_ocupado` no se podia crear en MariaDB.

El PR #35 se mergeo cuando la rama tenia un commit anterior al fix, asi que `main` todavia tiene los parametros de la funcion con la palabra `IN`. Con esos parametros la funcion no se crea y la aplicacion falla al crear una reserva.

## Qué incluye

- Los parametros de `fn_recurso_ocupado` quedan sin la palabra `IN`.
- Se deja escrito en el script el motivo, para que nadie vuelva a ponerla.
- La verificacion contra la base real queda registrada en el body del issue 2.08 y del PR 35.

## Causa

MariaDB acepta la palabra `IN` en los parametros de un procedimiento almacenado, pero no en los de una funcion: el `CREATE` falla con `ERROR 1064`. Las otras cinco funciones del proyecto no tienen parametros, asi que nunca se habia dado el caso. Los procedimientos si se creaban, y por eso el error aparecia recien en tiempo de ejecucion, cuando la aplicacion llamaba a la funcion que nunca existio.

La sintaxis sin el modo es la que funciona en MySQL y en MariaDB, asi que se usa la misma en los dos motores.

## Verificación

- `dotnet build SistemaCoworking\SistemaCoworking.csproj -t:Rebuild`: compilacion correcta, 0 errores.
- Los cinco scripts se ejecutan en orden sobre MariaDB 10.4 y quedan los 6 objetos creados (6 funciones y 7 procedimientos).
- La funcion se probo con datos reales: devuelve 1 cuando el recurso tiene una reserva confirmada que se superpone, 0 cuando no, 0 cuando la reserva editada es la misma que se excluye, y 0 si la reserva esta cancelada.
- `sp_alta_reserva` rechaza el alta superpuesta con `ERROR 1644 (45000)`, el error de negocio que la aplicacion sabe mostrar.

Closes #38
