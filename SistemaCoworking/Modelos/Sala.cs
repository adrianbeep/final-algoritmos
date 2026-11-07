    using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class Sala : Recurso
{
    // Reglas de negocio del precio diario de la sala.
    private const decimal RecargoProyector = 0.25m;

    public int CapacidadPersonas { get; set; }
    public bool TieneProyector { get; set; }

    /// <summary>
    /// Valida la capacidad de personas de la sala.
    /// </summary>
    public static bool ValidarCapacidad(int capacidad, out string? mensajeError)
    {
        if (capacidad <= 0)
        {
            mensajeError = "La capacidad de personas debe ser mayor a cero.";
            return false;
        }

        mensajeError = null;
        return true;
    }

    /// <summary>
    /// Datos especificos de la sala. Los de los otros tipos van en null.
    /// </summary>
    public override DetalleTipo Detalle =>
        new DetalleTipo(
            capacidadPersonas: CapacidadPersonas,
            tieneProyector: TieneProyector);

    public Sala(int id, int idTipoRecurso, string nombre, string ubicacion, decimal costo, int capacidadPersonas, bool tieneProyector, EstadoRecurso estado)
        : base(id, idTipoRecurso, nombre, ubicacion, costo, estado)
    {
        CapacidadPersonas = capacidadPersonas;
        TieneProyector = tieneProyector;
    }

    /// <summary>
    /// Precio por dia de la sala. Parte del costo base y suma un recargo del
    /// 25% si la sala incluye proyector.
    /// </summary>
    public override decimal CalcularPrecio()
    {
        decimal precio = Costo;

        if (TieneProyector)
            precio *= 1m + RecargoProyector;

        return Math.Round(precio, 2);
    }
}

