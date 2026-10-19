using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class OficinaPrivada : Recurso
{
    public decimal MetrosCuadrados { get; set; }

    public OficinaPrivada(int id, int idTipoRecurso, string nombre, string ubicacion, EstadoRecurso estado, decimal precioDia, decimal metrosCuadrados)
        : base(id, idTipoRecurso, nombre, ubicacion, estado, precioDia)
    {
        MetrosCuadrados = metrosCuadrados;
    }

    public override decimal OficinaPrivada.CalcularCosto(int dias)
    {
        decimal costoBase = PrecioDia * dias * 1.1m; // 10% adicional para oficinas privadas
        if (MetrosCuadrados > 20)
        {
            costoBase += 30 * dias; 
        }
        return costoBase;
    }
}