using MySqlConnector;
using SistemaCoworking.Datos;
using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios.Modales
{
    public partial class ReservaModal : Form
    {
        // El color de error es el mismo que usa el TextControl, y el de info
        // es el gris de la app.
        private static readonly Color ColorError = Paleta.BordeError;
        private static readonly Color ColorInfo = Paleta.TextoEtiqueta;

        private readonly Reserva? _reserva;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Reserva? Entidad { get; set; }

        //El DAO lo pasa el form. El modal lo usa para escribir: si la escritura
        //falla, el DAO lanza la excepcion, el modal la captura y muestra el
        //error, asi que el modal NO se cierra y el usuario puede corregir y
        //reintentar sin perder lo que escribio.
        private readonly ReservaDAO _dao;

        public ReservaModal(
            Reserva? reserva,
            List<Cliente>? clientes,
            List<Recurso>? recursos,
            ReservaDAO dao)
        {
            InitializeComponent();
            ConfigurarModal();

            _reserva = reserva;
            _dao = dao;

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

            // El modal se pinta con la paleta de la aplicacion: el gris del modal
            // lo separa del blanco de la grilla que tiene debajo.
            TemaModal.Aplicar(this);
            lblCrear.ForeColor = Paleta.Secundario;
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
            dtpInicio.Value = r.FechaInicio.Date;
            dtpFin.Value = r.FechaFin.Date;
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

            if (!Reserva.ValidarReserva(recurso, dtpInicio.Value, dtpFin.Value, out error))
            {
                errores = true;
                MostrarError(lblRecursoInfo, error);
            }

            if (errores) return;

            if (cliente is null || recurso is null) return;

            // La disponibilidad real la consulta la base: si hay otra reserva
            // confirmada en ese periodo, el recurso no esta disponible. La
            // misma regla la aplica sp_alta_reserva al guardar, asi que la
            // base sigue siendo la que decide.
            bool ocupado;
            try
            {
                ocupado = _dao.RecursoOcupado(
                    recurso.Id,
                    dtpInicio.Value,
                    dtpFin.Value,
                    _reserva?.Id ?? 0);
            }
            catch (MySqlException ex)
            {
                // El DAO lanza y aca se captura. El SQLSTATE 45000 es una regla de
                // negocio: el mensaje es del dominio y se muestra tal cual.
                if (ex.SqlState == "45000" || ex.Number == 1644)
                {
                    MessageBox.Show(
                        ex.Message,
                        "No se pudo completar la operacion",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Ocurrio un error al trabajar con la base de datos. Intente de nuevo."
                            + Environment.NewLine + Environment.NewLine + ex.Message,
                        "Error de base de datos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrio un error inesperado. Intente de nuevo."
                        + Environment.NewLine + Environment.NewLine + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (ocupado)
            {
                MostrarError(lblTotalInfo, "El recurso no está disponible en la fecha ingresada.");
                return;
            }

            // El total lo calcula el modelo: los dias, la regla de la reserva,
            // y el precio por dia, el polimorfismo de Recurso.
            Reserva borrador = ConstruirBorrador(cliente, recurso);
            borrador.TotalReserva = borrador.CalcularTotal(recurso);

            // La escritura la hace el modal, con el DAO que le paso el form.
            // Si falla, el DAO lanza la excepcion, el modal la captura y
            // muestra el error (por ejemplo, que el recurso no esta
            // disponible en esa fecha) y el modal SIGUE ABIERTO con lo que
            // cargo el usuario, para que elija otra fecha y reintente.
            if (!Guardar(borrador))
                return;

            Entidad = borrador;

            ModalConfiguration.CerrarModal(this, DialogResult.OK);
        }

        /// <summary>
        /// Escribe la reserva. El boton se deshabilita mientras dura la
        /// escritura para que un doble clic no dispare dos veces la misma
        /// operacion.
        /// </summary>
        private bool Guardar(Reserva reserva)
        {
            btnGuardar.Enabled = false;

            try
            {
                // Sin reserva es un alta, con reserva es una edicion: el modal ya
                // lo sabe, porque es el que tiene el _reserva.
                if (_reserva is null)
                {
                    _dao.Insertar(reserva);
                }
                else
                {
                    _dao.Modificar(reserva);
                }

                return true;
            }
            catch (MySqlException ex)
            {
                // El DAO lanza y aca se captura. El SQLSTATE 45000 es una regla de
                // negocio: el mensaje es del dominio y se muestra tal cual.
                if (ex.SqlState == "45000" || ex.Number == 1644)
                {
                    MessageBox.Show(
                        ex.Message,
                        "No se pudo completar la operacion",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Ocurrio un error al trabajar con la base de datos. Intente de nuevo."
                            + Environment.NewLine + Environment.NewLine + ex.Message,
                        "Error de base de datos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrio un error inesperado. Intente de nuevo."
                        + Environment.NewLine + Environment.NewLine + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
            finally
            {
                btnGuardar.Enabled = true;
            }
        }

        /// <summary>
        /// La reserva con lo que el usuario eligio hasta ahora. Se arma con las
        /// fechas SIN hora: el sistema reserva por dias, no por horarios.
        /// </summary>
        private Reserva ConstruirBorrador(Cliente? cliente, Recurso? recurso)
        {
            return new Reserva(
                _reserva?.Id ?? 0,
                cliente?.Id ?? 0,
                recurso?.Id ?? 0,
                dtpInicio.Value.Date,
                dtpFin.Value.Date,
                _reserva?.Estado ?? Reserva.EstadoReserva.Confirmada,
                0);
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
        /// La cantidad de dias y el total de la reserva. Los dos los calcula el
        /// modelo: la regla de los dias reservados y el precio por dia.
        /// </summary>
        private void ActualizarTotal()
        {
            Recurso? recurso = cmbRecursos.SelectedItem as Recurso;

            // El label vuelve a su color normal antes de cualquier salida: si
            // antes mostraba el error de disponibilidad, el usuario ya cambio
            // lo que lo provocaba.
            lblTotalInfo.ForeColor = ColorInfo;

            if (recurso is null)
            {
                lblTotalInfo.Text = "";
                lblTotal.Text = "";
                return;
            }

            Reserva borrador = ConstruirBorrador(cmbClientes.SelectedItem as Cliente, recurso);

            int dias = borrador.DiasReservados;

            if (dias == 0)
            {
                // Las fechas estan al reves: no hay dias que mostrar y el
                // error lo explica el label del recurso.
                lblTotalInfo.Text = "";
                lblTotal.Text = "";
                return;
            }

            lblTotalInfo.Text = $"{dias} {(dias == 1 ? "día" : "días")}";
            lblTotal.Text = $"{borrador.CalcularTotal(recurso):C}";
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
            // El error del recurso puede haber sido por las fechas, asi que
            // cambiar una fecha devuelve el label a su color normal.
            ActualizarInfoRecurso();
            ActualizarTotal();
        }

        private void dtpFin_ValueChanged(object? sender, EventArgs e)
        {
            // El error del recurso puede haber sido por las fechas, asi que
            // cambiar una fecha devuelve el label a su color normal.
            ActualizarInfoRecurso();
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
