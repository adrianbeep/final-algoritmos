# [Etapa 2] Clases del dominio (POO)

## Objetivo

Implementar la jerarquia de clases del dominio: clase abstracta `Recurso` con 3 clases hijas, clase `TipoRecurso` y la interfaz `IReservable`. Cumplir los requisitos de POO de la catedra (herencia, polimorfismo, clase abstracta, interfaz con sentido).

## Entregables

### Clase `TipoRecurso`

- [ ] Atributos: `Id` (int), `Nombre` (string), `Descripcion` (string)
- [ ] Solo lectura de datos por ahora (el DAO en Etapa 5 hara el alta/modificacion)

### Clase abstracta `Recurso`

- [ ] Atributos comunes: `Id`, `IdTipoRecurso`, `Nombre`, `Ubicacion`, `Estado`, `PrecioDia`
- [ ] Marcada como `abstract`
- [ ] Metodo abstracto `decimal CalcularCosto(int dias)`
- [ ] Metodo abstracto `bool PuedeReservarse(DateTime fecha)`

### Clases derivadas

- [ ] `Sala : Recurso` con `CapacidadPersonas` (int) y `TieneProyector` (bool)
- [ ] `PuestoTrabajo : Recurso` con `NumeroMonitor` (int)
- [ ] `OficinaPrivada : Recurso` con `MetrosCuadrados` (decimal)

### Logica de `CalcularCosto` por tipo

- [ ] `Sala.CalcularCosto(dias)` → aplica 10% de descuento (`PrecioDia * dias * 0.9`)
- [ ] `PuestoTrabajo.CalcularCosto(dias)` → precio base (`PrecioDia * dias`)
- [ ] `OficinaPrivada.CalcularCosto(dias)` → aplica 10% de recargo (`PrecioDia * dias * 1.1`)

### Logica de `PuedeReservarse`

- [ ] Implementacion basica: retorna `true` si `Estado == "Disponible"`, `false` en otro caso
- [ ] Se puede extender mas adelante si la catedra lo requiere

### Interfaz `IReservable`

- [ ] Metodos: `bool PuedeReservarse(DateTime fecha)` y `decimal CalcularCosto(int dias)`
- [ ] `Recurso` implementa `IReservable` indirectamente a traves de los metodos abstractos

## Criterio de pase

- [ ] Las 5 clases compilan sin errores
- [ ] Se puede instanciar `Sala`, `PuestoTrabajo` y `OficinaPrivada` directamente
- [ ] NO se puede instanciar `Recurso` directamente (es abstracta)
- [ ] El polimorfismo funciona: una variable de tipo `Recurso` puede contener cualquier clase derivada
- [ ] `CalcularCosto` da resultados distintos segun el tipo (ej: sala de 100/dia por 5 dias = 450, oficina misma = 550)
- [ ] `PuedeReservarse` retorna segun el estado

## Archivos a crear

- `SistemaCoworking/Models/TipoRecurso.cs`
- `SistemaCoworking/Models/Recurso.cs` (abstracta)
- `SistemaCoworking/Models/Sala.cs`
- `SistemaCoworking/Models/PuestoTrabajo.cs`
- `SistemaCoworking/Models/OficinaPrivada.cs`
- `SistemaCoworking/Models/IReservable.cs`

## Justificacion de las reglas de negocio (para la defensa)

El polimorfismo con `CalcularCosto` no es solo un requisito tecnico: refleja la realidad del dominio. Un coworking cobra distinto por tipo de recurso (salas con descuento para grupos, oficinas con recargo por privacidad, puestos precio base). Esto se modela con polimorfismo porque cada tipo TIENE una regla distinta. La catedra pide polimorfismo "con sentido" — esto lo es.

## Nota

Las clases NO se conectan a la base de datos todavia. Eso es la Etapa 3 (integracion) y la Etapa 5 (DAO). Por ahora las clases se pueden usar con datos hardcodeados para probar.
