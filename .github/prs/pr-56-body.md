## Contexto

Issue #55 - [Etapa Final] El instalador de la aplicacion.

El proyecto de instalador de Visual Studio no empaquetaba la aplicacion. El MSI
que salia de ahi tenia dentro solo `SistemaCoworking.dll` y el
`runtimeconfig.json`: sin ejecutable, sin dependencias (MySqlConnector,
ReaLTaiizor, Microsoft.Extensions.*) y sin `appsettings.json`. Lo que se
instalaba no arrancaba.

Ademas, el tipo de proyecto "Setup and Deployment" no esta instalado en Visual
Studio 2026, y el MSBuild de la IDE no puede cargar un `.vdproj` (error
MSB4025). Ese camino no tenia salida, asi que el instalador se rehace con WiX,
que es lo que Microsoft usa hoy para .NET y que se compila desde la terminal.

## Que incluye

**`Instalador/Product.wxs`** - Que se empaqueta y como se instala
- Empaqueta la carpeta del `dotnet publish`: el ejecutable, las dependencias, el
  `appsettings.json` y el runtime de .NET.
- `Scope="perUser"`: instala en `%LOCALAPPDATA%\Programs\Sistema Coworking` y
  **no pide permisos de administrador**. En Program Files el instalador falla con
  "Error 1925" si el usuario no es admin, que es el caso habitual en las PCs de
  la facultad.
- Acceso directo en el menu de Inicio y entrada de desinstalacion.
- Los satelitos de idioma se limpian en la desinstalacion.

**`Instalador/Instalador.wixproj`** - El proyecto de WiX
- WiX 5.0.2: las versiones 6 y 7 exigen aceptar la EULA del "OSMF fee".
- El SDK de WiX globa solo los `.wxs` de la carpeta. Si se agrega un
  `<Compile Include="Product.wxs" />`, el archivo entra dos veces y el build
  falla con WIX0089.

**`Instalador/build-installer.ps1`** - Un comando para armarlo
- Publica la app en self-contained y despues compila el MSI.
- El MSI queda en `Instalador/output/Instalador.msi` (45 MB).

**`.gitignore`** - `Instalador/publish/` y `Instalador/output/` son salidas de
compilacion, no van al repo.

**`README.md`** - Como generar el instalador y que hace.

## Fuera de alcance

- El `.vdproj` y el MSI roto que salian de ahi se borran.
- No se publica el MSI como release de GitHub.

## Verificacion

- `powershell -ExecutionPolicy Bypass -File Instalador\build-installer.ps1`:
  publica y arma el MSI, 0 errores.
- El MSI contiene 284 archivos: `SistemaCoworking.exe`, `appsettings.json`,
  `MySqlConnector.dll`, `ReaLTaiizor.dll`, las de `Microsoft.Extensions.*` y el
  runtime de .NET (`coreclr.dll`, `hostfxr.dll`).
- Instalacion real (`msiexec /i`): exit 0, 284 archivos en
  `%LOCALAPPDATA%\Programs\Sistema Coworking`, acceso directo en el menu de
  Inicio y entrada de desinstalacion.
- La aplicacion **instalada** arranca y sigue viva (verificado con el proceso
  del ejecutable ya instalado).
- Desinstalacion real (`msiexec /x`): exit 0, no queda la carpeta de
  instalacion, ni el acceso directo, ni la entrada de desinstalacion.

Closes #55
