using System.Drawing;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Abre un modal en su propia ventana y espera a que se cierre.
///
/// SON DOS VENTANAS: la principal de la app y la del modal. No hay velo.
///
/// POR QUE NO HAY VELO:
/// El velo con Opacity es una ventana layered (WS_EX_LAYERED), y los
/// controles hijos de un Form con Opacity heredan esa opacidad: si el modal
/// fuera hijo del velo, la tarjeta se veria translucida y se leeria la
/// grilla por detras. Por eso el velo necesita su propia ventana, y el
/// modal otra: tres ventanas para un modal.
///
/// Sin velo no hace falta nada de eso. La ventana del modal lleva la
/// tarjeta y se abre con ShowDialog(), que deshabilita la ventana
/// principal de verdad (medido con IsWindowEnabled: queda DESHABILITADA, y
/// un click de mouse no llega al boton de la app). La app no responde
/// mientras el modal esta abierto; lo que se pierde es el oscurecido.
///
/// POR QUE LA VENTANA DEL MODAL NO NECESITA OWNER:
/// Medido: ShowDialog() sin parametro ya deshabilita la ventana principal.
/// Con Owner el resultado es el mismo. Entonces el form no tiene que
/// pasarle nada: el modal se ancla solo sobre la ventana principal.
///
/// ESC: lo engancha ModalConfiguration.ConectarEsc con KeyPreview de esta
/// ventana, asi que ESC cierra el modal aunque el foco este en un campo de
/// texto.
/// </summary>
public static class ModalDialog
{
    /// <summary>
    /// Abre el modal y devuelve su DialogResult: OK si el usuario guardo,
    /// otra cosa si cancelo.
    ///
    /// <paramref name="ancla"/> es un control cualquiera de la ventana donde
    /// se abre el modal: de ahi se saca la ventana principal para centrarlo.
    /// </summary>
    public static DialogResult Mostrar(UserControl modal, Control ancla)
    {
        var ventana = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            StartPosition = FormStartPosition.Manual,
            ShowInTaskbar = false,
            MinimizeBox = false,
            MaximizeBox = false,
            BackColor = modal.BackColor,
            Size = modal.Size
        };

        modal.Location = Point.Empty;
        ventana.Controls.Add(modal);

        // La ventana toma la misma forma redondeada que el modal, pero con
        // su propio Region (un clon): el modal libera el suyo en su resize
        // y no debe dejar muerta la Region de la ventana.
        ModalConfiguration.SincronizarRegion(modal, ventana);

        Centrar(ventana, modal, ancla);

        try
        {
            return ventana.ShowDialog();
        }
        finally
        {
            ventana.Close();
            ventana.Dispose();
        }
    }

    /// <summary>
    /// Centra la ventana del modal sobre la ventana principal.
    /// </summary>
    private static void Centrar(Form ventana, UserControl modal, Control ancla)
    {
        Rectangle area = VentanaPrincipal(ancla)?.Bounds
                         ?? Screen.PrimaryScreen?.Bounds
                         ?? new Rectangle(0, 0, 800, 600);

        ventana.Bounds = new Rectangle(
            area.Left + (area.Width - modal.Width) / 2,
            area.Top + (area.Height - modal.Height) / 2,
            modal.Width,
            modal.Height);
    }

    /// <summary>
    /// La ventana de nivel superior que contiene al control. El modal se
    /// centra sobre ella, no sobre el panel embebido: los forms embebidos
    /// tienen TopLevel = false y no pueden ser ventanas principales.
    /// </summary>
    private static Form? VentanaPrincipal(Control control)
    {
        Control? actual = control;

        while (actual is not null)
        {
            if (actual is Form form && form.TopLevel) return form;
            actual = actual.Parent;
        }

        return null;
    }
}
