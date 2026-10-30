using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class OficinaPrivada : Recurso
{
    // Reglas de negocio del precio diario de la oficina privada.
    private const decimal RecargoPorMetros = 0.20m;
    private const decimal MetrosSinRecargo = 20m;

    public decimal MetrosCuadrados { get; set; }

    public OficinaPrivada(int id, int idTipoRecurso, string nombre, string ubicacion,decimal costo, decimal precioDia, decimal metrosCuadrados, EstadoRecurso estado)
        : base(id, idTipoRecurso, nombre, ubicacion,costo , precioDia, estado)
    {
        MetrosCuadrados = metrosCuadrados;
    }

    /// <summary>
    /// Precio por dia de la oficina privada. Parte del costo base y suma un
    /// 20% si supera los 20 m2.
    /// </summary>
    public override decimal CalcularPrecio()
    {
        decimal precio = Costo;

        if (MetrosCuadrados > MetrosSinRecargo)
            precio *= 1m + RecargoPorMetros;

        return Math.Round(precio, 2);
    }
}