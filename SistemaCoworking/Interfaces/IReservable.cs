using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Interfaces;

public interface IReservable
{
    public  bool PuedeReservarse(DateTime fechaInicio, DateTime fechaFin);

    public decimal CalcularPrecio();
}