using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios.UserControls;

public partial class RecursoModalControl : UserControl
{
    public event EventHandler? Closed;

    //El modal entrega el Recurso y el form decide si lo inserta o lo modifica
    public event EventHandler<Recurso>? Aceptado;

    //Si es null el modal es de alta, si no es de edicion
    private readonly Recurso? _recurso;

    //Borrador del recurso. Es el mismo objeto que se entrega al form al
    //guardar: el precio sale de aca, no de un calculo paralelo.
    //Es null mientras el formulario esta incompleto.
    private Recurso? _borrador;

    //Catalogo de tipos de recurso. Lo pasa el form.
    private readonly List<TipoRecurso> _tipos;

    public bool EsNuevo => _recurso is null;

    //Con cliente es edicion, sin cliente es alta
    public RecursoModalControl(Recurso? recurso = null, List<TipoRecurso>? tipos = null)
    {
        InitializeComponent();
        AplicarEstiloCard();
        ConfigurarTxt();
        ConectarEventos();

        // Los tipos los pasa el form: el modal no depende de la capa de datos,
        // igual que el modal de clientes.
        _tipos = tipos ?? new List<TipoRecurso>();

        CargarComboTipoRecurso(cmbTipo);
        CargarComboEstado(cmbEstado);

        InputConfiguration.ConfigLabel(label2);
        InputConfiguration.ConfigLabel(label3);
        InputConfiguration.ConfigLabel(label4);

        CheckBoxConfiguration.Config(chkProyector);
        ButtonConfiguration.ConfigPrimary(btnGuardar);
        ButtonConfiguration.ConfigSecondary(btnCancelar);

        _recurso = recurso;

        if (recurso is null)
        {
            lblCrear.Text = "Crear";
            Label1.Visible = false;
            lblId.Visible = false;
        }
        else
        {
            lblCrear.Text = "Editar";
            InputConfiguration.ConfigLabel(Label1);

            CargarRecurso(recurso);
        }
    }

    private void AplicarEstiloCard()
    {
        // Fondo opaco: sin esto el UserControl es transparente
        // y se ve el overlay gris a traves del modal.
        this.BackColor = Color.White;

        // Esquinas redondeadas
        AplicarRegionRedondeada();

        // Tipografia consistente
        this.Font = new Font("Segoe UI", 9F);
        this.ForeColor = Color.FromArgb(40, 40, 40);
    }

    private void AplicarRegionRedondeada()
    {
        int radio = 14;
        int diametro = radio * 2;

        var path = new GraphicsPath();
        path.AddArc(0, 0, diametro, diametro, 180, 90);
        path.AddArc(this.Width - diametro, 0, diametro, diametro, 270, 90);
        path.AddArc(this.Width - diametro, this.Height - diametro, diametro, diametro, 0, 90);
        path.AddArc(0, this.Height - diametro, diametro, diametro, 90, 90);
        path.CloseFigure();

        this.Region?.Dispose();
        this.Region = new Region(path);
        path.Dispose();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (this.Width > 0 && this.Height > 0)
            AplicarRegionRedondeada();
    }

    private void ConfigurarTxt()
    {
        txtNombre.Configurar("Nombre", "Ej. Sala Magna", TipoEntrada.LetrasYNumeros, 100);
        txtUbicacion.Configurar("Ubicacion", "Ej. Piso 2", TipoEntrada.LetrasYNumeros, 100);
        txtCosto.Configurar("Costo Base", "0.00", TipoEntrada.NumericoConDecimales);
        txtCapacidad.Configurar("Capacidad", "10", TipoEntrada.SoloNumeros);
        txtNumMonitores.Configurar("Monitores", "2", TipoEntrada.SoloNumeros);
        txtMetros.Configurar("Metros Cuadrados", "25", TipoEntrada.SoloNumeros);
    }

    private void CargarRecurso(Recurso r)
    {
        lblId.Text = r.Id.ToString();
        txtNombre.Value = r.Nombre;
        txtUbicacion.Value = r.Ubicacion;
        txtCosto.Value = r.Costo.ToString();

        // El combo de estados se selecciona directo por el Enum:
        cmbEstado.SelectedItem = r.Estado;

        cmbTipo.SelectedItem = cmbTipo.Items
            .OfType<TipoRecurso>()
            .FirstOrDefault(t => t.Id == r.IdTipoRecurso);

        switch (r)
        {
            case Sala sala:
                txtCapacidad.Value = sala.CapacidadPersonas.ToString();
                chkProyector.Checked = sala.TieneProyector;
                break;

            case PuestoTrabajo puesto:
                txtNumMonitores.Value = puesto.NumeroMonitor.ToString();
                break;

            case OficinaPrivada oficina:
                txtMetros.Value = oficina.MetrosCuadrados.ToString();
                break;
        }
    }

    private void CargarComboTipoRecurso(ComboBox combo)
    {
        ComboBoxConfiguration.ConfigTipos(combo, _tipos);
    }

    private void CargarComboEstado(ComboBox combo)
    {
        ComboBoxConfiguration.ConfigEstados(combo);
    }

    private void cmbTipo_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbTipo.SelectedItem is not TipoRecurso tipo)
            return;

        // Se prende solo el que coincide, los otros se apagan solos:
        pnSala.Visible = tipo.Nombre == "Sala";
        pnPuestoTrabajo.Visible = tipo.Nombre == "PuestoTrabajo";
        pnOficinaPrivada.Visible = tipo.Nombre == "OficinaPrivada";

        ActualizarPrecio();
    }

    private void ActualizarPrecio()
    {
        _borrador = ConstruirBorrador();

        // Sin formulario completo no hay precio: se muestra un guion y no un
        // cero, que seria mentira. El costo sigue cargado, y el precio sale
        // de el, asi que nunca puede ser cero mientras el costo sea mayor.
        // Pasa cuando cambia el tipo y todavia no se cargo el dato propio
        // del tipo nuevo: la fabrica no arma un recurso sin ese dato.
        lblPrecio.Text = _borrador is null
            ? "—"
            : $"{_borrador.CalcularPrecio():C2}";
    }

    /// <summary>
    /// Arma el recurso con lo que hay cargado en el formulario. Devuelve null
    /// si todavia no se puede: el factory devuelve null cuando falta el dato
    /// obligatorio del tipo.
    /// </summary>
    private Recurso? ConstruirBorrador()
    {
        TipoRecurso? tipo = ComboBoxConfiguration.TipoSeleccionado(cmbTipo);

        if (tipo is null)
            return null;

        if (!Recurso.TryParseCosto(txtCosto.Value, out decimal costo))
            return null;

        return RecursoFactory.Crear(
            tipo.Nombre,
            _recurso?.Id ?? 0,
            tipo.Id,
            txtNombre.Value,
            txtUbicacion.Value,
            costo,
            ComboBoxConfiguration.EstadoSeleccionado(cmbEstado),
            capacidadPersonas: LeerEntero(txtCapacidad),
            tieneProyector: chkProyector.Checked,
            numeroMonitor: LeerEntero(txtNumMonitores),
            metrosCuadrados: LeerDecimal(txtMetros));
    }

    /// <summary>
    /// Los cambios del formulario recalculan el precio.
    /// </summary>
    private void ConectarEventos()
    {
        // El cambio de tipo muestra los campos del tipo y recalcula el precio.
        // Se desenlaza primero para no quedar enganchado dos veces.
        cmbTipo.SelectedIndexChanged -= cmbTipo_SelectedIndexChanged;
        cmbTipo.SelectedIndexChanged += cmbTipo_SelectedIndexChanged;

        txtCosto.ValueChanged += (_, _) => ActualizarPrecio();
        txtCapacidad.ValueChanged += (_, _) => ActualizarPrecio();
        txtNumMonitores.ValueChanged += (_, _) => ActualizarPrecio();
        txtMetros.ValueChanged += (_, _) => ActualizarPrecio();
        chkProyector.CheckedChanged += (_, _) => ActualizarPrecio();
    }

    private static int? LeerEntero(TextControl campo)
    {
        return int.TryParse(campo.Value, out int valor) ? valor : null;
    }

    private static decimal? LeerDecimal(TextControl campo)
    {
        return decimal.TryParse(campo.Value, out decimal valor) ? valor : null;
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        //OfType<T> permite filtrar por el control
        //que necesito recorrer
        foreach (TextControl txt in this.Controls.OfType<TextControl>())
            txt.LimpiarError();

        bool errores = false;
        string? error;

        string nombre = txtNombre.Value.Trim();
        if (!Recurso.ValidarNombre(nombre, out error))
        {
            errores = true;
            txtNombre.MostrarError(error);
        }

        string ubicacion = txtUbicacion.Value.Trim();
        if (!Recurso.ValidarUbicacion(ubicacion, out error))
        {
            errores = true;
            txtUbicacion.MostrarError(error);
        }

        decimal costo = 0m;
        if (!Recurso.TryParseCosto(txtCosto.Value, out costo))
        {
            errores = true;
            txtCosto.MostrarError("Ingrese un numero para el costo.");
        }
        else if (!Recurso.ValidarCosto(costo, out error))
        {
            errores = true;
            txtCosto.MostrarError(error);
        }

        // Los datos del tipo seleccionado. Los de los otros tipos van en null
        // y no bloquean el guardado: sus campos estan ocultos.
        TipoRecurso? tipo = ComboBoxConfiguration.TipoSeleccionado(cmbTipo);
        int? capacidad = null;
        int? monitores = null;
        decimal? metros = null;
        bool? tieneProyector = null;

        if (tipo is null)
        {
            errores = true;
        }
        else
        {
            switch (tipo.Nombre)
            {
                case "Sala":
                    capacidad = LeerEntero(txtCapacidad);
                    tieneProyector = chkProyector.Checked;

                    if (!Sala.ValidarCapacidad(capacidad ?? 0, out error))
                    {
                        errores = true;
                        txtCapacidad.MostrarError(error);
                    }
                    break;

                case "PuestoTrabajo":
                    monitores = LeerEntero(txtNumMonitores);

                    if (!PuestoTrabajo.ValidarMonitores(monitores ?? 0, out error))
                    {
                        errores = true;
                        txtNumMonitores.MostrarError(error);
                    }
                    break;

                default:
                    metros = LeerDecimal(txtMetros);

                    if (!OficinaPrivada.ValidarMetrosCuadrados(metros ?? 0m, out error))
                    {
                        errores = true;
                        txtMetros.MostrarError(error);
                    }
                    break;
            }
        }

        if (errores)
            return;

        //El id 0 es el de un recurso que todavia no esta persistido
        int id = _recurso?.Id ?? 0;

        Recurso? recurso = RecursoFactory.Crear(
            tipo!.Nombre,
            id,
            tipo.Id,
            nombre,
            ubicacion,
            costo,
            ComboBoxConfiguration.EstadoSeleccionado(cmbEstado),
            capacidadPersonas: capacidad,
            tieneProyector: tieneProyector,
            numeroMonitor: monitores,
            metrosCuadrados: metros);

        // Con los datos validados la fabrica siempre arma el recurso. Si no,
        // algo se nos escapo: mejor no cerrar y perder lo que cargo el usuario.
        if (recurso is null)
            return;

        Aceptado?.Invoke(this, recurso);
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void pBoxCerrar_Click(object sender, EventArgs e)
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}
