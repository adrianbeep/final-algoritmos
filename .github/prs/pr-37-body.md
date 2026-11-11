## Contexto

Issue #36 - [Etapa 2] Shell del form principal: se elimina el panel sin uso y se arregla el dock del panel de contenido.

Limpieza del `FrmPrincipal`, que es el shell que hospeda los formularios embebidos de cada modulo. Dos cosas: un panel que quedo sin uso, y un `Dock = Fill` que no respetaba el `Dock` de los otros paneles.

## Qué incluye

**Se elimina `pnModal`**
- El panel `pnModal` era el contenedor del modal en el diseño original. Cuando los modales pasaron a ser ventanas propias que se abren con `ShowDialog()`, quedo sin uso: el constructor lo sacaba de `pnForms` en cada arranque con `pnForms.Controls.Remove(pnModal)` y no se pintaba nunca.
- Se borra el control del Designer, su campo y el `Remove` del constructor.
- `pnForms` queda como el unico hijo de si mismo, asi que tambien se van sus `SuspendLayout` y `ResumeLayout`.

**El `Dock = Fill` de `pnForms` respeta a los demas paneles**
- El z-order de WinForms define el orden de docking: los controles se dockean del indice mas alto al mas bajo. `pnForms` con `Dock = Fill` tiene que estar en el indice 0 para dockearse ultimo y respetar el area que dejan `panel1` (Top) y `flpSidebar` (Left).
- Estando mas adelante, se dockeaba primero y se comia el area de cliente completa: tapaba el header y el sidebar.
- Con `Dock = Fill` no hace falta posicionar el panel a mano: el motor de layout lo recalcula en cada passage. Por eso el `pnForms.Left = flpSidebar.Width` del tick sobraba y se elimino: la asignacion manual se pisaba en el siguiente layout y pelea con el dock.

**Transicion del sidebar mas rapida**
- El sidebar contrae y expande de 25 pixeles por tick en vez de 5.

## Verificación

- `dotnet build SistemaCoworking\SistemaCoworking.csproj -t:Rebuild`: compilacion correcta, 0 errores y 8 advertencias preexistentes (4 en `FrmPrincipal.cs`, 3 en `TextControl.cs` y 1 en `DataGridViewConfiguration.cs`).
- Con el form instanciado y medido: `pnForms` queda en (250,45) 977x720, no se superpone ni con `panel1` ni con `flpSidebar`, sigue al sidebar cuando este se contrae y sigue a la ventana cuando se redimensiona.

Closes #36
