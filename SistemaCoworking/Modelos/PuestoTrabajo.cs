using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class PuestoTrabajo : Recurso
{
    // Cada monitor adicional al primero suma un recargo al precio diario.
    private const decimal RecargoPorMonitor = 0.10m;

    public int NumeroMonitor { get; set; }

    public PuestoTrabajo(int id, int idTipoRecurso, string nombre, string ubicacion,decimal costo, decimal precioDia, int numeroMonitor, EstadoRecurso estado)
        : base(id, idTipoRecurso, nombre, ubicacion, costo, precioDia, estado)
    {
        NumeroMonitor = numeroMonitor;
    }

    /// <summary>
    /// Precio por dia del puesto de trabajo. Parte del costo base (el puesto
    /// no lleva recargo por tipo) y suma un 10% por cada monitor adicional
    /// al primero.
    /// </summary>
    public override decimal CalcularPrecio()
    {
        decimal precio = Costo;

        int monitoresExtra = Math.Max(0, NumeroMonitor - 1);
        if (monitoresExtra > 0)
            precio *= 1m + (RecargoPorMonitor * monitoresExtra);

        return Math.Round(precio, 2);
    }
}