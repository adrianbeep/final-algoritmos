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

    public FrmReservas()
    {
        InitializeComponent();
        CargarClientes();
        CargarRecursos();

        DataGridViewConfiguration.ConfigDGV(dgvReservas);
        CargarDataGridViewReservas(dgvReservas);
    }

    private void CargarClientes()
    {
        try
        {
            _clientes = _clienteDAO.ObtenerTodos();
        }
        catch (Exception ex)
        {
            // El DAO lanza y aca se captura: se muestra el error y el combo de
            // clientes queda vacio.
            ErroresDeBaseDeDatos.Mostrar(ex);
            _clientes = new List<Cliente>();
        }
    }

    private void CargarRecursos()
    {
        try
        {
            _recursos = _recursoDAO.ObtenerTodosComoLista();
        }
        catch (Exception ex)
        {
            // El DAO lanza y aca se captura: se muestra el error y el combo de
            // recursos queda vacio.
            ErroresDeBaseDeDatos.Mostrar(ex);
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
        catch (Exception ex)
        {
            // El DAO lanza y aca se captura: se muestra el error y la grilla
            // queda vacia, pero el usuario ya sabe que fallo la consulta.
            ErroresDeBaseDeDatos.Mostrar(ex);
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
    /// pasa este form. Si la escritura falla, el DAO ya mostro el error (por
    /// ejemplo, que el recurso no esta disponible en esa fecha) y el modal
    /// sigue abierto con lo que cargo el usuario: puede elegir otra fecha y
    /// reintentar.
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

    private void dgvReservas_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // Solo el boton de la fila hace algo, no cualquier celda.
        if (dgvReservas.Columns[e.ColumnIndex] is not DataGridViewButtonColumn boton) return;

        int id = Convert.ToInt32(dgvReservas.Rows[e.RowIndex].Cells["colId"].Value);

        // El boton de cancelar es otro camino: no abre el modal de edicion,
        // cancela la reserva.
        if (boton.Name == "colCancelar")
        {
            CancelarReserva(id);
            return;
        }

        Reserva? reserva = _reservaDAO.ObtenerPorId(id);
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
        catch (Exception ex)
        {
            ErroresDeBaseDeDatos.Mostrar(ex);
            return;
        }

        CargarDataGridViewReservas(dgvReservas); // refrescar la grilla
    }
}
