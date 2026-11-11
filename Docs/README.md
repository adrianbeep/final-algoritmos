# Documentación técnica

Documentación del Sistema de Coworking. Cada documento responde a un
requerimiento de la consigna del TP Final.

| Documento | Qué contiene | Requerimiento de la consigna que cubre |
|---|---|---|
| [DER.md](./DER.md) | Diagrama Entidad-Relación de la base: tablas, cardinalidades y decisiones de esquema | "Diagrama del modelo de datos" |
| [Diagrama-clases.md](./Diagrama-clases.md) | Diagrama de clases simplificado del dominio: la jerarquía `Recurso`, la clase abstracta, la interfaz `IReservable`, las entidades y las reglas de negocio | "Diagrama de clases simplificado", "clases principales" y "uso de herencia, abstractas e interfaces" |
| [DC.md](./DC.md) | Diagrama de contexto (nivel 0): el sistema entero como un proceso, las entidades externas y los flujos | Documentación del análisis funcional |
| [DFD.md](./DFD.md) | DFD de nivel 1: los cuatro procesos del sistema, los almacenes de datos y los flujos entre ellos | Documentación del análisis funcional |
| [Decisiones.md](./Decisiones.md) | Registro de decisiones de diseño: qué se decidió, por qué y qué alternativa se descartó | "Decisiones importantes tomadas por el grupo" |

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
