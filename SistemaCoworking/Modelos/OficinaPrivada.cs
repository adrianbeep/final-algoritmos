using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class OficinaPrivada : Recurso
{
    public decimal MetrosCuadrados { get; set; }

    public OficinaPrivada(int id, int idTipoRecurso, string nombre, string ubicacion,decimal costo, decimal precioDia, decimal metrosCuadrados)
        : base(id, idTipoRecurso, nombre, ubicacion,costo , precioDia)
    {
        MetrosCuadrados = metrosCuadrados;
    }

    public override decimal CalcularCosto(int dias)
    {
        decimal costoBase = PrecioDia * dias * 1.1m; // 10% adicional para oficinas privadas
        if (MetrosCuadrados > 20)
        {
            costoBase += 30 * dias; 
        }
        return costoBase;
    }
}