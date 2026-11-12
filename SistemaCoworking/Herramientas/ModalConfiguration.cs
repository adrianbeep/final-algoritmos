using System.Drawing.Drawing2D;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Configuracion comun de los formularios que se muestran como modal.
///
/// UNA SOLA LLAMADA, en el constructor del modal:
///
/// <code>
/// ModalConfiguration.ConfigModal(this, btnCancelar, pBoxCerrar);
/// </code>
///
/// deja el modal con el estilo de tarjeta, las esquinas redondeadas, y el
/// boton Cancelar, la `X` y la tecla ESC haciendo lo mismo: cerrar el modal
/// sin guardar. Para guardar, el modal arma la entidad y cierra con
/// <see cref="CerrarModal"/>.
///
/// El modal es un <see cref="Form"/>: no necesita ninguna ventana host, ni
/// un `Owner`, ni un helper para abrirlo. Se abre con
/// <c>modal.ShowDialog()</c> y el form lee el <see cref="DialogResult"/>.
/// </summary>
public static class ModalConfiguration
{
    private static readonly Color TextoPrimario = Paleta.Texto;
    private static readonly Color Fondo = Paleta.FondoModal;

    // Radio de las esquinas redondeadas, en pixeles.
    private const int RadioEsquina = 14;

    // Se comparte una sola instancia, como en ButtonConfiguration e
    // InputConfiguration: cada Font nuevo es un handle de GDI que hay que
    // liberar.
    private static readonly Font ModalFont = new Font("Segoe UI", 9F);

    /// <summary>
    /// Configura el modal completo. Esta es la unica llamada que hace el
    /// modal.
    /// </summary>
    /// <param name="modal">El formulario del modal.</param>
    /// <param name="btnCancelar">El boton que cancela.</param>
    /// <param name="pboxCerrar">La `X` de cerrar, si tiene.</param>
    public static void ConfigModal(
        Form modal,
        Button btnCancelar,
        PictureBox? pboxCerrar = null)
    {
        // El Designer fijo el Size de un UserControl. En un Form, el Size es
        // el de la ventana entera con bordes: sin bordes, el area de cliente
        // es la ventana, asi que el tamano se pasa a ClientSize para que el
        // modal mida lo que deberia.
        Size tamano = modal.Size;

        modal.FormBorderStyle = FormBorderStyle.None;
        modal.StartPosition = FormStartPosition.CenterScreen;
        modal.ShowInTaskbar = false;
        modal.ClientSize = tamano;

        // Fondo opaco: sin esto el modal se transparentaria con la app.
        modal.BackColor = Fondo;
        modal.ForeColor = TextoPrimario;
        modal.Font = ModalFont;

        AplicarRegionRedondeada(modal);

        // Si el modal cambia de tamaño, el recorte se rehace.
        modal.Resize += (s, e) => AplicarRegionRedondeada(modal);

        // ESC NATIVO: WinForms dispara el boton Cancel cuando se aprieta
        // ESC en un dialogo modal. No hace falta ningun ProcessCmdKey.
        modal.CancelButton = btnCancelar;

        EventHandler cancelar = (s, e) => CerrarModal(modal, DialogResult.Cancel);
        btnCancelar.Click += cancelar;
        if (pboxCerrar is not null) pboxCerrar.Click += cancelar;
    }

    /// <summary>
    /// Cierra el modal con el resultado indicado. El form que lo abrio lo
    /// lee con el DialogResult que devuelve <c>ShowDialog()</c>.
    /// </summary>
    public static void CerrarModal(Form modal, DialogResult resultado)
    {
        modal.DialogResult = resultado;
        modal.Close();
    }

    /// <summary>
    /// Recorta al modal con las esquinas redondeadas.
    ///
    /// El Region es del modal y de nadie mas: cada uno tiene el suyo, asi
    /// que no hay forma de que uno lo libere y deje muerto el del otro.
    /// </summary>
    private static void AplicarRegionRedondeada(Form modal)
    {
        int diametro = RadioEsquina * 2;

        var path = new GraphicsPath();
        path.AddArc(0, 0, diametro, diametro, 180, 90);
        path.AddArc(modal.Width - diametro, 0, diametro, diametro, 270, 90);
        path.AddArc(modal.Width - diametro, modal.Height - diametro, diametro, diametro, 0, 90);
        path.AddArc(0, modal.Height - diametro, diametro, diametro, 90, 90);
        path.CloseFigure();

        modal.Region?.Dispose();
        modal.Region = new Region(path);
        path.Dispose();
    }
}
