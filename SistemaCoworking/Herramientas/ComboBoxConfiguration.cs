using System.Drawing;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Estilo de los combos de la aplicacion.
///
/// Es lo unico que hace: darle el mismo aspecto que al resto de los
/// controles. Cargar los items es cosa del que abre el combo, no del
/// estilo.
///
/// Para que el combo muestre un texto y guarde un id, se bindea:
///
/// <code>
/// combo.DisplayMember = nameof(Cliente.NombreCompleto);
/// combo.ValueMember = nameof(Cliente.Id);
/// combo.DataSource = clientes;
/// </code>
///
/// Con eso, lo elegido se lee con <c>combo.SelectedValue</c> (el id).
/// </summary>
public static class ComboBoxConfiguration
{
    private static readonly Color TextoPrimario = Paleta.Texto;
    private static readonly Color Fondo = Paleta.FondoCampo;

    // Se comparte una sola instancia, como en ButtonConfiguration e
    // InputConfiguration: cada Font nuevo es un handle de GDI que hay que
    // liberar.
    private static readonly Font ComboFont = new Font("Segoe UI", 9.5F);

    /// <summary>
    /// Deja el combo con el estilo de la aplicacion: sin bordes, la misma
    /// fuente que los campos de texto, fondo blanco y el color de la app.
    /// </summary>
    public static void Configuracion(ComboBox combo)
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
