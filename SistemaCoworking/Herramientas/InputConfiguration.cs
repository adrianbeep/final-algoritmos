using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

public static class InputConfiguration
{
    private static readonly Color BordeReposo = Color.FromArgb(200, 200, 200);
    private static readonly Color BordeFocus = Color.FromArgb(0, 120, 215);
    private static readonly Color BordeError = Color.FromArgb(198, 40, 40);
    private static readonly Color PlaceholderColor = Color.FromArgb(150, 150, 150);
    private static readonly Color TextoPrimario = Color.FromArgb(40, 40, 40);
    private static readonly Font InputFont = new Font("Segoe UI", 9.5F);

    public static void Configuration(Panel panel, TextBox txt, string placeholder, Color colorFondo)
    {
        txt.PlaceholderText = placeholder;
        txt.AutoSize = false;
        txt.BorderStyle = BorderStyle.None;
        //txt.Dock = DockStyle.Fill;
        txt.BackColor = colorFondo;
        txt.Font = InputFont;
        txt.ForeColor = TextoPrimario;

        panel.BackColor = BordeReposo;

        txt.GotFocus += (s, e) =>
        {
            if (txt.Enabled && panel.BackColor != BordeError)
                panel.BackColor = BordeFocus;
        };

        txt.LostFocus += (s, e) =>
        {
            if (panel.BackColor != BordeError)
                panel.BackColor = BordeReposo;
        };
            
    }

    /// <summary>
    /// Hace que el TextBox acepte solo digitos (y opcionalmente signo negativo
    /// / decimal). No toca la apariencia - eso lo hace Configuration.
    /// </summary>
    public static void ConfigNumeric(
        TextBox txt,
        bool permitirDecimal = false,
        bool permitirNegativo = false)
    {
        txt.MaxLength = 18;

        txt.KeyPress += (s, e) =>
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if (permitirDecimal && (e.KeyChar == '.' || e.KeyChar == ',') &&
                !txt.Text.Contains('.') && !txt.Text.Contains(','))
                return;

            if (permitirNegativo && e.KeyChar == '-' &&
                txt.SelectionStart == 0 && !txt.Text.StartsWith("-"))
                return;

            e.Handled = true;
        };
    }
}