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
            // El DAO lanza y aca se captura: se muestra el error y la grilla
            // queda vacia, pero el usuario ya sabe que fallo la consulta.
            ErroresDeBaseDeDatos.Mostrar(ex);
            dgv.DataSource = null;
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
    /// Abre el modal de cliente.
    ///
    /// Sin cliente es un ALTA, con cliente es una EDICION: lo decide este
    /// form, que es el que abre el modal.
    ///
    /// El modal escribe en el momento de apretar Guardar, con el DAO que le
    /// pasa este form. Si la escritura falla, el DAO ya mostro el error y el
    /// modal sigue abierto con lo que cargo el usuario: puede corregir y
    /// reintentar. Por eso el modal no devuelve la entidad para que el form
    /// la escriba DESPUES: ShowDialog() solo vuelve cuando el modal se cerro,
    /// y para entonces los datos ya se perdieron.
    /// </summary>
    private void AbrirModalCliente(Cliente? cliente = null)
    {
        ClienteModal modal = new ClienteModal(cliente, _clienteDAO);

        if (modal.ShowDialog() != DialogResult.OK) return;

        CargarDataGridViewClientes(dgvClientes); // refrescar la grilla
    }

    private void btnCrearCliente_Click(object sender, EventArgs e)
    {
        AbrirModalCliente();
    }
}
