# [Etapa 2] Los titulos y los datos de la grilla de reservas se cortaban

## Objetivo

La grilla de reservas tiene doce columnas y vive en un panel de menos de 900 px.
Con el padding por defecto (12 px a cada lado) no entra ninguna cabecera: se ve
`Fe...`, `Fec...`, `Est...`, `No...`, `Tip...`. Y no solo los titulos: las
celdas tambien, porque las fechas salian como `10/12/2026 00:00:00` (con la
hora, que en un sistema que reserva por dias es ruido) y el boton `Cancelar`
quedaba clipped.

El resultado era una grilla ilegible en la pantalla principal del modulo.

## Entregables

- [ ] Los titulos de las columnas que se muestran quedan completos
- [ ] Ningun dato queda cortado
- [ ] Las fechas se muestran como `dd/MM/yyyy`, sin la hora
- [ ] El importe se muestra con separador de miles
- [ ] La columna `Id` no se muestra: es una clave interna
- [ ] La columna `Tipo` y la `Ubicacion` no se muestran: son atributos del
      recurso, que se ven en su modulo y en el modal de la reserva
- [ ] El click del boton de la fila sigue funcionando: lee el id de la columna
      oculta

## Criterio de pase

- [ ] La grilla no muestra ningun titulo ni dato con puntos suspensivos
- [ ] El id sigue disponible para el click, aunque la columna no se pinte
- [ ] El proyecto compila con 0 errores

## Fuera de alcance

- [ ] Agregar columnas: con nueve columnas visibles ya no entra nada mas

## Nota

El ancho de cada columna se reparte con `FillWeight`, que dice "esta columna
necesita tanto": se midio el ancho real del texto mas ancho de cada columna, con
la fuente de la grilla, y se puso ese numero como peso.

El padding de las celdas no se puede cambiar desde el constructor:
`DataGridView` solo deja escribir el estilo de la grilla antes de armarla, asi
que va columna por columna en el Designer. El de la cabecera si se puede, y se
achica en el constructor.

La verificacion se hizo midiendo el ancho real de cada texto y mirando
capturas de la aplicacion corriendo, no a ojo.
