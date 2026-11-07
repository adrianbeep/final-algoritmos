# [Etapa 2] Setup estructural del proyecto C#

## Objetivo

Crear la estructura base del proyecto WinForms en **.NET 10.0** con C# 14, con el layout principal (sidebar + panel central donde se embeben los demas forms) y la base visual. Se usa **RealTaizor** como libreria de UI para botones de ventana modernos, temas, y responsive.

## Entregables

### Configuracion del proyecto

- [ ] `.csproj` configurado con .NET 10.0 y Windows Forms
- [ ] Paquete NuGet: `Taiizor.Elements` (y dependencias que requiera)
- [ ] MySql.Data se agrega en Etapa 3
- [ ] Nullable habilitado
- [ ] LangVersion en latest
- [ ] File-scoped namespaces (`namespace X;` en vez de `namespace X { }`)

### Estructura de carpetas

- [ ] `Modelos/` para las clases del dominio (Recurso, Sala, Cliente, etc.)
- [ ] `Interfaces/` para los contratos (IReservable, etc.)
- [ ] `Datos/` para los DAOs y ConexionBD
- [ ] `Formularios/` para los forms secundarios (FrmClientes, FrmRecursos, FrmReservas, FrmDashboard)
- [ ] `FrmPrincipal.cs` queda en la raiz del proyecto (no en Formularios/) porque es la ventana raiz

### Formulario principal

- [ ] `FrmPrincipal` con sidebar fijo a la izquierda (aprox 240px de ancho)
- [ ] Panel central donde se embeben los forms hijos (Dashboard, Clientes, Recursos, Reservas)
- [ ] Sidebar con 4 botones: Dashboard, Clientes, Recursos, Reservas
- [ ] Cada boton del sidebar con texto (icono se puede agregar despues)
- [ ] Indicador visual de que modulo esta activo en el sidebar
- [ ] Metodo helper para embeber forms en el panel central:
  ```csharp
  private void MostrarFormularioEnPanel(Form formHijo)
  {
      panelContenido.Controls.Clear();
      formHijo.TopLevel = false;
      formHijo.FormBorderStyle = FormBorderStyle.None;
      formHijo.Dock = DockStyle.Fill;
      panelContenido.Controls.Add(formHijo);
      formHijo.Show();
  }
  ```
- [ ] Click en cada boton del sidebar llama al helper con el form correspondiente

### Patron de embeber forms

Todos los forms (excepto FrmPrincipal) se embeben en el panel central con `TopLevel = false` y `FormBorderStyle = None`. No se abren como ventanas separadas. Esto da el efecto de un "MDI moderno" donde el sidebar siempre queda visible y solo cambia el contenido central.

### Estilo base

- [ ] Tema de RealTaizor aplicado (claro u oscuro, a definir)
- [ ] Botones de ventana (minimizar, maximizar, cerrar) con estilo moderno via RealTaizor
- [ ] Sidebar con colores consistentes:
  - Color principal (sidebar, acentos)
  - Color de fondo del panel central
  - Color de texto
  - Color de acento para resaltar (boton activo, numeros del dashboard)
- [ ] Tipografia uniforme: Segoe UI en todo
- [ ] Espaciado consistente (Padding, Margin) entre elementos
- [ ] Layout responsive (los elementos se ajustan si el usuario redimensiona la ventana)

## Criterio de pase

- [ ] El proyecto compila sin warnings con .NET 10.0
- [ ] Al ejecutar, se ve el sidebar con 4 botones y un panel central vacio
- [ ] Click en cada boton del sidebar carga un placeholder (o un form temporal) en el panel central
- [ ] Los colores y tipografia son consistentes en todo el form
- [ ] Los forms embebidos ocupan todo el panel central sin barras ni bordes

## Archivos a crear

- `SistemaCoworking/SistemaCoworking.csproj`
- `SistemaCoworking/SistemaCoworking.sln` (o `.slnx` segun lo que genere VS 2026)
- `SistemaCoworking/Program.cs`
- `SistemaCoworking/FrmPrincipal.cs`
- `SistemaCoworking/FrmPrincipal.Designer.cs`
- `SistemaCoworking/FrmPrincipal.resx`
- `SistemaCoworking/Modelos/` (carpeta vacia por ahora, se llena en 2.02)
- `SistemaCoworking/Interfaces/` (carpeta vacia por ahora)
- `SistemaCoworking/Datos/` (carpeta vacia por ahora)
- `SistemaCoworking/Formularios/` (carpeta vacia por ahora, se llena en 2.03-2.06)

## Nota sobre compatibilidad

El proyecto es .NET 10.0 y usa el formato de solution que genera Visual Studio 2026 (puede ser `.slnx` en vez de `.sln`). Esto significa que se abre sin problemas en las notebooks con VS 2026, pero la PC del aula con VS 2022 no va a poder abrir la solution. Si en algun momento queres que sea compatible con VS 2022, hay que bajar a .NET 8.0 (que usa `.sln`).

## Nota

Este sub-issue NO incluye las clases del dominio ni los forms de los modulos. Eso es el sub-issue 2.02 (clases) y 2.03-2.06 (forms). Aca solo va la estructura, el layout y el form principal.
