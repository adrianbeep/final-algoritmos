using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Interfaces;

public interface IReservable
{
    public  bool PuedeReservarse(DateTime fechaInicio, DateTime fechaFin);

    /// <summary>
    /// Precio por dia del recurso, ya con los descuentos y recargos propios
    /// de su tipo. El total de una reserva es este valor por la cantidad de
    /// dias reservados.
    /// </summary>
    public decimal CalcularPrecio();
}