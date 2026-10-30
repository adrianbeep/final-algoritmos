namespace SistemaCoworking.Formularios
{
    partial class FrmRecursos
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
            btnCrearCliente = new Button();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            dgvRecursos = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colUbicacion = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            colPrecioDia = new DataGridViewTextBoxColumn();
            colTipoRecurso = new DataGridViewTextBoxColumn();
            colAcciones = new DataGridViewButtonColumn();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecursos).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 70);
            panel1.TabIndex = 1;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnCrearCliente);
            panel5.Dock = DockStyle.Right;
            panel5.Location = new Point(643, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(157, 70);
            panel5.TabIndex = 1;
            // 
            // btnCrearCliente
            // 
            btnCrearCliente.Location = new Point(17, 12);
            btnCrearCliente.Name = "btnCrearCliente";
            btnCrearCliente.Size = new Size(128, 37);
            btnCrearCliente.TabIndex = 0;
            btnCrearCliente.Text = "Nuevo";
            btnCrearCliente.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 380);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 70);
            panel2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 70);
            panel3.Name = "panel3";
            panel3.Size = new Size(35, 310);
            panel3.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Right;
            panel4.Location = new Point(765, 70);
            panel4.Name = "panel4";
            panel4.Size = new Size(35, 310);
            panel4.TabIndex = 4;
            // 
            // dgvRecursos
            // 
            dgvRecursos.BackgroundColor = Color.White;
            dgvRecursos.BorderStyle = BorderStyle.None;
            dgvRecursos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecursos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvRecursos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecursos.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colUbicacion, colEstado, colPrecioDia, colTipoRecurso, colAcciones });
            dgvRecursos.Dock = DockStyle.Fill;
            dgvRecursos.EnableHeadersVisualStyles = false;
            dgvRecursos.GridColor = Color.FromArgb(230, 230, 230);
            dgvRecursos.Location = new Point(35, 70);
            dgvRecursos.MultiSelect = false;
            dgvRecursos.Name = "dgvRecursos";
            dgvRecursos.RowHeadersVisible = false;
            dgvRecursos.RowHeadersWidth = 51;
            dgvRecursos.Size = new Size(730, 310);
            dgvRecursos.TabIndex = 5;
            // 
            // colId
            // 
            colId.DataPropertyName = "id_recurso";
            colId.HeaderText = "Id";
            colId.MinimumWidth = 6;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 125;
            // 
            // colNombre
            // 
            colNombre.DataPropertyName = "recurso_nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            colNombre.Width = 125;
            // 
            // colUbicacion
            // 
            colUbicacion.DataPropertyName = "recurso_ubicacion";
            colUbicacion.HeaderText = "Ubicacion";
            colUbicacion.MinimumWidth = 6;
            colUbicacion.Name = "colUbicacion";
            colUbicacion.ReadOnly = true;
            colUbicacion.Width = 125;
            // 
            // colEstado
            // 
            colEstado.DataPropertyName = "estado";
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            colEstado.Width = 125;
            // 
            // colPrecioDia
            // 
            colPrecioDia.DataPropertyName = "precio_dia";
            colPrecioDia.HeaderText = "Precio Dia";
            colPrecioDia.MinimumWidth = 6;
            colPrecioDia.Name = "colPrecioDia";
            colPrecioDia.ReadOnly = true;
            colPrecioDia.Width = 125;
            // 
            // colTipoRecurso
            // 
            colTipoRecurso.DataPropertyName = "nombre_tipo_recurso";
            colTipoRecurso.HeaderText = "Tipo Recurso";
            colTipoRecurso.MinimumWidth = 6;
            colTipoRecurso.Name = "colTipoRecurso";
            colTipoRecurso.ReadOnly = true;
            colTipoRecurso.Width = 125;
            // 
            // colAcciones
            // 
            colAcciones.HeaderText = "Acciones";
            colAcciones.MinimumWidth = 6;
            colAcciones.Name = "colAcciones";
            colAcciones.ReadOnly = true;
            colAcciones.Text = "Editar";
            colAcciones.UseColumnTextForButtonValue = true;
            colAcciones.Width = 125;
            // 
            // FrmRecursos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvRecursos);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmRecursos";
            StartPosition = FormStartPosition.Manual;
            Text = "FrmRecursos";
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRecursos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel5;
        private Button btnCrearCliente;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private DataGridView dgvRecursos;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colUbicacion;
        private DataGridViewTextBoxColumn colEstado;
        private DataGridViewTextBoxColumn colPrecioDia;
        private DataGridViewTextBoxColumn colTipoRecurso;
        private DataGridViewButtonColumn colAcciones;
    }
}