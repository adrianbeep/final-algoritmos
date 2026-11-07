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

        // Handlers del filtro de entrada. Se guardan para poder sacarlos si
        // el campo se reconfigura: si no, el filtro se aplicaria dos veces.
        private KeyPressEventHandler? _filtroTecla;
        private EventHandler? _filtroPegado;
        private bool _saneando;

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

        /// <summary>
        /// Se dispara cuando cambia el texto del campo. Sirve para recalcular
        /// lo que dependa del valor, como el precio de un recurso.
        /// </summary>
        public event EventHandler? ValueChanged;

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

        /// <summary>
        /// Cantidad maxima de caracteres. Mismo nombre que el TextBox de WinForms.
        /// </summary>
        [DefaultValue(32767)]
        public int MaxLength
        {
            get => txt.MaxLength;
            set => txt.MaxLength = value;
        }

        /// <summary>
        /// Configura la etiqueta, el placeholder y que caracteres se pueden
        /// ingresar. Es lo unico que hay que llamar para dejar el campo listo.
        ///
        /// El filtro no muestra errores: si el caracter no corresponde, la tecla
        /// no entra. Los errores de valor los reporta la validacion del modelo
        /// al guardar.
        /// </summary>
        public void Configurar(
            string etiqueta,
            string placeholder,
            TipoEntrada tipo = TipoEntrada.Texto,
            int? maxLength = null)
        {
            LblText = etiqueta;
            Placeholder = placeholder;

            if (maxLength.HasValue)
                MaxLength = maxLength.Value;

            // Si el campo ya estaba configurado, se sacan los handlers
            // anteriores antes de poner los nuevos.
            if (_filtroTecla is not null)
                txt.KeyPress -= _filtroTecla;

            if (_filtroPegado is not null)
                txt.TextChanged -= _filtroPegado;

            _filtroTecla = (s, e) =>
            {
                // Las teclas de control (borrar, enter, flechas) siempre pasan.
                if (char.IsControl(e.KeyChar))
                    return;

                if (!InputConfiguration.EsEntradaValida(e.KeyChar, txt.Text, tipo))
                    e.Handled = true;
            };

            txt.KeyPress += _filtroTecla;

            // Pegar no dispara KeyPress, asi que lo que entra por el
            // portapapeles se revisa aparte.
            _filtroPegado = (s, e) =>
            {
                // El saneo vuelve a disparar TextChanged: con el guard se evita
                // que el evento se dispare dos veces por el mismo cambio.
                if (_saneando)
                    return;

                SanearTexto(tipo);

                ValueChanged?.Invoke(this, EventArgs.Empty);
            };

            txt.TextChanged += _filtroPegado;
        }

        /// <summary>
        /// Saca del texto los caracteres que no corresponden al tipo de entrada
        /// y deja el cursor donde estaba.
        /// </summary>
        private void SanearTexto(TipoEntrada tipo)
        {
            if (_saneando)
                return;

            string limpio = InputConfiguration.Sanear(txt.Text, tipo);

            if (limpio == txt.Text)
                return;

            _saneando = true;

            int posicion = txt.SelectionStart;
            txt.Text = limpio;
            txt.SelectionStart = Math.Min(posicion, limpio.Length);

            _saneando = false;
        }

        // Constructor para VisualStudio
        public TextControl() : this("Field", "") { }

        public TextControl(string label, string placeholder)
        {
            InitializeComponent();

            lbl.Text = label;
            InputConfiguration.ConfigLabel(lbl);

            InputConfiguration.Configuration(barra, txt, placeholder, Color.White);

            this.TabStop = true;
            txt.TabIndex = 0;
            this.Click += (s, e) => txt.Focus();
            barra.Click += (s, e) => txt.Focus();
        }

        public void MostrarError(string? mensaje)
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

        /// <summary>
        /// Limpia el error de todos los TextControl que hay dentro del
        /// contenedor, incluidos los que viven dentro de un panel anidado.
        ///
        /// Recorre todo el arbol porque los campos especificos de cada tipo
        /// de recurso (capacidad, monitores, metros) estan dentro de
        /// paneles: si solo se miraran los hijos directos, el borde de error
        /// de esos campos nunca se limpiaria.
        /// </summary>
        public static void LimpiarErrores(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (control is TextControl txt)
                {
                    txt.LimpiarError();
                }

                LimpiarErrores(control);
            }
        }
    }
}