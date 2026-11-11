# [Etapa 3] Contrato uniforme de escritura en los DAO

## Objetivo

Que los tres DAO de escritura tengan el mismo contrato: `Insertar(entidad)` y `Modificar(entidad)`.

El spec de la etapa 3 ya lo pedia asi:

```
ClienteDAO: ObtenerTodos(), ObtenerPorId(id), Insertar(cliente), Modificar(cliente)
RecursoDAO: ObtenerTodos(), ObtenerPorId(id), Insertar(recurso), Modificar(recurso)
ReservaDAO: ObtenerTodos(), ObtenerPorId(id), Insertar(reserva), Modificar(reserva), Cancelar(id)
```

`RecursoDAO` lo cumple. `ClienteDAO.Insertar` recibe cinco parametros sueltos y
`ReservaDAO.Insertar` recibe otros cinco, y esa diferencia se filtra a los
modales, que hoy construyen la entidad y despues la desarman para pasarla de a
una pieza.

## Entregables

- [ ] `ClienteDAO.Insertar(Cliente cliente)` en vez de cinco parametros sueltos
- [ ] `ReservaDAO.Insertar(Reserva reserva)` en vez de cinco parametros sueltos
- [ ] `ClienteDAO.Modificar(Cliente c)` pasa a `Modificar(Cliente cliente)`, para que
      los tres DAO usen el mismo nombre de parametro
- [ ] `ClienteModal` y `ReservaModal` llaman a los DAO con la entidad
- [ ] El contrato queda escrito en el propio DAO: que asigna la base y que tiene que
      venir calculado
- [ ] Queda registrado como decision de diseno

## Criterio de pase

- [ ] Los tres DAO de escritura reciben la entidad
- [ ] Ningun DAO de escritura recibe parametros sueltos para dar de alta
- [ ] Los tres modales arman la entidad y se la pasan al DAO, sin desarmarla
- [ ] El alta de cliente y de reserva sigue funcionando contra la base real
- [ ] El proyecto compila con 0 errores

## Fuera de alcance

- [ ] Una interfaz generica `ICrudDAO<T>`: los tres DAO tienen contratos distintos
      (el cliente no recibe id, el recurso tiene datos de tipo y la reserva trae el
      total ya calculado) y la consigna no pide estructuras genericas propias
- [ ] `ReservaDAO.Cancelar(int id)`: no es un alta, cancela por identificador
- [ ] `TipoRecursoDAO`: es un catalogo de solo lectura, no escribe

## Nota

El detalle que hay que dejar escrito en el codigo, porque no es obvio:

- **El `Id` lo asigna la base.** En el alta, la entidad llega con `Id = 0` y el
  procedimientoStoredProcedure no lo recibe. El DAO lo ignora.
- **El `TotalReserva` lo calcula el modelo.** `Reserva.CalcularTotal(recurso)` necesita
  el recurso, que vive en el modal. El DAO no lo recalcula: recibe la reserva con el
  total ya calculado y lo escribe tal cual.
