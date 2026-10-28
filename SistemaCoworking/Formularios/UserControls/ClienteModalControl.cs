using SistemaCoworking.Datos;
using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios.UserControls;

public partial class ClienteModalControl : UserControl
{
    public event EventHandler? Closed;
    public bool Guardado { get; private set; } = false;
    private bool Insertar { get; set; } = true;

    private ClienteDAO ClienteDAO;

    //Constructor para Edicion
    public ClienteModalControl(Cliente cliente, ClienteDAO clienteDAO)
    {
        InitializeComponent();
        AplicarEstiloCard();
        ConfigurarTxt();

        Insertar = false;

        lblCrear.Text = "Editar";
        Label1.BackColor = Color.Transparent;
        Label1.ForeColor = Color.FromArgb(80, 80, 80);
        Label1.Font = new Font("Segoe UI", 9F);

        ButtonConfiguration.ConfigPrimary(btnGuardar);
        ButtonConfiguration.ConfigSecondary(btnCancelar);

        ClienteDAO = clienteDAO;

        CargarCliente(cliente);
    }

    //Constructor para Insercion
    public ClienteModalControl(ClienteDAO clienteDAO)
    {
        InitializeComponent();
        AplicarEstiloCard();
        ConfigurarTxt();

        Insertar = true;

        lblCrear.Text = "Crear";
        Label1.Visible = false;
        lblId.Visible = false;

        ClienteDAO = clienteDAO;

        ButtonConfiguration.ConfigPrimary(btnGuardar);
        ButtonConfiguration.ConfigSecondary(btnCancelar);
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

    private void CargarCliente(Cliente c)
    {
        lblId.Text = Convert.ToString(c.Id);
        txtNombre.Value = Convert.ToString(c.Nombre);
        txtApellido.Value = Convert.ToString(c.Apellido);
        txtDni.Value = Convert.ToString(c.Dni);
        txtTelefono.Value = Convert.ToString(c.Telefono);
        txtEmail.Value = Convert.ToString(c.Email);
    }

    //=================================
    //BOTONES
    //=================================
    private void btnGuardar_Click(object sender, EventArgs e)
    {
        //OfType<T> permite filtrar por el control
        //que necesito recorrer
        foreach (TextControl txt in this.Controls.OfType<TextControl>())
            txt.LimpiarError();

        bool errores = false;
        string? error;

        string nombre = txtNombre.Value.Trim();
        if (!Cliente.ValidarNombre(nombre, out error))
        {
            errores = true;
            txtNombre.MostrarError(error);
        }

        string apellido = txtApellido.Value.Trim();
        if (!Cliente.ValidarApellido(apellido, out error))
        {
            errores = true;
            txtApellido.MostrarError(error);
        }

        if (!Cliente.ValidarDni(txtDni.Value.Trim(), out error))
        {
            errores = true;
            txtDni.MostrarError(error);
        }

        string telefono = txtTelefono.Value.Trim();
        if (!Cliente.ValidarTelefono(telefono, out error))
        {
            errores = true;
            txtTelefono.MostrarError(error);
        }

        string email = txtEmail.Value.Trim();
        if (!Cliente.ValidarEmail(email, out error))
        {
            errores = true;
            txtEmail.MostrarError(error);
        }

        if (errores)
            return;

        int dni = int.Parse(txtDni.Value.Trim());

        if (Insertar)
        {
            ClienteDAO.Insertar(nombre, apellido, dni, telefono, email);
        }
        else
        {
            Cliente c = 
                new Cliente(int.Parse(lblId.Text), nombre, apellido, dni, telefono, email);
            
            ClienteDAO.Modificar(c);
        }

        Guardado = true;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        Guardado = false;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void ConfigurarTxt()
    {
        txtNombre.Placeholder = "Juan"; txtNombre.LblText = "Nombre";
        txtApellido.Placeholder = "Perez"; txtApellido.LblText = "Apellido";
        txtDni.Placeholder = "123456789"; txtDni.LblText = "Dni";
        txtTelefono.Placeholder = "542231234956"; txtTelefono.LblText = "Telefono";
        txtEmail.Placeholder = "juanperez@gmail.com"; txtEmail.LblText = "Email";
    }

    private void pBoxCerrar_Click(object sender, EventArgs e)
    {
        Guardado = false;
        Closed?.Invoke(this, EventArgs.Empty);
    }
}