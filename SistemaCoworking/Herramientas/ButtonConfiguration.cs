using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Herramientas;

public static class ButtonConfiguration
{
    // Primary (relleno)
    private static readonly Color PrimaryBg = Color.FromArgb(0, 120, 215);
    private static readonly Color PrimaryHover = Color.FromArgb(0, 95, 195);
    private static readonly Color PrimaryPress = Color.FromArgb(0, 75, 165);

    // Secondary (outline)
    private static readonly Color SecondaryBg = Color.White;
    private static readonly Color SecondaryFg = Color.FromArgb(0, 120, 215);
    private static readonly Color SecondaryBorde = Color.FromArgb(0, 120, 215);
    private static readonly Color SecondaryHover = Color.FromArgb(229, 243, 255);
    private static readonly Color SecondaryPress = Color.FromArgb(204, 228, 247);

    // Text (solo texto)
    private static readonly Color TextBg = Color.Transparent;
    private static readonly Color TextFg = Color.FromArgb(0, 120, 215);
    private static readonly Color TextHover = Color.FromArgb(229, 243, 255);

    private static readonly Font ButtonFont = new Font("Segoe UI Semibold", 10);

    public static void ConfigPrimary(Button btn)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.BackColor = PrimaryBg;
        btn.ForeColor = Color.White;
        btn.Font = ButtonFont;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = PrimaryHover;
        btn.FlatAppearance.MouseDownBackColor = PrimaryPress;
        btn.Cursor = Cursors.Hand;
        btn.UseCompatibleTextRendering = false;
    }

    public static void ConfigSecondary(Button btn)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.BackColor = SecondaryBg;
        btn.ForeColor = SecondaryFg;
        btn.Font = ButtonFont;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.BorderColor = SecondaryBorde;
        btn.FlatAppearance.MouseOverBackColor = SecondaryHover;
        btn.FlatAppearance.MouseDownBackColor = SecondaryPress;
        btn.Cursor = Cursors.Hand;
        btn.UseCompatibleTextRendering = false;
    }

    public static void ConfigText(Button btn)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.BackColor = TextBg;
        btn.ForeColor = TextFg;
        btn.Font = ButtonFont;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = TextHover;
        btn.Cursor = Cursors.Hand;
        btn.UseCompatibleTextRendering = false;
    }
}
