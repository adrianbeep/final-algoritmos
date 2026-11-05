namespace SistemaCoworking.Modelos;

/// <summary>
/// Datos especificos de un tipo de recurso, con la forma que espera la base:
/// las columnas que no aplican al tipo van en null.
///
/// Cada clase derivada sabe cuales son los suyos, asi que la capa de datos
/// puede mandarlos al procedimiento almacenado sin conocer las clases
/// concretas: solo escribe lo que el recurso declara.
///
/// No guarda nada: es una proyeccion del recurso, no un segundo lugar donde
/// vivir el dato.
/// </summary>
public class DetalleTipo
{
    public int? CapacidadPersonas { get; }
    public bool? TieneProyector { get; }
    public int? NumeroMonitor { get; }
    public decimal? MetrosCuadrados { get; }

    public DetalleTipo(
        int? capacidadPersonas = null,
        bool? tieneProyector = null,
        int? numeroMonitor = null,
        decimal? metrosCuadrados = null)
    {
        CapacidadPersonas = capacidadPersonas;
        TieneProyector = tieneProyector;
        NumeroMonitor = numeroMonitor;
        MetrosCuadrados = metrosCuadrados;
    }
}
