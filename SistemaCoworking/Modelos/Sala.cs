    using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class Sala : Recurso
{
    public int CapacidadPersonas { get; set; }
    public bool TieneProyector { get; set; }

    public Sala(int id, int idTipoRecurso, string nombre, string ubicacion, decimal costo, decimal precioDia, int capacidadPersonas, bool tieneProyector)
        : base(id, idTipoRecurso, nombre, ubicacion, costo,  precioDia)
    {
        CapacidadPersonas = capacidadPersonas;
        TieneProyector = tieneProyector;
    }

    public override decimal CalcularCosto(int dias)
    {
        decimal costoBase = PrecioDia * dias;
        if (TieneProyector)
        {
            costoBase += 50 * dias; 
        }
        return costoBase;
    }
}

