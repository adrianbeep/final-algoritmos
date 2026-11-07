using System.Drawing.Drawing2D;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Estilo y comportamiento comun de los modales: los UserControl que se
/// muestran dentro de una ventana propia (ver <see cref="ModalOverlay"/>).
///
/// POR QUE LA REGION NO SE COMPARTE CON LA VENTANA HOST:
/// Medido en .NET 10: el setter de <see cref="Control.Region"/> guarda el
/// objeto tal cual (misma instancia) y dispose el anterior. Si la ventana
/// host recibe el MISMO Region que el modal, el modal lo dispostea en su
/// resize y la ventana host queda con un Region muerto: al leerla tira
/// "ArgumentException: Parameter is not valid". Por eso el host recibe un
/// CLON, que es independiente del Region del modal: el modal puede
/// liberar el suyo sin romper el del host.
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
    /// Deja el modal con el fondo opaco y la tipografia de las tarjetas.
    /// El fondo opaco es obligatorio: sin esto el UserControl es
    /// transparente y se ve el overlay gris a traves del modal.
    /// </summary>
    public static void AplicarEstilo(UserControl modal)
    {
        modal.BackColor = Fondo;
        modal.ForeColor = TextoPrimario;
        modal.Font = ModalFont;
    }

    /// <summary>
    /// Recorta el modal con las esquinas redondeadas.
    /// Libera el Region anterior antes de asignar el nuevo: sin esto cada
    /// resize deja un Region huerfano (handle de GDI).
    /// </summary>
    public static void AplicarRegionRedondeada(UserControl modal)
    {
        int radio = RadioEsquina;
        int diametro = radio * 2;

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
    /// Region para la ventana que hospeda al modal.
    /// Devuelve un CLON, nunca el mismo objeto: asi el modal puede
    /// liberar el suyo en el resize sin dejar muerta la Region del host.
    /// Si el modal todavia no tiene Region, devuelve null.
    /// </summary>
    public static Region? RegionParaElHost(UserControl modal)
    {
        return modal.Region?.Clone();
    }

    /// <summary>
    /// Sincroniza la forma de la ventana host con la del modal y la vuelve
    /// a sincronizar cada vez que el modal cambia de tamaño.
    /// </summary>
    public static void SincronizarRegion(UserControl modal, Form host)
    {
        AplicarRegionAlHost(modal, host);

        // El modal puede cambiar de tamaño (por ejemplo, si se ajusta el
        // contenido): la ventana host tiene que seguir la misma forma.
        modal.Resize += (s, e) => AplicarRegionAlHost(modal, host);
    }

    /// <summary>
    /// Conecta el boton Cancelar y el PictureBox de la X al mismo cierre.
    /// Los dos hacen exactamente lo mismo. Si <paramref name="alCerrar"/> es
    /// null, no se conecta nada.
    /// </summary>
    public static void ConectarCierre(
        UserControl modal,
        Button btnCancelar,
        PictureBox pboxCerrar,
        Action alCerrar)
    {
        if (alCerrar is null) return;

        EventHandler cerrar = (s, e) =>
        {
            // Si el modal ya se libero, el callback ya no tiene a quien
            // avisarle.
            if (modal.IsDisposed) return;

            alCerrar();
        };

        btnCancelar.Click += cerrar;
        pboxCerrar.Click += cerrar;
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
    /// ventana host: con <see cref="Form.KeyPreview"/> el Form ve la tecla
    /// ANTES que el control enfocado, y como el handler la marca como
    /// manejada, el control enfocado nunca la ve. Asi el ESC se dispara una
    /// sola vez, y las demas teclas siguen llegando al control enfocado.
    /// </summary>
    public static void ConectarEsc(UserControl modal, Action alCancelar)
    {
        if (alCancelar is null) return;

        void AlPresionarEsc(KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            alCancelar();
        }

        // El KeyDown del modal sirve cuando el foco esta en el modal.
        modal.KeyDown += (s, e) => AlPresionarEsc(e);

        bool conectado = false;

        void ConectarAlHost()
        {
            if (conectado) return;

            Form? host = modal.FindForm();
            if (host is null) return;

            conectado = true;
            host.KeyPreview = true;
            host.KeyDown += (s, e) => AlPresionarEsc(e);
        }

        ConectarAlHost();

        // Si todavia no esta hospedado, se espera a que lo agreguen a una
        // ventana para enganchar el preview de esa ventana.
        if (!conectado)
        {
            modal.ParentChanged += (s, e) => ConectarAlHost();
        }
    }

    /// <summary>
    /// Cierra la ventana que hospeda al modal, dejandole el DialogResult
    /// para que el form que abrio el modal sepa que paso al volver de
    /// ShowDialog().
    /// Si el modal no esta en ninguna ventana, no hace nada.
    /// </summary>
    public static void CerrarModal(UserControl modal, DialogResult resultado)
    {
        Form? host = modal.FindForm();
        if (host is null) return;

        host.DialogResult = resultado;
        host.Close();
    }

    /// <summary>
    /// Recorta al modal con las esquinas redondeadas y le pasa a la ventana
    /// host un CLON de esa forma.
    /// </summary>
    private static void AplicarRegionAlHost(UserControl modal, Form host)
    {
        // La forma se recalcula con el tamaño actual del modal.
        if (modal.Width > 0 && modal.Height > 0)
        {
            AplicarRegionRedondeada(modal);
        }

        Region? nuevo = RegionParaElHost(modal);

        // El Region anterior del host se libera antes de asignar el nuevo:
        // si no, cada resize deja un Region huerfano (handle de GDI).
        Region? anterior = host.Region;
        host.Region = nuevo;
        anterior?.Dispose();
    }
}
