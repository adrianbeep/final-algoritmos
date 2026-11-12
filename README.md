# Sistema Coworking

> Sistema de Gestión de Coworking y Reservas

Aplicación de escritorio para administrar un espacio de coworking, sus recursos (salas de reunión, puestos de trabajo, oficinas privadas) y las reservas realizadas por los clientes. Es un trabajo práctico final integrador de las materias **Algoritmos II**, **Bases de Datos** y **Prácticas Profesionalizantes II** del Instituto Argentino de la Empresa, ciclo lectivo 2026.

---

## Descripción del problema

Un espacio de coworking dispone de distintos recursos que pueden ser utilizados por clientes mediante reservas. El sistema permite:

- Registrar y administrar los recursos del coworking
- Administrar los clientes
- Generar reservas con control de superposiciones
- Consultar disponibilidad y estado de las reservas
- Visualizar métricas operativas en un dashboard

El sistema impide situaciones inconsistentes como reservar un recurso inexistente, reservar un recurso no disponible, superponer reservas para el mismo recurso, registrar datos inválidos o eliminar información necesaria para operaciones existentes.

---

## Tecnologías

| Tecnología | Uso |
|---|---|
| C# | Lenguaje principal |
| Windows Forms | Framework de interfaz gráfica |
| .NET 10.0 | Runtime |
| MySQL o MariaDB | Base de datos relacional |
| Visual Studio 2026 | IDE de desarrollo |
| MySqlConnector | Conector de base de datos |
| Git + GitHub | Control de versiones |
| GitHub Projects | Tablero Kanban de seguimiento |

---

## Desarrolladores

- **Adrian Naum**
- **Valentina Lespade**
- **Santiago Castillo**

**Docente a cargo:** Prof. Lic. Adrián Aroca

---

## Información del proyecto

- **Inicio de desarrollo:** 16/10/2026
- **Ciclo lectivo:** 2026
- **Carrera:** Analista de Sistemas de Información
- **Institución:** Instituto Argentino de la Empresa
- **Asignatura:** Algoritmos II

---

## Estructura del repositorio

```
final-algoritmos/
├── README.md                       Este archivo
├── LICENSE                         Licencia del proyecto (MIT)
├── .gitignore                      Archivos ignorados por Git
├── .github/
│   ├── ROADMAP.md                  Roadmap completo del proyecto
│   └── issues/                     Templates de sub-issues
├── Consignas/                      Requerimientos del TP
│   ├── Consignas Propuesta de la Catedra.html
│   └── Pautas de Presentacion.html
├── Docs/                           Documentación técnica
│   ├── README.md                   Índice de la documentación
│   ├── DC.md                       Diagrama de contexto
│   ├── DFD.md                      DFD de nivel 1 y flujos de escritura
│   ├── DER.md                      Diagrama Entidad-Relación
│   ├── Diagrama-clases.md          Diagrama de clases
│   ├── Decisiones.md               Decisiones de diseño
│   ├── Diccionario-datos.md        Diccionario de datos de la base
│   └── catalogo-clientes.json      Ejemplo de exportación JSON
├── Instalador/                     Empaquetado del instalador (WiX)
│   ├── build-installer.ps1         Script que arma el MSI
│   ├── Instalador.wixproj          Proyecto de WiX
│   └── Product.wxs                 Paquete del instalador
├── Scripts SQL/                    Scripts de base de datos
│   ├── Coworking_db.sql            Base, tablas y datos iniciales
│   ├── Vistas.sql                  Las 2 vistas
│   ├── Funciones.sql               Las 6 funciones
│   ├── Procedimientos_Almacenados.sql  Los 7 procedimientos almacenados
│   └── Triggers.sql                Los 2 triggers de auditoría
├── Instalador/                     El instalador de la aplicación (WiX)
│   ├── Product.wxs                 Qué se empaqueta y cómo se instala
│   ├── Instalador.wixproj          Proyecto de WiX que arma el MSI
│   └── build-installer.ps1          Publica la app y genera el MSI
└── SistemaCoworking/               Proyecto C# (Windows Forms)
    └── [archivos del .slnx y el proyecto]
```

---

## Cómo clonar y ejecutar

### Requisitos previos

- .NET 10.0 SDK
- MySQL Server 8.0 o superior
- Visual Studio 2026 con workload ".NET desktop development"
- MySQL Workbench
- Git

> **Nota:** el proyecto apunta a `net10.0-windows` y la solution es un `.slnx`, el formato que genera Visual Studio 2026. Visual Studio 2022 no abre un `.slnx` ni compila contra .NET 10. Si tenés VS 2022, actualizá a VS 2026, o compilé con el SDK de .NET 10 desde la terminal: `dotnet build SistemaCoworking/SistemaCoworking.csproj`.

### Clonar el repositorio

```bash
git clone https://github.com/adrianbeep/final-algoritmos.git
cd final-algoritmos
```

### Crear la base de datos

La aplicación **usa procedimientos almacenados, vistas y funciones**: no alcanza con crear las tablas. Hay que ejecutar los cinco scripts de `Scripts SQL/`, en este orden:

| # | Script | Qué crea |
|---|---|---|
| 1 | `Coworking_db.sql` | La base `coworking`, las 6 tablas y los datos iniciales |
| 2 | `Vistas.sql` | `vw_reservas_completas`, `vw_recursos_estado` |
| 3 | `Funciones.sql` | Las 6 funciones `fn_*` |
| 4 | `Procedimientos_Almacenados.sql` | Los 7 procedimientos `sp_*` |
| 5 | `Triggers.sql` | Los 2 triggers de auditoría |

> Los cinco scripts incluyen `USE coworking;`, así que se pueden correr uno detrás de otro en cualquier cliente (línea de comandos o Workbench) sin tener que seleccionar el esquema a mano.

Pasos:

1. Abrir MySQL Workbench y conectarse al servidor local
2. Ejecutar `Scripts SQL/Coworking_db.sql`
3. Seleccionar la base `coworking` como esquema por defecto
4. Ejecutar `Vistas.sql`, `Funciones.sql`, `Procedimientos_Almacenados.sql` y `Triggers.sql`, en ese orden
5. Verificar que quedaron los 7 procedimientos, las 2 vistas, las 6 funciones y los 2 triggers:

```sql
SHOW PROCEDURE STATUS WHERE Db = 'coworking';
SHOW FULL TABLES WHERE Db = 'coworking';
```

### Abrir el proyecto

1. Abrir Visual Studio 2026
2. Abrir la solución: `SistemaCoworking/SistemaCoworking.slnx`
3. Compilar la solución (`Ctrl + Shift + B`)
4. Ejecutar con `F5`

> **Cadena de conexión:** la configuración es **externa**. `ConexionBD` lee el archivo `appsettings.json` (copiado automáticamente al directorio de salida por el `.csproj`) mediante `Microsoft.Extensions.Configuration`. Para apuntar a otro servidor o cambiar el usuario, se edita `SistemaCoworking/appsettings.json`:
>
> ```json
> {
>   "ConnectionStrings": {
>     "Default": "server=localhost;port=3306;database=coworking;uid=root;pwd=;"
>   }
> }
> ```
>
> El usuario y la contraseña por defecto son `root` y vacío. Si tu MySQL local tiene otra contraseña, cambiala acá antes de ejecutar.

## El instalador

La aplicación se puede instalar en cualquier PC con Windows, **sin tener que instalar
.NET ni MySQL antes**: el instalador es *self-contained* y lleva adentro el runtime
y todas las dependencias.

**Dónde está el MSI:**

```
Instalador\output\Instalador.msi
```

Ese archivo es el que se entrega y el que se abre con doble clic para instalar.
Se instala por usuario, en `%LOCALAPPDATA%\Programs\Sistema Coworking`, así que
**no pide permisos de administrador** y funciona en cualquier equipo, incluso en
los de la facultad donde el usuario no es administrador. Deja además un acceso
directo en el Menú de Inicio.

Para desinstalarlo: Menú de Inicio → "Sistema Coworking" → Desinstalar, o

```powershell
msiexec /x "%LOCALAPPDATA%\Programs\Sistema Coworking\Instalador.msi"
```

**Cómo se recompila** (por si hay que regenerarlo después de un cambio):

```powershell
.\Instalador\build-installer.ps1
```

El script hace dos pasos: un `dotnet publish` en *self-contained* que deja la
aplicación y el runtime de .NET en `Instalador\publish\`, y después el
empaquetado con WiX, que produce el MSI en `Instalador\output\`. La carpeta
`Instalador\publish\` es una salida de compilación: se puede borrar y
regenerar sin perder nada.

> **Ojo con la versión:** el número de versión del paquete está en
> `Instalador\Product.wxs` (`Version="1.0.2"`). Cada vez que se genera un MSI
> nuevo hay que **subirla**, porque si se recompila con la misma versión que la
> que ya está instalada, Windows Installer no reemplaza los archivos y la
> aplicación instalada sigue siendo la anterior.

> Las carpetas `Instalador\publish\` e `Instalador\obj\` están en el `.gitignore`:
> son salidas de compilación y no van al repositorio. El `.msi` tampoco se versiona,
> se entrega como archivo aparte.

### Generar el instalador

El instalador es un proyecto de [WiX](https://wixtoolset.org/) que vive en `Instalador/`. Se arma con un script, sin necesidad de la IDE:

```powershell
powershell -ExecutionPolicy Bypass -File Instalador\build-installer.ps1
```

El script hace dos pasos: publica la aplicación en **self-contained** (se lleva el runtime de .NET adentro, así que en la PC donde se instala no hace falta tener .NET) y después arma el MSI con esa carpeta adentro.

El instalador queda en `Instalador/output/Instalador.msi` (unos 45 MB) y se instala con doble clic, o:

```powershell
msiexec /i "Instalador\output\Instalador.msi"
```

Qué hace:

- Instala en `%LOCALAPPDATA%\Programs\Sistema Coworking`. **No pide permisos de administrador**, para que funcione también en las PCs donde el usuario no es admin.
- Deja un acceso directo en el menú de Inicio.
- Se desinstala desde *Configuración → Aplicaciones* o con `msiexec /x`.

> **Nota:** el MSI es una salida de compilación, así que no está en el repo. Se regenera con el script de arriba. La app instalada lee la cadena de conexión del `appsettings.json` que queda junto al ejecutable; para apuntar a otro servidor, se edita ese archivo en la carpeta de instalación.

---

## Documentación

- [Roadmap completo](./.github/ROADMAP.md) — plan de trabajo por etapas
- [Documentación técnica](./Docs/) — índice de la documentación
- [Diccionario de datos](./Docs/Diccionario-datos.md) — tablas, vistas, funciones, procedimientos y triggers, campo por campo
- [Diagrama Entidad-Relación](./Docs/DER.md) — modelo de datos de la base
- [Diagrama de clases](./Docs/Diagrama-clases.md) — modelo de dominio: herencia, clase abstracta e interfaz
- [Diagrama de contexto](./Docs/DC.md) — el sistema completo y la operación principal
- [DFD y flujos de las operaciones](./Docs/DFD.md) — los procesos del sistema y el recorrido de cada escritura
- [Decisiones de diseño](./Docs/Decisiones.md) - qué se decidió, por qué y qué alternativa se descartó
- [Consignas del TP](./Consignas/) — requerimientos originales de la cátedra
- [Issues abiertos](https://github.com/adrianbeep/final-algoritmos/issues) — sub-tareas en curso

> La documentación técnica mínima y el manual de usuario con capturas se entregan
> en el documento final de la cátedra (PDF). Los archivos de `Docs/` son el
> respaldo técnico en Markdown.

---

## Licencia

Este proyecto está bajo la licencia MIT. Ver el archivo [LICENSE](./LICENSE) para más detalles.
