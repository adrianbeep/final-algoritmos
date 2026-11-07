## Contexto

Issue #31 - [Etapa 2] Mejora del mecanismo de modales.

Los modales de Clientes y de Recursos funcionaban, pero el mecanismo que los abria no era un modal
real: abria la ventana con `Show()` en vez de `ShowDialog()`, nomaneja ESC, y el velo vivia en
variables `static` compartidas. Tres consecuencias concretas: el modal no bloqueaba la aplicacion de
fondo, el velo podia quedar pegado en pantalla cuando fallaba una escritura, y el `Region` redondeado
se liberaba antes de tiempo.

Este PR se cierra **antes** de escribir `FrmReservas` y su modal, que es el modal mas complejo del
sistema (elige cliente, elige recurso, toma un rango de fechas y calcula el total). Si el mecanismo
no se cerraba ahora, el modal de Reservas heredaba cada defecto y encima los duplicaba por tercera
vez.

## Que incluye

**El modal pasa a ser un Form**

Un `UserControl` no puede ser una ventana, asi que necesitaba una ventana host que lo contuviera
(`ModalOverlay`, y despues `ModalDialog`). Ahora el modal **es** un `Form` y se abre con
`ShowDialog()` a secas: no hay host, ni `Owner`, ni helper. De eso salen tres cosas:

- El ESC es **nativo**: `CancelButton` de WinForms, sin `KeyPreview` ni `ProcessCmdKey`.
- El `Region` redondeado es del modal y de nadie mas. Se acabo el clon, `SincronizarRegion` y el bug
  de la `Region` compartida, que dejaba muerta la de la ventana host.
- Se borran `ModalOverlay` y `ModalDialog`.

**El modal devuelve la entidad, el form persiste**

El modal arma la entidad, la deja en su propiedad `Entidad` y cierra con `DialogResult.OK`. El form
escribe en la base **despues** de que el modal cerro. Con el contrato anterior (`Aceptado`/`Closed`)
la escritura pasaba dentro del handler, con el modal abierto: una `MySqlException` dejaba el velo
clavado y la aplicacion inutilizable.

**El velo se saca**

El velo con `Opacity` es una ventana layered (`WS_EX_LAYERED`), y los controles hijos de un Form con
`Opacity` heredan esa opacidad: si el modal fuera hijo del velo, la tarjeta se veria translucida. Por
eso el velo necesitaba su propia ventana, y el modal otra: **tres ventanas para un modal**. Medido que
`ShowDialog()` sin parametro ya deshabilita la ventana principal a nivel nativo, asi que el velo no
hacia falta. Lo que se pierde es el oscurecido, que es decoracion y se puede agregar despues sin
tocar la arquitectura.

**Una sola llamada para configurar el modal**

`ModalConfiguration.ConfigModal(this, btnCancelar, pBoxCerrar)` deja el modal con el estilo de
tarjeta, las esquinas redondeadas, y el boton Cancelar, la `X` y la tecla ESC haciendo lo mismo. Los
tres modales (Cliente, Recurso y Reserva) son iguales en estructura: lo unico que cambia son los
controles que declara cada uno. Por eso lo repetido va en una clase estatica, al estilo del resto de
los helpers de `Herramientas/`, y no en una clase base con herencia.

**Combo buscable**

`ComboBoxConfiguration.ConfigBuscable` carga un combo editable en el que el usuario escribe y la
lista se filtra por `Contains` (no por `StartsWith`, que es lo que hace el autocompletado nativo, que
por eso queda desactivado). Con el texto vacio se muestra la lista entera.

**Dos bugs**

- La limpieza de errores al guardar recorria solo los hijos directos del modal, asi que
  `txtCapacidad`, `txtNumMonitores` y `txtMetros` (dentro de `pnSala`, `pnPuestoTrabajo` y
  `pnOficinaPrivada`) nunca limpiaban su borde de error. Ahora `TextControl.LimpiarErrores` recorre
  todo el arbol.
- El combo perdia la seleccion al elegir un item de la lista desplegada: el `SelectedItem` quedaba
  en `null` aunque el texto se viera bien, con lo que el form no podia leer lo que el usuario habia
  elegido. La causa era reconstruir la lista dentro del `TextChanged` que dispara la propia
  seleccion.

**Los formularios quedan con una historia de cuatro frases**

1. `FrmPrincipal` abre un formulario embebido en el panel, segun el boton del sidebar.
2. Cada formulario embebido (`FrmClientes`, `FrmRecursos`, `FrmReservas`) es el listado de su
   entidad. No edita nada: abre el modal y espera.
3. El modal es un `Form` mas. Se abre con `ShowDialog()`, que espera a que se cierre y devuelve
   `DialogResult.OK` si el usuario guardo.
4. Si volvio `OK`, el modal deja la entidad en su propiedad `Entidad` y el listado la escribe en la
   base.

## Cambios en la base

Ninguno.

## Verificacion

- `dotnet build`: 0 errores. Los warnings que quedan (8) son preexistentes y estan en `TextControl.cs`,
  `FrmPrincipal.cs` y `DataGridViewConfiguration.cs`.
- Banco de pruebas propio contra el `ClienteModalControl` real: es un `Form` sin bordes ni barra de
  tareas, el `Region` tiene la forma del `ClientSize`, `ESC` devuelve `Cancel` con el foco en un campo
  de texto, la ventana principal queda **nativamente deshabilitada** con 0 clicks al fondo, el boton
  Cancelar y la `X` devuelven `Cancel`, y guardar devuelve `OK` con la entidad en la propiedad.
- Banco contra el `RecursoModalControl` real: la limpieza de errores llega a los controles anidados
  (`txtCapacidad`, `txtNumMonitores`, `txtMetros`) y a los directos.
- Banco del combo con el tipo `Cliente` real: filtra por nombre, por apellido y por DNI, con el texto
  vacio muestra la lista entera, conserva el texto escrito y el caret al escribir, y al elegir un item
  de la lista queda seleccionado.
- Medido en .NET 10 con `IsWindowEnabled` (no con `Control.Enabled`, que cachea el valor
  administrado y miente): `ShowDialog()` sin `Owner` ya deshabilita la ventana principal.

## Pendiente

- El `try/catch` de `MySqlException` y del `SQLSTATE 45000` en los DAO es de la Etapa 4.
- Unificar el tipo de retorno de los DAO (`DataTable` vs `List<T>`) es de la Etapa 5.
- El modulo de Reservas sigue en su propio issue: #20.

Closes #31
