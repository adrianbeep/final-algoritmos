using MySqlConnector;
using SistemaCoworking.Datos;
using SistemaCoworking.Formularios.UserControls;
using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios;

public partial class FrmReservas : Form
{
    private ClienteDAO _clienteDAO = new ClienteDAO();
    private RecursoDAO _recursoDAO = new RecursoDAO();
    private ReservaDAO _reservaDAO = new ReservaDAO();

    //Listas para el modal
    private List<Cliente> _clientes = new List<Cliente>();
    private List<Recurso> _recursos = new List<Recurso>();

    // Boton de cancelar atenuado: la reserva no esta en estado Confirmada.
    private static readonly Color BotonDeshabilitadoFondo = Color.FromArgb(224, 224, 224);
    private static readonly Color BotonDeshabilitadoBorde = Color.FromArgb(190, 190, 190);
    private static readonly Color BotonDeshabilitadoTexto = Color.FromArgb(150, 150, 150);

    public FrmReservas()
    {
        InitializeComponent();
        CargarClientes();
        CargarRecursos();

        DataGridViewConfiguration.ConfigDGV(dgvReservas);

        // Esta grilla tiene once columnas visibles en menos de 900 px. Con el
        // padding por defecto los titulos quedan cortados, asi que el de la
        // cabecera achica. El de las celdas no se puede cambiar desde aca:
        // DataGridView solo deja escribir el estilo de la grilla antes de
        // armarla, asi que va columna por columna en el Designer.
        dgvReservas.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 10, 6, 10);

        CargarDataGridViewReservas(dgvReservas);

        ButtonConfiguration.ConfigPrimary(btnCrearReserva);
    }

    private void CargarClientes()
    {
        try
        {
            _clientes = _clienteDAO.ObtenerTodosComoLista();
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

            _clientes = new List<Cliente>();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Ocurrio un error inesperado. Intente de nuevo."
                    + Environment.NewLine + Environment.NewLine + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            _clientes = new List<Cliente>();
        }
    }

    private void CargarRecursos()
    {
        try
        {
            _recursos = _recursoDAO.ObtenerTodosComoLista();
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

            _recursos = new List<Recurso>();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Ocurrio un error inesperado. Intente de nuevo."
                    + Environment.NewLine + Environment.NewLine + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            _recursos = new List<Recurso>();
        }
    }

    private void CargarDataGridViewReservas(DataGridView dgv)
    {
        try
        {
            dgv.DataSource = null;
            dgv.DataSource = _reservaDAO.ObtenerTodos();
            dgv.ClearSelection();
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

            dgv.DataSource = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Ocurrio un error inesperado. Intente de nuevo."
                    + Environment.NewLine + Environment.NewLine + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            dgv.DataSource = null;
        }
    }

    /// <summary>
    /// Abre el modal de reserva.
    ///
    /// Sin reserva es un ALTA, con reserva es una EDICION: lo decide este
    /// form, que es el que abre el modal.
    ///
    /// El modal escribe en el momento de apretar Guardar, con el DAO que le
    /// pasa este form. Si la escritura falla, el DAO lanza la excepcion (por
    /// ejemplo, que el recurso no esta disponible en esa fecha), el modal la
    /// captura y muestra el error, y el modal sigue abierto con lo que cargo el
    /// usuario: puede elegir otra fecha y reintentar.
    /// </summary>
    private void AbrirModalReserva(Reserva? reserva = null)
    {
        ReservaModal modal = new ReservaModal(reserva, _clientes, _recursos, _reservaDAO);

        if (modal.ShowDialog() != DialogResult.OK) return;

        CargarDataGridViewReservas(dgvReservas); // refrescar la grilla
    }

    private void btnCrearReserva_Click(object sender, EventArgs e)
    {
        AbrirModalReserva();

    }

    /// <summary>
    /// Solo se puede cancelar una reserva en estado Confirmada: es la misma
    /// regla que aplica el SP sp_cancelar_reserva. Aca se resuelve antes para
    /// que el boton aparezca atenuado y la accion no llegue a la base.
    /// </summary>
    private static bool EsCancelable(DataGridViewRow fila)
    {
        object? valor = fila.Cells["colEstado"].Value;

        return valor is not null
            && Enum.TryParse<Reserva.EstadoReserva>(valor.ToString(), out Reserva.EstadoReserva estado)
            && estado == Reserva.EstadoReserva.Confirmada;
    }

    /// <summary>
    /// Atenua el boton de cancelar cuando la reserva no esta en estado
    /// Confirmada: el usuario ve que la accion no aplica antes de hacer click.
    ///
    /// El color va al estilo propio de la celda (celda.Style) porque es lo que
    /// lee el pintado de la grilla. e.CellStyle no sirve para marcar esta
    /// celda: es la mezcla final de estilos y la fila la arma sola.
    /// </summary>
    private void dgvReservas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvReservas.Columns[e.ColumnIndex].Name != "colCancelar") return;
        if (EsCancelable(dgvReservas.Rows[e.RowIndex])) return;

        // El boton queda atenuado: se ve que la accion no aplica.
        var celda = dgvReservas.Rows[e.RowIndex].Cells[e.ColumnIndex];

        celda.Style.BackColor = BotonDeshabilitadoFondo;
        celda.Style.SelectionBackColor = BotonDeshabilitadoBorde;
        celda.Style.ForeColor = BotonDeshabilitadoTexto;

        e.CellStyle.BackColor = BotonDeshabilitadoFondo;
        e.CellStyle.SelectionBackColor = BotonDeshabilitadoBorde;
        e.CellStyle.ForeColor = BotonDeshabilitadoTexto;
    }

    private void dgvReservas_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (e.ColumnIndex < 0) return;

        // Solo el boton de la fila hace algo, no cualquier celda.
        if (dgvReservas.Columns[e.ColumnIndex] is not DataGridViewButtonColumn boton) return;

        int id = Convert.ToInt32(dgvReservas.Rows[e.RowIndex].Cells["colId"].Value);

        // El boton de cancelar es otro camino: no abre el modal de edicion,
        // cancela la reserva.
        if (boton.Name == "colCancelar")
        {
            if (!EsCancelable(dgvReservas.Rows[e.RowIndex]))
            {
                // El boton esta atenuado: se explica igual por que no pasa nada.
                MessageBox.Show(
                    "Solo se pueden cancelar las reservas en estado Confirmada.",
                    "Cancelar reserva",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            CancelarReserva(id);
            return;
        }

        Reserva? reserva;
        try
        {
            reserva = _reservaDAO.ObtenerPorId(id);
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

        if (reserva is null)
        {
            MessageBox.Show("No se encontro la reserva.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AbrirModalReserva(reserva);
    }

    /// <summary>
    /// Cancela una reserva.
    ///
    /// Cancelar NO borra: la reserva queda en estado Cancelada, con su
    /// historial, y el trigger de la base la audita sola. Por eso se pide
    /// confirmacion antes: es una accion que cambia el estado de la agenda.
    /// </summary>
    private void CancelarReserva(int id)
    {
        DialogResult confirmacion = MessageBox.Show(
            "¿Cancelar la reserva? La reserva queda cancelada, no se elimina.",
            "Cancelar reserva",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes) return;

        // El DAO lanza y aca se captura: se muestra el error con el mensaje del
        // SP y la reserva no se cancela.
        try
        {
            _reservaDAO.Cancelar(id);
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

        CargarDataGridViewReservas(dgvReservas); // refrescar la grilla
    }
}
