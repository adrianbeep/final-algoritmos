namespace SistemaCoworking.Formularios.Modales
{
    partial class ReservaModal
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
            cmbClientes = new ComboBox();
            label2 = new Label();
            pBoxCerrar = new PictureBox();
            lblId = new Label();
            label8 = new Label();
            lblCrear = new Label();
            btnCancelar = new Button();
            btnGuardar = new Button();
            Label1 = new Label();
            lblClienteInfo = new Label();
            label3 = new Label();
            cmbRecursos = new ComboBox();
            lblRecursoInfo = new Label();
            label4 = new Label();
            dtpInicio = new DateTimePicker();
            label5 = new Label();
            dtpFin = new DateTimePicker();
            lblTotalInfo = new Label();
            lblTotal = new Label();
            lblEstadoInfo = new Label();
            ((System.ComponentModel.ISupportInitialize)pBoxCerrar).BeginInit();
            SuspendLayout();
            // 
            // cmbClientes
            // 
            cmbClientes.FormattingEnabled = true;
            cmbClientes.Location = new Point(12, 141);
            cmbClientes.Name = "cmbClientes";
            cmbClientes.Size = new Size(394, 28);
            cmbClientes.TabIndex = 67;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 116);
            label2.Name = "label2";
            label2.Size = new Size(61, 20);
            label2.TabIndex = 57;
            label2.Text = "Clientes";
            // 
            // pBoxCerrar
            // 
            pBoxCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pBoxCerrar.Image = Properties.Resources.icons8_close_24_black;
            pBoxCerrar.Location = new Point(367, -1);
            pBoxCerrar.Name = "pBoxCerrar";
            pBoxCerrar.Size = new Size(48, 41);
            pBoxCerrar.SizeMode = PictureBoxSizeMode.CenterImage;
            pBoxCerrar.TabIndex = 56;
            pBoxCerrar.TabStop = false;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 10F);
            lblId.Location = new Point(232, 40);
            lblId.Name = "lblId";
            lblId.Size = new Size(0, 23);
            lblId.TabIndex = 55;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.FromArgb(0, 99, 200);
            label8.Location = new Point(12, 40);
            label8.Name = "label8";
            label8.Size = new Size(118, 38);
            label8.TabIndex = 54;
            label8.Text = "Reserva";
            // 
            // lblCrear
            // 
            lblCrear.AutoSize = true;
            lblCrear.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCrear.ForeColor = Color.FromArgb(138, 143, 152);
            lblCrear.Location = new Point(12, 9);
            lblCrear.Name = "lblCrear";
            lblCrear.Size = new Size(70, 31);
            lblCrear.TabIndex = 53;
            lblCrear.Text = "Crear";
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom;
            btnCancelar.Location = new Point(246, 441);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(160, 45);
            btnCancelar.TabIndex = 52;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Bottom;
            btnGuardar.Location = new Point(12, 441);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(160, 45);
            btnGuardar.TabIndex = 51;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 10F);
            Label1.Location = new Point(232, 17);
            Label1.Name = "Label1";
            Label1.Size = new Size(25, 23);
            Label1.TabIndex = 50;
            Label1.Text = "Id";
            // 
            // lblClienteInfo
            // 
            lblClienteInfo.Location = new Point(12, 172);
            lblClienteInfo.Name = "lblClienteInfo";
            lblClienteInfo.Size = new Size(394, 23);
            lblClienteInfo.TabIndex = 68;
            lblClienteInfo.Text = "Clientes";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 211);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 69;
            label3.Text = "Recursos";
            // 
            // cmbRecursos
            // 
            cmbRecursos.FormattingEnabled = true;
            cmbRecursos.Location = new Point(12, 234);
            cmbRecursos.Name = "cmbRecursos";
            cmbRecursos.Size = new Size(394, 28);
            cmbRecursos.TabIndex = 70;
            // 
            // lblRecursoInfo
            // 
            lblRecursoInfo.Location = new Point(12, 265);
            lblRecursoInfo.Name = "lblRecursoInfo";
            lblRecursoInfo.Size = new Size(394, 23);
            lblRecursoInfo.TabIndex = 71;
            lblRecursoInfo.Text = "Clientes";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 310);
            label4.Name = "label4";
            label4.Size = new Size(87, 20);
            label4.TabIndex = 72;
            label4.Text = "Fecha Inicio";
            // 
            // dtpInicio
            // 
            dtpInicio.CustomFormat = "yyyy-MM-dd";
            dtpInicio.Format = DateTimePickerFormat.Custom;
            dtpInicio.Location = new Point(12, 333);
            dtpInicio.Name = "dtpInicio";
            dtpInicio.ShowUpDown = true;
            dtpInicio.Size = new Size(160, 27);
            dtpInicio.TabIndex = 73;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(246, 310);
            label5.Name = "label5";
            label5.Size = new Size(70, 20);
            label5.TabIndex = 74;
            label5.Text = "Fecha Fin";
            // 
            // dtpFin
            // 
            dtpFin.CustomFormat = "yyyy-MM-dd";
            dtpFin.Format = DateTimePickerFormat.Custom;
            dtpFin.Location = new Point(246, 333);
            dtpFin.Name = "dtpFin";
            dtpFin.ShowUpDown = true;
            dtpFin.Size = new Size(160, 27);
            dtpFin.TabIndex = 75;
            // 
            // lblTotalInfo
            // 
            lblTotalInfo.Location = new Point(12, 381);
            lblTotalInfo.Name = "lblTotalInfo";
            lblTotalInfo.Size = new Size(232, 40);
            lblTotalInfo.TabIndex = 76;
            lblTotalInfo.Text = "Clientes";
            // 
            // lblTotal
            // 
            lblTotal.Location = new Point(246, 381);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(160, 40);
            lblTotal.TabIndex = 77;
            lblTotal.Text = "Clientes";
            // 
            // lblEstadoInfo
            // 
            lblEstadoInfo.AutoSize = true;
            lblEstadoInfo.Location = new Point(12, 87);
            lblEstadoInfo.Name = "lblEstadoInfo";
            lblEstadoInfo.Size = new Size(57, 20);
            lblEstadoInfo.TabIndex = 78;
            lblEstadoInfo.Text = "Estado:";
            // 
            // ReservaModal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 493);
            Controls.Add(lblEstadoInfo);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalInfo);
            Controls.Add(dtpFin);
            Controls.Add(label5);
            Controls.Add(dtpInicio);
            Controls.Add(label4);
            Controls.Add(lblRecursoInfo);
            Controls.Add(cmbRecursos);
            Controls.Add(label3);
            Controls.Add(lblClienteInfo);
            Controls.Add(cmbClientes);
            Controls.Add(label2);
            Controls.Add(pBoxCerrar);
            Controls.Add(lblId);
            Controls.Add(label8);
            Controls.Add(lblCrear);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(Label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ReservaModal";
            Text = "Reservas";
            btnGuardar.Click += btnGuardar_Click;
            cmbClientes.SelectedIndexChanged += cmbClientes_SelectedIndexChanged;
            cmbRecursos.SelectedIndexChanged += cmbRecursos_SelectedIndexChanged;
            dtpInicio.ValueChanged += dtpInicio_ValueChanged;
            dtpFin.ValueChanged += dtpFin_ValueChanged;
            Load += ReservaModal_Load;
            ((System.ComponentModel.ISupportInitialize)pBoxCerrar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox cmbClientes;
        private Label label2;
        private PictureBox pBoxCerrar;
        private Label lblId;
        private Label label8;
        private Label lblCrear;
        private Button btnCancelar;
        private Button btnGuardar;
        private Label Label1;
        private Label lblClienteInfo;
        private Label label3;
        private ComboBox cmbRecursos;
        private Label lblRecursoInfo;
        private Label label4;
        private DateTimePicker dtpInicio;
        private Label label5;
        private DateTimePicker dtpFin;
        private Label lblTotalInfo;
        private Label lblTotal;
        private Label lblEstadoInfo;
    }
}