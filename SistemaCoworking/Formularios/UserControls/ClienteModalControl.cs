using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Formularios.UserControls
{
    public partial class ClienteModalControl : UserControl
    {
        public event EventHandler? Closed;
        public bool Saved { get; private set; } = false;

        //Constructor para Edicion
        public ClienteModalControl(Cliente cliente)
        {
            InitializeComponent();
            AplicarEstiloCard();
            ConfigurarTxt();

            lblCrear.Text = "Editar";

            ButtonConfiguration.ConfigPrimary(btnGuardar);
            ButtonConfiguration.ConfigSecondary(btnCancelar);

            CargarCliente(cliente);
        }

        //Constructor para Insercion
        public ClienteModalControl()
        {
            InitializeComponent();
            AplicarEstiloCard();
            ConfigurarTxt();

            lblCrear.Text = "Crear";
            Label1.Visible = false;
            lblId.Visible = false;

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

            // Labels: fondo transparente para que se fundan con el card
            foreach (Label lbl in new[] { Label1, Label2, label3, label4, label5, label6 })
            {
                lbl.BackColor = Color.Transparent;
                lbl.ForeColor = Color.FromArgb(90, 90, 90);
                lbl.Font = new Font("Segoe UI", 9F);
            }
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
            txtNombre.Text = Convert.ToString(c.Nombre);
            txtApellido.Text = Convert.ToString(c.Apellido);
            txtDni.Text = Convert.ToString(c.Dni);
            txtTelefono.Text = Convert.ToString(c.Telefono);
            txtEmail.Text = Convert.ToString(c.Email);
        }

        private void ConfigurarTxt()
        {
            InputConfiguration.Configuration(pnBarraNombre, txtNombre, "Juan", Color.White);
            InputConfiguration.Configuration(pnBarraApellido, txtApellido, "Perez", Color.White);
            InputConfiguration.Configuration(pnBarraDni, txtDni, "123456789", Color.White);
            InputConfiguration.Configuration(pnBarraTelefono, txtTelefono, "+542231234956", Color.White);
            InputConfiguration.Configuration(pnBarraEmail, txtEmail, "juanperez@gmail.com", Color.White);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Acá va la lógica de guardado real (llamar al DAO.Actualizar)
            Saved = true;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Saved = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}