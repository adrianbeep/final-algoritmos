using System;
using System.Windows.Forms;
using SistemaCoworking.Datos;
using SistemaCoworking.Formularios.UserControls;
using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Formularios;

/// <summary>
/// El listado de clientes.
///
/// Es un form embebido en el panel del FrmPrincipal. No edita nada: abre el
/// modal de cliente y espera.
/// </summary>
public partial class FrmClientes : Form
{
    private ClienteDAO _clienteDAO = new ClienteDAO();

    public FrmClientes()
    {
        InitializeComponent();

        DataGridViewConfiguration.ConfigDGV(dgvClientes);
        CargarDataGridViewClientes(dgvClientes);

        ButtonConfiguration.ConfigPrimary(btnCrearCliente);
    }

    private void CargarDataGridViewClientes(DataGridView dgv)
    {
        try
        {
            dgv.DataSource = null;
            dgv.DataSource = _clienteDAO.ObtenerTodos();
            dgv.ClearSelection();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al cargar clientes: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Click en el boton de la fila: abre el modal para editar ese cliente.
    /// </summary>
    private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // Solo el boton de la fila abre el modal, no cualquier celda.
        if (dgvClientes.Columns[e.ColumnIndex] is not DataGridViewButtonColumn) return;

        int id = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["colId"].Value);

        Cliente? cliente = _clienteDAO.ObtenerPorId(id);
        if (cliente is null)
        {
            MessageBox.Show("No se encontro el cliente.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AbrirModalCliente(cliente);
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
    private void AbrirModalCliente(Cliente? cliente = null)
    {
        ClienteModalControl modal = new ClienteModalControl(cliente);

        if (modal.ShowDialog() != DialogResult.OK) return;

        Cliente? datos = modal.Entidad;
        if (datos is null) return;

        if (cliente is null)
        {
            _clienteDAO.Insertar(
                datos.Nombre,
                datos.Apellido,
                datos.Dni,
                datos.Telefono,
                datos.Email);
        }
        else
        {
            _clienteDAO.Modificar(datos);
        }

        CargarDataGridViewClientes(dgvClientes); // refrescar la grilla
    }

    private void btnCrearCliente_Click(object sender, EventArgs e)
    {
        AbrirModalCliente();
    }
}
