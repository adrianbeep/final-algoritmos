## Contexto

Issue #24 - [Etapa 3] Integracion C# + MySQL: ConexionBD y capa DAO.

Completa la capa de datos: los 5 DAOs, la externalizacion de la cadena de conexion a `appsettings.json` y la validacion de configuracion al arrancar. El PR #27 quedo en draft con la capa a medias; este lo cierra con los 5 DAOs implementados.

## Que incluye

**Commit 1 - Capa de acceso a datos**

- `ConexionBD`: la cadena de conexion en un solo lugar; `CrearConexion()` devuelve la `MySqlConnection` sin abrir y la abre cada DAO dentro de su `using`.
- `ClienteDAO`: `ObtenerTodos()`, `ObtenerPorId(id)`, `Insertar()` -> `sp_alta_cliente`, `Modificar()` -> `sp_modificar_cliente`.
- `TipoRecursoDAO`: `ObtenerTodos()`, solo lectura para el ComboBox de Recursos.
- `RecursoDAO`: `ObtenerTodos()` sobre `vw_recursos_estado` y `ObtenerPorId()` sobre el JOIN de `recursos + recursos_detalle + tipo_recursos`, con la jerarquia reconstruida por `switch` sobre `nombre_tipo_recurso`. `Insertar()` -> `sp_alta_recurso`, `Modificar()` -> `sp_modificar_recurso`.
- `ReservaDAO`: `ObtenerTodos()` sobre `vw_reservas_completas`, `ObtenerPorId(id)`, `Insertar()` -> `sp_alta_reserva`, `Modificar()` -> `sp_modificar_reserva`, `Cancelar(id)` -> `sp_cancelar_reserva`.
- `DashboardDAO`: expone las 5 funciones almacenadas para las tarjetas del dashboard.
- `Recurso` y sus 3 clases hijas: el constructor recibe el estado, que antes se pisaba con `Disponible` y hacia que un recurso `Inhabilitado` apareciera reservable.

**Commit 2 - Configuracion externa**

- `appsettings.json` versionado con la clave `ConnectionStrings:Default`.
- `ConexionBD` lee el JSON con `ConfigurationBuilder` y valida que la clave exista.
- `Program` valida la configuracion una sola vez al arrancar y avisa con un `MessageBox` claro, en lugar de fallar con una excepcion de MySQL en la primera consulta.
- `csproj`: paquete `Microsoft.Extensions.Configuration.Json` y copia del JSON a la carpeta de salida.

## Cambios en la base

- `recursos.estado` queda alineado con `Recurso.EstadoRecurso` (`EnMantenimiento`, sin espacio) y se actualizan los 2 procedimientos queTodavia recibian el valor viejo.
- El seed de `recursos` completa la columna `costo`, que era `not null check(costo > 0)` y no se informaba. Los 8 valores quedan consistentes: `CalcularPrecio()` devuelve exactamente el `precio_dia` de cada fila.

## Verificacion

- `dotnet build`: 0 errores. `Modelos/` y `Datos/` sin warnings.
- `git grep "server=|uid=|pwd=" -- "*.cs"`: 0 resultados.
- Probado el `ConexionBD` real contra el `appsettings.json`: la cadena se lee bien, `CrearConexion()` devuelve la conexion cerrada, y las dos rutas de falla (archivo faltante y clave faltante) devuelven un mensaje claro con la ruta exacta.
- Los parametros de cada DAO se verificaron 1:1 contra los que declaran los procedimientos almacenados.

## Pendiente

Los formularios que consumen esta capa se siguen en sus propios issues:

- #18 Form Recursos
- #20 Form Reservas
- #21 Dashboard

Closes #24
