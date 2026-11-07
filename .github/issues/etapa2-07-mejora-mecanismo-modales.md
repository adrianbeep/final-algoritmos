# [Etapa 2] Mejora del mecanismo de modales

## Objetivo

Los modales de Clientes y de Recursos funcionan, pero el mecanismo que los abre (`ModalOverlay`) no es un modal real: abre la ventana con `Show()` en vez de `ShowDialog()`, no maneja ESC, y el estado del velo vive en variables `static`. Eso produce tres consecuencias concretas: el modal no bloquea la aplicacion de fondo, el velo puede quedar pegado en pantalla cuando falla una escritura, y el `Region` redondeado se libera antes de tiempo.

Este issue endurece el mecanismo de modales **antes** de escribir `FrmReservas` y su modal, que es el modal mas complejo del sistema (elige cliente, elige recurso, toma un rango de fechas y calcula el total). Si el mecanismo no se corrige ahora, el modal de Reservas hereda cada uno de estos defectos y encima hay que duplicarlos por tercera vez.

El alcance no es solo "arreglar el velo": es dejar el mecanismo en una forma que el modal de Reservas pueda copiar sin arrastrar los problemas.

## Entregables

### Modales con modalidad real

- [ ] La ventana que hospeda el modal se abre con `ShowDialog()` en vez de `Show()`
- [ ] Con el modal abierto, la ventana principal queda **nativamente deshabilitada** (`IsWindowEnabled(hwnd)` = `False`), no solo tapada por el velo
- [ ] Un click real sobre el area del velo no llega al boton de la ventana principal, **incluso con el velo oculto**
- [ ] `ESC` cierra el modal como cancelacion, sin guardar nada. `ESC` no es gratis: sin un `CancelButton` asignado, `ESC` no hace nada (el `DialogResult` queda en `None` y la ventana sigue visible)
- [ ] El boton Cancelar y la `X` de cierre hacen exactamente lo mismo
- [ ] La `X` de cierre tiene `TabStop = true` y se puede alcanzar con el teclado
- [ ] El modal expone su resultado con `DialogResult` en vez de depender de eventos propios
- [ ] La ventana del modal tiene **una sola relacion de owner**: o `Owner` seteado y `ShowDialog()` sin parametro, o `ShowDialog(owner)` con `Owner` en `null`. `ShowDialog(owner)` sobre una ventana que ya tiene `Owner` tira `ArgumentException: Referencia circular del control`
- [ ] `ShowDialog()` bloquea al que llama: el codigo que abre el modal sigue recien cuando el modal cerro

### El modal devuelve la entidad, el form persiste

- [ ] El modal levanta `Aceptado` y `Closed` en ese orden: primero el resultado, despues el cierre
- [ ] El form escribe en la base **despues** de que el modal cerro, no dentro del handler de `Aceptado`
- [ ] Si la escritura falla, el velo ya esta bajado: la aplicacion no queda bloqueada
- [ ] La excepcion se propaga al hilo de la UI (el `try/catch` del DAO es de la Etapa 4), pero el modal ya se cerro

### El velo deja de ser estado global

- [ ] `ModalOverlay` no usa variables `static`: el velo es un objeto por ventana principal
- [ ] Los handlers de `Resize` y `Move` del owner se desuscriben cuando el velo se destruye
- [ ] `FrmPrincipal` destruye el velo al cerrarse
- [ ] Los forms embebidos no reciben el velo por constructor: lo piden por una interfaz o no lo necesitan
- [ ] El fallback `new Form { Opacity = 0 }` desaparece: no quedan ventanas top-level sin dueno ni `Dispose`
- [ ] El velo queda nativamente deshabilitado por `ShowDialog()` (es parte de la cadena de owners). Eso no rompe el bloqueo: la ventana principal tambien queda deshabilitada, asi que el click se cae igual. **Hay que verificar a ojo que el dim se sigue viendo igual con el modal abierto**, porque si el velo deshabilitado pinta distinto, hay que reasignarle el `Opacity` al abrir el modal

### Region con ciclo de vida propio

- [ ] El `Form` que hospeda el modal tiene su propia `Region`, no comparte el objeto con el `UserControl`: `ReferenceEquals(ventana.Region, modal.Region)` da `False`
- [ ] La `Region` que tiene la ventana no queda disposteada: leerla no tira `ArgumentException: Parameter is not valid`
- [ ] El recorte redondeado se recalcula cuando el modal cambia de tamaño

### Limpieza de errores en controles anidados

- [ ] La limpieza de errores al guardar recorre los controles anidados, no solo los hijos directos
- [ ] En el modal de Recursos, `txtCapacidad`, `txtNumMonitores` y `txtMetros` limpian su borde de error (hoy viven dentro de `pnSala`, `pnPuestoTrabajo` y `pnOficinaPrivada` y nunca se limpian)
- [ ] En el modal de Clientes se mantiene el comportamiento actual (los 5 campos son hijos directos)

### Lo repetido de los modales sale a una clase estatica

- [ ] `Herramientas/ModalConfiguration.cs`: clase estatica, al estilo de `ButtonConfiguration`, `InputConfiguration`, `ComboBoxConfiguration`, `DataGridViewConfiguration` y `CheckBoxConfiguration`
- [ ] La clase estatica concentra: el estilo de tarjeta (fondo, tipografia, `ForeColor`), la `Region` redondeada, el recalculo de la `Region` al redimensionar, y el wiring del boton Cancelar y de la `X`
- [ ] `ClienteModalControl` y `RecursoModalControl` la llaman desde el constructor y quedan sin codigo repetido entre si
- [ ] Los tres modales (Cliente, Recurso y Reserva) son iguales en estructura: los mismos eventos, los mismos botones y el mismo ciclo de vida. Lo unico que cambia son los controles que cada uno declara. Por eso lo repetido va en una clase estatica y no en una clase base con herencia
- [ ] La `Region` que se le pasa a la ventana host es una **copia**: el `UserControl` puede dispostear la suya sin matar la de la ventana
- [ ] La clase estatica no depende de la capa de datos: sigue siendo valido el criterio de D-005 (el modal no toca el DAO)

### ComboBox buscable

- [ ] `ComboBoxConfiguration` suma un metodo para cargar un combo con busqueda interna: `DropDownStyle = DropDown`, el usuario escribe y la lista se filtra
- [ ] Con el texto vacio se muestra la lista entera
- [ ] La busqueda es `Contains` e insensible a mayusculas, **no** `StartsWith`
- [ ] El combo **no** usa `AutoCompleteMode` ni `AutoCompleteSource`: el autocompletado nativo del `ComboBox` busca por `StartsWith` y no sirve
- [ ] El filtro no se realimenta: recargar los `Items` no vuelve a disparar el filtro
- [ ] La seleccion se conserva al filtrar: si el item seleccionado sigue en la lista, queda seleccionado
- [ ] El combo de clientes del modal de Reservas busca por nombre, apellido o DNI
- [ ] El combo de recursos del modal de Reservas busca por nombre o ubicacion

### El modal recibe lo que necesita, no el DAO

- [ ] La base comun de los modales no conoce la capa de datos: recibe por parametro todo lo que necesita
- [ ] El modal de Recursos recibe el catalogo de tipos (ya lo hace hoy)
- [ ] El modal de Reservas recibe un `List<Cliente>` y un `List<Recurso>`, igual que el modal de Recursos recibe el `List<TipoRecurso>`
- [ ] La superposicion de reservas no se valida en el modal: la valida el procedimiento almacenado al guardar. El modal no puede saber que reservas tiene cada recurso
- [ ] La disponibilidad no se resuelve en el modal: el estado del recurso (`Disponible`) se filtra en el combo, y el conflicto de fechas lo rechaza el SP
- [ ] El modal de Clientes sigue recibiendo la entidad a editar, no la lista: la busca el form

### Lo repetido de los forms de listado NO se extrae

La duplicacion del lado de los forms se deja como esta. Se evaluo extraerla a una clase estatica
y se descarto: la parte que se repite (cargar la grilla, detectar la columna de boton, leer el id)
son pocas lineas y cada una es trivial, mientras que la que de verdad se repite (abrir el modal,
cablear los eventos, persistir, refrescar) es distinta en cada form porque cada uno tiene su DAO,
su modal y su mensaje de error. Ademas, la carga de la grilla no es igual en los dos: `ClienteDAO`
devuelve `List<Cliente>` y `RecursoDAO` devuelve `DataTable` (unificar eso es de la Etapa 5).

- [ ] `FrmClientes` y `FrmRecursos` conservan su carga de grilla, su deteccion de la columna de accion y su lectura del id, cada uno con su propio DAO y su propio mensaje de error


### Fugas de recursos en la UI

- [ ] Las fuentes de `DataGridViewConfiguration` son estaticas: no se crean por llamada ni por celda pintada
- [ ] `CellPainting` se desuscribe antes de suscribirse de nuevo
- [ ] Queda decidido y documentado si el form embebido se recrea en cada click del sidebar o se conserva la instancia

## Criterio de pase

- [ ] El modal de Clientes y el de Recursos se cierran con ESC, con Cancelar y con la `X`
- [ ] Con el modal abierto, un click real sobre el area del velo no llega a la ventana principal
- [ ] El velo no queda en pantalla al cerrar el modal por cualquier via
- [ ] Un alta con DNI repetido no deja el velo en pantalla: el modal se cerro antes de escribir en la base
- [ ] Los errores de `txtCapacidad`, `txtNumMonitores` y `txtMetros` se limpian al reintentar con datos validos
- [ ] `ReferenceEquals(ventana.Region, modal.Region)` da `False` y leer la `Region` de la ventana no tira excepcion
- [ ] `grep` de `static` en `Herramientas/ModalOverlay.cs` no devuelve nada: el velo es un objeto por ventana
- [ ] `grep` de `ShowDialog` en `Herramientas/ModalOverlay.cs` devuelve la apertura del modal
- [ ] `grep` de `CancelButton` o de `ProcessCmdKey` en los modales da un resultado: sin eso, ESC no cierra nada
- [ ] No hay dos modales que implementen el estilo de tarjeta por separado
- [ ] `Herramientas/ModalConfiguration.cs` existe y los dos UserControl modales la usan: no queda codigo de tarjeta, `Region`, `OnResize`, Cancelar ni `X` repetido entre los dos
- [ ] `ReservaModalControl` se escribe sin copiar nada: declara sus controles y su validacion, y el mecanismo lo trae de `ModalConfiguration`
- [ ] La base comun de los modales sirve para un modal que no necesita datos, uno que recibe un catalogo y uno que recibe dos listas: ninguno de los tres conoce el DAO
- [ ] El combo buscable filtra con `Contains`, con el texto vacio muestra la lista entera, y el combo no usa `AutoCompleteMode` ni `AutoCompleteSource`
- [ ] `FrmReservas` se puede escribir sin copiar el mecanismo: el modal devuelve un resultado y el form persiste
- [ ] El proyecto compila sin warnings

## Fuera de alcance

- [ ] `try/catch` de `MySqlException` y del `SQLSTATE 45000` en los DAO -> Etapa 4
- [ ] Unificar el tipo de retorno de los DAO (`DataTable` vs `List<T>`) -> Etapa 5
- [ ] `FrmReservas` y su modal -> issue 2.05
- [ ] `DashboardDAO` y las tarjetas del dashboard -> issue 3.01
- [ ] Correccion del README (dice que la cadena de conexion vive en `ConexionBD`, pero ya sale de `appsettings.json`) -> Etapa 6

## Pendiente que se cierra en la Etapa 4

Si al cerrar este issue queda wiring a medias, se anota explicitamente en el issue de la Etapa 4
para que no se pierda:

- [ ] `ClienteDAO` / `RecursoDAO` — el `SIGNAL 45000` por DNI o email repetidoTodavia no se maneja: la excepcion sube al hilo de la UI
- [ ] `RecursoDAO.ObtenerPorId` — `Enum.Parse` sobre `EstadoRecurso` y `RecursoFactory.Crear` lanzan si el dato de la base no coincide con lo esperado

## Archivos a crear

- `SistemaCoworking/Herramientas/ModalConfiguration.cs` — la clase estatica con lo repetido de los modales (tarjeta, `Region`, Cancelar, `X`, ESC)

## Archivos a modificar

- `SistemaCoworking/Herramientas/ModalOverlay.cs` — el velo deja de ser `static` y pasa a ser un objeto por ventana; el modal se abre con `ShowDialog()`
- `SistemaCoworking/Herramientas/DataGridViewConfiguration.cs` — fuentes estaticas y desuscripcion del `CellPainting`
- `SistemaCoworking/Herramientas/ComboBoxConfiguration.cs` — suma el combo buscable con filtro interno
- `SistemaCoworking/Formularios/UserControls/ClienteModalControl.cs` — usa `ModalConfiguration`; la `Region` deja de liberarse antes de tiempo
- `SistemaCoworking/Formularios/UserControls/RecursoModalControl.cs` — usa `ModalConfiguration`; la limpieza de errores recorre los controles anidados
- `SistemaCoworking/Formularios/FrmClientes.cs` — persiste despues de que el modal cerro
- `SistemaCoworking/Formularios/FrmRecursos.cs` — persiste despues de que el modal cerro
- `SistemaCoworking/FrmPrincipal.cs` — crea y destruye el velo; deja de inyectarlo por constructor

## Nota

Este issue sale del analisis de la arquitectura de la capa de presentacion. El shell, los forms
embebidos y la separacion entre modal y capa de datos estan bien: ese diseno no se toca. Lo que se
cambia es el mecanismo del modal, que hoy es una ventana que *parece* modal pero no lo es.

La razon de hacerlo ahora y no despues es simple: el modal de Reservas es el mas complejo del
sistema, y si el mecanismo arrastra el velo pegado, la `Region` liberada y la falta de ESC, el modal
mas importante del proyecto es el que los va a arrastrar. Y si los dos modales que ya existen
resuelven el ciclo de vida a mano, el tercero lo resuelve a mano tambien, con tres copias distintas.

La separacion "el modal devuelve la entidad, el form la persiste" es el cambio de fondo: separa
recoger el dato de guardarlo. Con eso, el error de MySQL deja de poder dejar la aplicacion
bloqueada, y el `try/catch` del DAO queda para la Etapa 4, que es donde corresponde.

El modal se decide mantener: la aplicacion va con estetica de SPA, con el detalle de la entidad
en una ventana propia sobre un velo oscuro en vez de un panel de detalle embebido en el listado.
Es una decision de producto, no de arquitectura, y es legitima. Lo que este issue garantiza es que
el mecanismo que la sostiene quede bien: con las restricciones ya medidas, no con suposiciones.
