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
            Dgwrecursos = new DataGridView();
            Tipoderecurso = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            panel2 = new Panel();
            button2 = new Button();
            button1 = new Button();
            btnNuevo = new Button();
            lblNumeroMonitor = new Label();
            CkbSi = new CheckBox();
            lblTieneProyector = new Label();
            Numericgeneral = new NumericUpDown();
            lblGenral = new Label();
            textBox2 = new TextBox();
            comboBox1 = new ComboBox();
            textBox1 = new TextBox();
            TxtNombre = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            CboTipo = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)Dgwrecursos).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Numericgeneral).BeginInit();
            SuspendLayout();
            // 
            // Dgwrecursos
            // 
            Dgwrecursos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Dgwrecursos.Columns.AddRange(new DataGridViewColumn[] { Tipoderecurso });
            Dgwrecursos.Location = new Point(1, 12);
            Dgwrecursos.Name = "Dgwrecursos";
            Dgwrecursos.RowHeadersWidth = 51;
            Dgwrecursos.Size = new Size(179, 434);
            Dgwrecursos.TabIndex = 0;
            // 
            // Tipoderecurso
            // 
            Tipoderecurso.HeaderText = "Tipo de Recurso";
            Tipoderecurso.MinimumWidth = 6;
            Tipoderecurso.Name = "Tipoderecurso";
            Tipoderecurso.Width = 125;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(lblNumeroMonitor);
            panel1.Controls.Add(CkbSi);
            panel1.Controls.Add(lblTieneProyector);
            panel1.Controls.Add(Numericgeneral);
            panel1.Controls.Add(lblGenral);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(TxtNombre);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(CboTipo);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(183, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(617, 450);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(btnNuevo);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 400);
            panel2.Name = "panel2";
            panel2.Size = new Size(617, 50);
            panel2.TabIndex = 15;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(253, 5);
            button2.Name = "button2";
            button2.Size = new Size(102, 38);
            button2.TabIndex = 2;
            button2.Text = "Cancelar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(459, 5);
            button1.Name = "button1";
            button1.Size = new Size(102, 38);
            button1.TabIndex = 1;
            button1.Text = "Guardar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnNuevo
            // 
            btnNuevo.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNuevo.Location = new Point(32, 5);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(102, 38);
            btnNuevo.TabIndex = 0;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            // 
            // lblNumeroMonitor
            // 
            lblNumeroMonitor.AutoSize = true;
            lblNumeroMonitor.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNumeroMonitor.Location = new Point(32, 278);
            lblNumeroMonitor.Name = "lblNumeroMonitor";
            lblNumeroMonitor.Size = new Size(0, 25);
            lblNumeroMonitor.TabIndex = 14;
            // 
            // CkbSi
            // 
            CkbSi.AutoSize = true;
            CkbSi.Enabled = false;
            CkbSi.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CkbSi.Location = new Point(177, 331);
            CkbSi.Name = "CkbSi";
            CkbSi.Size = new Size(45, 27);
            CkbSi.TabIndex = 13;
            CkbSi.Text = "Si";
            CkbSi.UseVisualStyleBackColor = true;
            // 
            // lblTieneProyector
            // 
            lblTieneProyector.AutoSize = true;
            lblTieneProyector.Enabled = false;
            lblTieneProyector.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTieneProyector.Location = new Point(32, 330);
            lblTieneProyector.Name = "lblTieneProyector";
            lblTieneProyector.Size = new Size(139, 25);
            lblTieneProyector.TabIndex = 12;
            lblTieneProyector.Text = "Tiene proyector:";
            // 
            // Numericgeneral
            // 
            Numericgeneral.Location = new Point(237, 279);
            Numericgeneral.Name = "Numericgeneral";
            Numericgeneral.Size = new Size(118, 27);
            Numericgeneral.TabIndex = 11;
            // 
            // lblGenral
            // 
            lblGenral.AutoSize = true;
            lblGenral.Enabled = false;
            lblGenral.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGenral.Location = new Point(30, 278);
            lblGenral.Name = "lblGenral";
            lblGenral.Size = new Size(0, 25);
            lblGenral.TabIndex = 10;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(32, 221);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(153, 30);
            textBox2.TabIndex = 9;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Sala", "PuestoTrabajo", "OficinaPrivada" });
            comboBox1.Location = new Point(31, 131);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(152, 31);
            comboBox1.TabIndex = 8;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(225, 131);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(336, 30);
            textBox1.TabIndex = 7;
            // 
            // TxtNombre
            // 
            TxtNombre.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtNombre.Location = new Point(224, 58);
            TxtNombre.Name = "TxtNombre";
            TxtNombre.Size = new Size(336, 30);
            TxtNombre.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(31, 189);
            label5.Name = "label5";
            label5.Size = new Size(93, 25);
            label5.TabIndex = 5;
            label5.Text = "Precio dia:";
            label5.Click += label5_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(30, 98);
            label4.Name = "label4";
            label4.Size = new Size(70, 25);
            label4.TabIndex = 4;
            label4.Text = "Estado:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(225, 98);
            label3.Name = "label3";
            label3.Size = new Size(93, 25);
            label3.TabIndex = 3;
            label3.Text = "Ubicacion:";
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(224, 30);
            label2.Name = "label2";
            label2.Size = new Size(82, 25);
            label2.TabIndex = 2;
            label2.Text = "Nombre:";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(30, 28);
            label1.Name = "label1";
            label1.Size = new Size(139, 25);
            label1.TabIndex = 1;
            label1.Text = "Tipo de recurso:";
            // 
            // CboTipo
            // 
            CboTipo.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CboTipo.FormattingEnabled = true;
            CboTipo.Items.AddRange(new object[] { "Sala", "PuestoTrabajo", "OficinaPrivada" });
            CboTipo.Location = new Point(31, 59);
            CboTipo.Name = "CboTipo";
            CboTipo.Size = new Size(152, 31);
            CboTipo.TabIndex = 0;
            CboTipo.SelectedIndexChanged += CboTipo_SelectedIndexChanged;
            // 
            // FrmRecursos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(Dgwrecursos);
            Name = "FrmRecursos";
            Text = "FrmRecursos";
            ((System.ComponentModel.ISupportInitialize)Dgwrecursos).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Numericgeneral).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView Dgwrecursos;
        private DataGridViewTextBoxColumn Tipoderecurso;
        private Panel panel1;
        private Label label1;
        private ComboBox CboTipo;
        private Label label2;
        private Label label5;
        private Label label4;
        private Label label3;
        private ComboBox comboBox1;
        private TextBox textBox1;
        private TextBox TxtNombre;
        private TextBox textBox2;
        private NumericUpDown Numericgeneral;
        private Label lblGenral;
        private Label lblNumeroMonitor;
        private CheckBox CkbSi;
        private Label lblTieneProyector;
        private Panel panel2;
        private Button button1;
        private Button btnNuevo;
        private Button button2;
    }
}