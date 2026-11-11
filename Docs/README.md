# Documentación técnica

Documentación del Sistema de Gestión de Coworking y Reservas. Este archivo es el
índice: desde acá se llega a todos los documentos.

| Documento | Qué contiene | Requerimiento que cubre |
|---|---|---|
| [Diccionario-datos.md](./Diccionario-datos.md) | Las 6 tablas campo por campo (tipo, precisión, nulabilidad, default, PK, FK, UNIQUE, CHECK, descripción funcional y quién escribe el dato), las 2 vistas, las 6 funciones, los 7 procedimientos almacenados y los 2 triggers | "Breve documentación técnica del sistema" |
| [DER.md](./DER.md) | Diagrama Entidad-Relación de la base: tablas, cardinalidades, valores de los ENUM y decisiones de esquema | "Diagrama del modelo de datos" |
| [Diagrama-clases.md](./Diagrama-clases.md) | Diagrama de clases simplificado del dominio: la jerarquía `Recurso`, la clase abstracta, la interfaz `IReservable`, las entidades y las reglas de negocio | "Diagrama de clases simplificado", "clases principales" y "uso de herencia, abstractas e interfaces" |
| [DC.md](./DC.md) | Diagrama de contexto: el sistema completo como un proceso (nivel 0) y la operación principal "gestionar una reserva" | Documentación del análisis funcional |
| [DFD.md](./DFD.md) | DFD de nivel 1 con los cuatro procesos, los almacenes D1 a D5 y los flujos, más los diagramas de flujo de las operaciones de escritura | Documentación del análisis funcional |
| [Decisiones.md](./Decisiones.md) | Registro de decisiones de diseño: qué se decidió, por qué y qué alternativa se descartó | "Decisiones importantes tomadas por el grupo" |

### Documentos en preparación

- **Documentación técnica mínima** — objetivo del sistema, modelo de datos, clases
  principales, uso de herencia, abstractas e interfaces, objetos de MySQL
  utilizados y por qué, manejo de errores y decisiones del grupo.
- **Manual de usuario** — recorrido completo de la aplicación, con capturas reales.

## Documentos de la cátedra

En [`Consignas/`](../Consignas/):

- **`Consignas Propuesta de la Catedra.html`** — la consigna del TP Final: objetivo, escenario, etapas, entregables y criterios de evaluación.
- **`Pautas de Presentacion.html`** — el formato que debe cumplir el documento final: portada obligatoria, A4, márgenes de 2,54 cm, Arial 11, interlineado 1,5, texto justificado, índice, páginas numeradas, referencias APA 7 y el nombre sugerido del PDF.

## Base de datos

Los scripts están en [`Scripts SQL/`](../Scripts%20SQL/) y se ejecutan en el
orden indicado en el [README principal](../README.md#crear-la-base-de-datos):

```
Coworking_db.sql  →  Vistas.sql  →  Funciones.sql  →  Procedimientos_Almacenados.sql  →  Triggers.sql
```

Los cinco scripts incluyen `USE coworking;`: se pueden correr uno detrás de otro
sin seleccionar el esquema a mano.

## Código

El proyecto .NET está en [`SistemaCoworking/`](../SistemaCoworking/):

- `Modelos/` — el dominio: `Cliente`, `Recurso` (abstracta) y sus tres derivadas, `Reserva`, `TipoRecurso`, `DetalleTipo` y `RecursoFactory`.
- `Interfaces/` — `IReservable`.
- `Datos/` — los DAO y `ConexionBD`: acceso a MySQL, sin reglas de negocio.
- `Formularios/` — las pantallas de Windows Forms.
- `Herramientas/` — helpers de appearance y de filtrado de la entrada.
