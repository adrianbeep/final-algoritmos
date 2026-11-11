namespace SistemaCoworking.Formularios
{
    partial class FrmReservas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel5 = new Panel();
            btnCrearReserva = new Button();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            dgvReservas = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colFechaInicio = new DataGridViewTextBoxColumn();
            colFechaFin = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colDni = new DataGridViewTextBoxColumn();
            colRecursoNombre = new DataGridViewTextBoxColumn();
            colRecursoUbicacion = new DataGridViewTextBoxColumn();
            colTipoRecurso = new DataGridViewTextBoxColumn();
            colEditar = new DataGridViewButtonColumn();
            colCancelar = new DataGridViewButtonColumn();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReservas).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 70);
            panel1.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnCrearReserva);
            panel5.Dock = DockStyle.Right;
            panel5.Location = new Point(643, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(157, 70);
            panel5.TabIndex = 1;
            // 
            // btnCrearReserva
            // 
            btnCrearReserva.Location = new Point(17, 12);
            btnCrearReserva.Name = "btnCrearReserva";
            btnCrearReserva.Size = new Size(128, 37);
            btnCrearReserva.TabIndex = 0;
            btnCrearReserva.Text = "Nuevo";
            btnCrearReserva.UseVisualStyleBackColor = true;
            btnCrearReserva.Click += btnCrearReserva_Click;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 380);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 70);
            panel2.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 70);
            panel3.Name = "panel3";
            panel3.Size = new Size(35, 310);
            panel3.TabIndex = 4;
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Right;
            panel4.Location = new Point(765, 70);
            panel4.Name = "panel4";
            panel4.Size = new Size(35, 310);
            panel4.TabIndex = 5;
            // 
            // dgvReservas
            // 
            dgvReservas.BackgroundColor = Color.White;
            dgvReservas.BorderStyle = BorderStyle.None;
            dgvReservas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReservas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvReservas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReservas.Columns.AddRange(new DataGridViewColumn[] { colId, colFechaInicio, colFechaFin, colEstado, colTotal, colNombre, colDni, colRecursoNombre, colRecursoUbicacion, colTipoRecurso, colEditar, colCancelar });
            dgvReservas.Dock = DockStyle.Fill;
            dgvReservas.EnableHeadersVisualStyles = false;
            dgvReservas.GridColor = Color.FromArgb(230, 230, 230);
            dgvReservas.Location = new Point(35, 70);
            dgvReservas.MultiSelect = false;
            dgvReservas.Name = "dgvReservas";
            dgvReservas.RowHeadersVisible = false;
            dgvReservas.RowHeadersWidth = 51;
            dgvReservas.Size = new Size(730, 310);
            dgvReservas.TabIndex = 6;
            dgvReservas.CellContentClick += dgvReservas_CellContentClick_1;
            dgvReservas.CellFormatting += dgvReservas_CellFormatting;
            // 
            // colId
            // 
            colId.DataPropertyName = "id_reserva";
            colId.HeaderText = "Id";
            colId.MinimumWidth = 6;
            colId.Name = "colId";
            colId.Visible = false;
            colId.ReadOnly = true;
            colId.Width = 125;
            // 
            // colFechaInicio
            // 
            colFechaInicio.DataPropertyName = "fecha_inicio";
            colFechaInicio.HeaderText = "Inicio";
            colFechaInicio.DefaultCellStyle.Format = "dd/MM/yyyy";
            colFechaInicio.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colFechaInicio.FillWeight = 100;
            colFechaInicio.MinimumWidth = 6;
            colFechaInicio.Name = "colFechaInicio";
            colFechaInicio.ReadOnly = true;
            colFechaInicio.Width = 125;
            // 
            // colFechaFin
            // 
            colFechaFin.DataPropertyName = "fecha_fin";
            colFechaFin.HeaderText = "Fin";
            colFechaFin.DefaultCellStyle.Format = "dd/MM/yyyy";
            colFechaFin.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colFechaFin.FillWeight = 100;
            colFechaFin.MinimumWidth = 6;
            colFechaFin.Name = "colFechaFin";
            colFechaFin.ReadOnly = true;
            colFechaFin.Width = 125;
            // 
            // colEstado
            // 
            colEstado.DataPropertyName = "estado_reserva";
            colEstado.HeaderText = "Estado";
            colEstado.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colEstado.FillWeight = 102;
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            colEstado.Width = 125;
            // 
            // colTotal
            // 
            colTotal.DataPropertyName = "total_reserva";
            colTotal.HeaderText = "Total";
            colTotal.DefaultCellStyle.Format = "N0";
            colTotal.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colTotal.FillWeight = 67;
            colTotal.MinimumWidth = 6;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            colTotal.Width = 125;
            // 
            // colNombre
            // 
            colNombre.DataPropertyName = "cliente_nombre";
            colNombre.HeaderText = "Cliente";
            colNombre.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colNombre.FillWeight = 127;
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            colNombre.Width = 125;
            // 
            // colDni
            // 
            colDni.DataPropertyName = "dni";
            colDni.HeaderText = "Dni";
            colDni.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colDni.FillWeight = 88;
            colDni.MinimumWidth = 6;
            colDni.Name = "colDni";
            colDni.ReadOnly = true;
            colDni.Width = 125;
            // 
            // colRecursoNombre
            // 
            colRecursoNombre.DataPropertyName = "recurso_nombre";
            colRecursoNombre.HeaderText = "Recurso";
            colRecursoNombre.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colRecursoNombre.FillWeight = 86;
            colRecursoNombre.MinimumWidth = 6;
            colRecursoNombre.Name = "colRecursoNombre";
            colRecursoNombre.ReadOnly = true;
            colRecursoNombre.Width = 125;
            // 
            // colRecursoUbicacion
            // 
            colRecursoUbicacion.DataPropertyName = "recurso_ubicacion";
            colRecursoUbicacion.HeaderText = "Ubic.";
            colRecursoUbicacion.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colRecursoUbicacion.FillWeight = 95;
            colRecursoUbicacion.MinimumWidth = 6;
            colRecursoUbicacion.Name = "colRecursoUbicacion";
            colRecursoUbicacion.Visible = false;
            colRecursoUbicacion.ReadOnly = true;
            colRecursoUbicacion.Width = 125;
            // 
            // colTipoRecurso
            // 
            colTipoRecurso.DataPropertyName = "nombre_tipo_recurso";
            colTipoRecurso.HeaderText = "Tipo";
            colTipoRecurso.DefaultCellStyle.Padding = new Padding(6, 8, 6, 8);
            colTipoRecurso.FillWeight = 55;
            colTipoRecurso.MinimumWidth = 6;
            colTipoRecurso.Name = "colTipoRecurso";
            colTipoRecurso.Visible = false;
            colTipoRecurso.ReadOnly = true;
            colTipoRecurso.Width = 125;
            // 
            // colEditar
            // 
            colEditar.HeaderText = "Editar";
            colEditar.FillWeight = 69;
            colEditar.MinimumWidth = 6;
            colEditar.Name = "colEditar";
            colEditar.ReadOnly = true;
            colEditar.Width = 125;
            // 
            // colCancelar
            // 
            colCancelar.HeaderText = "Cancelar";
            colCancelar.FillWeight = 91;
            colCancelar.MinimumWidth = 6;
            colCancelar.Name = "colCancelar";
            colCancelar.ReadOnly = true;
            colCancelar.Width = 125;
            // 
            // FrmReservas
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvReservas);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmReservas";
            StartPosition = FormStartPosition.Manual;
            Text = "FrmReservas";
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReservas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel5;
        private Button btnCrearReserva;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private DataGridView dgvReservas;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colFechaInicio;
        private DataGridViewTextBoxColumn colFechaFin;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colDni;
        private DataGridViewTextBoxColumn colRecursoNombre;
        private DataGridViewTextBoxColumn colRecursoUbicacion;
        private DataGridViewTextBoxColumn colTipoRecurso;
        private DataGridViewButtonColumn colEditar;
        private DataGridViewButtonColumn colCancelar;
    }
}