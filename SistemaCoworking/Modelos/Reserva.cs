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
}