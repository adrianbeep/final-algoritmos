using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class PuestoTrabajo : Recurso
{
    public int NumeroMonitor { get; set; }

    public PuestoTrabajo(int id, int idTipoRecurso, string nombre, string ubicacion, decimal precioDia, int numeroMonitor)
        : base(id, idTipoRecurso, nombre, ubicacion, precioDia)
    {
        NumeroMonitor = numeroMonitor;
    }

    public override decimal CalcularCosto(int dias)
    {
        decimal costoBase = PrecioDia * dias;
        if (NumeroMonitor > 1)
        {
            costoBase += 20 * dias;
        }
        return costoBase;
    }
}