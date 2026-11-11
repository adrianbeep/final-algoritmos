# [Etapa 2] Shell del form principal: se elimina el panel sin uso y se arregla el dock del panel de contenido

## Objetivo

Limpiar el contenedor del form principal (`FrmPrincipal`), que es el shell que hospeda los formularios embebidos de cada modulo.

El panel `pnModal` era el contenedor del modal en el diseño original. Cuando los modales pasaron a ser ventanas propias que se abren con `ShowDialog()`, el panel quedo sin uso: el constructor lo sacaba de `pnForms` en cada arranque y no se pintaba nunca.

Ademas, el `Dock = Fill` de `pnForms` no respetaba el `Dock` de los otros paneles del form: como `pnForms` estaba mas adelante en el z-order, se dockeaba primero y se comia el area de cliente completa, tapando el header y el sidebar.

## Entregables

- [ ] Se elimina el control `pnModal` del Designer y su campo
- [ ] Se elimina el `pnForms.Controls.Remove(pnModal)` del constructor, que ya no tenia sentido
- [ ] `pnForms` queda con `Dock = Fill` en el indice 0 de `Controls`, para que se dockee al ultimo y respete el area que dejan `panel1` (Top) y `flpSidebar` (Left)
- [ ] La transicion del sidebar se acelera de 5 a 25 pixeles por tick

## Criterio de pase

- [ ] El panel de contenido no se superpone ni con el header ni con el sidebar
- [ ] El panel de contenido sigue al sidebar cuando este se contrae
- [ ] El panel de contenido sigue a la ventana cuando se redimensiona
- [ ] El proyecto compila con 0 errores

## Fuera de alcance

- [ ] Reemplazar los modales de ventana por un panel embebido: los modales siguen siendo ventanas propias con `ShowDialog()`

## Archivos a crear o modificar

- `SistemaCoworking/FrmPrincipal.Designer.cs`
- `SistemaCoworking/FrmPrincipal.cs`

## Nota

El z-order de WinForms define el orden de docking: los controles se dockean del indice mas alto al mas bajo. Por eso un `Fill` tiene que estar en el indice 0 para dockearse al ultimo, y un panel con `Dock = None` intercalado no altera el layout de los demas.

Con `Dock = Fill` no hay que posicionar el panel a mano: el motor de layout lo recalcula en cada passage. Por eso el `pnForms.Left = flpSidebar.Width` del tick sobraba y se elimino: la asignacion manual se pisaba en el siguiente layout y pelea con el dock.
