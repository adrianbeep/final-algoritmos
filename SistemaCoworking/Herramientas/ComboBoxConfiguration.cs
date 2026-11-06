using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Herramientas;

public static class ComboBoxConfiguration
{
    private static readonly Color TextoPrimario = Color.FromArgb(40, 40, 40);
    private static readonly Color Fondo = Color.White;

    // Se comparte una sola instancia, como en ButtonConfiguration e
    // InputConfiguration: cada Font nuevo es un handle de GDI que hay que
    // liberar.
    private static readonly Font ComboFont = new Font("Segoe UI", 9.5F);

    /// <summary>
    /// Carga el combo con los estados del recurso.
    /// Los nombres del enum son los mismos valores que usa el enum de MySQL en
    /// recursos.estado, asi que el item se puede mandar al procedimiento
    /// almacenado tal cual, sin traducir.
    /// </summary>
    public static void ConfigEstados(
        ComboBox combo,
        Recurso.EstadoRecurso? seleccionado = null)
    {
        AplicarEstilo(combo);

        combo.Items.Clear();
        combo.Items.AddRange(
            Enum.GetValues<Recurso.EstadoRecurso>().Cast<object>().ToArray());

        combo.SelectedItem = seleccionado ?? Recurso.EstadoRecurso.Disponible;
    }

    /// <summary>
    /// Carga el combo con los tipos de recurso. Cada item es el TipoRecurso
    /// completo: el combo muestra el nombre y el SelectedItem sigue siendo el
    /// objeto, asi que el Id se lee sin andar buscando por indice.
    /// </summary>
    public static void ConfigTipos(
        ComboBox combo,
        List<TipoRecurso> tipos,
        TipoRecurso? seleccionado = null)
    {
        AplicarEstilo(combo);

        combo.Items.Clear();
        foreach (TipoRecurso tipo in tipos)
        {
            combo.Items.Add(tipo);
        }

        combo.SelectedItem =
            seleccionado ?? (tipos.Count > 0 ? tipos[0] : null);
    }

    /// <summary>
    /// Estado seleccionado en el combo de estados.
    /// </summary>
    public static Recurso.EstadoRecurso EstadoSeleccionado(ComboBox combo)
    {
        return combo.SelectedItem is Recurso.EstadoRecurso estado
            ? estado
            : Recurso.EstadoRecurso.Disponible;
    }

    /// <summary>
    /// Tipo de recurso seleccionado en el combo de tipos, o null si no hay
    /// ninguno cargado.
    /// </summary>
    public static TipoRecurso? TipoSeleccionado(ComboBox combo)
    {
        return combo.SelectedItem as TipoRecurso;
    }

    private static void AplicarEstilo(ComboBox combo)
    {
        // Lista desplegable sin edicion: el usuario elige, no escribe.
        combo.DropDownStyle = ComboBoxStyle.DropDownList;

        // La misma fuente que usa el TextControl, para que el combo se vea
        // igual que los campos de texto.
        combo.Font = ComboFont;
        combo.BackColor = Fondo;
        combo.ForeColor = TextoPrimario;
    }
}
