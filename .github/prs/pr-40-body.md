## Contexto

Issue #29 - [Etapa Final] Paquete de documentacion tecnica.

Cierra los entregables de documentacion que faltaban. El issue se escribio cuando los formularios eran
placeholders; hoy el sistema esta implementado y la documentacion va atras.

## Que incluye

**Diccionario de datos (`Docs/Diccionario-datos.md`, nuevo)**
- Las 6 tablas campo por campo: tipo, precision, nulabilidad, default, PK, FK, UNIQUE y CHECK
  transcrito textualmente del script, mas una segunda tabla con la descripcion funcional y quien escribe
  el dato (seed, `sp_alta_*`, `sp_modificar_*`, `sp_cancelar_reserva`, trigger o lectura por DAO).
- Las 2 vistas, las 6 funciones, los 7 procedimientos almacenados y los 2 triggers.
- Se aclara que `auditoria_reservas.id_reserva` no declara FK: la relacion con `reservas` es logica y la
  mantiene el trigger, no el motor.
- La fuente de verdad es el esquema realmente desplegado, cotejado contra `Scripts SQL/`.

**DER (`Docs/DER.md`)**
- Se agrega la columna `costo decimal(10,2) not null check(costo > 0)` de `recursos`, que no figuraba.
- Se documenta que la relacion con `auditoria_reservas` es logica y no una FK declarada.

**Diagrama de contexto (`Docs/DC.md`)**
- Se agrega el diagrama de la operacion principal "gestionar una reserva", con los flujos de solicitud,
  validacion contra el servidor, rechazo por `SIGNAL SQLSTATE '45000'` y confirmacion con el trigger de
  auditoria.

**Flujos de las operaciones de escritura (`Docs/DFD.md`)**
- Alta de reserva (el flujo principal), con rombos de decision, y los flujos secundarios de alta y
  modificacion de cliente, alta y modificacion de recurso, y cancelacion y modificacion de reserva.
- Una tabla final que mapea las 40 decisiones a su origen: CHECK, UNIQUE, FK, ENUM, validacion dentro
  de un procedimiento almacenado o validacion en C#.

**Indice (`Docs/README.md` y `README.md` raiz)**
- `Docs/README.md` pasa a ser el indice navegable de toda la documentacion.
- El `README.md` raiz enlaza los documentos nuevos.

## Fuera de alcance

Este PR no incluye la documentacion tecnica minima ni el manual de usuario: los escribe otro integrante
del equipo.

## Verificacion

- El diccionario se cotejo campo por campo contra `information_schema.COLUMNS` de la base real: 33
  columnas documentadas, 33 en la base, 0 discrepancias de tipo, nulabilidad, default, claves y
  restricciones.
- Los 15 objetos programados de la base (6 funciones, 7 procedimientos, 2 triggers) son los 15 citados
  en el diccionario.
- Los bloques Mermaid se parsearon y renderizaron para comprobar que no tienen errores y que no salen
  mas anchos que la pantalla.
- Cada decision de los diagramas de flujo es rastreable a una regla concreta del script SQL.
- `dotnet build SistemaCoworking\SistemaCoworking.csproj -t:Rebuild`: compilacion correcta, 0 errores.

Closes #29
