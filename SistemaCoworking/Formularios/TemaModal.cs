using SistemaCoworking.Formularios.UserControls;
using SistemaCoworking.Herramientas;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios;

/// <summary>
/// Aplica la paleta de <see cref="Paleta"/> a los controles de un modal.
///
/// Se hace en codigo y no en el Designer por dos razones.
///
/// La primera: el Designer deja el fondo de los TextControl en blanco, y ese
/// blanco es el mismo del DataGridView que tiene debajo del modal. Si el
/// Designer volviera a ponerlo, el modal volveria a mezclarse con la grilla.
///
/// La segunda: el recorrido se detiene en cada TextControl. Adentro del control
/// hay una barra que es la linea de abajo del campo, y un panel con el icono
/// de error: si el recorrido bajara adentro, los pondria del color del modal y
/// el campo perderia su linea.
///
/// Los combos y los botones ya tienen su propio estilo
/// (<see cref="ComboBoxConfiguration"/> y <see cref="ButtonConfiguration"/>), asi
/// que aca no se tocan.
/// </summary>
public static class TemaModal
{
    /// <summary>
    /// Pinta el modal con la paleta de la aplicacion.
    /// </summary>
    public static void Aplicar(Form modal)
    {
        Recorrer(modal);
    }

    private static void Recorrer(Control control)
    {
        switch (control)
        {
            case TextControl campo:
                // El campo no es una caja: es una etiqueta, una linea y el texto.
                // Comparte el fondo del modal y se diferencia por la linea.
                campo.BackColor = Paleta.FondoModal;
                return;

            case ComboBox combo:
                combo.BackColor = Paleta.FondoCampo;
                combo.ForeColor = Paleta.Texto;
                break;

            case DateTimePicker fecha:
                fecha.BackColor = Paleta.FondoCampo;
                fecha.ForeColor = Paleta.Texto;
                break;

            case CheckBox check:
                check.BackColor = Paleta.FondoModal;
                check.ForeColor = Paleta.TextoEtiqueta;
                break;

            case Label etiqueta:
                // Las etiquetas de valor (el precio calculado, por ejemplo) las
                // deja el Designer con fondo blanco: sobre el gris del modal
                // se veria una caja blanca en medio del formulario.
                if (etiqueta.BackColor == Color.White)
                {
                    etiqueta.BackColor = Paleta.FondoModal;
                }
                break;

            case Panel panel:
                panel.BackColor = Paleta.FondoModal;
                break;
        }

        foreach (Control hijo in control.Controls)
        {
            Recorrer(hijo);
        }
    }
}
