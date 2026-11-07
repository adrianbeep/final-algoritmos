using System.Drawing;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// El velo oscuro y la ventana del modal, para una ventana principal.
///
/// SON TRES VENTANAS EN TOTAL:
///   1. la ventana principal (FrmPrincipal)
///   2. el velo: el dim oscuro, con Opacity
///   3. la ventana del modal: la tarjeta, opaca, que contiene el UserControl
///
/// POR QUE EL MODAL VA EN OTRA VENTANA Y NO DENTRO DEL VELO:
/// Un Form con Opacity es una ventana layered (WS_EX_LAYERED) y ahi el
/// compositor del sistema mezcla contra todo lo que hay detras. Los
/// controles hijos de un Form con Opacity heredan esa opacidad: si el modal
/// fuera hijo del velo, se veria translucido y se leeria la grilla por
/// detras. Por eso el modal va en su propia ventana opaca.
///
/// POR QUE NO HAY ESTADO GLOBAL:
/// El velo pertenece a una ventana principal, no a la aplicacion. Antes
/// vivia en campos static: dos ventanas principales distintas compartian el
/// mismo velo y la misma ventana de modal. Ahora cada ventana principal
/// tiene su propio ModalOverlay.
/// </summary>
public sealed class ModalOverlay : IDisposable
{
    /// <summary>Opacidad del velo (0.0 a 1.0). 0.55 = se ve la app a traves.</summary>
    public const double DefaultOpacity = 0.55;

    private readonly Form _owner;
    private readonly Form _velo;

    private Form? _ventana;
    private UserControl? _modal;
    private bool _dispose;

    /// <summary>El velo: el dim oscuro.</summary>
    public Form Velo => _velo;

    /// <summary>
    /// Crea el velo como ventana propia, clockeada al owner.
    /// </summary>
    public ModalOverlay(Form owner, double opacidad = DefaultOpacity)
    {
        _owner = owner;

        _velo = new Form
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

        // Handlers con nombre, no lambdas: asi se pueden desuscribir al
        // destruir, y el owner no queda con el velo colgado.
        owner.Resize += OwnerCambioDeTamano;
        owner.Move += OwnerCambioDeTamano;
    }

    /// <summary>
    /// Abre el modal y espera a que se cierre. Devuelve el DialogResult:
    /// <see cref="DialogResult.OK"/> si el usuario guardo, y otra cosa si
    /// cancelo.
    ///
    /// El modal se cierra solo, con
    /// <see cref="ModalConfiguration.CerrarModal"/>: el que decide el
    /// resultado es el modal, no el form.
    /// </summary>
    public DialogResult Mostrar(UserControl modal)
    {
        CerrarVentana();

        // El owner todavia no estaba posicionado cuando se creo el velo,
        // asi que hay que resincronizar los bounds antes de centrar.
        _velo.Bounds = _owner.Bounds;

        var ventana = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            StartPosition = FormStartPosition.Manual,
            ShowInTaskbar = false,
            MinimizeBox = false,
            MaximizeBox = false,
            BackColor = modal.BackColor,
            Size = modal.Size,

            // El Owner de la ventana del modal es la ventana principal, NO
            // el velo. Medido en .NET 10: si la ventana del modal tiene por
            // Owner una ventana que ya es propiedad de la principal, el
            // ShowDialog(owner) tira "ArgumentException: Referencia
            // circular del control".
            Owner = _owner,

            // Un CLON de la Region del modal, no el Region del modal: el
            // modal libera el suyo en su resize y no debe dejar muerta la
            // Region de esta ventana.
            Region = ModalConfiguration.RegionParaLaVentana(modal)
        };

        modal.Location = Point.Empty;
        ventana.Controls.Add(modal);

        // Si el modal cambia de tamaño, la ventana sigue su misma forma.
        ModalConfiguration.SincronizarRegion(modal, ventana);

        _ventana = ventana;
        _modal = modal;

        _velo.Show();
        _velo.BringToFront();
        Centrar(ventana, modal);

        try
        {
            // ShowDialog() sin parametro. Medido: el ShowDialog no
            // deshabilita por su cuenta al Owner de la propiedad; el
            // Owner de la ventana del modal es la ventana principal, y al
            // deshabilitarla se deshabilita tambien el velo y toda la app
            // queda sin responder.
            return ventana.ShowDialog();
        }
        finally
        {
            // El velo se baja siempre, pase lo que pase. Si la escritura
            // en la base revienta, la aplicacion no puede quedar con el
            // dim clavado.
            _velo.Hide();
            CerrarVentana();
        }
    }

    /// <summary>
    /// Cierra la ventana del modal y destruye el velo. La ventana principal
    /// deberia llamar a esto cuando se cierra.
    /// </summary>
    public void Dispose()
    {
        if (_dispose) return;
        _dispose = true;

        _owner.Resize -= OwnerCambioDeTamano;
        _owner.Move -= OwnerCambioDeTamano;

        CerrarVentana();

        _velo.Close();
        _velo.Dispose();
    }

    private void OwnerCambioDeTamano(object? sender, EventArgs e)
    {
        Recolocar();
    }

    /// <summary>
    /// Cierra y libera la ventana del modal, si hay una.
    /// </summary>
    private void CerrarVentana()
    {
        if (_ventana is null) return;

        _ventana.Close();
        _ventana.Dispose();

        _ventana = null;
        _modal = null;
    }

    /// <summary>
    /// Centra la ventana del modal sobre el velo.
    /// </summary>
    private void Centrar(Form ventana, UserControl modal)
    {
        ventana.Bounds = new Rectangle(
            _velo.Left + (_velo.Width - modal.Width) / 2,
            _velo.Top + (_velo.Height - modal.Height) / 2,
            modal.Width,
            modal.Height);
    }

    /// <summary>
    /// El velo sigue a la ventana principal y la ventana del modal se
    /// vuelve a centrar.
    /// </summary>
    private void Recolocar()
    {
        if (_ventana is null || _modal is null) return;

        _velo.Bounds = _owner.Bounds;
        Centrar(_ventana, _modal);
    }
}
