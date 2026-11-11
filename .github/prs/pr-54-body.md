## Contexto

Issue #53 - [Etapa 2] Los modales pasan a su propia carpeta.

En `Formularios/UserControls/` conviven el `TextControl`, que si es un UserControl,
con los tres modales, que son Forms. La carpeta miente sobre lo que hay adentro y
el namespace `SistemaCoworking.Formularios.UserControls` dice lo mismo.

## Que incluye

**La carpeta `Formularios/Modales/`**
- `ClienteModal`, `RecursoModal` y `ReservaModal` pasan de `Formularios/UserControls/`
  a `Formularios/Modales/`, con su `.Designer.cs` y su `.resx`.
- Los tres declaran el namespace `SistemaCoworking.Formularios.Modales`.
- Los que usan el `TextControl` (`ClienteModal` y `RecursoModal`) importan
  `SistemaCoworking.Formularios.UserControls`. El `ReservaModal` no: sus campos
  son dos combos y dos selectores de fecha.

**La carpeta `Formularios/UserControls/`**
- Queda solo el `TextControl`, que si es un UserControl.

**Los tres formularios**
- `FrmClientes`, `FrmRecursos` y `FrmReservas` importan el namespace nuevo en
  lugar del viejo.

## Fuera de alcance

- No se renombra nada ni se cambia la forma de abrir los modales: solo se mueven
  de carpeta.
- No se mueven los UserControls a otra carpeta.

## Verificacion

- `dotnet build SistemaCoworking\SistemaCoworking.csproj -t:Rebuild`: compilacion
  correcta, 0 errores y 7 advertencias preexistentes (las mismas de antes del
  cambio: CS8625 en `FrmPrincipal` y CS8618 en `TextControl`).
- El cambio es atomico: el movimiento de los archivos y el de los namespaces van
  en el mismo commit, asi que el proyecto compila en cada commit de la rama.

Closes #53
