## Contexto

Issue #51 - [Etapa 3] Boton de exportar el catalogo de clientes en el formulario.

El PR #45 dejo el metodo `ExportarCatalogoClientes()` listo en `FrmClientes`, pero
el boton que lo dispara quedo sin agregar. El roadmap lo lista como pendiente y sin
el boton la exportacion a JSON no se puede ejecutar desde la aplicacion: la unica
forma de tener el `.json` es llamar al metodo a mano.

De paso quedan dos detalles de la interfaz que se ven al usar la aplicacion: el
boton de "Nueva reserva" no tiene el estilo primario (el de clientes si) y las
tarjetas del panel de indicadores no quedan alineadas con el ancho del panel.

## Que incluye

**Boton de exportar (`FrmClientes`)**
- Boton "Exportar" en la franja superior del formulario, con el estilo secundario
  que usa la app.
- El evento `btnExportar_Click` llama a `ExportarCatalogoClientes()`, que ya se
  encarga del `SaveFileDialog`, de la serializacion y del manejo de errores de D-010.

**Estilo de los botones de las grillas**
- `FrmReservas` aplica `ButtonConfiguration.ConfigPrimary(btnCrearReserva)`, igual
  que hace `FrmClientes` con el boton de crear cliente.

**Dashboard (`FrmDashboard`)**
- Las etiquetas de las cuatro tarjetas se dimensionan al ancho real de la tarjeta.
- El titulo pasa a "Metricas del sistema": las cuatro tarjetas muestran metricas.
- Se sacan del Designer las propiedades que ya venian por defecto: `AutoSize` de las
  etiquetas y `FlowDirection` y `WrapContents` del `FlowLayoutPanel`. Los valores
  por defecto de WinForms para esas dos ultimas son `LeftToRight` y `True`, asi que
  el comportamiento en ejecucion no cambia.

**Roadmap (`.github/ROADMAP.md`)**
- Se sacan de la tabla de pendientes el dashboard con tarjetas KPI (PR #46) y el
  boton de exportar el catalogo a JSON (este PR).
- Se marcan los entregables de las etapas 2 y 3 que ya estan hechos. Queda
  pendiente solo el quinto entregable del dashboard: los accesos directos a las
  acciones mas comunes.

## Fuera de alcance

- Importar clientes desde un archivo JSON: la consigna pide una operacion, no las
  dos, y la importacion exige resolver duplicados y escribir por el DAO.
- Exportar reservas o recursos.
- Los accesos directos del dashboard a las acciones mas comunes.

## Verificacion

- `dotnet build SistemaCoworking\SistemaCoworking.csproj -t:Rebuild`: compilacion
  correcta, 0 errores y 7 advertencias preexistentes.
- El boton invoca el metodo `ExportarCatalogoClientes()` que ya se habia verificado
  contra la base real en el PR #45 (el `.json` sale con `Id`, `Nombre`, `Apellido`,
  `Dni`, `Telefono` y `Email`, y se vuelve a leer con `JsonSerializer.Deserialize`).

Closes #51
