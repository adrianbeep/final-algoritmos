namespace SistemaCoworking.Formularios
{
    partial class FrmDashboard
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
            panelEncabezado = new Panel();
            lblTitulo = new Label();
            btnActualizar = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            card1 = new Panel();
            lblValorCard1 = new Label();
            lblTituloCard1 = new Label();
            lblDetalleCard1 = new Label();
            card2 = new Panel();
            lblValorCard2 = new Label();
            lblTituloCard2 = new Label();
            lblDetalleCard2 = new Label();
            card3 = new Panel();
            lblValorCard3 = new Label();
            lblTituloCard3 = new Label();
            lblDetalleCard3 = new Label();
            card4 = new Panel();
            lblValorCard4 = new Label();
            lblTituloCard4 = new Label();
            lblDetalleCard4 = new Label();
            panelEncabezado.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            card1.SuspendLayout();
            card2.SuspendLayout();
            card3.SuspendLayout();
            card4.SuspendLayout();
            SuspendLayout();
            // 
            // panelEncabezado
            // 
            panelEncabezado.BackColor = Color.White;
            panelEncabezado.Controls.Add(lblTitulo);
            panelEncabezado.Controls.Add(btnActualizar);
            panelEncabezado.Dock = DockStyle.Top;
            panelEncabezado.Location = new Point(0, 0);
            panelEncabezado.Name = "panelEncabezado";
            panelEncabezado.Padding = new Padding(24, 0, 24, 0);
            panelEncabezado.Size = new Size(800, 70);
            panelEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI Semibold", 15F);
            lblTitulo.ForeColor = Color.FromArgb(40, 40, 40);
            lblTitulo.Location = new Point(24, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(632, 70);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Metricas del sistema";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnActualizar
            // 
            btnActualizar.Dock = DockStyle.Right;
            btnActualizar.Location = new Point(656, 0);
            btnActualizar.Margin = new Padding(12, 17, 0, 17);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(120, 70);
            btnActualizar.TabIndex = 1;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Controls.Add(card1);
            flowLayoutPanel1.Controls.Add(card2);
            flowLayoutPanel1.Controls.Add(card3);
            flowLayoutPanel1.Controls.Add(card4);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 70);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(12);
            flowLayoutPanel1.Size = new Size(800, 380);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // card1
            // 
            card1.BackColor = Color.White;
            card1.BorderStyle = BorderStyle.FixedSingle;
            card1.Controls.Add(lblValorCard1);
            card1.Controls.Add(lblTituloCard1);
            card1.Controls.Add(lblDetalleCard1);
            card1.Location = new Point(24, 24);
            card1.Margin = new Padding(12);
            card1.Name = "card1";
            card1.Size = new Size(210, 140);
            card1.TabIndex = 0;
            // 
            // lblValorCard1
            // 
            lblValorCard1.Dock = DockStyle.Fill;
            lblValorCard1.Font = new Font("Segoe UI Semibold", 26F);
            lblValorCard1.ForeColor = Color.FromArgb(0, 120, 215);
            lblValorCard1.Location = new Point(0, 34);
            lblValorCard1.Name = "lblValorCard1";
            lblValorCard1.Padding = new Padding(16, 0, 0, 0);
            lblValorCard1.Size = new Size(208, 74);
            lblValorCard1.TabIndex = 1;
            lblValorCard1.Text = "-";
            lblValorCard1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTituloCard1
            // 
            lblTituloCard1.Dock = DockStyle.Top;
            lblTituloCard1.Font = new Font("Segoe UI", 9.5F);
            lblTituloCard1.ForeColor = Color.FromArgb(138, 143, 152);
            lblTituloCard1.Location = new Point(0, 0);
            lblTituloCard1.Name = "lblTituloCard1";
            lblTituloCard1.Padding = new Padding(16, 0, 0, 0);
            lblTituloCard1.Size = new Size(208, 34);
            lblTituloCard1.TabIndex = 0;
            lblTituloCard1.Text = "Total de clientes";
            lblTituloCard1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDetalleCard1
            // 
            lblDetalleCard1.BackColor = Color.White;
            lblDetalleCard1.Dock = DockStyle.Bottom;
            lblDetalleCard1.Font = new Font("Segoe UI", 9F);
            lblDetalleCard1.ForeColor = Color.FromArgb(138, 143, 152);
            lblDetalleCard1.Location = new Point(0, 108);
            lblDetalleCard1.Name = "lblDetalleCard1";
            lblDetalleCard1.Padding = new Padding(16, 0, 0, 0);
            lblDetalleCard1.Size = new Size(208, 30);
            lblDetalleCard1.TabIndex = 2;
            lblDetalleCard1.Text = "Clientes registrados";
            lblDetalleCard1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // card2
            // 
            card2.BackColor = Color.White;
            card2.BorderStyle = BorderStyle.FixedSingle;
            card2.Controls.Add(lblValorCard2);
            card2.Controls.Add(lblTituloCard2);
            card2.Controls.Add(lblDetalleCard2);
            card2.Location = new Point(258, 24);
            card2.Margin = new Padding(12);
            card2.Name = "card2";
            card2.Size = new Size(210, 140);
            card2.TabIndex = 1;
            // 
            // lblValorCard2
            // 
            lblValorCard2.Dock = DockStyle.Fill;
            lblValorCard2.Font = new Font("Segoe UI Semibold", 26F);
            lblValorCard2.ForeColor = Color.FromArgb(255, 159, 28);
            lblValorCard2.Location = new Point(0, 34);
            lblValorCard2.Name = "lblValorCard2";
            lblValorCard2.Padding = new Padding(16, 0, 0, 0);
            lblValorCard2.Size = new Size(208, 74);
            lblValorCard2.TabIndex = 1;
            lblValorCard2.Text = "-";
            lblValorCard2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTituloCard2
            // 
            lblTituloCard2.Dock = DockStyle.Top;
            lblTituloCard2.Font = new Font("Segoe UI", 9.5F);
            lblTituloCard2.ForeColor = Color.FromArgb(138, 143, 152);
            lblTituloCard2.Location = new Point(0, 0);
            lblTituloCard2.Name = "lblTituloCard2";
            lblTituloCard2.Padding = new Padding(16, 0, 0, 0);
            lblTituloCard2.Size = new Size(208, 34);
            lblTituloCard2.TabIndex = 0;
            lblTituloCard2.Text = "Reservas por finalizar";
            lblTituloCard2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDetalleCard2
            // 
            lblDetalleCard2.BackColor = Color.White;
            lblDetalleCard2.Dock = DockStyle.Bottom;
            lblDetalleCard2.Font = new Font("Segoe UI", 9F);
            lblDetalleCard2.ForeColor = Color.FromArgb(138, 143, 152);
            lblDetalleCard2.Location = new Point(0, 108);
            lblDetalleCard2.Name = "lblDetalleCard2";
            lblDetalleCard2.Padding = new Padding(16, 0, 0, 0);
            lblDetalleCard2.Size = new Size(208, 30);
            lblDetalleCard2.TabIndex = 2;
            lblDetalleCard2.Text = "Reservas en curso";
            lblDetalleCard2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // card3
            // 
            card3.BackColor = Color.White;
            card3.BorderStyle = BorderStyle.FixedSingle;
            card3.Controls.Add(lblValorCard3);
            card3.Controls.Add(lblTituloCard3);
            card3.Controls.Add(lblDetalleCard3);
            card3.Location = new Point(492, 24);
            card3.Margin = new Padding(12);
            card3.Name = "card3";
            card3.Size = new Size(210, 140);
            card3.TabIndex = 2;
            // 
            // lblValorCard3
            // 
            lblValorCard3.Dock = DockStyle.Fill;
            lblValorCard3.Font = new Font("Segoe UI Semibold", 26F);
            lblValorCard3.ForeColor = Color.FromArgb(56, 142, 60);
            lblValorCard3.Location = new Point(0, 34);
            lblValorCard3.Name = "lblValorCard3";
            lblValorCard3.Padding = new Padding(16, 0, 0, 0);
            lblValorCard3.Size = new Size(208, 74);
            lblValorCard3.TabIndex = 1;
            lblValorCard3.Text = "-";
            lblValorCard3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTituloCard3
            // 
            lblTituloCard3.Dock = DockStyle.Top;
            lblTituloCard3.Font = new Font("Segoe UI", 9.5F);
            lblTituloCard3.ForeColor = Color.FromArgb(138, 143, 152);
            lblTituloCard3.Location = new Point(0, 0);
            lblTituloCard3.Name = "lblTituloCard3";
            lblTituloCard3.Padding = new Padding(16, 0, 0, 0);
            lblTituloCard3.Size = new Size(208, 34);
            lblTituloCard3.TabIndex = 0;
            lblTituloCard3.Text = "Reservas de hoy";
            lblTituloCard3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDetalleCard3
            // 
            lblDetalleCard3.BackColor = Color.White;
            lblDetalleCard3.Dock = DockStyle.Bottom;
            lblDetalleCard3.Font = new Font("Segoe UI", 9F);
            lblDetalleCard3.ForeColor = Color.FromArgb(138, 143, 152);
            lblDetalleCard3.Location = new Point(0, 108);
            lblDetalleCard3.Name = "lblDetalleCard3";
            lblDetalleCard3.Padding = new Padding(16, 0, 0, 0);
            lblDetalleCard3.Size = new Size(208, 30);
            lblDetalleCard3.TabIndex = 2;
            lblDetalleCard3.Text = "Reservas que empiezan hoy";
            lblDetalleCard3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // card4
            // 
            card4.BackColor = Color.White;
            card4.BorderStyle = BorderStyle.FixedSingle;
            card4.Controls.Add(lblValorCard4);
            card4.Controls.Add(lblTituloCard4);
            card4.Controls.Add(lblDetalleCard4);
            card4.Location = new Point(24, 188);
            card4.Margin = new Padding(12);
            card4.Name = "card4";
            card4.Size = new Size(210, 140);
            card4.TabIndex = 3;
            // 
            // lblValorCard4
            // 
            lblValorCard4.Dock = DockStyle.Fill;
            lblValorCard4.Font = new Font("Segoe UI Semibold", 26F);
            lblValorCard4.ForeColor = Color.FromArgb(56, 142, 60);
            lblValorCard4.Location = new Point(0, 34);
            lblValorCard4.Name = "lblValorCard4";
            lblValorCard4.Padding = new Padding(16, 0, 0, 0);
            lblValorCard4.Size = new Size(208, 74);
            lblValorCard4.TabIndex = 1;
            lblValorCard4.Text = "-";
            lblValorCard4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTituloCard4
            // 
            lblTituloCard4.Dock = DockStyle.Top;
            lblTituloCard4.Font = new Font("Segoe UI", 9.5F);
            lblTituloCard4.ForeColor = Color.FromArgb(138, 143, 152);
            lblTituloCard4.Location = new Point(0, 0);
            lblTituloCard4.Name = "lblTituloCard4";
            lblTituloCard4.Padding = new Padding(16, 0, 0, 0);
            lblTituloCard4.Size = new Size(208, 34);
            lblTituloCard4.TabIndex = 0;
            lblTituloCard4.Text = "Ocupacion de hoy";
            lblTituloCard4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDetalleCard4
            // 
            lblDetalleCard4.BackColor = Color.White;
            lblDetalleCard4.Dock = DockStyle.Bottom;
            lblDetalleCard4.Font = new Font("Segoe UI", 9F);
            lblDetalleCard4.ForeColor = Color.FromArgb(138, 143, 152);
            lblDetalleCard4.Location = new Point(0, 108);
            lblDetalleCard4.Name = "lblDetalleCard4";
            lblDetalleCard4.Padding = new Padding(16, 0, 0, 0);
            lblDetalleCard4.Size = new Size(208, 30);
            lblDetalleCard4.TabIndex = 2;
            lblDetalleCard4.Text = "Recursos reservados hoy";
            lblDetalleCard4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FrmDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panelEncabezado);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmDashboard";
            StartPosition = FormStartPosition.Manual;
            Text = "FrmDashboard";
            panelEncabezado.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            card1.ResumeLayout(false);
            card2.ResumeLayout(false);
            card3.ResumeLayout(false);
            card4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelEncabezado;
        private Label lblTitulo;
        private Button btnActualizar;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel card1;
        private Label lblValorCard1;
        private Label lblTituloCard1;
        private Label lblDetalleCard1;
        private Panel card2;
        private Label lblValorCard2;
        private Label lblTituloCard2;
        private Label lblDetalleCard2;
        private Panel card3;
        private Label lblValorCard3;
        private Label lblTituloCard3;
        private Label lblDetalleCard3;
        private Panel card4;
        private Label lblValorCard4;
        private Label lblTituloCard4;
        private Label lblDetalleCard4;
    }
}
