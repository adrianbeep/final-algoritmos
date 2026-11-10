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
    /// La cantidad de dias que ocupa la reserva. El sistema reserva por dias,
    /// no por horarios: una reserva con la misma fecha de inicio y de fin
    /// ocupa UN dia completo. El conteo es inclusivo, asi que del 10/03 al
    /// 12/03 son 3 dias.
    /// </summary>
    public int DiasReservados => Math.Max(0, (FechaFin.Date - FechaInicio.Date).Days + 1);

    /// <summary>
    /// El total de la reserva: el precio por dia del recurso por la cantidad
    /// de dias reservados. El precio por dia sale del polimorfismo, de
    /// <see cref="Recurso.CalcularPrecio"/>.
    /// </summary>
    public decimal CalcularTotal(Recurso recurso) => Math.Round(recurso.PrecioDia * DiasReservados, 2);

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
    /// La fecha de fin no puede ser anterior a la de inicio. Se comparan solo
    /// las fechas: el sistema reserva por dias, asi que la misma fecha de
    /// inicio y de fin es una reserva de un dia completo.
    /// </summary>
    public static bool ValidarFechas(DateTime fechaInicio, DateTime fechaFin, out string? mensajeError)
    {
        if (fechaFin.Date < fechaInicio.Date)
        {
            mensajeError = "La fecha de fin no puede ser anterior a la fecha de inicio.";
            return false;
        }

        mensajeError = null;
        return true;
    }
}
