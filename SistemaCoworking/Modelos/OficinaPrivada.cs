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

    public override decimal CalcularPrecio()
    {
        
    }
}