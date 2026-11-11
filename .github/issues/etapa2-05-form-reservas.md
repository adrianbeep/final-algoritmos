# [Etapa 2] Form Reservas

## Objetivo

Implementar el formulario de gestion de reservas con el patron Listado + Modal, permitiendo alta, modificacion y cancelacion. Este form es el mas complejo del sistema: es el primero con dos combobox alimentados desde la base (clientes y recursos) y con un total que se recalcula solo mientras el usuario escribe.

El detalle de la reserva va en un modal, como el de Clientes y el de Recursos: el listado queda en el panel embebido del `FrmPrincipal` y la edicion ocurre en una ventana propia sobre el velo oscuro. La baja logica de la reserva no va en la botonera: es una accion de la fila.

## Entregables

### Layout del formulario

- [ ] DataGridView mostrando todas las reservas vigentes
- [ ] Columna de acciones con 2 botones por fila: `Editar` y `Cancelar reserva`
- [ ] Botonera del form con 3 botones:
  - `Nuevo` — abre el modal en modo alta
  - `Editar` — abre el modal con los datos de la fila seleccionada
  - `Cancelar` — limpia el estado y deselecciona la fila
- [ ] `Editar` solo se habilita cuando hay una fila seleccionada
- [ ] El modal de la reserva tiene los mismos 2 botones que los otros modales: `Guardar` y `Cancelar`

### El modal de la reserva

- [ ] Campos:
  - Id (oculto o readonly)
  - ComboBox de Cliente (muestra nombre + apellido, guarda id)
  - ComboBox de Recurso (muestra nombre del recurso, guarda id)
  - DateTimePicker para fecha_inicio
  - DateTimePicker para fecha_fin
  - Label con el total de la reserva
- [ ] El modal recibe un `List<Cliente>` y un `List<Recurso>` por parametro, igual que el modal de Recursos recibe el `List<TipoRecurso>`
- [ ] El form es el que consulta la base y se los pasa: el modal no conoce el DAO (se mantiene D-005)

### Combos buscables

- [ ] Los dos combobox son buscables: se escribe y la lista se filtra
- [ ] Con el texto vacio se muestra la lista entera
- [ ] La busqueda es por `Contains` e insensible a mayusculas, no por `StartsWith`
- [ ] El combo de clientes busca por nombre, apellido o DNI
- [ ] El combo de recursos busca por nombre o ubicacion
- [ ] El combo no usa `AutoCompleteMode` ni `AutoCompleteSource`: el autocompletado nativo busca con `StartsWith`

### Comportamiento

- [ ] Click en `Editar` de una fila -> abre el modal con los datos de esa reserva
- [ ] Click en `Nuevo` -> abre el modal vacio, en modo alta
- [ ] `Editar` de la botonera -> abre el modal con los datos de la fila seleccionada
- [ ] `Cancelar` de la botonera -> limpia el estado y deselecciona la fila
- [ ] El modal se comporta igual que el de Clientes y el de Recursos: mismos eventos, mismos botones, mismo ciclo de vida. Lo unico que cambia son los controles que declara
- [ ] Al seleccionar cliente y recurso, el combo guarda el id internamente
- [ ] Al cambiar las fechas o el recurso, se recalcula el total
- [ ] El total sale de `Recurso.CalcularPrecio()` por la cantidad de dias reservados: la regla de precio es la del polimorfismo, no un `switch` en la UI
- [ ] `Cancelar reserva` en la columna de acciones:
  - Solo habilitado si la reserva esta en estado "Confirmada"
  - Pide confirmacion con MessageBox
  - Cambia el estado a "Cancelada" (baja logica)
  - Refresca la grilla

### Validaciones en C#

- [ ] Cliente y recurso seleccionados
- [ ] fecha_fin > fecha_inicio
- [ ] Calculo del total correcto segun el tipo de recurso (polimorfismo)
- [ ] La superposicion de reservas se avisa en el modal antes de guardar: el modal consulta la funcion `fn_recurso_ocupado` y `sp_alta_reserva` la vuelve a validar al guardar

## Criterio de pase

- [ ] El form se ve consistente con el resto
- [ ] Se pueden crear reservas nuevas
- [ ] Se pueden modificar reservas existentes
- [ ] `Cancelar reserva` funciona (cambia el estado a Cancelada y refresca la grilla)
- [ ] El total se calcula usando el polimorfismo de `CalcularPrecio()`
- [ ] Los dos combos filtran la lista mientras se escribe, y con el texto vacio muestran la lista entera

## Fuera de alcance

- [ ] `ReservaDAO` y el consumo de `sp_alta_reserva`, `sp_modificar_reserva` y `sp_cancelar_reserva` -> Etapa 3
- [ ] Captura de `MySqlException` y del `SQLSTATE 45000` -> Etapa 4

## Archivos a crear

- `SistemaCoworking/Formularios/FrmReservas.cs`
- `SistemaCoworking/Formularios/UserControls/ReservaModalControl.cs`

## Nota

Este form es el que mas evidencia muestra de la integracion POO + UI: la UI usa el polimorfismo de las clases para calcular el total automaticamente. No hay un `switch (tipo)` ni un `if-else` en C# para calcular el costo: el metodo polimorfico `CalcularPrecio()` se encarga.

Este spec se actualizo despues de decidir que el detalle de las entidades va en un modal y no en un panel embebido, para que quede alineado con el modal de Clientes y el de Recursos, y con la mecanica de modal que define el issue 2.07. Tres cosas mas cambiaron: la botonera del form quedo con 3 botones (`Nuevo`, `Editar`, `Cancelar`); la baja logica salio de la botonera y paso a ser una accion de la fila, para que el modal no tenga un "Cancelar" y un "Cancelar reserva" lado a lado; y los combos son buscables, con filtro por `Contains`.

Los tres modales son iguales en estructura: los mismos eventos, los mismos botones y el mismo ciclo de vida. Lo unico que cambia entre uno y otro son los controles que cada uno declara. Por eso lo repetido vive en `ModalConfiguration` y no en una clase base con herencia.
