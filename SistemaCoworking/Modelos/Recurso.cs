using System;
using System.Collections.Generic;
using System.Text;
using SistemaCoworking.Interfaces;

namespace SistemaCoworking.Modelos;

public abstract class Recurso : IReservable
{
    public int Id { get; set; }
    public int IdTipoRecurso { get; set; }
    public string Nombre { get; set; }
    public string Ubicacion { get; set; } 

    public enum EstadoRecurso
    {
        Disponible,
        Ocupado,
        Mantenimiento
    }
    public EstadoRecurso Estado { get; set; }
    public decimal PrecioDia { get; set; }


    public Recurso(int  id, int idTipoRecurso, string nombre, string ubicacion, decimal precioDia)
    {
        Id = id;
        IdTipoRecurso = idTipoRecurso;
        Nombre = nombre;
        Ubicacion = ubicacion;
        PrecioDia = precioDia;
        Estado = EstadoRecurso.Disponible;

    }

    public  abstract decimal CalcularCosto(int dias);

    public virtual bool PuedeReservarse(DateTime fechaInicio, DateTime fechaFin)
    {
        if (Estado != EstadoRecurso.Disponible)
        {
            return false;
        }
        if (fechaInicio >= fechaFin)
        {
            return false;
        }
        return true;
    }

}
