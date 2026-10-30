using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SistemaCoworking;
using SistemaCoworking.Datos;
using SistemaCoworking.Formularios.UserControls;
using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Formularios;

public partial class FrmClientes : Form
{
    private ClienteDAO _clienteDAO = new ClienteDAO();
    private readonly Form _modalOverlay;

    public FrmClientes(Form? modalOverlay = null)
    {
        InitializeComponent();
        _modalOverlay = modalOverlay ?? new Form { Opacity = 0 };

        DataGridViewConfiguration.ConfigDGV(dgvClientes);
        CargarDataGridViewClientes(dgvClientes);

        ButtonConfiguration.ConfigPrimary(btnCrearCliente);
    }

    private void FrmClientes_Load(object sender, EventArgs e)
    {
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

    private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        DataGridViewColumn col = dgvClientes.Columns[e.ColumnIndex];

        // Solo actuar en columnas de tipo botón
        if (col is not DataGridViewButtonColumn) return;

        DataGridViewRow fila = dgvClientes.Rows[e.RowIndex];
        Cliente? c =
            _clienteDAO.ObtenerPorId(
                Convert.ToInt32(fila.Cells["colId"].Value)
            );

        if (c is null)
        {
            MessageBox.Show("No se encontro el cliente.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AbrirModalUsuario(c);
    }

    //Sin cliente es alta, con cliente es edicion
    private void AbrirModalUsuario(Cliente? cliente = null)
    {
        ClienteModalControl modal = new ClienteModalControl(cliente);

        modal.Closed += (s, args) =>
        {
            ModalOverlay.Close(_modalOverlay);
        };

        modal.Aceptado += (s, c) =>
        {
            if (modal.EsNuevo)
            {
                _clienteDAO.Insertar(
                    c.Nombre,
                    c.Apellido,
                    c.Dni,
                    c.Telefono,
                    c.Email);
            }
            else
            {
                _clienteDAO.Modificar(c);
            }

            CargarDataGridViewClientes(dgvClientes); // refrescar la grilla
        };

        ModalOverlay.Show(_modalOverlay, modal);
    }

    private void btnCrearCliente_Click(object sender, EventArgs e)
    {
        AbrirModalUsuario();
    }
}
