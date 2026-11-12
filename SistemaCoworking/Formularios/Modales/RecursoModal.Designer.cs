using SistemaCoworking.Formularios.UserControls;

namespace SistemaCoworking.Formularios.Modales
{
    partial class RecursoModal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecursoModal));
            pBoxCerrar = new PictureBox();
            lblId = new Label();
            label8 = new Label();
            lblCrear = new Label();
            btnCancelar = new Button();
            btnGuardar = new Button();
            Label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtNombre = new TextControl();
            txtUbicacion = new TextControl();
            txtCosto = new TextControl();
            label4 = new Label();
            pnSala = new Panel();
            chkProyector = new CheckBox();
            txtCapacidad = new TextControl();
            pnPuestoTrabajo = new Panel();
            txtNumMonitores = new TextControl();
            pnOficinaPrivada = new Panel();
            txtMetros = new TextControl();
            lblPrecio = new Label();
            cmbTipo = new ComboBox();
            cmbEstado = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pBoxCerrar).BeginInit();
            pnSala.SuspendLayout();
            pnPuestoTrabajo.SuspendLayout();
            pnOficinaPrivada.SuspendLayout();
            SuspendLayout();
            // 
            // pBoxCerrar
            // 
            pBoxCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pBoxCerrar.Image = Properties.Resources.icons8_close_24_black;
            pBoxCerrar.Location = new Point(392, 0);
            pBoxCerrar.Name = "pBoxCerrar";
            pBoxCerrar.Size = new Size(48, 41);
            pBoxCerrar.SizeMode = PictureBoxSizeMode.CenterImage;
            pBoxCerrar.TabIndex = 36;
            pBoxCerrar.TabStop = false;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 10F);
            lblId.Location = new Point(241, 41);
            lblId.Name = "lblId";
            lblId.Size = new Size(0, 23);
            lblId.TabIndex = 35;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.FromArgb(0, 99, 200);
            label8.Location = new Point(21, 41);
            label8.Name = "label8";
            label8.Size = new Size(119, 38);
            label8.TabIndex = 34;
            label8.Text = "Recurso";
            // 
            // lblCrear
            // 
            lblCrear.AutoSize = true;
            lblCrear.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCrear.ForeColor = Color.FromArgb(138, 143, 152);
            lblCrear.Location = new Point(21, 10);
            lblCrear.Name = "lblCrear";
            lblCrear.Size = new Size(70, 31);
            lblCrear.TabIndex = 33;
            lblCrear.Text = "Crear";
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom;
            btnCancelar.Location = new Point(256, 443);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(160, 45);
            btnCancelar.TabIndex = 32;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Bottom;
            btnGuardar.Location = new Point(21, 443);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(160, 45);
            btnGuardar.TabIndex = 31;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 10F);
            Label1.Location = new Point(241, 18);
            Label1.Name = "Label1";
            Label1.Size = new Size(25, 23);
            Label1.TabIndex = 26;
            Label1.Text = "Id";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 97);
            label2.Name = "label2";
            label2.Size = new Size(95, 20);
            label2.TabIndex = 38;
            label2.Text = "Tipo Recurso";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(241, 97);
            label3.Name = "label3";
            label3.Size = new Size(54, 20);
            label3.TabIndex = 40;
            label3.Text = "Estado";
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.White;
            txtNombre.LblText = "Nombre";
            txtNombre.Location = new Point(21, 167);
            txtNombre.Name = "txtNombre";
            txtNombre.Placeholder = null;
            txtNombre.Size = new Size(175, 66);
            txtNombre.TabIndex = 41;
            // 
            // txtUbicacion
            // 
            txtUbicacion.BackColor = Color.White;
            txtUbicacion.LblText = "Ubicacion";
            txtUbicacion.Location = new Point(241, 167);
            txtUbicacion.Name = "txtUbicacion";
            txtUbicacion.Placeholder = null;
            txtUbicacion.Size = new Size(175, 66);
            txtUbicacion.TabIndex = 42;
            // 
            // txtCosto
            // 
            txtCosto.BackColor = Color.White;
            txtCosto.LblText = "Costo Base";
            txtCosto.Location = new Point(21, 260);
            txtCosto.Name = "txtCosto";
            txtCosto.Placeholder = null;
            txtCosto.Size = new Size(175, 66);
            txtCosto.TabIndex = 43;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.ForeColor = Color.FromArgb(100, 100, 100);
            label4.Location = new Point(241, 260);
            label4.Name = "label4";
            label4.Size = new Size(174, 20);
            label4.TabIndex = 44;
            label4.Text = "Precio Diario";
            // 
            // pnSala
            // 
            pnSala.Controls.Add(chkProyector);
            pnSala.Controls.Add(txtCapacidad);
            pnSala.Location = new Point(21, 350);
            pnSala.Name = "pnSala";
            pnSala.Size = new Size(395, 73);
            pnSala.TabIndex = 45;
            // 
            // chkProyector
            // 
            chkProyector.Location = new Point(234, 23);
            chkProyector.Name = "chkProyector";
            chkProyector.Size = new Size(126, 30);
            chkProyector.TabIndex = 45;
            chkProyector.Text = "Proyector";
            chkProyector.UseVisualStyleBackColor = true;
            // 
            // txtCapacidad
            // 
            txtCapacidad.BackColor = Color.White;
            txtCapacidad.LblText = "Capacidad";
            txtCapacidad.Location = new Point(0, 4);
            txtCapacidad.Name = "txtCapacidad";
            txtCapacidad.Placeholder = null;
            txtCapacidad.Size = new Size(175, 66);
            txtCapacidad.TabIndex = 44;
            // 
            // pnPuestoTrabajo
            // 
            pnPuestoTrabajo.Controls.Add(txtNumMonitores);
            pnPuestoTrabajo.Location = new Point(21, 350);
            pnPuestoTrabajo.Name = "pnPuestoTrabajo";
            pnPuestoTrabajo.Size = new Size(395, 73);
            pnPuestoTrabajo.TabIndex = 46;
            pnPuestoTrabajo.Visible = false;
            // 
            // txtNumMonitores
            // 
            txtNumMonitores.BackColor = Color.White;
            txtNumMonitores.LblText = "Monitores";
            txtNumMonitores.Location = new Point(0, 4);
            txtNumMonitores.Name = "txtNumMonitores";
            txtNumMonitores.Placeholder = null;
            txtNumMonitores.Size = new Size(175, 66);
            txtNumMonitores.TabIndex = 44;
            // 
            // pnOficinaPrivada
            // 
            pnOficinaPrivada.Controls.Add(txtMetros);
            pnOficinaPrivada.Location = new Point(21, 350);
            pnOficinaPrivada.Name = "pnOficinaPrivada";
            pnOficinaPrivada.Size = new Size(395, 73);
            pnOficinaPrivada.TabIndex = 46;
            // 
            // txtMetros
            // 
            txtMetros.BackColor = Color.White;
            txtMetros.LblText = "Metros Cuadrados";
            txtMetros.Location = new Point(0, 4);
            txtMetros.Name = "txtMetros";
            txtMetros.Placeholder = null;
            txtMetros.Size = new Size(175, 66);
            txtMetros.TabIndex = 44;
            // 
            // lblPrecio
            // 
            lblPrecio.BackColor = Color.White;
            lblPrecio.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrecio.ForeColor = Color.FromArgb(16, 124, 65);
            lblPrecio.Location = new Point(241, 280);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(174, 46);
            lblPrecio.TabIndex = 47;
            lblPrecio.Text = "—";
            lblPrecio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbTipo
            // 
            cmbTipo.FormattingEnabled = true;
            cmbTipo.Location = new Point(21, 122);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.Size = new Size(175, 28);
            cmbTipo.TabIndex = 48;
            // 
            // cmbEstado
            // 
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Location = new Point(241, 120);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(175, 28);
            cmbEstado.TabIndex = 49;
            // 
            // RecursoModal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(440, 500);
            Controls.Add(cmbEstado);
            Controls.Add(cmbTipo);
            Controls.Add(lblPrecio);
            Controls.Add(pnSala);
            Controls.Add(pnPuestoTrabajo);
            Controls.Add(label4);
            Controls.Add(txtCosto);
            Controls.Add(txtUbicacion);
            Controls.Add(txtNombre);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pBoxCerrar);
            Controls.Add(lblId);
            Controls.Add(label8);
            Controls.Add(lblCrear);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(Label1);
            Controls.Add(pnOficinaPrivada);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RecursoModal";
            Text = "Recursos";
            ((System.ComponentModel.ISupportInitialize)pBoxCerrar).EndInit();
            pnSala.ResumeLayout(false);
            pnPuestoTrabajo.ResumeLayout(false);
            pnOficinaPrivada.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pBoxCerrar;
        private Label lblId;
        private Label label8;
        private Label lblCrear;
        private Button btnCancelar;
        private Button btnGuardar;
        private Label Label1;
        private Label label2;
        private Label label3;
        private TextControl txtNombre;
        private TextControl txtUbicacion;
        private TextControl txtCosto;
        private Label label4;
        private Panel pnSala;
        private TextControl txtCapacidad;
        private CheckBox chkProyector;
        private Panel pnPuestoTrabajo;
        private TextControl txtNumMonitores;
        private Panel pnOficinaPrivada;
        private TextControl txtMetros;
        private Label lblPrecio;
        private ComboBox cmbTipo;
        private ComboBox cmbEstado;
    }
}
