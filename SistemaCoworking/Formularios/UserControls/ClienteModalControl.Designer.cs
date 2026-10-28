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
            btnGuardar = new Button();
            btnCancelar = new Button();
            lblCrear = new Label();
            label8 = new Label();
            lblId = new Label();
            txtNombre = new TextControl();
            txtApellido = new TextControl();
            txtDni = new TextControl();
            txtTelefono = new TextControl();
            txtEmail = new TextControl();
            pBoxCerrar = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pBoxCerrar).BeginInit();
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
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Bottom;
            btnGuardar.Location = new Point(21, 367);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(160, 45);
            btnGuardar.TabIndex = 5;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom;
            btnCancelar.Location = new Point(255, 367);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(160, 45);
            btnCancelar.TabIndex = 6;
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
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 10F);
            lblId.Location = new Point(241, 41);
            lblId.Name = "lblId";
            lblId.Size = new Size(0, 23);
            lblId.TabIndex = 23;
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.White;
            txtNombre.LblText = null;
            txtNombre.Location = new Point(24, 101);
            txtNombre.Name = "txtNombre";
            txtNombre.Placeholder = null;
            txtNombre.Size = new Size(175, 54);
            txtNombre.TabIndex = 0;
            // 
            // txtApellido
            // 
            txtApellido.BackColor = Color.White;
            txtApellido.LblText = null;
            txtApellido.Location = new Point(245, 101);
            txtApellido.Name = "txtApellido";
            txtApellido.Placeholder = null;
            txtApellido.Size = new Size(175, 54);
            txtApellido.TabIndex = 1;
            // 
            // txtDni
            // 
            txtDni.BackColor = Color.White;
            txtDni.LblText = null;
            txtDni.Location = new Point(24, 185);
            txtDni.Name = "txtDni";
            txtDni.Placeholder = null;
            txtDni.Size = new Size(175, 54);
            txtDni.TabIndex = 2;
            // 
            // txtTelefono
            // 
            txtTelefono.BackColor = Color.White;
            txtTelefono.LblText = null;
            txtTelefono.Location = new Point(245, 185);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Placeholder = null;
            txtTelefono.Size = new Size(175, 54);
            txtTelefono.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.White;
            txtEmail.LblText = null;
            txtEmail.Location = new Point(24, 267);
            txtEmail.Name = "txtEmail";
            txtEmail.Placeholder = null;
            txtEmail.Size = new Size(294, 54);
            txtEmail.TabIndex = 4;
            // 
            // pBoxCerrar
            // 
            pBoxCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pBoxCerrar.Image = Properties.Resources.icons8_close_24_black;
            pBoxCerrar.Location = new Point(392, 0);
            pBoxCerrar.Name = "pBoxCerrar";
            pBoxCerrar.Size = new Size(48, 41);
            pBoxCerrar.SizeMode = PictureBoxSizeMode.CenterImage;
            pBoxCerrar.TabIndex = 24;
            pBoxCerrar.TabStop = false;
            pBoxCerrar.Click += pBoxCerrar_Click;
            // 
            // ClienteModalControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pBoxCerrar);
            Controls.Add(txtEmail);
            Controls.Add(txtTelefono);
            Controls.Add(txtDni);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(lblId);
            Controls.Add(label8);
            Controls.Add(lblCrear);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(Label1);
            Name = "ClienteModalControl";
            Size = new Size(440, 428);
            ((System.ComponentModel.ISupportInitialize)pBoxCerrar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Label1;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblCrear;
        private Label label8;
        private Label lblId;
        private TextControl txtNombre;
        private TextControl txtApellido;
        private TextControl txtEmail;
        private TextControl txtDni;
        private TextControl txtTelefono;
        private PictureBox pBoxCerrar;
    }
}
