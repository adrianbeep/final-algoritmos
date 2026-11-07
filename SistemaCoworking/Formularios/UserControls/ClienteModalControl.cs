using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios.UserControls;

public partial class ClienteModalControl : Form
{
    //Si es null el modal es de alta, si no es de edicion
    private readonly Cliente? _cliente;

    //El modal entrega el cliente al form, que decide si lo inserta o lo
    //modifica. El atributo le dice al serializador del diseñador que esta
    //propiedad no se guarda: sin el, el analizador de WinForms la toma por
    //error y rompe el build (WFO1000).
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Cliente? Entidad { get; set; }

    //Con cliente es edicion, sin cliente es alta
    public ClienteModalControl(Cliente? cliente = null)
    {
        InitializeComponent();

        // Una sola llamada: estilo de tarjeta, esquinas redondeadas, y el
        // boton Cancelar, la X y el ESC haciendo lo mismo.
        ModalConfiguration.ConfigModal(this, btnCancelar, pBoxCerrar);

        ConfigurarTxt();

        ButtonConfiguration.ConfigPrimary(btnGuardar);
        ButtonConfiguration.ConfigSecondary(btnCancelar);

        _cliente = cliente;

        if (cliente is null)
        {
            lblCrear.Text = "Crear";
            Label1.Visible = false;
            lblId.Visible = false;
        }
        else
        {
            lblCrear.Text = "Editar";
            InputConfiguration.ConfigLabel(Label1);

            CargarCliente(cliente);
        }
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
        TextControl.LimpiarErrores(this);

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

        //El id 0 es el de un cliente que todavia no esta persistido
        int id = _cliente?.Id ?? 0;

        Cliente cliente = new Cliente(id, nombre, apellido, dni, telefono, email);

        // El modal entrega la entidad y cierra. El form la persiste DESPUES,
        // cuando ShowDialog() ya devolvio: si la escritura falla, el modal
        // ya cerro y la aplicacion no queda bloqueada.
        Entidad = cliente;
        ModalConfiguration.CerrarModal(this, DialogResult.OK);
    }

    private void ConfigurarTxt()
    {
        txtNombre.Configurar("Nombre", "Juan", TipoEntrada.SoloLetras, 100);
        txtApellido.Configurar("Apellido", "Perez", TipoEntrada.SoloLetras, 100);
        txtDni.Configurar("Dni", "123456789", TipoEntrada.SoloNumeros, 18);
        txtTelefono.Configurar("Telefono", "542231234956", TipoEntrada.LetrasYNumeros, 30);
        txtEmail.Configurar("Email", "juanperez@gmail.com", TipoEntrada.LetrasYNumeros, 100);
    }
}