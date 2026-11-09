using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class Reserva
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public int IdRecurso { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    
    public enum EstadoReserva
    {
        Confirmada,
        Cancelada
    }
    public EstadoReserva Estado { get; set; }
    public decimal TotalReserva { get; set; }

    public Reserva(int id, int idCliente, int idRecurso, DateTime fechaInicio, DateTime fechaFin, EstadoReserva estadoReserva, decimal totalReserva)
    {
        Id = id;
        IdCliente = idCliente;
        IdRecurso = idRecurso;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        Estado = estadoReserva;
        TotalReserva = totalReserva;
    }

    /// <summary>
    /// La reserva tiene que ser de un cliente.
    /// </summary>
    public static bool ValidarCliente(Cliente? cliente, out string? mensajeError)
    {
        if (cliente is null)
        {
            mensajeError = "Elija un cliente.";
            return false;
        }

        mensajeError = null;
        return true;
    }

    /// <summary>
    /// La reserva tiene que ser de un recurso, y el recurso tiene que estar
    /// disponible: si no, el procedimiento almacenado lo rechaza.
    /// </summary>
    public static bool ValidarRecurso(Recurso? recurso, out string? mensajeError)
    {
        if (recurso is null)
        {
            mensajeError = "Elija un recurso.";
            return false;
        }

        if (recurso.Estado != Recurso.EstadoRecurso.Disponible)
        {
            mensajeError = "El recurso no está disponible.";
            return false;
        }

        mensajeError = null;
        return true;
    }

    /// <summary>
    /// La fecha de fin tiene que ser posterior a la de inicio.
    /// </summary>
    public static bool ValidarFechas(DateTime fechaInicio, DateTime fechaFin, out string? mensajeError)
    {
        if (fechaFin <= fechaInicio)
        {
            mensajeError = "La fecha de fin debe ser posterior a la fecha de inicio.";
            return false;
        }

        mensajeError = null;
        return true;
    }
}
