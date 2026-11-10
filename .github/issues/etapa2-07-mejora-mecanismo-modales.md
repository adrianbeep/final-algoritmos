# [Etapa 2] Mejora del mecanismo de modales

## Objetivo

Los modales de Clientes y de Recursos funcionan, pero el mecanismo que los abre no es un modal real:
abre la ventana con `Show()` en vez de `ShowDialog()`, no maneja ESC, y el velo vive en variables
`static` compartidas. Eso produce tres consecuencias concretas: el modal no bloquea la aplicacion de
fondo, el velo puede quedar pegado en pantalla cuando falla una escritura, y el `Region` redondeado
se libera antes de tiempo.

Este issue endurece el mecanismo de modales **antes** de escribir `FrmReservas` y su modal, que es el
modal mas complejo del sistema (elige cliente, elige recurso, toma un rango de fechas y calcula el
total). Si el mecanismo no se corrige ahora, el modal de Reservas hereda cada uno de estos defectos y
encima hay que duplicarlos por tercera vez.

El alcance no es solo "arreglar el modal": es dejar el mecanismo en una forma que el modal de
Reservas pueda copiar sin arrastrar los problemas, y que se pueda explicar en cuatro frases.

## Como se explica

1. `FrmPrincipal` es el shell: abre un formulario embebido en el panel, segun el boton del sidebar.
2. Cada formulario embebido (`FrmClientes`, `FrmRecursos`, `FrmReservas`) es el listado de su entidad.
   No edita nada: abre el modal y espera.
3. El modal es un `Form` mas. Se abre con `ShowDialog()`, que espera a que se cierre y devuelve
   `DialogResult.OK` si el usuario guardo.
4. Si volvio `OK`, el modal deja la entidad en su propiedad `Entidad` y el listado la escribe en la
   base.

Eso es todo. Lo que hay alrededor (los helpers de `Herramientas/`) es estilo visual: lo hace la
IA para que los formularios se vean modernos.

## Entregables

### El modal es un Form

- [ ] `ClienteModalControl` y `RecursoModalControl` son `Form`, no `UserControl`: un `UserControl`
      no puede ser una ventana, asi que necesitaba una ventana host que lo contuviera
- [ ] Se borran los eventos `Aceptado` y `Closed`: el resultado viaja en el `DialogResult` y la
      entidad en la propiedad `Entidad`
- [ ] La propiedad `Entidad` lleva `[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]`:
      sin el, el analizador de WinForms la toma por error y rompe el build (WFO1000)
- [ ] El `Form` del modal se queda sin bordes, sin barra de tareas y centrado en la pantalla

### Modales con modalidad real

- [ ] El modal se abre con `ShowDialog()` en vez de `Show()`
- [ ] Con el modal abierto, la ventana principal queda **nativamente deshabilitada** (`IsWindowEnabled(hwnd)` = `False`): la app no responde. Medido: da igual que el modal tenga `Owner` o no, `ShowDialog()` sin parametro ya la deshabilita
- [ ] Un click real de mouse sobre la ventana principal no llega a sus botones
- [ ] `ESC` cierra el modal como cancelacion, sin guardar nada. `ESC` no es gratis: sin un `CancelButton` asignado, `ESC` no hace nada (el `DialogResult` queda en `None` y la ventana sigue visible)
- [ ] El boton Cancelar y la `X` de cierre hacen exactamente lo mismo
- [ ] La `X` de cierre tiene `TabStop = true` y se puede alcanzar con el teclado
- [ ] `ShowDialog()` bloquea al que llama: el codigo que abre el modal sigue recien cuando el modal cerro

### El modal escribe, y solo cierra si la escritura tuvo exito

- [ ] El modal recibe el DAO por constructor: el modal es el que sabe si es un alta o una edicion
- [ ] Al apretar Guardar, el modal arma la entidad y **escribe en la base**
- [ ] Si la escritura falla, el modal **sigue abierto** con lo que cargo el usuario: el error ya se mostro, asi que puede corregir y reintentar sin perder lo que escribio
- [ ] El boton Guardar se deshabilita mientras dura la escritura, para que un doble clic no dispare dos veces la misma operacion
- [ ] Recien si la escritura tuvo exito, el modal se cierra con `DialogResult.OK`
- [ ] El `try/catch` vive en la **capa de presentacion**, no en el DAO: el DAO lanza y lo capturan el modal y los forms

### Sin velo: el modal va en su propia ventana y nada mas

El velo con `Opacity` es una ventana layered (`WS_EX_LAYERED`), y los controles hijos de un Form con
`Opacity` heredan esa opacidad: si el modal fuera hijo del velo, la tarjeta se veria translucida. Por
eso el velo necesitaba su propia ventana, y el modal otra: **tres ventanas para un modal**.

Sin velo no hace falta nada de eso: son **dos ventanas**, la principal y la del modal, y la
modalidad la da el `ShowDialog()`. La app no responde mientras el modal esta abierto; lo que se
pierde es el oscurecido.

- [ ] `ModalOverlay` (clase estatica con estado global) y `ModalDialog` se borran: no hacen falta
- [ ] `FrmPrincipal` deja de crear el velo: no tiene ningun `Overlay` que mantener
- [ ] Los forms embebidos **no reciben nada** por constructor
- [ ] El fallback `new Form { Opacity = 0 }` desaparece: no quedan ventanas top-level sin dueno ni `Dispose`
- [ ] El dim, si se lo quiere despues, se agrega como decoracion: no toca la arquitectura

### Region con ciclo de vida propio

- [ ] El `Region` redondeado es del modal y de nadie mas: cada uno tiene el suyo, asi que no hay forma
      de que uno lo libere y deje muerto el del otro
- [ ] El recorte se rehace cuando el modal cambia de tamaño
- [ ] Ningun `Region` queda disposteado: leerlo no tira `ArgumentException: Parameter is not valid`

### Limpieza de errores en controles anidados

- [ ] La limpieza de errores al guardar recorre los controles anidados, no solo los hijos directos
- [ ] En el modal de Recursos, `txtCapacidad`, `txtNumMonitores` y `txtMetros` limpian su borde de error (hoy viven dentro de `pnSala`, `pnPuestoTrabajo` y `pnOficinaPrivada` y nunca se limpian)
- [ ] En el modal de Clientes se mantiene el comportamiento actual (los 5 campos son hijos directos)

### Lo repetido de los modales sale a una clase estatica

- [ ] `Herramientas/ModalConfiguration.cs`: clase estatica, al estilo de `ButtonConfiguration`, `InputConfiguration`, `ComboBoxConfiguration`, `DataGridViewConfiguration` y `CheckBoxConfiguration`
- [ ] **Una sola llamada** configura el modal completo: `ModalConfiguration.ConfigModal(this, btnCancelar, pBoxCerrar)`. Deja el estilo de tarjeta (fondo opaco, tipografia, `ForeColor`), las esquinas redondeadas, y el boton Cancelar, la `X` y la tecla ESC haciendo lo mismo
- [ ] `ClienteModalControl` y `RecursoModalControl` la llaman desde el constructor y quedan sin codigo repetido entre si
- [ ] Los tres modales (Cliente, Recurso y Reserva) son iguales en estructura: los mismos botones y el mismo ciclo de vida. Lo unico que cambia son los controles que cada uno declara. Por eso lo repetido va en una clase estatica y no en una clase base con herencia
- [ ] La clase estatica no depende de la capa de datos: sigue siendo valido el criterio de D-005 (el modal no toca el DAO)

### ComboBox con estilo de la app

- [ ] `ComboBoxConfiguration` queda como helper **solo estetico**, con un unico metodo: `Configuracion(combo)`. Deja el combo sin bordes, con la misma fuente que los campos de texto, fondo blanco y el color de la app
- [ ] Cargar los items es cosa del que abre el combo, no del helper: cada modal bindea su combo con `DisplayMember`, `ValueMember` y `DataSource`
- [ ] El `Value` del combo es el `Id`, asi que lo elegido se lee con `SelectedValue` y la edicion se abre con `SelectedValue = id`
- [ ] Para que el combo muestre nombre y apellido, `Cliente` tiene la propiedad `Descripcion` y su `ToString()` la devuelve: el combo muestra "Juan Perez - DNI 12345678"
- [ ] `Recurso` tiene lo mismo: `Descripcion` es "Sala A - Planta baja" y su `ToString()` la devuelve
- [ ] El combo de estado es un enum: no lleva `DataSource`, se cargan los valores y se lee con `SelectedItem`

### El modal recibe lo que necesita, no el DAO

- [ ] La base comun de los modales no conoce la capa de datos: recibe por parametro todo lo que necesita
- [ ] El modal de Recursos recibe el catalogo de tipos (ya lo hace hoy)
- [ ] El modal de Reservas recibe un `List<Cliente>` y un `List<Recurso>`, igual que el modal de Recursos recibe el `List<TipoRecurso>`
- [ ] La superposicion de reservas se avisa en el modal antes de guardar: el modal consulta a la base la funcion `fn_recurso_ocupado`. `sp_alta_reserva` la vuelve a validar al guardar
- [ ] La disponibilidad se resuelve tambien en el modal: el estado del recurso (`Disponible`) se filtra en el combo, y el conflicto de fechas se consulta a la base antes de guardar
- [ ] El modal de Clientes sigue recibiendo la entidad a editar, no la lista: la busca el form

### Lo repetido de los forms de listado NO se extrae

La duplicacion del lado de los forms se deja como esta. Se evaluo extraerla a una clase estatica
y se descarto: la parte que se repite (cargar la grilla, detectar la columna de boton, leer el id)
son pocas lineas y cada una es trivial, mientras que la que de verdad se repite (abrir el modal,
persistir, refrescar) es distinta en cada form porque cada uno tiene su DAO, su modal y su mensaje
de error. Ademas, la carga de la grilla no es igual en los dos: `ClienteDAO` devuelve `List<Cliente>`
y `RecursoDAO` devuelve `DataTable` (unificar eso es de la Etapa 5).

- [ ] `FrmClientes` y `FrmRecursos` conservan su carga de grilla, su deteccion de la columna de accion y su lectura del id, cada uno con su propio DAO y su propio mensaje de error

### Fugas de recursos en la UI

- [ ] Las fuentes de `DataGridViewConfiguration` son estaticas: no se crean por llamada ni por celda repintada
- [ ] `CellPainting` se desuscribe antes de suscribirse de nuevo
- [ ] **Decidido**: el form embebido se **recrea** en cada click del sidebar. Se elige asi porque es lo simple y garantiza que el listado muestre los datos mas frescos: al volver a un modulo se vuelve a consultar la base. El costo es una consulta extra por navegacion, que con el volumen del sistema no importa.

## Criterio de pase

- [ ] El modal de Clientes y el de Recursos se cierran con ESC, con Cancelar y con la `X`
- [ ] Con el modal abierto, un click real de mouse sobre la ventana principal no llega a sus botones
- [ ] Un alta con DNI repetido no deja la aplicacion bloqueada: el modal se cerro con el error del SP y el usuario puede corregir
- [ ] Los errores de `txtCapacidad`, `txtNumMonitores` y `txtMetros` se limpian al reintentar con datos validos
- [ ] `grep` de `Aceptado` o de `Closed` en los modales no devuelve nada: el resultado viaja en el `DialogResult`
- [ ] `grep` de `CancelButton` en los modales da un resultado: sin eso, ESC no cierra nada
- [ ] No hay dos modales que implementen el estilo de tarjeta por separado
- [ ] `ReservaModalControl` se escribe sin copiar nada: declara sus controles y su validacion, y el mecanismo lo trae de `ModalConfiguration`
- [ ] El combo tiene el estilo de la app con una sola llamada: `ComboBoxConfiguration.Configuracion(combo)`
- [ ] El combo bindeado muestra el texto y guarda el Id como `Value`: lo elegido se lee con `SelectedValue`
- [ ] `grep` de `try` en `Datos/` no devuelve nada: el DAO lanza y el error lo capturan el modal y los forms
- [ ] `grep` de `MessageBox` en `Datos/` no devuelve nada: la capa de datos no muestra mensajes
- [ ] `FrmReservas` se puede escribir sin copiar el mecanismo: el modal escribe y solo cierra si tuvo exito
- [ ] El proyecto compila sin errores

## Fuera de alcance

- [ ] Unificar el tipo de retorno de los DAO (`DataTable` vs `List<T>`) -> Etapa 5
- [ ] `FrmReservas` y su modal -> issue 2.05
- [ ] `DashboardDAO` y las tarjetas del dashboard -> issue 3.01
- [ ] Correccion del README (dice que la cadena de conexion vive en `ConexionBD`, pero ya sale de `appsettings.json`) -> Etapa 6

## Pendiente que se cierra en la Etapa 4

Si al cerrar este issue queda wiring a medias, se anota explicitamente en el issue de la Etapa 4
para que no se pierda:

- [ ] `RecursoDAO.ObtenerPorId` — `Enum.Parse` sobre `EstadoRecurso` y `RecursoFactory.Crear` lanzan si el dato de la base no coincide con lo esperado. Con el `try/catch` en la presentacion, el error se muestra en vez de romper la app, pero el `Enum.Parse` case-sensitive conviene revisarlo
- [ ] El dashboard (issue 3.01) tiene que capturar los errores de las 5 funciones en el form, como hacen los demas modulos

## Archivos a crear

- `SistemaCoworking/Herramientas/ModalConfiguration.cs` — la clase estatica con lo repetido de los modales (tarjeta, `Region`, Cancelar, `X`, ESC)

## Archivos a modificar

- `SistemaCoworking/Herramientas/DataGridViewConfiguration.cs` — fuentes estaticas y desuscripcion del `CellPainting`
- `SistemaCoworking/Herramientas/ComboBoxConfiguration.cs` — suma el combo buscable con filtro interno
- `SistemaCoworking/Formularios/UserControls/ClienteModalControl.cs` — pasa a ser `Form`; usa `ModalConfiguration`; deja la entidad en `Entidad`
- `SistemaCoworking/Formularios/UserControls/RecursoModalControl.cs` — pasa a ser `Form`; usa `ModalConfiguration`; la limpieza de errores recorre los controles anidados
- `SistemaCoworking/Formularios/FrmClientes.cs` — abre el modal con `ShowDialog()` y persiste despues
- `SistemaCoworking/Formularios/FrmRecursos.cs` — abre el modal con `ShowDialog()` y persiste despues
- `SistemaCoworking/FrmPrincipal.cs` — deja de crear el velo

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
en una ventana propia en vez de un panel de detalle embebido en el listado. Es una decision de
producto, no de arquitectura, y es legitima.

Lo que este issue garantiza es que el mecanismo que la sostiene quede bien, con las restricciones
ya medidas y no con suposiciones: el modal abre con `ShowDialog()`, la app queda deshabilitada de
verdad mientras esta abierto, ESC cierra, y el `Region` redondeado no se deja muerto. El velo se
saco: con el eran tres ventanas y un `Form` layered, y no justificaba la complejidad. Y el modal
paso a ser un `Form`: asi no necesita ventana host, ni `Owner`, ni ningun helper para abrirlo.
