# Fix: la funcion fn_recurso_ocupado no se podia crear en MariaDB

## Objetivo

El PR #35 se mergeo en un commit anterior al fix, asi que `main` todavia tiene los parametros de la funcion con la palabra `IN`. Con esos parametros la funcion NO se crea en MariaDB, y la aplicacion falla al crear una reserva con `FUNCTION coworking.fn_recurso_ocupado does not exist`.

## Entregables

- [ ] Los parametros de `fn_recurso_ocupado` quedan sin la palabra `IN`
- [ ] Se deja escrito en el script el motivo, para que nadie vuelva a ponerla
- [ ] La verificacion contra la base real queda registrada en el issue 2.08 y en el PR 35

## Criterio de pase

- [ ] Los cinco scripts se ejecutan en orden sobre MariaDB sin errores
- [ ] Quedan los 6 objetos creados: 6 funciones y 7 procedimientos
- [ ] La funcion devuelve 1 con una reserva confirmada que se superpone, 0 sin superposicion, 0 excluyendo la reserva que se esta editando y 0 con una reserva cancelada
- [ ] `sp_alta_reserva` rechaza el alta superpuesta con `ERROR 1644 (45000)`, que es el error de negocio que la aplicacion sabe mostrar

## Causa

MariaDB acepta la palabra `IN` en los parametros de un procedimiento almacenado, pero no en los de una funcion: el `CREATE` falla con `ERROR 1064` ("You have an error in your SQL syntax ... near 'in p_id_recurso int,'").

Las otras cinco funciones del proyecto no tienen parametros, asi que nunca se habia dado el caso. Los procedimientos si se creaban, porque su sintaxis es valida en MariaDB, y por eso el error aparecia recien en tiempo de ejecucion, cuando la aplicacion llamaba a la funcion que nunca existio.

La sintaxis sin el modo es la que funciona en MySQL y en MariaDB, asi que se usa la misma en los dos motores.

## Archivos a crear o modificar

- `Scripts SQL/Funciones.sql`
- `.github/issues/etapa2-08-reservas-disponibilidad.md`
- `.github/prs/pr-35-body.md`

## Nota

Este fix es la continuacion del issue 2.08, que se cerro con el PR #35. El PR se mergeo cuando la rama tenia un commit anterior, asi que quedo fuera.
