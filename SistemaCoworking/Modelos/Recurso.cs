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
        EnMantenimiento,
        Inhabilitado
    }
    public EstadoRecurso Estado { get; set; }
    public decimal Costo { get; set; }

    /// <summary>
    /// Precio por dia del recurso. Es un valor DERIVADO y no un dato guardado:
    /// la unica fuente de verdad es <see cref="CalcularPrecio"/>, que aplica el
    /// recargo propio de cada tipo sobre <see cref="Costo"/>. Guardarlo como
    /// atributo permitia que el precio quedara desincronizado del costo base.
    /// </summary>
    public decimal PrecioDia => CalcularPrecio();

    /// <summary>
    /// Datos especificos del tipo, con la forma que espera la base. Por
    /// defecto no hay datos especificos; cada clase derivada devuelve los
    /// suyos. Es lo que permite que la capa de datos mande los parametros del
    /// detalle al procedimiento almacenado sin conocer las clases concretas.
    /// </summary>
    public virtual DetalleTipo Detalle => new DetalleTipo();


    public Recurso(int  id, int idTipoRecurso, string nombre, string ubicacion, decimal costo, EstadoRecurso estado)
    {
        Id = id;
        IdTipoRecurso = idTipoRecurso;
        Nombre = nombre;
        Ubicacion = ubicacion;
        Costo = costo;
        Estado = estado;
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