using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SistemaCoworking.Datos;
using SistemaCoworking.Formularios.UserControls;
using SistemaCoworking.Herramientas;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Formularios;

/// <summary>
/// El listado de recursos.
///
/// Es un form embebido en el panel del FrmPrincipal. No edita nada: abre el
/// modal de recurso y espera.
/// </summary>
public partial class FrmRecursos : Form
{
    private RecursoDAO _recursoDAO = new RecursoDAO();
    private TipoRecursoDAO _tipoRecursoDAO = new TipoRecursoDAO();

    //Catalogo de tipos de recurso. Se carga una vez y se le pasa al modal:
    //el modal no depende de la capa de datos.
    private List<TipoRecurso> _tiposRecurso = new List<TipoRecurso>();

    public FrmRecursos()
    {
        InitializeComponent();

        DataGridViewConfiguration.ConfigDGV(dgvRecursos);
        CargarDataGridViewRecursos(dgvRecursos);
        CargarTiposRecurso();

        ButtonConfiguration.ConfigPrimary(btnCrearRecurso);
    }

    private void CargarTiposRecurso()
    {
        try
        {
            _tiposRecurso = _tipoRecursoDAO.ObtenerTodos();
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

            _tiposRecurso = new List<TipoRecurso>();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Ocurrio un error inesperado. Intente de nuevo."
                    + Environment.NewLine + Environment.NewLine + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            _tiposRecurso = new List<TipoRecurso>();
        }
    }

    private void CargarDataGridViewRecursos(DataGridView dgv)
    {
        try
        {
            dgv.DataSource = null;
            dgv.DataSource = _recursoDAO.ObtenerTodos();
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
    /// Click en el boton de la fila: abre el modal para editar ese recurso.
    /// </summary>
    private void dgvRecursos_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // Solo el boton de la fila abre el modal, no cualquier celda.
        if (dgvRecursos.Columns[e.ColumnIndex] is not DataGridViewButtonColumn) return;

        int id = Convert.ToInt32(dgvRecursos.Rows[e.RowIndex].Cells["colId"].Value);

        Recurso? recurso;
        try
        {
            recurso = _recursoDAO.ObtenerPorId(id);
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

        if (recurso is null)
        {
            MessageBox.Show("No se encontro el recurso.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AbrirModalRecurso(recurso);
    }

    /// <summary>
    /// Abre el modal de recurso.
    ///
    /// Sin recurso es un ALTA, con recurso es una EDICION: lo decide este
    /// form, que es el que abre el modal.
    ///
    /// El modal escribe en el momento de apretar Guardar, con el DAO que le
    /// pasa este form. Si la escritura falla, el DAO lanza la excepcion, el
    /// modal la captura y muestra el error, y el modal sigue abierto con lo
    /// que cargo el usuario: puede corregir y reintentar.
    /// </summary>
    private void AbrirModalRecurso(Recurso? recurso = null)
    {
        RecursoModal modal = new RecursoModal(recurso, _tiposRecurso, _recursoDAO);

        if (modal.ShowDialog() != DialogResult.OK) return;

        CargarDataGridViewRecursos(dgvRecursos); // refrescar la grilla
    }

    private void btnCrearRecurso_Click(object sender, EventArgs e)
    {
        AbrirModalRecurso();
    }
}
