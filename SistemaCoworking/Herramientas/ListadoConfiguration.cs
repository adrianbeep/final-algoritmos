namespace SistemaCoworking.Herramientas;

/// <summary>
/// Utilidades para los listados en grilla: cargar los datos, detectar las
/// columnas de accion y leer el id de la fila.
/// </summary>
public static class ListadoConfiguration
{
    /// <summary>
    /// Carga la grilla con los datos que devuelve <paramref name="origen"/>.
    /// La excepcion se maneja aca y no se propaga: el error se muestra en
    /// un MessageBox y la grilla queda como estaba.
    /// </summary>
    public static void CargarGrilla<T>(
        DataGridView dgv,
        Func<List<T>> origen,
        string mensajeError)
    {
        try
        {
            dgv.DataSource = null;
            dgv.DataSource = origen();
            dgv.ClearSelection();
        }
        catch (Exception ex)
        {
            MessageBox.Show(mensajeError + ": " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// True si la columna es de tipo boton (las de editar o cancelar).
    /// Si el indice esta fuera de rango, devuelve false.
    /// </summary>
    public static bool EsColumnaDeAccion(DataGridView dgv, int indiceColumna)
    {
        if (indiceColumna < 0 || indiceColumna >= dgv.Columns.Count)
            return false;

        return dgv.Columns[indiceColumna] is DataGridViewButtonColumn;
    }

    /// <summary>
    /// Id de la fila, leido de la columna <paramref name="nombreColumnaId"/>.
    /// Devuelve 0 si el id no se puede leer: la lectura no debe romper el
    /// click en la grilla.
    /// </summary>
    public static int IdDeLaFila(DataGridView dgv, int indiceFila, string nombreColumnaId)
    {
        try
        {
            if (indiceFila < 0 || indiceFila >= dgv.Rows.Count)
                return 0;

            if (!dgv.Columns.Contains(nombreColumnaId))
                return 0;

            object? valor = dgv.Rows[indiceFila].Cells[nombreColumnaId].Value;

            if (valor is int id)
                return id;

            return int.TryParse(valor?.ToString(), out int idParseado) ? idParseado : 0;
        }
        catch (Exception)
        {
            // Ningun error al leer el id puede romper el click en la grilla.
            return 0;
        }
    }
}
