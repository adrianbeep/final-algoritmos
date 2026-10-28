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
    public decimal Costo { get; set; }
    public decimal PrecioDia { get; set; }


    public Recurso(int  id, int idTipoRecurso, string nombre, string ubicacion, decimal costo, decimal precioDia)
    {
        Id = id;
        IdTipoRecurso = idTipoRecurso;
        Nombre = nombre;
        Ubicacion = ubicacion;
        Costo = costo;
        PrecioDia = precioDia;
        Estado = EstadoRecurso.Disponible;
    }

    /// <summary>
    /// Precio por dia del recurso. Cada tipo implementa sus propias reglas de
    /// descuento y recargo sobre el costo base.
    /// </summary>
    public abstract decimal CalcularPrecio();

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

    // Solo validamos datos que realmente tengan sentido
    public static bool ValidarNombre(string nombre, out string? mensajeError)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            mensajeError = "Ingrese un nombre.";
            return false;
        }

        if (nombre.Length > 100)
        {
            mensajeError = "Ingrese un nombre menor a 100 caracteres";
            return false;
        }

        mensajeError = null;
        return true;
    }
}