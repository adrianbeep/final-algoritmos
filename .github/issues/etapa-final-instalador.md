# [Etapa Final] El instalador de la aplicacion

## Objetivo

El proyecto de instalador de Visual Studio (`.vdproj`) no empaquetaba la
aplicacion: el MSI que salia de ahi contenia solo `SistemaCoworking.dll` y el
`runtimeconfig.json`. Le faltaban el ejecutable, las dependencias
(MySqlConnector, ReaLTaiizor, Microsoft.Extensions.*) y el `appsettings.json`,
asi que lo que se instalaba no arrancaba.

Ademas, el tipo de proyecto "Setup and Deployment" no esta instalado en Visual
Studio 2026, y el MSBuild de la IDE tampoco puede cargar un `.vdproj`: el
proyecto era un camino sin salida.

El instalador se rehace con WiX, que es lo que Microsoft usa hoy para .NET, y
que se compila desde la terminal sin depender de la IDE.

## Entregables

- [ ] El instalador empaqueta el ejecutable, las dependencias, el
      `appsettings.json` y el runtime de .NET
- [ ] La publicacion es self-contained: en la PC donde se instala no hace
      falta tener .NET
- [ ] El instalador no pide permisos de administrador
- [ ] Deja un acceso directo en el menu de Inicio
- [ ] Se desinstala limpiando los archivos, el acceso directo y la entrada de
      desinstalacion
- [ ] El proyecto del instalador queda en el repo y se arma con un comando
- [ ] El README explica como generarlo

## Criterio de pase

- [ ] El MSI se genera con `Instalador\build-installer.ps1`
- [ ] Al instalarlo, la aplicacion arranca
- [ ] Al desinstalarlo, no queda nada de la app en la maquina

## Fuera de alcance

- [ ] Un instalador por maquina (en Program Files): ese necesita permisos de
      administrador y en las PCs de la facultad falla con el error 1925
- [ ] Publicar el MSI como release de GitHub

## Nota

El paquete se deja como `perUser` a proposito: en Program Files el instalador
falla con "Error 1925" si el usuario no es administrador, y en las PCs de la
facultad pasa seguido. Para una entrega que se instala en cualquier
computadora, es la diferencia entre que funcione y que no.

WiX queda en la version 5.0.2 porque las versiones 6 y 7 exigen aceptar la
EULA del "OSMF fee", que es un cargo por mantenimiento.

El `.vdproj` y el MSI que habian salido de ahi se borran: no se pueden
compilar y el MSI no instala una aplicacion que funcione.
