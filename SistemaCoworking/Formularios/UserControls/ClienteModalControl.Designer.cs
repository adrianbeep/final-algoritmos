namespace SistemaCoworking.Formularios.UserControls
{
    partial class ClienteModalControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Label1 = new Label();
            Label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            lblCrear = new Label();
            label8 = new Label();
            panel2 = new Panel();
            pnBarraNombre = new Panel();
            txtNombre = new TextBox();
            panel3 = new Panel();
            pnBarraApellido = new Panel();
            txtApellido = new TextBox();
            panel4 = new Panel();
            pnBarraDni = new Panel();
            txtDni = new TextBox();
            panel5 = new Panel();
            pnBarraTelefono = new Panel();
            txtTelefono = new TextBox();
            panel6 = new Panel();
            pnBarraEmail = new Panel();
            txtEmail = new TextBox();
            lblId = new Label();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 10F);
            Label1.Location = new Point(241, 18);
            Label1.Name = "Label1";
            Label1.Size = new Size(25, 23);
            Label1.TabIndex = 0;
            Label1.Text = "Id";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Font = new Font("Segoe UI", 10F);
            Label2.Location = new Point(21, 101);
            Label2.Name = "Label2";
            Label2.Size = new Size(73, 23);
            Label2.TabIndex = 2;
            Label2.Text = "Nombre";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(241, 101);
            label3.Name = "label3";
            label3.Size = new Size(72, 23);
            label3.TabIndex = 4;
            label3.Text = "Apellido";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(22, 185);
            label4.Name = "label4";
            label4.Size = new Size(36, 23);
            label4.TabIndex = 6;
            label4.Text = "Dni";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.Location = new Point(241, 185);
            label5.Name = "label5";
            label5.Size = new Size(74, 23);
            label5.TabIndex = 8;
            label5.Text = "Telefono";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(21, 267);
            label6.Name = "label6";
            label6.Size = new Size(51, 23);
            label6.TabIndex = 10;
            label6.Text = "Email";
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(21, 367);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(160, 45);
            btnGuardar.TabIndex = 12;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(255, 367);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(160, 45);
            btnCancelar.TabIndex = 13;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // lblCrear
            // 
            lblCrear.AutoSize = true;
            lblCrear.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCrear.ForeColor = Color.FromArgb(138, 143, 152);
            lblCrear.Location = new Point(21, 10);
            lblCrear.Name = "lblCrear";
            lblCrear.Size = new Size(70, 31);
            lblCrear.TabIndex = 14;
            lblCrear.Text = "Crear";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.FromArgb(0, 99, 200);
            label8.Location = new Point(21, 41);
            label8.Name = "label8";
            label8.Size = new Size(108, 38);
            label8.TabIndex = 15;
            label8.Text = "Cliente";
            // 
            // panel2
            // 
            panel2.Controls.Add(pnBarraNombre);
            panel2.Controls.Add(txtNombre);
            panel2.Location = new Point(24, 127);
            panel2.Name = "panel2";
            panel2.Size = new Size(175, 27);
            panel2.TabIndex = 18;
            // 
            // pnBarraNombre
            // 
            pnBarraNombre.Dock = DockStyle.Bottom;
            pnBarraNombre.Location = new Point(0, 23);
            pnBarraNombre.Name = "pnBarraNombre";
            pnBarraNombre.Size = new Size(175, 4);
            pnBarraNombre.TabIndex = 2;
            // 
            // txtNombre
            // 
            txtNombre.BorderStyle = BorderStyle.None;
            txtNombre.Dock = DockStyle.Fill;
            txtNombre.Location = new Point(0, 0);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(175, 20);
            txtNombre.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(pnBarraApellido);
            panel3.Controls.Add(txtApellido);
            panel3.Location = new Point(245, 127);
            panel3.Name = "panel3";
            panel3.Size = new Size(175, 27);
            panel3.TabIndex = 19;
            // 
            // pnBarraApellido
            // 
            pnBarraApellido.Dock = DockStyle.Bottom;
            pnBarraApellido.Location = new Point(0, 23);
            pnBarraApellido.Name = "pnBarraApellido";
            pnBarraApellido.Size = new Size(175, 4);
            pnBarraApellido.TabIndex = 2;
            // 
            // txtApellido
            // 
            txtApellido.BorderStyle = BorderStyle.None;
            txtApellido.Dock = DockStyle.Fill;
            txtApellido.Location = new Point(0, 0);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(175, 20);
            txtApellido.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(pnBarraDni);
            panel4.Controls.Add(txtDni);
            panel4.Location = new Point(24, 211);
            panel4.Name = "panel4";
            panel4.Size = new Size(175, 27);
            panel4.TabIndex = 20;
            // 
            // pnBarraDni
            // 
            pnBarraDni.Dock = DockStyle.Bottom;
            pnBarraDni.Location = new Point(0, 23);
            pnBarraDni.Name = "pnBarraDni";
            pnBarraDni.Size = new Size(175, 4);
            pnBarraDni.TabIndex = 2;
            // 
            // txtDni
            // 
            txtDni.BorderStyle = BorderStyle.None;
            txtDni.Dock = DockStyle.Fill;
            txtDni.Location = new Point(0, 0);
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(175, 20);
            txtDni.TabIndex = 1;
            // 
            // panel5
            // 
            panel5.Controls.Add(pnBarraTelefono);
            panel5.Controls.Add(txtTelefono);
            panel5.Location = new Point(245, 211);
            panel5.Name = "panel5";
            panel5.Size = new Size(175, 27);
            panel5.TabIndex = 21;
            // 
            // pnBarraTelefono
            // 
            pnBarraTelefono.Dock = DockStyle.Bottom;
            pnBarraTelefono.Location = new Point(0, 23);
            pnBarraTelefono.Name = "pnBarraTelefono";
            pnBarraTelefono.Size = new Size(175, 4);
            pnBarraTelefono.TabIndex = 2;
            // 
            // txtTelefono
            // 
            txtTelefono.BorderStyle = BorderStyle.None;
            txtTelefono.Dock = DockStyle.Fill;
            txtTelefono.Location = new Point(0, 0);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(175, 20);
            txtTelefono.TabIndex = 1;
            // 
            // panel6
            // 
            panel6.Controls.Add(pnBarraEmail);
            panel6.Controls.Add(txtEmail);
            panel6.Location = new Point(24, 293);
            panel6.Name = "panel6";
            panel6.Size = new Size(291, 27);
            panel6.TabIndex = 22;
            // 
            // pnBarraEmail
            // 
            pnBarraEmail.Dock = DockStyle.Bottom;
            pnBarraEmail.Location = new Point(0, 23);
            pnBarraEmail.Name = "pnBarraEmail";
            pnBarraEmail.Size = new Size(291, 4);
            pnBarraEmail.TabIndex = 2;
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.None;
            txtEmail.Dock = DockStyle.Fill;
            txtEmail.Location = new Point(0, 0);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(291, 20);
            txtEmail.TabIndex = 1;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 10F);
            lblId.Location = new Point(241, 41);
            lblId.Name = "lblId";
            lblId.Size = new Size(25, 23);
            lblId.TabIndex = 23;
            lblId.Text = "Id";
            // 
            // ClienteModalControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lblId);
            Controls.Add(panel6);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(label8);
            Controls.Add(lblCrear);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(Label2);
            Controls.Add(Label1);
            Name = "ClienteModalControl";
            Size = new Size(440, 428);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Label1;
        private Label Label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblCrear;
        private Label label8;
        private Panel panel2;
        private Panel pnBarraNombre;
        private TextBox txtNombre;
        private Panel panel3;
        private Panel pnBarraApellido;
        private TextBox txtApellido;
        private Panel panel4;
        private Panel pnBarraDni;
        private TextBox txtDni;
        private Panel panel5;
        private Panel pnBarraTelefono;
        private TextBox txtTelefono;
        private Panel panel6;
        private Panel pnBarraEmail;
        private TextBox txtEmail;
        private Label lblId;
    }
}
