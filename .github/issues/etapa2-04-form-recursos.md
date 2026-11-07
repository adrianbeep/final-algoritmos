# [Etapa 2] Form Recursos

## Objetivo

Implementar el formulario de gestion de recursos con el patron Listado + Detalle, permitiendo alta y modificacion. El detalle debe mostrar campos especificos segun el tipo seleccionado, usando las clases `Sala`, `PuestoTrabajo` u `OficinaPrivada` que ya estan definidas en el sub-issue 2.02.

## Entregables

### Layout del formulario

- [ ] DataGridView a la izquierda mostrando todos los recursos (con columna de tipo)
- [ ] Panel de detalle a la derecha con los campos:
  - Id (oculto o readonly)
  - ComboBox de Tipo de Recurso (Sala / PuestoTrabajo / OficinaPrivada) arriba
  - Campos comunes: Nombre, Ubicacion, Estado (ComboBox), PrecioDia
  - Campos especificos que se muestran/ocultan segun el tipo:
    - Si tipo = Sala: CapacidadPersonas (NumericUpDown), TieneProyector (CheckBox)
    - Si tipo = PuestoTrabajo: NumeroMonitor (NumericUpDown)
    - Si tipo = OficinaPrivada: MetrosCuadrados (NumericUpDown)
- [ ] Botonera: Nuevo, Guardar, Cancelar (sin Eliminar)

### Comportamiento dinamico

- [ ] Al cambiar el ComboBox de tipo, se muestran solo los campos especificos de ese tipo
- [ ] Los campos de los otros tipos se ocultan (no se limpian, solo se ocultan)
- [ ] Al Guardar, se instancia la clase derivada correcta (`Sala`, `PuestoTrabajo` u `OficinaPrivada`) segun el tipo seleccionado
- [ ] El polimorfismo se usa: el DAO recibe un `Recurso` (tipo base) y la implementacion concreta resuelve que subclase es

### Validaciones en C#

- [ ] Nombre, ubicacion, estado, precio obligatorios
- [ ] PrecioDia > 0
- [ ] CapacidadPersonas > 0 si es Sala
- [ ] NumeroMonitor > 0 si es Puesto
- [ ] MetrosCuadrados > 0 si es Oficina

## Criterio de pase

- [ ] El form se ve consistente con el resto
- [ ] Al cambiar el tipo, los campos especificos se muestran/ocultan
- [ ] Se pueden dar de alta y modificar recursos de los 3 tipos
- [ ] El polimorfismo funciona: internamente se crean `Sala`, `PuestoTrabajo` u `OficinaPrivada` segun corresponda

## Archivo a crear

- `SistemaCoworking/Forms/FormRecursos.cs`

## Nota

Este es el form donde el polimorfismo se ve en accion: la UI manda un tipo, el back end instancia la clase concreta, y la logica de negocio (como `CalcularCosto`) se ejecuta segun la subclase. Es donde se demuestra que el diseno de clases no fue al pedo.
