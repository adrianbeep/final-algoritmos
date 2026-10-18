# Diagrama Entidad-Relacion (DER)

## Diagrama

```mermaid
erDiagram
    clientes {
        int id PK
        varchar nombre
        varchar apellido
        int dni UK
        varchar telefono
        varchar email UK
    }

    tipo_recursos {
        int id PK
        varchar nombre_tipo_recurso
        text descripcion
    }

    recursos {
        int id PK
        int id_tipo_recurso FK
        varchar nombre
        enum estado
        varchar ubicacion
        decimal precio_dia
    }

    recursos_detalle {
        int id_recurso PK,FK
        int capacidad_personas "NULL - solo Sala"
        boolean tiene_proyector "NULL - solo Sala"
        int numero_monitor "NULL - solo PuestoTrabajo"
        int metros_cuadrados "NULL - solo OficinaPrivada"
    }

    reservas {
        int id PK
        int id_cliente FK
        int id_recurso FK
        datetime fecha_inicio
        datetime fecha_fin
        enum estado_reserva
        decimal total_reserva
    }

    auditoria_reservas {
        int id PK
        int id_reserva FK
        enum accion
        datetime fecha_accion
        text detalle
    }

    clientes ||--o{ reservas : "realiza"
    tipo_recursos ||--o{ recursos : "clasifica"
    recursos ||--o{ reservas : "es reservado en"
    recursos ||--|| recursos_detalle : "tiene detalles"
    reservas ||--o{ auditoria_reservas : "es auditada en"
```

## Relaciones

| Origen | Cardinalidad | Destino | Descripcion |
|---|---|---|---|
| clientes | 1 a N | reservas | Un cliente puede tener muchas reservas |
| tipo_recursos | 1 a N | recursos | Un tipo clasifica muchos recursos |
| recursos | 1 a N | reservas | Un recurso puede tener muchas reservas en el tiempo |
| recursos | 1 a 1 | recursos_detalle | Un recurso tiene un detalle (1:1) con campos especificos por tipo |
| reservas | 1 a N | auditoria_reservas | Una reserva genera multiples entradas de auditoria (insert, cancel) |

## Decisiones de diseno

### Tabla `recursos_detalle` separada

En lugar de poner todos los campos en `recursos` (lo que generaria redundancia y NULLs en todas las filas), se creo una tabla 1:1 que guarda solo los campos especificos de cada tipo de recurso:

- **Sala**: `capacidad_personas`, `tiene_proyector`
- **PuestoTrabajo**: `numero_monitor`
- **OficinaPrivada**: `metros_cuadrados`

Justificacion: la FK desde `reservas` apunta a `recursos` (no a tres tablas separadas), el JOIN del DAO en C# es simple, y la validacion de campos obligatorios por tipo se hace en los SPs.

### Tabla `auditoria_reservas` separada

Independiente de la tabla principal de reservas, registra automaticamente (via trigger) las operaciones de alta y cancelacion. Sirve para tener un historial sin afectar la performance de las consultas de reservas.

### Estados como ENUM

Los campos `recursos.estado`, `reservas.estado_reserva` y `auditoria_reservas.accion` son ENUM para restringir los valores validos a nivel de schema. Esto evita inserciones con estados invalidos.

## Cardinalidad completa

- `clientes` 1 --- 0..N `reservas` (un cliente puede no tener reservas o tener muchas)
- `tipo_recursos` 1 --- 0..N `recursos` (un tipo puede no tener recursos asignados, pero normalmente tendra varios)
- `recursos` 1 --- 0..N `reservas` (un recurso puede no estar reservado nunca, o tener muchas reservas en distintos momentos)
- `recursos` 1 --- 1 `recursos_detalle` (1:1 estricto, todo recurso tiene su detalle)
- `reservas` 1 --- 0..N `auditoria_reservas` (una reserva puede tener 0 auditorias si fue creada antes de los triggers, o varias si fue creada y luego cancelada)
