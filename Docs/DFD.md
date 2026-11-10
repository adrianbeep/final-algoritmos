# DFD de nivel 1

El DFD de nivel 1 **abre el sistema del [diagrama de contexto](./DC.md)** y
muestra los procesos internos que lo componen, los almacenes de datos donde se
guarda la informacion y los flujos entre ellos.

La entidad externa `Base de datos MySQL` del nivel 0 es la que se abre aqui: se
convierte en los almacenes `D1` a `D5`.

## El diagrama

```mermaid
flowchart TB
    P["Personal del coworking"]

    P1(("1.0<br/>Administrar clientes"))
    P2(("2.0<br/>Administrar recursos"))
    P3(("3.0<br/>Administrar reservas"))
    P4(("4.0<br/>Generar indicadores y reportes"))

    D1[("D1 Clientes")]
    D2[("D2 Recursos")]
    D3[("D3 Reservas")]
    D4[("D4 Tipos de recurso")]
    D5[("D5 Auditoria de reservas")]

    P -->|"datos del cliente"| P1
    P1 -->|"alta o modificacion"| D1
    D1 -->|"listado de clientes"| P1
    P1 -->|"resultado de la operacion"| P

    P -->|"datos del recurso: nombre, tipo, ubicacion, costo, estado y caracteristicas"| P2
    P2 -->|"alta o modificacion"| D2
    D2 -->|"listado de recursos"| P2
    P2 -->|"consulta del catalogo de tipos"| D4
    P2 -->|"resultado de la operacion"| P

    P -->|"solicitud de reserva: cliente, recurso y fechas"| P3
    P3 -->|"existencia y estado del cliente"| D1
    P3 -->|"existencia, estado y precio del recurso"| D2
    P3 -->|"reservas del recurso, para el control de superposicion"| D3
    P3 -->|"reserva confirmada, modificada o cancelada"| D3
    P3 -->|"confirmacion o error de negocio"| P
    D3 -.->|"evento de auditoria (trigger)"| D5

    P -->|"criterio del reporte"| P4
    P4 -->|"datos de clientes"| D1
    P4 -->|"datos de recursos"| D2
    P4 -->|"datos de reservas"| D3
    P4 -->|"indicadores y listados"| P
```

## Los procesos

| Proceso | Que hace | Donde esta en el codigo |
|---|---|---|
| `1.0 Administrar clientes` | Alta y modificacion de clientes, y listado para verlos | `Formularios/FrmClientes.cs` |
| `2.0 Administrar recursos` | Alta y modificacion de recursos, con las caracteristicas que exige cada tipo y su estado. Listado para verlos | `Formularios/FrmRecursos.cs` |
| `3.0 Administrar reservas` | Agenda, modifica y cancela reservas. Valida cliente, recurso, fechas y superposicion | `Formularios/FrmReservas.cs` |
| `4.0 Generar indicadores y reportes` | Dashboard con los cinco indicadores del coworking | `Formularios/FrmDashboard.cs` |

## Los almacenes de datos

| Almacen | Que guarda | Tabla |
|---|---|---|
| `D1 Clientes` | Los clientes del coworking | `clientes` |
| `D2 Recursos` | Los recursos y el detalle que corresponde a su tipo | `recursos`, `recursos_detalle` |
| `D3 Reservas` | Las reservas, con su estado y su total | `reservas` |
| `D4 Tipos de recurso` | El catalogo de tipos de recurso | `tipo_recursos` |
| `D5 Auditoria de reservas` | El historial de altas y cancelaciones | `auditoria_reservas` |

## Los flujos que vale la pena explicar

**El proceso `3.0` es el que se apoya en los demas.** No guarda un derivado de
la reserva: la pide a `D1` y a `D2` para saber si el cliente y el recurso
existen y estan disponibles, y a `D3` para saber si el recurso ya esta
reservado en esas fechas. Por eso el control de superposicion necesita leer
`D3` antes de escribir en `D3`.

**La auditoria no la escribe la aplicacion.** La flecha punteada de `D3` a `D5`
representa el trigger de la base: cada alta y cada cancelacion se auditan sola,
sin que ningun proceso del sistema tenga que pedirlo. Por eso `D5` no tiene
ninguna flecha de entrada desde el personal.

**Los errores de negocio no son un flujo de datos, son una respuesta.** El
proceso `3.0` devuelve al personal una confirmacion o el mensaje de error de
la regla que fallo, y esa respuesta sale hacia la misma entidad externa que
entra. Es lo que en el nivel 0 se resume en el flujo `Sistema -> Personal`.

## Dos cosas que el diagrama muestra y el codigo todavia no

El diagrama describe lo que el sistema **hace**, y en dos lugares el codigo
todavia no llega. No son decisiones de diseno: son pendientes de cerrar.

**La cancelacion de reservas no se puede ejecutar desde la aplicacion.** La
cadena existe y funciona: `ReservaDAO.Cancelar(id)` llama a `sp_cancelar_reserva`,
que pasa la reserva a `Cancelada` y dispara el trigger de auditoria. Pero la
grilla de `FrmReservas` tiene una columna de boton `colCancelar` que, al
hacerse clic, cae en el mismo manejador que la de editar y abre el modal de
edicion en vez de cancelar. El flujo `reserva confirmada, modificada o
cancelada` esta dibujado porque el sistema lo soporta; falta exponerlo.

**No hay busqueda ni baja logica.** Ninguna pantalla busca y ninguna da de
baja: el listado se muestra completo y la modificacion es una edicion. Por eso
los flujos de `D1` y `D2` hacia sus procesos dicen solo `listado`.

## Balance con el nivel 0

| Flujo del nivel 0 | Donde aparece en el nivel 1 |
|---|---|
| Personal -> Sistema: clientes, recursos y reservas | `P -> 1.0`, `P -> 2.0`, `P -> 3.0` |
| Sistema -> Personal: resultados, listados y errores | `1.0 -> P`, `2.0 -> P`, `3.0 -> P`, `4.0 -> P` |
| Sistema -> Base de datos: consultas y datos | `Procesos -> D1..D5` |
| Base de datos -> Sistema: datos almacenados | `D1..D5 -> Procesos` |

La entidad externa `Base de datos MySQL` desaparece del nivel 1 porque sus
almacenes quedan explicitados: es la diferencia entre dibujar una caja negra y
abrirla.

## Convenciones

| Forma | Significado |
|---|---|
| Circulo numerado | Proceso. El numero (`1.0`, `2.0`, ...) es la convencion de Gane-Sarson para identificarlo y para el balance entre niveles |
| Rectangulo | Entidad externa |
| Cilindro | Almacen de datos |
| Flecha continua | Flujo de datos |
| Flecha punteada | Flujo generado por la base de datos, no por la aplicacion |
