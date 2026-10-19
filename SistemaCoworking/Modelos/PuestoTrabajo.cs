using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class PuestoTrabajo : Recurso
{
    public int NumeroMonitor { get; set; }

    public PuestoTrabajo(int id, int idTipoRecurso, string nombre, string ubicacion, EstadoRecurso estado, decimal precioDia, int numeroMonitor)
        : base(id, idTipoRecurso, nombre, ubicacion, estado, precioDia)
    {
        NumeroMonitor = numeroMonitor;
    }
}