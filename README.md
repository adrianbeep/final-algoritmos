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
| .NET 8.0 | Runtime |
| MySQL | Base de datos relacional |
| Visual Studio 2022 | IDE de desarrollo |
| MySql.Data | Conector de base de datos |
| Git + GitHub | Control de versiones |
| GitHub Projects | Tablero Kanban de seguimiento |

---

## Desarrolladores

- **Adrian Naum** (legajo)
- **Valentina Lesspade** (legajo)
- **Santiago Castillo** (legajo)

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
│   ├── DER.md                      Diagrama Entidad-Relación
│   ├── Diagrama-clases.md          Diagrama de clases
│   └── Decisiones.md               Decisiones de diseño
├── Scripts SQL/                    Scripts de base de datos
│   └── Coworking_db.sql
└── SistemaCoworking/               Proyecto C# (Windows Forms)
    └── [archivos del .sln y el proyecto]
```

---

## Cómo clonar y ejecutar

### Requisitos previos

- .NET 8.0 SDK
- MySQL Server 8.0 o superior
- Visual Studio 2022 con workload ".NET desktop development"
- MySQL Workbench
- Git

### Clonar el repositorio

```bash
git clone https://github.com/adrianbeep/final-algoritmos.git
cd final-algoritmos
```

### Crear la base de datos

1. Abrir MySQL Workbench y conectarse al servidor local
2. Ejecutar el script completo: `Scripts SQL/Coworking_db.sql`
3. Verificar que se creó la base `coworking` con las tablas y datos iniciales

### Abrir el proyecto

1. Abrir Visual Studio 2022
2. Abrir la solución: `SistemaCoworking/SistemaCoworking.sln`
3. Compilar la solución (`Ctrl + Shift + B`)
4. Ejecutar con `F5`

> Nota: mientras no esté implementada la Etapa 6 (configuración externa), la cadena de conexión se configura directamente en la clase `ConexionBD`.

---

## Documentación

- [Roadmap completo](./.github/ROADMAP.md) — plan de trabajo por etapas
- [Documentación técnica](./Docs/) — DER, diagrama de clases, decisiones de diseño
- [Consignas del TP](./Consignas/) — requerimientos originales de la cátedra
- [Issues abiertos](https://github.com/adrianbeep/final-algoritmos/issues) — sub-tareas en curso

---

## Licencia

Este proyecto está bajo la licencia MIT. Ver el archivo [LICENSE](./LICENSE) para más detalles.
