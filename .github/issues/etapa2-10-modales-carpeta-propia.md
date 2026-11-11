# [Etapa 2] Los modales pasan a su propia carpeta

## Objetivo

En `SistemaCoworking/Formularios/UserControls/` conviven el `TextControl`, que si
es un UserControl, con los tres modales (`ClienteModal`, `RecursoModal` y
`ReservaModal`), que son Forms. La carpeta miente sobre lo que hay adentro: un
modal no es un UserControl, y el namespace `SistemaCoworking.Formularios.UserControls`
dice lo mismo.

La consigna pide que el equipo pueda explicar todo lo que entrega, y esto se
explica mejor con una carpeta por tipo de control.

## Entregables

- [ ] Los tres modales pasan a `SistemaCoworking/Formularios/Modales/`
- [ ] En `Formularios/UserControls/` queda solo el `TextControl`
- [ ] Los modales declaran el namespace `SistemaCoworking.Formularios.Modales`
- [ ] Los modales que usan el `TextControl` importan el namespace de los
      UserControls
- [ ] Los tres formularios que abren los modales importan el namespace nuevo
- [ ] El proyecto compila con 0 errores

## Criterio de pase

- [ ] El modal de cliente, el de recurso y el de reserva se abren y funcionan
      como antes
- [ ] El proyecto compila con 0 errores

## Fuera de alcance

- [ ] Renombrar los modales o cambiar la forma de abrirlos: solo se mueven los
      archivos de sitio
- [ ] Mover los UserControls a otra carpeta

## Nota

Los tres modales van juntos porque los tres son el mismo tipo de pantalla: un
formulario modal que se abre sobre el listado y devuelve el resultado.

El `TextControl` se queda donde esta porque si es un UserControl de verdad, y es
el unico que queda en la carpeta.

El `ReservaModal` es el unico que no importa el namespace de los UserControls:
no usa el `TextControl`, porque sus campos son un combo de clientes, un combo de
recursos y dos selectores de fecha.
