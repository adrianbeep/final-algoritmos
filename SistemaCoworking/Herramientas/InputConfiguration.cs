using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Herramientas;

public static class InputConfiguration
{
    private static readonly Color BordeReposo = Color.FromArgb(200, 200, 200);
    private static readonly Color BordeFocus = Color.FromArgb(0, 120, 215);
    private static readonly Color PlaceholderColor = Color.FromArgb(150, 150, 150);
    private static readonly Color TextoPrimario = Color.FromArgb(40, 40, 40);
    private static readonly Font InputFont = new Font("Segoe UI", 9.5F);

    public static void Configuration(Panel panel, TextBox txt, string placeholder, Color colorFondo)
    {
        txt.PlaceholderText = placeholder;
        txt.AutoSize = false;
        txt.BorderStyle = BorderStyle.None;
        txt.Dock = DockStyle.Fill;
        txt.BackColor = colorFondo;
        txt.Font = InputFont;
        txt.ForeColor = TextoPrimario;

        panel.BackColor = BordeReposo;

        // Estados de la barra
        txt.GotFocus += (s, e) =>
        {
            if (txt.Enabled)
                panel.BackColor = BordeFocus;
        };

        txt.LostFocus += (s, e) =>
            panel.BackColor = BordeReposo;
    }

}