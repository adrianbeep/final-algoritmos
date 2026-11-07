using System.Drawing.Drawing2D;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Configuracion comun de los UserControl que se muestran como modal.
///
/// UNA SOLA LLAMADA, en el constructor del modal:
///
/// <code>
/// ModalConfiguration.ConfigModal(this, btnCancelar, pBoxCerrar);
/// </code>
///
/// deja el modal con el estilo de tarjeta, las esquinas redondeadas, y el
/// boton Cancelar, la X y la tecla ESC haciendo lo mismo: cerrar el modal
/// sin guardar. Para guardar, el modal arma la entidad y cierra con
/// <see cref="CerrarModal"/>.
/// </summary>
public static class ModalConfiguration
{
    private static readonly Color TextoPrimario = Color.FromArgb(40, 40, 40);
    private static readonly Color Fondo = Color.White;

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
    /// <param name="modal">El UserControl del modal.</param>
    /// <param name="btnCancelar">El boton que cancela.</param>
    /// <param name="pboxCerrar">La X de cerrar, si tiene.</param>
    public static void ConfigModal(
        UserControl modal,
        Button btnCancelar,
        PictureBox? pboxCerrar = null)
    {
        // Fondo opaco: sin esto el UserControl es transparente y se ve el
        // fondo de la app a traves del modal.
        modal.BackColor = Fondo;
        modal.ForeColor = TextoPrimario;
        modal.Font = ModalFont;

        AplicarRegionRedondeada(modal);

        ConectarCierre(modal, btnCancelar, pboxCerrar);
        ConectarEsc(modal);
    }

    /// <summary>
    /// Cierra la ventana del modal con el resultado indicado. El form que
    /// abrio el modal lo lee con el DialogResult que devuelve
    /// <see cref="ModalDialog.Mostrar"/>.
    /// </summary>
    public static void CerrarModal(UserControl modal, DialogResult resultado)
    {
        Form? ventana = modal.FindForm();
        if (ventana is null) return;

        ventana.DialogResult = resultado;
        ventana.Close();
    }

    /// <summary>
    /// Le da a la ventana del modal la misma forma redondeada que el modal.
    ///
    /// La ventana recibe un CLON del Region del modal, nunca el mismo
    /// objeto: el setter de Region guarda la instancia tal cual y el modal
    /// libera el suyo en su resize. Si compartieran el objeto, la ventana
    /// quedaria con un Region muerto, y al leerla tiraria
    /// "ArgumentException: Parameter is not valid".
    /// </summary>
    public static void SincronizarRegion(UserControl modal, Form ventana)
    {
        // Primero la del modal, despues el clon para la ventana.
        AplicarRegionRedondeada(modal);

        Region? anterior = ventana.Region;
        ventana.Region = modal.Region?.Clone();
        anterior?.Dispose();
    }

    /// <summary>
    /// Recorta al modal con las esquinas redondeadas.
    /// </summary>
    private static void AplicarRegionRedondeada(UserControl modal)
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

    /// <summary>
    /// Conecta el boton Cancelar y la X al mismo cierre: los dos cancelan.
    /// </summary>
    private static void ConectarCierre(
        UserControl modal,
        Button btnCancelar,
        PictureBox? pboxCerrar)
    {
        EventHandler cerrar = (s, e) => CerrarModal(modal, DialogResult.Cancel);

        btnCancelar.Click += cerrar;
        if (pboxCerrar is not null) pboxCerrar.Click += cerrar;
    }

    /// <summary>
    /// Conecta la tecla ESC al cierre del modal.
    ///
    /// OJO CON EL FOCO: medido en .NET 10, <see cref="Control.KeyDown"/> NO
    /// sube del control enfocado al UserControl. En el codigo de WinForms,
    /// ProcessKeyEventArgs solo se dispara en el control que recibio el
    /// mensaje, y la cadena de padres solo se consulta en el preview
    /// (ProcessKeyPreview). Con el foco en un TextControl hijo, la tecla ESC
    /// se perdia.
    ///
    /// Por eso, ademas del KeyDown del modal, se usa el preview de la
    /// ventana que lo hospeda: con <see cref="Form.KeyPreview"/> el Form ve
    /// la tecla ANTES que el control enfocado, y como el handler la marca
    /// como manejada, el control enfocado nunca la ve. Asi el ESC se dispara
    /// una sola vez, y las demas teclas siguen llegando al control enfocado.
    /// </summary>
    private static void ConectarEsc(UserControl modal)
    {
        void AlPresionarEsc(KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape) return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            CerrarModal(modal, DialogResult.Cancel);
        }

        // El KeyDown del modal sirve cuando el foco esta en el modal.
        modal.KeyDown += (s, e) => AlPresionarEsc(e);

        bool conectado = false;

        void ConectarAVentana()
        {
            if (conectado) return;

            Form? ventana = modal.FindForm();
            if (ventana is null) return;

            conectado = true;
            ventana.KeyPreview = true;
            ventana.KeyDown += (s, e) => AlPresionarEsc(e);
        }

        ConectarAVentana();

        // Si todavia no esta hospedado, se espera a que lo agreguen a una
        // ventana para enganchar el preview de esa ventana.
        if (!conectado)
        {
            modal.ParentChanged += (s, e) => ConectarAVentana();
        }
    }
}
