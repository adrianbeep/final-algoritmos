## Descripcion

PR de setup del proyecto C#. Cierra el sub-issue #16.

### Lo que incluye

**Commit 1 — Setup estructural del proyecto WinForms**

- Proyecto WinForms en .NET 10.0 con C# 14
- `FrmPrincipal` con sidebar oscuro, 4 botones (Dashboard, Clientes, Recursos, Reservas)
- Patron form embed: los forms se embeben en `pnForms` con `TopLevel=false`, `FormBorderStyle=None`, `Dock=Fill`
- Animacion del sidebar con Timer (250px expandido, 60px colapsado)
- Helper `CargarFormularioAPanel` con tracking de form activo y boton activo
- Dashboard se carga al inicio
- 21 iconos en `Recursos/Iconos/` para usar en los forms
- Estructura de carpetas: `Modelos/`, `Datos/`, `Formularios/`, `Interfaces/`, `Recursos/`, `Properties/`
- `.gitignore` actualizado para excluir `.vs/`

**Commit 2 — Skeletons de clases del dominio**

- `Interfaces/IReservable.cs` (skeleton, metodos a implementar)
- `Modelos/Recurso.cs` (skeleton, abstract class a implementar)
- `Modelos/Sala.cs`, `PuestoTrabajo.cs`, `OficinaPrivada.cs` (skeletons)
- `Modelos/Cliente.cs`, `Reserva.cs`, `TipoRecurso.cs` (skeletons)

### Por que skeletons en este PR

Las clases del dominio se commitean vacias en este PR para que la estructura del proyecto este completa. Las implementaciones reales (propiedades, metodos, polimorfismo con `CalcularCosto`, herencia) van en PRs separados:

- **#17** (Clases del dominio) — implementa las propiedades, herencia, polimorfismo
- **#18** (Form Clientes) — implementa el form
- **#19** (Form Recursos) — implementa el form
- **#20** (Form Reservas) — implementa el form
- **#21** (Dashboard) — implementa el dashboard

### Patron de form embed (por que no MDI)

- MDI nativo tiene parpadeos al redimensionar
- MDI mantiene multiples forms en memoria, mayor consumo
- MDI exige manejo manual de cual es el form "activo"
- El look de MDI es Windows clasico, no moderno

Form embed simula un SPA (Single Page Application): un solo form visible a la vez, sidebar fijo, contenido que cambia.

### Archivos a crear (en PRs futuros)

- Implementaciones reales de las 7 clases del dominio (#17)
- Implementacion de los 4 forms secundarios (#18-#20)
- Implementacion del dashboard (#21)
- DAOs y ConexionBD (Etapa 3)
- Manejo de errores (Etapa 4)

Closes #16
