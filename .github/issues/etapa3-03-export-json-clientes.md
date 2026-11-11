# [Etapa 3] Exportacion del catalogo de clientes a un archivo JSON

## Objetivo

Incorporar una operacion de exportacion mediante JSON, que es lo que pide la
consigna en la etapa de "Intercambio y configuracion":

> Incorporar una operacion de importacion o exportacion mediante JSON. Debe
> utilizar objetos reales del sistema y tener una finalidad concreta.

La finalidad concreta es sacar el catalogo de clientes a un archivo para
revisarlo, compartirlo o volver a importarlo despues. El archivo generado es
ademas el "archivo JSON de ejemplo" que pide el entregable final.

## Entregables

- [ ] Un metodo privado en `FrmClientes` que exporta el catalogo de clientes a
      un archivo JSON
- [ ] La exportacion usa los objetos reales del modelo (`Cliente`), no las filas
      de la grilla
- [ ] El archivo sale con los nombres de las propiedades del dominio y con la
      propiedad `Descripcion` excluida, porque es un dato derivado que se
      calcula para mostrar
- [ ] Si la escritura falla, se avisa con el manejo de errores explicito que
      quedo en D-010
- [ ] Boton `Exportar` en la franja superior del formulario, al lado del `Nuevo`

## Criterio de pase

- [ ] El boton genera un `.json` con el catalogo de clientes
- [ ] El JSON se puede abrir y leer: es un arreglo de objetos con `Id`,
      `Nombre`, `Apellido`, `Dni`, `Telefono` y `Email`
- [ ] El archivo se puede volver a leer con `JsonSerializer.Deserialize` y
      devuelve clientes con los mismos datos
- [ ] El proyecto compila con 0 errores

## Fuera de alcance

- [ ] Importar clientes desde un archivo JSON: la consigna pide una operacion,
      no las dos, y la importacion exige resolver duplicados y escribir por el
      DAO. Queda para una entrega posterior
- [ ] Exportar reservas o recursos: tambien serian validas, pero el catalogo de
      clientes es el que sale del modelo sin datos derivados

## Nota

El boton y su evento los agrega el equipo: el metodo queda listo para que se
llame desde `btnExportar_Click`.

La razon de usar `ObtenerTodosComoLista()` y no las filas de la grilla es que
la consigna pide "objetos reales del sistema". El `DataTable` de la grilla son
filas de la base, con los nombres de las columnas, y `System.Text.Json` no lo
serializa de forma utilizable. La lista, en cambio, son las entidades del
dominio: el JSON sale con los nombres de las propiedades del modelo.
