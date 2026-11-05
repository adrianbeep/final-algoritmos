using System;

namespace SistemaCoworking.Modelos;

/// <summary>
/// Fabrica unica de recursos. Es el UNICO lugar del sistema que sabe que
/// "Sala" se construye como <see cref="Sala"/>, "PuestoTrabajo" como
/// <see cref="PuestoTrabajo"/> y "OficinaPrivada" como <see cref="OficinaPrivada"/>.
///
/// Vive en el DOMINIO y no en la capa de datos porque hay dos caminos que
/// necesitan crear el recurso concreto:
///
/// - la capa de datos, que convierte una fila de la base en un objeto;
/// - el formulario de recursos, que convierte lo que tipeo el usuario en un objeto.
///
/// Si la fabrica viviera en el DAO, el formulario no podria usarla sin depender
/// de la capa de datos, y el "switch" que decide la clase concreta quedaria
/// duplicado en los dos caminos.
///
/// La regla de precio de cada tipo NO va aca: vive en <see cref="Recurso.CalcularPrecio"/>,
/// que es polimorfismo del modelo. Esta clase solo decide que clase se construye.
/// </summary>
public static class RecursoFactory
{
    /// <summary>
    /// Crea el recurso concreto que corresponde al tipo indicado.
    /// Los datos especificos del tipo que no aplican se mandan en null.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Si el tipo no existe o si falta el dato obligatorio del tipo.
    /// </exception>
    public static Recurso Crear(
        string nombreTipo,
        int id,
        int idTipoRecurso,
        string nombre,
        string ubicacion,
        decimal costo,
        Recurso.EstadoRecurso estado,
        int? capacidadPersonas = null,
        bool? tieneProyector = null,
        int? numeroMonitor = null,
        decimal? metrosCuadrados = null)
    {
        switch (nombreTipo)
        {
            case "Sala":
                if (capacidadPersonas is null)
                {
                    throw new InvalidOperationException(
                        "Una Sala requiere la capacidad de personas.");
                }

                return new Sala(
                    id, idTipoRecurso, nombre, ubicacion, costo,
                    capacidadPersonas.Value, tieneProyector ?? false, estado);

            case "PuestoTrabajo":
                if (numeroMonitor is null)
                {
                    throw new InvalidOperationException(
                        "Un PuestoTrabajo requiere la cantidad de monitores.");
                }

                return new PuestoTrabajo(
                    id, idTipoRecurso, nombre, ubicacion, costo,
                    numeroMonitor.Value, estado);

            case "OficinaPrivada":
                if (metrosCuadrados is null)
                {
                    throw new InvalidOperationException(
                        "Una OficinaPrivada requiere los metros cuadrados.");
                }

                return new OficinaPrivada(
                    id, idTipoRecurso, nombre, ubicacion, costo,
                    metrosCuadrados.Value, estado);

            default:
                throw new InvalidOperationException(
                    $"Tipo de recurso desconocido: '{nombreTipo}'");
        }
    }
}
