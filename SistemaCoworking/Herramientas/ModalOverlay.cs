using System.Drawing;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Velo oscuro para modales.
///
/// POR QUE UN FORM Y NO UN PANEL:
/// Un Panel hijo con BackColor alfa NO compone con sus controles hermanos en
/// WinForms: el blend ocurre contra el BackColor del Form padre, no contra lo
/// dibujado debajo. Medido: sidebar 23,24,29 -> 118 y DGV 255,255,255 -> 118,
/// mismo valor. Un Form con Opacity usa ventana layered (WS_EX_LAYERED) y ahi
/// el compositor del SI mezcla contra todo lo que hay detras.
///
/// POR QUE EL MODAL VA EN OTRA VENTANA:
/// Los controles hijos de un Form con Opacity heredan esa opacidad. Si el modal
/// fuera hijo del velo, se veria translucido (se leeria el DGV por detras).
/// Por eso el modal va en su propia ventana opaca, con Owner = velo.
/// </summary>
public static class ModalOverlay
{
    /// <summary>Opacidad del velo (0.0 a 1.0). 0.55 = se ve la app a traves.</summary>
    public const double DefaultOpacity = 0.55;

    private static Form? _vevo;
    private static Form? _ventanaModal;

    /// <summary>Crea el velo como ventana propia, clockeada al owner, y lo devuelve.</summary>
    public static Form Configure(Form owner, double opacidad = DefaultOpacity)
    {
        Destruir();

        var vevo = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            StartPosition = FormStartPosition.Manual,
            ShowInTaskbar = false,
            MinimizeBox = false,
            MaximizeBox = false,
            BackColor = Color.Black,
            Opacity = opacidad,
            Bounds = owner.Bounds,
            Owner = owner
        };

        _vevo = vevo;

        owner.Resize += (s, e) => Recolocar();
        owner.Move += (s, e) => Recolocar();

        return vevo;
    }

    /// <summary>Muestra el modal centrado, en su propia ventana opaca.</summary>
    public static void Show(Form overlay, UserControl modal)
    {
        CerrarVentanaModal();

        // El owner todavia no estaba posicionado cuando se creo el velo,
        // asi que hay que resincronizar los bounds antes de centrar.
        if (overlay.Owner is not null)
            overlay.Bounds = overlay.Owner.Bounds;

        var ventana = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            StartPosition = FormStartPosition.Manual,
            ShowInTaskbar = false,
            MinimizeBox = false,
            MaximizeBox = false,
            BackColor = modal.BackColor,
            Size = modal.Size,
            Owner = overlay,
            // Hereda la forma del modal: sin esto el fondo blanco del host
            // pinta un rectangulo y tapa las esquinas redondeadas.
            Region = modal.Region
        };

        modal.Location = Point.Empty;
        ventana.Controls.Add(modal);
        _ventanaModal = ventana;

        overlay.Show();
        overlay.BringToFront();

        CentrarModal(overlay, ventana, modal);
        ventana.Show();
        ventana.BringToFront();
        modal.Focus();
    }

    public static void Close(Form overlay)
    {
        CerrarVentanaModal();
        overlay.Hide();
    }

    private static void CentrarModal(Form overlay, Form ventana, UserControl modal)
    {
        ventana.Bounds = new Rectangle(
            overlay.Left + (overlay.Width - modal.Width) / 2,
            overlay.Top + (overlay.Height - modal.Height) / 2,
            modal.Width,
            modal.Height
        );
    }

    private static void Recolocar()
    {
        if (_vevo is null || _ventanaModal is null || _ventanaModal.Controls.Count == 0)
            return;

        var modal = _ventanaModal.Controls[0] as UserControl;
        if (modal is null) return;

        _vevo.Bounds = _vevo.Owner?.Bounds ?? _vevo.Bounds;
        CentrarModal(_vevo, _ventanaModal, modal);
    }

    private static void CerrarVentanaModal()
    {
        if (_ventanaModal is null) return;
        _ventanaModal.Close();
        _ventanaModal.Dispose();
        _ventanaModal = null;
    }

    private static void Destruir()
    {
        CerrarVentanaModal();
        if (_vevo is null) return;
        _vevo.Close();
        _vevo.Dispose();
        _vevo = null;
    }
}