namespace SistemaCoworking.Formularios.UserControls
{
    partial class TextControl
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
            components = new System.ComponentModel.Container();
            barra = new Panel();
            pnPbox = new Panel();
            pBox = new PictureBox();
            txt = new TextBox();
            lbl = new Label();
            toolTip = new ToolTip(components);
            pnPbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pBox).BeginInit();
            SuspendLayout();
            // 
            // barra
            // 
            barra.Dock = DockStyle.Bottom;
            barra.Location = new Point(0, 49);
            barra.Name = "barra";
            barra.Size = new Size(215, 4);
            barra.TabIndex = 0;
            // 
            // pnPbox
            // 
            pnPbox.Anchor = AnchorStyles.Right;
            pnPbox.BackColor = Color.White;
            pnPbox.Controls.Add(pBox);
            pnPbox.Location = new Point(184, 0);
            pnPbox.Name = "pnPbox";
            pnPbox.Size = new Size(31, 49);
            pnPbox.TabIndex = 2;
            // 
            // pBox
            // 
            pBox.Dock = DockStyle.Bottom;
            pBox.Image = Properties.Resources.icons8_cancel_24;
            pBox.Location = new Point(0, 23);
            pBox.Name = "pBox";
            pBox.Size = new Size(31, 26);
            pBox.SizeMode = PictureBoxSizeMode.CenterImage;
            pBox.TabIndex = 0;
            pBox.TabStop = false;
            pBox.Visible = false;
            // 
            // txt
            // 
            txt.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txt.BackColor = Color.White;
            txt.BorderStyle = BorderStyle.None;
            txt.Location = new Point(0, 29);
            txt.Margin = new Padding(0, 3, 3, 3);
            txt.Name = "txt";
            txt.Size = new Size(181, 20);
            txt.TabIndex = 3;
            // 
            // lbl
            // 
            lbl.Dock = DockStyle.Top;
            lbl.Location = new Point(0, 0);
            lbl.Name = "lbl";
            lbl.Size = new Size(215, 20);
            lbl.TabIndex = 4;
            lbl.Text = "label1";
            // 
            // TextControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lbl);
            Controls.Add(pnPbox);
            Controls.Add(barra);
            Controls.Add(txt);
            Name = "TextControl";
            Size = new Size(215, 53);
            pnPbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel barra;
        private Panel pnPbox;
        private System.Windows.Forms.PictureBox pBox;
        private TextBox txt;
        private Label lbl;
        private ToolTip toolTip;
    }
}
