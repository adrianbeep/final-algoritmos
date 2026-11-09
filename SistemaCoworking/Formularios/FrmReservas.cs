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
            MessageBox.Show("Error al cargar clientes: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("Error al cargar recursos: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("Error al cargar reservas: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Abre el modal de cliente y, si el usuario guardo, lo escribe en la base.
    ///
    /// Sin cliente es un ALTA, con cliente es una EDICION: lo decide este
    /// form, que es el que abre el modal.
    ///
    /// ShowDialog() espera a que el modal se cierre y devuelve
    /// DialogResult.OK si el usuario guardo. Cuando guardo, el modal deja el
    /// cliente en su propiedad Entidad, y recien ahi se escribe en la base.
    /// Por eso la escritura va despues: si la base falla, el modal ya cerro
    /// y la aplicacion no queda bloqueada.
    /// </summary>
    private void AbrirModalReserva(Reserva? reserva = null)
    {
        ReservaModal modal = new ReservaModal(reserva, _clientes, _recursos);

        if (modal.ShowDialog() != DialogResult.OK) return;

        Reserva? datos = modal.Entidad;
        if (datos is null) return;

        if (reserva is null)
        {
            _reservaDAO.Insertar(
                datos.IdCliente,
                datos.IdRecurso,
                datos.FechaInicio,
                datos.FechaFin,
                datos.TotalReserva
            );
        }
        else
        {
            _reservaDAO.Modificar(datos);
        }

        CargarDataGridViewReservas(dgvReservas); // refrescar la grilla
    }

    private void btnCrearReserva_Click(object sender, EventArgs e)
    {
        AbrirModalReserva();

    }

    private void dgvReservas_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // Solo el boton de la fila abre el modal, no cualquier celda.
        if (dgvReservas.Columns[e.ColumnIndex] is not DataGridViewButtonColumn) return;

        int id = Convert.ToInt32(dgvReservas.Rows[e.RowIndex].Cells["colId"].Value);

        Reserva? reserva = _reservaDAO.ObtenerPorId(id);
        if (reserva is null)
        {
            MessageBox.Show("No se encontro la reserva.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AbrirModalReserva(reserva);
    }
}
