using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios.UserControls
{
    public partial class ReservaModal : Form
    {
        // El color de error es el mismo que usa el TextControl, y el de info
        // es el gris de la app.
        private static readonly Color ColorError = Color.FromArgb(198, 40, 40);
        private static readonly Color ColorInfo = Color.FromArgb(138, 143, 152);

        private readonly Reserva? _reserva;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Reserva? Entidad { get; set; }

        public ReservaModal(Reserva? reserva = null, List<Cliente>? clientes = null, List<Recurso>? recursos = null)
        {
            InitializeComponent();
            ConfigurarModal();

            _reserva = reserva;

            // Los combos se bindean: el Value es el Id, asi que lo elegido
            // se lee con SelectedValue.
            CargarComboClientes(clientes);
            CargarComboRecursos(recursos);

            // Si reserva es nulo, es Creacion, sino Edicion.
            if (reserva is null)
            {
                lblCrear.Text = "Crear";
            }
            else
            {
                lblCrear.Text = "Editar";
                Label1.Visible = true;
                lblId.Visible = true;
                CargarReserva(reserva);
            }
        }

        private void ConfigurarModal()
        {
            ModalConfiguration.ConfigModal(this, btnCancelar, pBoxCerrar);
            InputConfiguration.ConfigLabel(Label1);
            InputConfiguration.ConfigLabel(label2);
            InputConfiguration.ConfigLabel(label4);
            InputConfiguration.ConfigLabel(label5);
            InputConfiguration.ConfigLabel(lblEstadoInfo);
            InputConfiguration.ConfigLabel(lblClienteInfo);
            InputConfiguration.ConfigLabel(lblRecursoInfo);
            ButtonConfiguration.ConfigPrimary(btnGuardar);
            ButtonConfiguration.ConfigSecondary(btnCancelar);
        }

        private void CargarComboClientes(List<Cliente>? clientes)
        {
            ComboBoxConfiguration.Configuracion(cmbClientes);
            cmbClientes.DisplayMember = nameof(Cliente.Descripcion);
            cmbClientes.ValueMember = nameof(Cliente.Id);
            cmbClientes.DataSource = clientes ?? new List<Cliente>();
        }

        private void CargarComboRecursos(List<Recurso>? recursos)
        {
            ComboBoxConfiguration.Configuracion(cmbRecursos);
            cmbRecursos.DisplayMember = nameof(Recurso.Descripcion);
            cmbRecursos.ValueMember = nameof(Recurso.Id);
            cmbRecursos.DataSource = recursos ?? new List<Recurso>();
        }

        private void CargarReserva(Reserva r)
        {
            lblId.Text = r.Id.ToString();
            cmbClientes.SelectedValue = r.IdCliente;
            cmbRecursos.SelectedValue = r.IdRecurso;
            dtpInicio.Value = r.FechaInicio;
            dtpFin.Value = r.FechaFin;
            lblEstadoInfo.Text = $"Estado: {r.Estado}";
        }

        //=================================
        //GUARDAR
        //=================================
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Se limpian los mensajes y se vuelve a mostrar la info de lo
            // elegido: si algo falla, el error la reemplaza.
            ActualizarInfoCliente();
            ActualizarInfoRecurso();
            ActualizarTotal();

            Cliente? cliente = cmbClientes.SelectedItem as Cliente;
            Recurso? recurso = cmbRecursos.SelectedItem as Recurso;

            bool errores = false;
            string? error;

            if (!Reserva.ValidarCliente(cliente, out error))
            {
                errores = true;
                MostrarError(lblClienteInfo, error);
            }

            if (!Reserva.ValidarRecurso(recurso, out error))
            {
                errores = true;
                MostrarError(lblRecursoInfo, error);
            }

            if (!Reserva.ValidarFechas(dtpInicio.Value, dtpFin.Value, out error))
            {
                errores = true;
                MostrarError(lblTotalInfo, error);
            }

            if (errores) return;

            // El id 0 es el de una reserva que todavia no esta persistida.
            int id = _reserva?.Id ?? 0;

            Entidad = new Reserva(
                id,
                cliente?.Id ?? 0,
                recurso?.Id ?? 0,
                dtpInicio.Value,
                dtpFin.Value,
                _reserva?.Estado ?? Reserva.EstadoReserva.Confirmada,
                CalcularTotal(recurso));

            ModalConfiguration.CerrarModal(this, DialogResult.OK);
        }

        /// <summary>
        /// El total de la reserva: el precio por dia del recurso por la
        /// cantidad de dias. El precio por dia sale del polimorfismo, de
        /// <see cref="Recurso.CalcularPrecio"/>.
        /// </summary>
        private decimal CalcularTotal(Recurso? recurso)
        {
            if (recurso is null) return 0;

            int dias = (dtpFin.Value - dtpInicio.Value).Days;

            if (dias <= 0) return 0;

            return Math.Round(recurso.PrecioDia * dias, 2);
        }

        /// <summary>
        /// Muestra el error de una validacion en el label de abajo del
        /// control.
        /// </summary>
        private static void MostrarError(Label label, string? mensaje)
        {
            label.ForeColor = ColorError;
            label.Text = mensaje ?? "";
        }

        /// <summary>
        /// Info del cliente elegido: nada. El combo ya muestra el nombre, el
        /// apellido y el DNI, asi que repetirlo aqui seria mostrarlo dos
        /// veces. Este label queda para el error de la validacion.
        /// </summary>
        private void ActualizarInfoCliente()
        {
            lblClienteInfo.ForeColor = ColorInfo;
            lblClienteInfo.Text = "";
        }

        /// <summary>
        /// Info del recurso elegido: el precio por dia. El combo ya muestra
        /// el nombre y la ubicacion.
        /// </summary>
        private void ActualizarInfoRecurso()
        {
            Recurso? recurso = cmbRecursos.SelectedItem as Recurso;

            lblRecursoInfo.ForeColor = ColorInfo;
            lblRecursoInfo.Text = recurso is null
                ? ""
                : $"{recurso.PrecioDia:C} por día";
        }

        /// <summary>
        /// La cantidad de dias y el total de la reserva.
        /// </summary>
        private void ActualizarTotal()
        {
            Recurso? recurso = cmbRecursos.SelectedItem as Recurso;
            if (recurso is null)
            {
                lblTotalInfo.Text = "";
                lblTotal.Text = "";
                return;
            }

            int dias = (dtpFin.Value - dtpInicio.Value).Days;

            if (dias <= 0) return;
            
            lblTotalInfo.ForeColor = ColorInfo;
            lblTotalInfo.Text = $"{dias} {(dias == 1 ? "día" : "días")}";
            lblTotal.Text = $"{CalcularTotal(recurso):C}";
        }

        //=================================
        //CAMBIOS
        //=================================
        private void cmbClientes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarInfoCliente();
        }

        private void cmbRecursos_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarInfoRecurso();
            ActualizarTotal();
        }

        private void dtpInicio_ValueChanged(object? sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void dtpFin_ValueChanged(object? sender, EventArgs e)
        {
            ActualizarTotal();
        }

        private void ReservaModal_Load(object sender, EventArgs e)
        {
            ActualizarInfoCliente();
            ActualizarInfoRecurso();
            ActualizarTotal();
        }
    }
}
