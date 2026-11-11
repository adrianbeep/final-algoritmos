# [Etapa Final] Paquete de documentacion tecnica

## Objetivo

Armar el paquete de documentacion tecnica del sistema, que hoy esta incompleto. La consigna pide como
entregable una "breve documentacion tecnica del sistema", "instrucciones necesarias para ejecutar el
proyecto", el "diagrama del modelo de datos" y el "diagrama de clases simplificado", y ademas un manual
para el usuario final.

La documentacion se escribe contra el **codigo real** (`SistemaCoworking/` y `Scripts SQL/`), no contra
el ROADMAP ni contra la consigna. Cuando el codigo y el ROADMAP no coinciden, manda el codigo, y la
diferencia se anota en `Docs/Decisiones.md`.

### Estado real al 11/11/2026

Este issue se escribio cuando los formularios eran placeholders. Hoy:

- `Docs/Decisiones.md` esta **completo**: 7 decisiones de diseño en formato ADR (D-001 a D-007).
- `Docs/Diagrama-clases.md` esta **completo**: diagrama del dominio con la jerarquia `Recurso`, la
  interfaz `IReservable`, las reglas de negocio y la trazabilidad con el codigo.
- `Docs/DC.md` y `Docs/DFD.md` existen: el diagrama de contexto de nivel 0 y el DFD de nivel 1.
- `Docs/DER.md` existe, pero **no coincide** con `Scripts SQL/Coworking_db.sql`.
- La base tiene **6 funciones** almacenadas, no 5: se sumo `fn_recurso_ocupado`.
- Los cuatro modulos de la interfaz estan implementados (`FrmClientes`, `FrmRecursos`, `FrmReservas`,
  `FrmDashboard`), asi que el manual ya no tiene los bloqueantes que tenia.

## Alcance de esta issue

### Queda en este trabajo

- [ ] `Docs/Diccionario-datos.md` — nuevo. Diccionario de datos completo
- [ ] `Docs/DER.md` — actualizar contra el script actual
- [ ] `Docs/DC.md` — agregar el diagrama de contexto de la operacion principal
- [ ] `Docs/DFD.md` — agregar los diagramas de flujo de las operaciones de escritura
- [ ] `Docs/README.md` — pasar a indice navegable de toda la documentacion
- [ ] `README.md` raiz — actualizar la seccion "Documentacion"

### Queda fuera de este trabajo

- [ ] `Docs/Documentacion-tecnica.md` — lo escribe otro integrante del equipo
- [ ] `Docs/Manual-usuario.md` — lo escribe otro integrante del equipo, con capturas reales de la
      aplicacion corriendo

## Entregables

### 1. Diccionario de datos

Una ficha por tabla, con las columnas de la base documentadas campo por campo.

- [ ] Cubre las 6 tablas: `clientes`, `tipo_recursos`, `recursos`, `recursos_detalle`, `reservas` y
      `auditoria_reservas`
- [ ] Cada ficha tiene una tabla de estructura con las columnas: nombre del campo, tipo de dato,
      longitud o precision, nullable / not null, valor por default, PK, FK (y a que tabla apunta),
      UNIQUE y CHECK
- [ ] Cada ficha tiene una segunda tabla con las columnas: nombre del campo, descripcion funcional y
      quien escribe el dato
- [ ] La columna "quien escribe el dato" distingue entre: seed inicial, `sp_alta_*`, `sp_modificar_*`,
      `sp_cancelar_reserva`, trigger de auditoria y lectura por DAO
- [ ] Los CHECK se transcriben textualmente desde el script
- [ ] Se documentan las 2 vistas (`vw_reservas_completas` y `vw_recursos_estado`) con sus columnas
      derivadas y que DAO las consume
- [ ] Se documentan las 6 funciones almacenadas con su retorno y donde se usa cada una. Ojo:
      `fn_recurso_ocupado` no la usa ninguna tarjeta del dashboard: la consulta el modal de reservas
      antes de guardar, y es la misma que usan los procedimientos almacenados
- [ ] Se documentan los 7 procedimientos almacenados con su firma de parametros y su proposito
- [ ] Se documentan los 2 triggers con su evento y que registran
- [ ] Se aclara que `auditoria_reservas.id_reserva` **no tiene FK declarada**: la relacion con `reservas`
      es logica y la mantiene el trigger, no el motor de la base

> Nota de formato: las dos tablas por entidad (estructura y significado) en vez de una sola de once
> columnas. En A4 con margenes de 2,54 cm y Arial 11 no entran once columnas de forma legible, y las
> pautas de presentacion exigen que las tablas sean legibles. El contenido es el mismo.

### 2. Diagrama Entidad-Relacion (DER)

`Docs/DER.md` ya existe y tiene la base, pero **no coincide con el script actual**. Hay que actualizarlo.

- [ ] Las 6 tablas con todas sus columnas, tipo exacto, nullability, default, PK, FK, UNIQUE y CHECK
- [ ] El diagrama refleja el estado real de `Scripts SQL/Coworking_db.sql`, no una version idealizada
- [ ] Correccion obligatoria: la tabla `recursos` tiene la columna `costo decimal(10,2) not null
      check(costo > 0)`, que hoy **no figura** en el diagrama
- [ ] Se aclara que `auditoria_reservas.id_reserva` **no tiene FK declarada** en el script: la relacion con
      `reservas` es logica y la mantiene el trigger, no el motor
- [ ] La relacion `recursos` <-> `recursos_detalle` queda marcada como 1:1 estricta
- [ ] Se documenta la relacion con `tipo_recursos` y el hecho de que no existe CRUD sobre esa tabla
      porque sus valores son fijos
- [ ] Los ENUM aparecen con sus valores literales: `recursos.estado`, `reservas.estado_reserva`,
      `auditoria_reservas.accion`
- [ ] Se conservan las decisiones de diseno ya escritas y se agregan las que surjan de esta etapa

### 3. Diagrama de contexto de la operacion principal

`Docs/DC.md` ya tiene el diagrama de contexto del sistema completo (nivel 0). Se le agrega el de la
operacion principal.

- [ ] Diagrama centrado en la operacion "Gestionar Reserva", con el mismo actor y el detalle de los flujos
- [ ] Flujo de solicitud: el administrador elige cliente, recurso, `fecha_inicio` y `fecha_fin`
- [ ] Flujo de validacion contra el servidor: el sistema consulta disponibilidad y recibe el resultado
- [ ] Flujo de rechazo: mensaje de negocio devuelto por `sp_alta_reserva` y mostrado al usuario
- [ ] Flujo de confirmacion: la reserva queda registrada y el trigger de auditoria genera la entrada
- [ ] Formato Mermaid, consistente con el diagrama que ya esta en el archivo

### 4. Diagramas de flujo de las operaciones de escritura

`Docs/DFD.md` ya tiene el DFD de nivel 1. Se le agregan los flujos de las operaciones de escritura.

Flujo principal, el **alta de reserva**, que es donde se concentran las validaciones de negocio:

- [ ] Inicio: el administrador abre el modulo de Reservas y presiona "Nuevo"
- [ ] Seleccion de cliente, recurso, `fecha_inicio` y `fecha_fin`
- [ ] Validacion de cliente existente (FK `reservas.id_cliente`)
- [ ] Validacion de recurso existente y en estado `Disponible` (no `EnMantenimiento` ni `Inhabilitado`)
- [ ] Validacion de periodo: `DATE(fecha_inicio) <= DATE(fecha_fin)`, porque el sistema reserva por dias
      y la misma fecha de inicio y de fin es una reserva de un dia completo
- [ ] Validacion de no superposicion: la consulta `fn_recurso_ocupado` y la vuelve a validar
      `sp_alta_reserva`
- [ ] Calculo de `total_reserva` a partir del `CalcularPrecio()` del tipo de recurso
- [ ] Camino de error: la excepcion del SP llega como `MySqlException` con `SQLSTATE 45000` y se muestra
      el mensaje de negocio al usuario
- [ ] Camino de exito: confirmacion, refresco del `DataGridView` y registro en `auditoria_reservas`

Flujos secundarios, al menos estos tres:

- [ ] **Alta y modificacion de cliente**: validacion de DNI unico y email unico antes del `INSERT`
- [ ] **Alta y modificacion de recurso**: escritura en dos tablas (`recursos` + `recursos_detalle`) con
      transaction, y la aparicion de los campos especificos segun el tipo elegido en el ComboBox
- [ ] **Cancelacion y modificacion de reserva**: revalidacion de disponibilidad y actualizacion de estado

- [ ] Los flujos se dibujan con rombos de decision y rectangulos de proceso, no con textos en un cuadrado
- [ ] Cada decision del diagrama es rastreable a una regla concreta: CHECK, FK, ENUM o validacion del SP
- [ ] El diagrama se acompana de una tabla que mapea cada decision a su origen en el codigo

> Nota de accuracy: el ROADMAP y la consigna hablan de `START TRANSACTION` en `sp_alta_reserva`, pero los
> procedimientos de reserva **no** llevan transaction, porque escriben una sola tabla. El flujo se dibuja
> como esta en el codigo.

### 5. Indice de la documentacion

- [ ] `Docs/README.md` pasa a ser el indice navegable de toda la documentacion, con enlaces a cada
      documento y una linea de descripcion de cada uno
- [ ] El `README.md` raiz actualiza la seccion "Documentacion" para enlazar los documentos nuevos

## Criterio de pase

- [ ] El diccionario cubre las 6 tablas, las 2 vistas, las 6 funciones, los 7 procedimientos y los 2
      triggers
- [ ] El DER coincide **campo por campo** con `Scripts SQL/Coworking_db.sql`: se coteja tabla por tabla
- [ ] Ningun CHECK del diagrama o del diccionario difiere del script
- [ ] Cada decision de los diagramas de flujo es rastreable a una regla del SQL
- [ ] Todos los diagramas son Mermaid y se renderizan correctamente en GitHub
- [ ] `Docs/README.md` funciona como indice: se entra por ahi y se llega a todo
- [ ] Ninguna documentacion contradice al codigo; si hay contradiccion, se corrige la documentacion

## Archivos a crear/modificar

- `Docs/Diccionario-datos.md` — nuevo. Diccionario de datos completo
- `Docs/DER.md` — actualizar contra el script actual
- `Docs/DC.md` — agregar el diagrama de contexto de la operacion principal
- `Docs/DFD.md` — agregar los diagramas de flujo de las operaciones de escritura
- `Docs/README.md` — pasar a indice navegable
- `README.md` — actualizar la seccion "Documentacion"

## Nota

- Los diagramas van en Mermaid porque `Docs/DER.md` ya lo usa y GitHub lo renderiza nativo. No depende de
  que el lector tenga una herramienta externa instalada.
- Si para la defensa se necesita un PNG o PDF de cada diagrama, se exporta aparte como entregable de
  presentacion, pero la fuente editable queda en Mermaid dentro de `Docs/`.
- Los archivos del repositorio se nombran sin tildes; el texto lleva tildes, igual que el resto de la
  documentacion.
