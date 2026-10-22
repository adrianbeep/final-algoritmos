namespace SistemaCoworking
{
    partial class FrmPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            panel1 = new Panel();
            nightControlBox1 = new ReaLTaiizor.Controls.NightControlBox();
            label1 = new Label();
            btnHamburguesa = new PictureBox();
            flpSidebar = new FlowLayoutPanel();
            pnDashboard = new Panel();
            btnDashboard = new Button();
            pnClientes = new Panel();
            btnClientes = new Button();
            pnRecursos = new Panel();
            btnRecursos = new Button();
            pnReservas = new Panel();
            btnReservas = new Button();
            pnModal = new Panel();
            sidebarTransicion = new System.Windows.Forms.Timer(components);
            pnForms = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnHamburguesa).BeginInit();
            flpSidebar.SuspendLayout();
            pnDashboard.SuspendLayout();
            pnClientes.SuspendLayout();
            pnRecursos.SuspendLayout();
            pnReservas.SuspendLayout();
            pnForms.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(nightControlBox1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnHamburguesa);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1227, 45);
            panel1.TabIndex = 0;
            // 
            // nightControlBox1
            // 
            nightControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nightControlBox1.BackColor = Color.Transparent;
            nightControlBox1.CloseHoverColor = Color.FromArgb(199, 80, 80);
            nightControlBox1.CloseHoverForeColor = Color.White;
            nightControlBox1.DefaultLocation = true;
            nightControlBox1.DisableMaximizeColor = Color.FromArgb(105, 105, 105);
            nightControlBox1.DisableMinimizeColor = Color.FromArgb(105, 105, 105);
            nightControlBox1.EnableCloseColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.EnableMaximizeButton = true;
            nightControlBox1.EnableMaximizeColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.EnableMinimizeButton = true;
            nightControlBox1.EnableMinimizeColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.Location = new Point(1088, 0);
            nightControlBox1.MaximizeHoverColor = Color.FromArgb(15, 255, 255, 255);
            nightControlBox1.MaximizeHoverForeColor = Color.White;
            nightControlBox1.MinimizeHoverColor = Color.FromArgb(15, 255, 255, 255);
            nightControlBox1.MinimizeHoverForeColor = Color.White;
            nightControlBox1.Name = "nightControlBox1";
            nightControlBox1.Size = new Size(139, 31);
            nightControlBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(67, 9);
            label1.Name = "label1";
            label1.Size = new Size(296, 23);
            label1.TabIndex = 2;
            label1.Text = "GESTION DE RESERVAS COWORKING";
            // 
            // btnHamburguesa
            // 
            btnHamburguesa.Image = (Image)resources.GetObject("btnHamburguesa.Image");
            btnHamburguesa.Location = new Point(3, 4);
            btnHamburguesa.Name = "btnHamburguesa";
            btnHamburguesa.Size = new Size(47, 38);
            btnHamburguesa.SizeMode = PictureBoxSizeMode.CenterImage;
            btnHamburguesa.TabIndex = 1;
            btnHamburguesa.TabStop = false;
            btnHamburguesa.Click += btnHamburguesa_Click;
            // 
            // flpSidebar
            // 
            flpSidebar.BackColor = Color.FromArgb(23, 24, 29);
            flpSidebar.Controls.Add(pnDashboard);
            flpSidebar.Controls.Add(pnClientes);
            flpSidebar.Controls.Add(pnRecursos);
            flpSidebar.Controls.Add(pnReservas);
            flpSidebar.Dock = DockStyle.Left;
            flpSidebar.FlowDirection = FlowDirection.TopDown;
            flpSidebar.Location = new Point(0, 45);
            flpSidebar.Name = "flpSidebar";
            flpSidebar.Size = new Size(250, 720);
            flpSidebar.TabIndex = 1;
            // 
            // pnDashboard
            // 
            pnDashboard.Controls.Add(btnDashboard);
            pnDashboard.Location = new Point(3, 3);
            pnDashboard.Name = "pnDashboard";
            pnDashboard.Size = new Size(253, 63);
            pnDashboard.TabIndex = 2;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(23, 24, 29);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Image = (Image)resources.GetObject("btnDashboard.Image");
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(-15, -15);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(25, 0, 0, 0);
            btnDashboard.Size = new Size(278, 91);
            btnDashboard.TabIndex = 3;
            btnDashboard.Text = "          Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // pnClientes
            // 
            pnClientes.Controls.Add(btnClientes);
            pnClientes.Location = new Point(3, 72);
            pnClientes.Name = "pnClientes";
            pnClientes.Size = new Size(253, 63);
            pnClientes.TabIndex = 3;
            // 
            // btnClientes
            // 
            btnClientes.BackColor = Color.FromArgb(23, 24, 29);
            btnClientes.ForeColor = Color.White;
            btnClientes.Image = (Image)resources.GetObject("btnClientes.Image");
            btnClientes.ImageAlign = ContentAlignment.MiddleLeft;
            btnClientes.Location = new Point(-15, -15);
            btnClientes.Name = "btnClientes";
            btnClientes.Padding = new Padding(25, 0, 0, 0);
            btnClientes.Size = new Size(278, 91);
            btnClientes.TabIndex = 3;
            btnClientes.Text = "          Clientes";
            btnClientes.TextAlign = ContentAlignment.MiddleLeft;
            btnClientes.UseVisualStyleBackColor = false;
            btnClientes.Click += btnClientes_Click;
            // 
            // pnRecursos
            // 
            pnRecursos.Controls.Add(btnRecursos);
            pnRecursos.Location = new Point(3, 141);
            pnRecursos.Name = "pnRecursos";
            pnRecursos.Size = new Size(253, 63);
            pnRecursos.TabIndex = 4;
            // 
            // btnRecursos
            // 
            btnRecursos.BackColor = Color.FromArgb(23, 24, 29);
            btnRecursos.ForeColor = Color.White;
            btnRecursos.Image = (Image)resources.GetObject("btnRecursos.Image");
            btnRecursos.ImageAlign = ContentAlignment.MiddleLeft;
            btnRecursos.Location = new Point(-15, -15);
            btnRecursos.Name = "btnRecursos";
            btnRecursos.Padding = new Padding(25, 0, 0, 0);
            btnRecursos.Size = new Size(278, 91);
            btnRecursos.TabIndex = 3;
            btnRecursos.Text = "          Recursos";
            btnRecursos.TextAlign = ContentAlignment.MiddleLeft;
            btnRecursos.UseVisualStyleBackColor = false;
            btnRecursos.Click += btnRecursos_Click;
            // 
            // pnReservas
            // 
            pnReservas.Controls.Add(btnReservas);
            pnReservas.Location = new Point(3, 210);
            pnReservas.Name = "pnReservas";
            pnReservas.Size = new Size(253, 63);
            pnReservas.TabIndex = 5;
            // 
            // btnReservas
            // 
            btnReservas.BackColor = Color.FromArgb(23, 24, 29);
            btnReservas.ForeColor = Color.White;
            btnReservas.Image = (Image)resources.GetObject("btnReservas.Image");
            btnReservas.ImageAlign = ContentAlignment.MiddleLeft;
            btnReservas.Location = new Point(-15, -15);
            btnReservas.Name = "btnReservas";
            btnReservas.Padding = new Padding(25, 0, 0, 0);
            btnReservas.Size = new Size(278, 91);
            btnReservas.TabIndex = 3;
            btnReservas.Text = "          Reservas";
            btnReservas.TextAlign = ContentAlignment.MiddleLeft;
            btnReservas.UseVisualStyleBackColor = false;
            btnReservas.Click += btnReservas_Click;
            // 
            // pnModal
            // 
            pnModal.Location = new Point(-238, 0);
            pnModal.Name = "pnModal";
            pnModal.Size = new Size(1212, 650);
            pnModal.TabIndex = 0;
            pnModal.Visible = false;
            // 
            // sidebarTransicion
            // 
            sidebarTransicion.Interval = 10;
            sidebarTransicion.Tick += sidebarTransicion_Tick;
            // 
            // pnForms
            // 
            pnForms.Controls.Add(pnModal);
            pnForms.Dock = DockStyle.Fill;
            pnForms.Location = new Point(250, 45);
            pnForms.Name = "pnForms";
            pnForms.Size = new Size(977, 720);
            pnForms.TabIndex = 2;
            // 
            // FrmPrincipal
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1227, 765);
            Controls.Add(pnForms);
            Controls.Add(flpSidebar);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)btnHamburguesa).EndInit();
            flpSidebar.ResumeLayout(false);
            pnDashboard.ResumeLayout(false);
            pnClientes.ResumeLayout(false);
            pnRecursos.ResumeLayout(false);
            pnReservas.ResumeLayout(false);
            pnForms.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private PictureBox btnHamburguesa;
        private FlowLayoutPanel flpSidebar;
        private ReaLTaiizor.Controls.NightControlBox nightControlBox1;
        private Panel pnDashboard;
        private Button btnDashboard;
        private Panel pnClientes;
        private Button btnClientes;
        private Panel pnRecursos;
        private Button btnRecursos;
        private Panel pnReservas;
        private Button btnReservas;
        private System.Windows.Forms.Timer sidebarTransicion;
        private Panel pnForms;
        private Panel pnModal;
    }
}
