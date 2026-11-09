## Contexto

Issue #20 - [Etapa 2] Form Reservas.

Implementación del formulario de gestión de reservas con el patrón Listado + Modal, permitiendo alta, modificación y visualización de reservas sobre la grilla.

## Qué incluye

**Formulario de Reservas (`FrmReservas`)**
- Configuración y carga del `DataGridView` con las reservas existentes vía `ReservaDAO`.
- Apertura del modal en modo alta (`btnCrearReserva`) y en modo edición desde el botón de la fila seleccionada.
- Persistencia posterior al cierre del modal (`ShowDialog()` con `DialogResult.OK`), insertando o modificando según corresponda y refrescando la grilla.
- Carga desacoplada de colecciones de `Cliente` y `Recurso` para alimentar los selectores sin acoplar el modal a la base de datos.

**Modal de Reservas (`ReservaModal`)**
- Implementado como `Form` modal autónomo, configurado mediante `ModalConfiguration.ConfigModal`.
- Combobox buscables por filtro `Contains` para clientes y recursos con selección por Id.
- Selectores de fecha `DateTimePicker` para `fecha_inicio` y `fecha_fin`.
- Recálculo dinámico del total de la reserva aprovechando el polimorfismo de `Recurso.CalcularPrecio()`.
- Validaciones en C# de cliente/recurso seleccionados, consistencia de rango de fechas y generación de la entidad `Reserva`.

**Estandarización de nombres de modales**
- Renombrado de `ClienteModalControl` a `ClienteModal` y `RecursoModalControl` a `RecursoModal` (junto con sus diseñadores y recursos), reflejando que son formularios modales (`Form`) y no controles de usuario (`UserControl`).

## Verificación

- `dotnet build SistemaCoworking\SistemaCoworking.slnx`: compilación exitosa (0 errores).
- Advertencia CS8625 de nulabilidad corregida en la firma de `AbrirModalReserva`.

Closes #20
