using MySqlConnector;
using System;
using System.Text.Json;
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
    /// Click en el boton de la fila: abre el modal para editar ese cliente.
    /// </summary>
    private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // Solo el boton de la fila abre el modal, no cualquier celda.
        if (dgvClientes.Columns[e.ColumnIndex] is not DataGridViewButtonColumn) return;

        int id = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["colId"].Value);

        Cliente? cliente;
        try
        {
            cliente = _clienteDAO.ObtenerPorId(id);
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
    /// pasa este form. Si la escritura falla, el DAO lanza la excepcion, el
    /// modal la captura y muestra el error, y el modal sigue abierto con lo
    /// que cargo el usuario: puede corregir y reintentar. Por eso el modal no
    /// devuelve la entidad para que el form
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

    /// <summary>
    /// Exporta el catalogo de clientes a un archivo JSON.
    ///
    /// Usa los objetos reales del modelo (Cliente), no las filas de la grilla:
    /// el JSON sale con los nombres de las propiedades del dominio y no con
    /// los de las columnas de la base. El DataTable de la grilla son filas de
    /// la base, y JsonSerializer no las serializa de forma utilizable.
    ///
    /// El destino se puede pasar para no preguntar. Si no se pasa, se le
    /// pregunta al usuario con un SaveFileDialog.
    /// </summary>
    private void ExportarCatalogoClientes(string? ruta = null)
    {
        try
        {
            List<Cliente> clientes = _clienteDAO.ObtenerTodosComoLista();

            string json = JsonSerializer.Serialize(
                clientes,
                new JsonSerializerOptions { WriteIndented = true });

            if (ruta is null)
            {
                using SaveFileDialog dialogo = new SaveFileDialog
                {
                    Title = "Exportar catalogo de clientes",
                    Filter = "Archivos JSON (*.json)|*.json|Todos los archivos (*.*)|*.*",
                    FileName = "catalogo-clientes.json"
                };

                if (dialogo.ShowDialog() != DialogResult.OK) return;

                ruta = dialogo.FileName;
            }

            File.WriteAllText(ruta, json);

            MessageBox.Show(
                $"Catalogo exportado con {clientes.Count} clientes.\n\n{ruta}",
                "Exportacion correcta",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Ocurrio un error inesperado. Intente de nuevo."
                    + Environment.NewLine + Environment.NewLine + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
