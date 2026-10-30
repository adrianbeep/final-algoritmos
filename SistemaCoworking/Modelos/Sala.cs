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

    public Sala(int id, int idTipoRecurso, string nombre, string ubicacion, decimal costo, decimal precioDia, int capacidadPersonas, bool tieneProyector, EstadoRecurso estado)
        : base(id, idTipoRecurso, nombre, ubicacion, costo,  precioDia, estado)
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

