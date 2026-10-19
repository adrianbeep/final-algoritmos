using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Interfaces;

public interface IReservable
{
    public  bool PuedeReservarse(dateTime fechaInicio, DateTime fechaFin);
    
}