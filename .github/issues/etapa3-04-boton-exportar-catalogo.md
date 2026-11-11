# [Etapa 3] Boton de exportar el catalogo de clientes en el formulario

## Objetivo

El PR #45 dejo el metodo `ExportarCatalogoClientes()` listo en `FrmClientes`, pero
el boton que lo dispara quedo sin agregar. El roadmap lo lista como pendiente:

> **Boton de exportar el catalogo a JSON** | El metodo y el archivo de ejemplo estan
> listos (PR #45); falta agregar el boton al formulario

Sin el boton, la exportacion a JSON no se puede ejecutar desde la aplicacion: la
unica forma de tener el `.json` es llamar al metodo a mano.

De paso quedan dos detalles de la interfaz que se ven al usar la aplicacion: el
boton de "Nueva reserva" no tiene el estilo primario (el de clientes si), y las
tarjetas del panel de indicadores no quedan alineadas con el ancho del panel.

## Entregables

- [ ] Boton "Exportar" en la franja superior de `FrmClientes`, con el estilo
      secundario
- [ ] El boton llama a `ExportarCatalogoClientes()`, el metodo que ya existe
- [ ] El boton de "Nueva reserva" de `FrmReservas` usa el estilo primario, como
      el de clientes
- [ ] Las tarjetas del dashboard quedan alineadas con el ancho del panel

## Criterio de pase

- [ ] El boton genera un `.json` con el catalogo de clientes
- [ ] El JSON se puede abrir y leer: es un arreglo de objetos con `Id`, `Nombre`,
      `Apellido`, `Dni`, `Telefono` y `Email`
- [ ] El proyecto compila con 0 errores

## Fuera de alcance

- [ ] Importar clientes desde un archivo JSON: la consigna pide una operacion, no
      las dos, y la importacion exige resolver duplicados y escribir por el DAO
- [ ] Exportar reservas o recursos

## Nota

El metodo no se toca: `ExportarCatalogoClientes()` ya serializa las entidades
`Cliente` del modelo, no las filas de la grilla. Este PR solo agrega el boton que
lo invoca y deja parejos los botones de las dos grillas.

El boton va en la franja superior del formulario, al lado del boton de "Nuevo",
que es donde la consigna pide los controles de accion del listado.
