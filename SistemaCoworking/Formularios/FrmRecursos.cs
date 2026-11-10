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
        catch (Exception ex)
        {
            // El DAO lanza y aca se captura: se muestra el error y el combo
            // de tipos queda vacio.
            ErroresDeBaseDeDatos.Mostrar(ex);
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
        catch (Exception ex)
        {
            // El DAO lanza y aca se captura: se muestra el error y la grilla
            // queda vacia, pero el usuario ya sabe que fallo la consulta.
            ErroresDeBaseDeDatos.Mostrar(ex);
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

        Recurso? recurso = _recursoDAO.ObtenerPorId(id);
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
    /// pasa este form. Si la escritura falla, el DAO ya mostro el error y el
    /// modal sigue abierto con lo que cargo el usuario: puede corregir y
    /// reintentar.
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
