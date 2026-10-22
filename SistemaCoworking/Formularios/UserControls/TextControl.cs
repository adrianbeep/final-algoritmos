using SistemaCoworking.Herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SistemaCoworking.Formularios.UserControls
{

    public partial class TextControl : UserControl
    {
        private static readonly Color BordeReposo = Color.FromArgb(200, 200, 200);
        private static readonly Color BordeFocus = Color.FromArgb(0, 120, 215);
        private static readonly Color BordeError = Color.FromArgb(198, 40, 40);
        private string _placeholder;
        private string _value;
        private string _lblText;

        // https://learn.microsoft.com/es-es/dotnet/desktop/winforms/compiler-messages/wfo1000
        // el [DefaultValue("")] es porque me tiraba el error de arriba

        [DefaultValue("")]
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                if (txt != null) txt.PlaceholderText = value;
            }
        }

        [DefaultValue("")]
        public string Value //no es Text pq ya tiene una propiedad el usercontrol llamada asi.
        {
            // por si quedan dudas
            // esta propiedad permite acceder al texto del txt interno mediante el get
            // y setear directo al txt
            get => txt.Text;
            set
            {
                _value = value;
                txt.Text = _value;
            }
        }

        [DefaultValue("")]
        public string LblText
        {
            get => _lblText;
            set
            {
                _lblText = value;
                lbl.Text = value;
            }
        }

        // Constructor para VisualStudio
        public TextControl() : this("Field", "") { }

        public TextControl(string label, string placeholder)
        {
            InitializeComponent();

            lbl.Text = label;
            lbl.BackColor = Color.Transparent;
            lbl.ForeColor = Color.FromArgb(80, 80, 80);
            lbl.Font = new Font("Segoe UI", 9F);

            InputConfiguration.Configuration(barra, txt, placeholder, Color.White);

            this.TabStop = true;
            txt.TabIndex = 0;
            this.Click += (s, e) => txt.Focus();
            barra.Click += (s, e) => txt.Focus();
        }

        public void MostrarError(string mensaje)
        {
            barra.BackColor = BordeError;
            pBox.Visible = true;
            toolTip.SetToolTip(pBox, mensaje);
        }

        public void LimpiarError()
        {
            barra.BackColor = BordeReposo;
            pBox.Visible = false;
            toolTip.SetToolTip(pBox, "");
        }
    }
}