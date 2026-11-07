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

public partial class FrmRecursos : Form
{
    private RecursoDAO _recursoDAO = new RecursoDAO();
    private TipoRecursoDAO _tipoRecursoDAO = new TipoRecursoDAO();
    private readonly Form _modalOverlay;

    //Catalogo de tipos de recurso. Se carga una vez y se le pasa al modal:
    //el modal no depende de la capa de datos.
    private List<TipoRecurso> _tiposRecurso = new List<TipoRecurso>();

    public FrmRecursos(Form? modalOverlay = null)
    {
        InitializeComponent();
        _modalOverlay = modalOverlay ?? new Form { Opacity = 0 };

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
            MessageBox.Show("Error al cargar los tipos de recurso: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("Error al cargar recursos: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void dgvRecursos_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        DataGridViewColumn col = dgvRecursos.Columns[e.ColumnIndex];

        // Solo actuar en columnas de tipo botón
        if (col is not DataGridViewButtonColumn) return;

        DataGridViewRow fila = dgvRecursos.Rows[e.RowIndex];
        Recurso? r =
            _recursoDAO.ObtenerPorId(
                Convert.ToInt32(fila.Cells["colId"].Value)
            );

        if (r is null)
        {
            MessageBox.Show("No se encontro el recurso.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AbrirModalRecurso(r);
    }

    //Sin cliente es alta, con cliente es edicion
    private void AbrirModalRecurso(Recurso? recurso = null)
    {
        RecursoModalControl modal = new RecursoModalControl(recurso, _tiposRecurso);

        modal.Closed += (s, args) =>
        {
            ModalOverlay.Close(_modalOverlay);
        };

        modal.Aceptado += (s, r) =>
        {
            if (modal.EsNuevo)
            {
                _recursoDAO.Insertar(r);
            }
            else
            {
                _recursoDAO.Modificar(r);
            }

            CargarDataGridViewRecursos(dgvRecursos); // refrescar la grilla
        };

        ModalOverlay.Show(_modalOverlay, modal);
    }

    private void btnCrearRecurso_Click(object sender, EventArgs e)
    {
        AbrirModalRecurso();
    }
}