namespace SistemaCoworking.Formularios
{
    partial class FrmClientes
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
            pnDataGridView = new Panel();
            dgvClientes = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colDni = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colAcciones = new DataGridViewButtonColumn();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            pnDataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 70);
            panel1.TabIndex = 0;
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
            btnCrearCliente.Click += btnCrearCliente_Click;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 380);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 70);
            panel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 70);
            panel3.Name = "panel3";
            panel3.Size = new Size(35, 310);
            panel3.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Right;
            panel4.Location = new Point(765, 70);
            panel4.Name = "panel4";
            panel4.Size = new Size(35, 310);
            panel4.TabIndex = 3;
            // 
            // pnDataGridView
            // 
            pnDataGridView.Controls.Add(dgvClientes);
            pnDataGridView.Dock = DockStyle.Fill;
            pnDataGridView.Location = new Point(35, 70);
            pnDataGridView.Name = "pnDataGridView";
            pnDataGridView.Size = new Size(730, 310);
            pnDataGridView.TabIndex = 4;
            // 
            // dgvClientes
            // 
            dgvClientes.BackgroundColor = Color.White;
            dgvClientes.BorderStyle = BorderStyle.None;
            dgvClientes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvClientes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colApellido, colDni, colTelefono, colEmail, colAcciones });
            dgvClientes.Dock = DockStyle.Fill;
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.GridColor = Color.FromArgb(230, 230, 230);
            dgvClientes.Location = new Point(0, 0);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.RowHeadersWidth = 51;
            dgvClientes.Size = new Size(730, 310);
            dgvClientes.TabIndex = 0;
            dgvClientes.CellContentClick += dgvClientes_CellContentClick;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "Id";
            colId.MinimumWidth = 6;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 125;
            // 
            // colNombre
            // 
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            colNombre.Width = 125;
            // 
            // colApellido
            // 
            colApellido.DataPropertyName = "Apellido";
            colApellido.HeaderText = "Apellido";
            colApellido.MinimumWidth = 6;
            colApellido.Name = "colApellido";
            colApellido.ReadOnly = true;
            colApellido.Width = 125;
            // 
            // colDni
            // 
            colDni.DataPropertyName = "Dni";
            colDni.HeaderText = "Dni";
            colDni.MinimumWidth = 6;
            colDni.Name = "colDni";
            colDni.ReadOnly = true;
            colDni.Width = 125;
            // 
            // colTelefono
            // 
            colTelefono.DataPropertyName = "Telefono";
            colTelefono.HeaderText = "Telefono";
            colTelefono.MinimumWidth = 6;
            colTelefono.Name = "colTelefono";
            colTelefono.ReadOnly = true;
            colTelefono.Width = 125;
            // 
            // colEmail
            // 
            colEmail.DataPropertyName = "Email";
            colEmail.HeaderText = "Email";
            colEmail.MinimumWidth = 6;
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            colEmail.Width = 125;
            // 
            // colAcciones
            // 
            colAcciones.HeaderText = "Acciones";
            colAcciones.MinimumWidth = 6;
            colAcciones.Name = "colAcciones";
            colAcciones.Text = "Editar";
            colAcciones.UseColumnTextForButtonValue = true;
            colAcciones.Width = 125;
            // 
            // FrmClientes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnDataGridView);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmClientes";
            StartPosition = FormStartPosition.Manual;
            Text = "FrmClientes";
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            pnDataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel pnDataGridView;
        private DataGridView dgvClientes;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colDni;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewButtonColumn colAcciones;
        private Button btnCrearCliente;
        private Panel panel5;
    }
}