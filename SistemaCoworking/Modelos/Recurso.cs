using System;
using System.Collections.Generic;
using System.Globalization;
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

    /// <summary>
    /// Valida la ubicacion del recurso.
    /// </summary>
    public static bool ValidarUbicacion(string ubicacion, out string? mensajeError)
    {
        if (string.IsNullOrEmpty(ubicacion))
        {
            mensajeError = "Ingrese la ubicacion.";
            return false;
        }

        if (ubicacion.Length > 100)
        {
            mensajeError = "Ingrese una ubicacion menor a 100 caracteres";
            return false;
        }

        mensajeError = null;
        return true;
    }

    /// <summary>
    /// Convierte el texto del campo de costo a decimal.
    ///
    /// Se saca el separador de miles de la cultura y se parsea con el
    /// separador decimal de la cultura: en es-AR "1.500" son mil quinientos y
    /// "1,50" es uno con cincuenta. No se aceptan separadores de miles en el
    /// numero en si: "1500" es mil quinientos.
    /// </summary>
    public static bool TryParseCosto(string texto, out decimal costo)
    {
        costo = 0m;

        if (string.IsNullOrWhiteSpace(texto))
            return false;

        NumberFormatInfo formato = CultureInfo.CurrentCulture.NumberFormat;
        string normalizado = texto.Trim();

        if (formato.NumberGroupSeparator != formato.NumberDecimalSeparator)
        {
            normalizado = normalizado.Replace(
                formato.NumberGroupSeparator.ToString(),
                string.Empty);
        }

        return decimal.TryParse(
            normalizado,
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
            CultureInfo.CurrentCulture,
            out costo);
    }

    /// <summary>
    /// Valida el costo base del recurso.
    /// </summary>
    public static bool ValidarCosto(decimal costo, out string? mensajeError)
    {
        if (costo <= 0)
        {
            mensajeError = "El costo debe ser mayor a cero.";
            return false;
        }

        mensajeError = null;
        return true;
    }
}