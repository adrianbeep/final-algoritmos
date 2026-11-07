# [Etapa 2] Form Clientes

## Objetivo

Implementar el formulario de gestion de clientes con el patron Listado + Detalle, permitiendo dar de alta y modificar clientes. La consulta a la base de datos se hace en la Etapa 3, por ahora funciona con datos de prueba en memoria.

## Entregables

### Layout del formulario

- [ ] DataGridView a la izquierda mostrando todos los clientes
- [ ] Panel de detalle a la derecha con los campos del cliente:
  - Id (oculto o readonly, autogenerado)
  - Nombre (TextBox)
  - Apellido (TextBox)
  - DNI (TextBox, validacion de solo numeros)
  - Telefono (TextBox)
  - Email (TextBox, validacion basica de formato)
- [ ] Botonera:
  - Nuevo (limpia los campos)
  - Guardar (alta o modificacion segun el contexto)
  - Cancelar (limpia los campos y deselecciona la fila)
- [ ] NO boton Eliminar (decidimos no hacer delete logico)

### Buscador

- [ ] TextBox de busqueda arriba del DataGridView
- [ ] Filtro en tiempo real (evento TextChanged) por nombre, apellido, DNI o email
- [ ] Case-insensitive, usa `LIKE %texto%`

### Comportamiento

- [ ] Click en una fila del grid → carga los datos en el panel de detalle
- [ ] Boton Nuevo → habilita los campos para carga
- [ ] Boton Guardar → valida campos obligatorios y guarda (alta o update segun haya Id)
- [ ] Boton Cancelar → limpia y vuelve a estado inicial
- [ ] Validaciones en C#:
  - Campos obligatorios no vacios
  - DNI solo numeros
  - Email con formato basico (contiene @ y .)

## Criterio de pase

- [ ] El form se ve consistente con el sidebar (mismo color, tipografia, espaciado)
- [ ] Se pueden cargar clientes desde la UI
- [ ] Se pueden modificar clientes existentes
- [ ] El buscador filtra correctamente
- [ ] Las validaciones en C# funcionan (no se puede guardar sin nombre, etc.)

## Nota

Por ahora el form trabaja con una lista hardcodeada en memoria (ej: 5 clientes de prueba). En la Etapa 3 se reemplaza por la llamada al DAO que consulta la BD.
