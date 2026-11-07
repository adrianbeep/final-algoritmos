# [Etapa 2] Form Reservas

## Objetivo

Implementar el formulario de gestion de reservas con el patron Listado + Detalle, permitiendo alta, modificacion y cancelacion. Este form es el mas complejo porque muestra la consulta de reservas (vista + UDFs + validaciones de superposicion).

## Entregables

### Layout del formulario

- [ ] DataGridView a la izquierda mostrando todas las reservas vigentes
- [ ] Panel de detalle a la derecha con los campos:
  - Id (oculto o readonly)
  - ComboBox de Cliente (muestra nombre + apellido, guarda id)
  - ComboBox de Recurso (muestra nombre del recurso, guarda id)
  - DateTimePicker para fecha_inicio
  - DateTimePicker para fecha_fin
  - Label con total_reserva calculado (usando el polimorfismo de `CalcularCosto`)
- [ ] Botonera:
  - Nuevo
  - Guardar (alta o modificacion)
  - Cancelar (limpia)
  - **CANCELAR RESERVA** (boton rojo, solo habilitado si hay reserva seleccionada y esta Confirmada)

### Comportamiento

- [ ] Click en una fila → carga los datos en el detalle
- [ ] Al seleccionar cliente y recurso, el ComboBox guarda el id internamente
- [ ] Al cambiar fechas o recurso, se recalcula el total usando `Recurso.CalcularCosto(dias)`
- [ ] Boton CANCELAR RESERVA:
  - Solo habilitado si la reserva seleccionada esta en estado "Confirmada"
  - Pide confirmacion con MessageBox
  - Cambia el estado a "Cancelada" (baja logica)
  - Refresca la grilla

### Validaciones en C#

- [ ] Cliente y recurso seleccionados
- [ ] fecha_fin > fecha_inicio
- [ ] Calculo del total correcto segun el tipo de recurso (polimorfismo)

## Criterio de pase

- [ ] El form se ve consistente con el resto
- [ ] Se pueden crear reservas nuevas
- [ ] Se pueden modificar reservas existentes
- [ ] El boton CANCELAR RESERVA funciona (cambia estado, refresca grilla)
- [ ] El total se calcula usando el polimorfismo de `CalcularCosto`

## Archivo a crear

- `SistemaCoworking/Forms/FormReservas.cs`

## Nota

Este form es el que mas evidencia muestra de la integracion POO + UI: la UI usa el polimorfismo de las clases para calcular el total automaticamente. No hay un `switch (tipo)` ni un `if-else` en C# para calcular el costo: el metodo polimorfico `CalcularCosto` se encarga.
