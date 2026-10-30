using SistemaCoworking.Datos;
using SistemaCoworking.Herramientas;
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
    private readonly Form _modalOverlay;

    public FrmRecursos(Form? modalOverlay = null)
    {
        InitializeComponent();
        _modalOverlay = modalOverlay ?? new Form { Opacity = 0 };

        DataGridViewConfiguration.ConfigDGV(dgvRecursos);
        CargarDataGridViewRecursos(dgvRecursos);
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
}