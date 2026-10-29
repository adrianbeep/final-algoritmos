# [Etapa 3] Integración C# + MySQL: ConexionBD y capa DAO

## Objetivo

Conectar la aplicacion WinForms con la base MySQL construida en la Etapa 1, centralizando la
conexion en `ConexionBD` y encapsulando todo el acceso a datos en la capa `Datos/`. A partir de
este issue ningun formulario contiene SQL: cada modulo consume la base a traves de su DAO.

La cadena de conexion sale de `appsettings.json` y `ConexionBD` la lee de ahi, de modo que ningun
`.cs` del proyecto tenga los datos de conexion escritos dentro.

Flujo objetivo: `Form -> logica -> DAO -> ConexionBD -> MySQL`

## Entregables

### ConexionBD

- [ ] `ConexionBD` como clase estatica, con la cadena de conexion en un unico lugar
- [ ] Metodo `CrearConexion()` que devuelve una `MySqlConnection` nueva, sin abrirla
- [ ] La conexion se abre dentro del `using` de cada DAO, nunca en el formulario
- [ ] Ningun `.cs` fuera de `Datos/` contiene `new MySqlConnection`, `MySqlCommand` o SQL embebido

### Configuracion externa de la conexion

La cadena de conexion no se escribe dentro de `ConexionBD`: se configura por fuera y se lee de
`appsettings.json`. Es el mismo criterio que aplica la consigna en los requerimientos evolutivos,
adelantado a esta etapa para poder versionar la capa `Datos/` sin dejar los datos de conexion
escritos en el codigo.

- [ ] `SistemaCoworking/appsettings.json` con la cadena en `ConnectionStrings:Default`
- [ ] `ConexionBD` lee la cadena desde `appsettings.json` en vez de tenerla hardcodeada
- [ ] `appsettings.json` queda versionado: es configuracion de la app, no un secreto (base local)
- [ ] Ningun `.cs` del proyecto contiene `server=`, `uid=` ni `pwd=`
- [ ] Si falta `appsettings.json` o la clave, la app avisa con un mensaje claro en lugar de fallar
      con una excepcion de MySQL al primer `Open()`

### Capa DAO

- [ ] `ClienteDAO`: `ObtenerTodos()`, `ObtenerPorId(id)`, `Insertar(cliente)`, `Modificar(cliente)`
- [ ] `RecursoDAO`: `ObtenerTodos()`, `ObtenerPorId(id)`, `Insertar(recurso)`, `Modificar(recurso)`
- [ ] `ReservaDAO`: `ObtenerTodos()`, `ObtenerPorId(id)`, `Insertar(reserva)`, `Modificar(reserva)`, `Cancelar(id)`
- [ ] `TipoRecursoDAO`: `ObtenerTodos()` — solo lectura, carga el ComboBox de Recursos
- [ ] `DashboardDAO`: metricas para las tarjetas — expone las 5 funciones almacenadas
- [ ] Cada DAO devuelve clases de `Modelos/`, nunca `DataTable` ni `DataSet`

### Consumo de procedimientos almacenados

- [ ] `ClienteDAO.Insertar`      -> `sp_alta_cliente`
- [ ] `ClienteDAO.Modificar`     -> `sp_modificar_cliente`
- [ ] `RecursoDAO.Insertar`      -> `sp_alta_recurso`
- [ ] `RecursoDAO.Modificar`     -> `sp_modificar_recurso`
- [ ] `ReservaDAO.Insertar`      -> `sp_alta_reserva`
- [ ] `ReservaDAO.Modificar`     -> `sp_modificar_reserva`
- [ ] `ReservaDAO.Cancelar`      -> `sp_cancelar_reserva`
- [ ] Todos con `CommandType.StoredProcedure` y `MySqlParameter`
- [ ] Cero concatenacion de parametros en el texto SQL

### Consumo de vistas

- [ ] `ReservaDAO.ObtenerTodos()` consulta `vw_reservas_completas`
- [ ] `RecursoDAO.ObtenerTodos()` consulta `vw_recursos_estado`
- [ ] `RecursoDAO` reconstruye la jerarquia con `switch` sobre `nombre_tipo_recurso` al mapear el JOIN `recursos + recursos_detalle + tipo_recursos`
- [ ] `ClienteDAO.ObtenerTodos()` mantiene `SELECT` directo sobre `clientes` (no hay vista que lo cubra y la consigna no exige un SP para toda lectura)

### Consumo de funciones almacenadas

- [ ] `fn_total_clientes()`
- [ ] `fn_reservas_por_finalizar()`
- [ ] `fn_reservas_hoy()`
- [ ] `fn_recursos_disponibles()`
- [ ] `fn_recursos_reservados_hoy()`
- [ ] `FrmDashboard` deja de ser un placeholder y muestra las tarjetas con estos valores

### Formularios

- [ ] `FrmClientes` reemplaza la lista en memoria por `ClienteDAO.ObtenerTodos()`
- [ ] `ClienteModalControl.btnGuardar` llama a `Insertar` o `Modificar` segun haya Id
- [ ] `FrmRecursos` deja de ser un placeholder
- [ ] `FrmReservas` deja de ser un placeholder

## Criterio de pase

- [ ] El proyecto compila sin warnings
- [ ] El listado de clientes viene de la base, no de una lista en memoria
- [ ] Alta y modificacion de cliente persisten y sobreviven al reinicio de la app
- [ ] Los 7 procedimientos, las 2 vistas y las 5 funciones se invocan desde C#
- [ ] `grep` de `SELECT|INSERT|UPDATE|DELETE|MySqlCommand` fuera de `Datos/` no devuelve nada
- [ ] `grep` de `server=|uid=|pwd=` en los `.cs` no devuelve nada: la cadena sale de `appsettings.json`
- [ ] Cargar un cliente con DNI repetido falla (comportamiento esperado en esta etapa: el `SIGNAL 45000` todavia no se maneja, eso es la Etapa 4)

## Fuera de alcance

- [ ] Captura de `MySqlException` y del `SQLSTATE 45000` -> Etapa 4
- [ ] Importacion/exportacion JSON de objetos del sistema -> Etapa 6

## Pendiente que se cierra en la Etapa 4

Si al cerrar este issue queda wiring a medias, se anota explicitamente en el issue de la Etapa 4
para que no se pierda:

- [ ] `<modulo>` — `<que falta conectar>`

## Nota

Este issue **no** maneja errores de MySQL a proposito. Los 7 procedimientos levantan
`SIGNAL SQLSTATE '45000'` en sus validaciones de negocio, asi que un alta con DNI repetido
va a romper la aplicacion. Ese es el comportamiento esperado: la Etapa 4 existe justamente
para convertir ese crash en un mensaje que el usuario entienda.