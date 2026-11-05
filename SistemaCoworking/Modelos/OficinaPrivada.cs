using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class OficinaPrivada : Recurso
{
    // Reglas de negocio del precio diario de la oficina privada.
    private const decimal RecargoPorMetros = 0.20m;
    private const decimal MetrosSinRecargo = 20m;

    public decimal MetrosCuadrados { get; set; }

    /// <summary>
    /// Datos especificos de la oficina privada. Los de los otros tipos van en null.
    /// </summary>
    public override DetalleTipo Detalle =>
        new DetalleTipo(metrosCuadrados: MetrosCuadrados);

    /// <summary>
    /// Valida los metros cuadrados. La columna metros_cuadrados de la base es
    /// entera, asi que un valor con decimales no se puede guardar sin perder
    /// precision: se rechaza en vez de truncar en silencio.
    /// </summary>
    public static bool ValidarMetrosCuadrados(decimal metros, out string? mensajeError)
    {
        if (metros <= 0)
        {
            mensajeError = "Ingrese los metros cuadrados.";
            return false;
        }

        if (metros != decimal.Truncate(metros))
        {
            mensajeError = "Los metros cuadrados deben ser un numero entero.";
            return false;
        }

        mensajeError = null;
        return true;
    }

    public OficinaPrivada(int id, int idTipoRecurso, string nombre, string ubicacion, decimal costo, decimal metrosCuadrados, EstadoRecurso estado)
        : base(id, idTipoRecurso, nombre, ubicacion, costo, estado)
    {
        MetrosCuadrados = metrosCuadrados;
    }

    /// <summary>
    /// Precio por dia de la oficina privada. Parte del costo base y suma un
    /// 20% si supera los 20 m2.
    /// </summary>
    public override decimal CalcularPrecio()
    {
        decimal precio = Costo;

        if (MetrosCuadrados > MetrosSinRecargo)
            precio *= 1m + RecargoPorMetros;

        return Math.Round(precio, 2);
    }
}