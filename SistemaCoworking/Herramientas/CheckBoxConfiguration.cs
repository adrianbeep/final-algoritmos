using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Proporciona configuraciones visuales modernas para controles CheckBox de Windows Forms,
/// manteniendo la paleta de colores y la tipografía uniforme del sistema (Segoe UI, Accent Blue).
/// </summary>
public static class CheckBoxConfiguration
{
    // Paleta compartida del sistema (alineada con ButtonConfiguration e InputConfiguration)
    private static readonly Color AccentBlue = Color.FromArgb(0, 120, 215);
    private static readonly Color AccentBlueHover = Color.FromArgb(0, 95, 195);
    private static readonly Color AccentBluePress = Color.FromArgb(0, 75, 165);

    private static readonly Color BordeReposo = Color.FromArgb(180, 180, 180);
    private static readonly Color BordeHover = Color.FromArgb(0, 120, 215);

    private static readonly Color SwitchFondoOff = Color.FromArgb(220, 224, 230);
    private static readonly Color SwitchFondoOffHover = Color.FromArgb(205, 210, 218);

    private static readonly Color TextoPrimario = Color.FromArgb(40, 40, 40);
    private static readonly Color ControlDisabled = Color.FromArgb(170, 170, 170);
    private static readonly Color FondoDisabled = Color.FromArgb(238, 238, 238);

    private static readonly Font CheckFont = new Font("Segoe UI", 9.5F);

    private enum CheckBoxStyle
    {
        ModernBox,
        Switch
    }

    private sealed class ControlState
    {
        public bool IsHovered { get; set; }
        public bool IsPressed { get; set; }
        public CheckBoxStyle Style { get; set; } = CheckBoxStyle.ModernBox;
    }

    // Tabla débil para asociar el estado interactivo a cada control sin provocar fugas de memoria.
    private static readonly ConditionalWeakTable<CheckBox, ControlState> States = new();

    /// <summary>
    /// Configura un CheckBox con diseño moderno Fluent:
    /// caja con esquinas redondeadas, tilde vectorial suavizada y retroalimentación al hover.
    /// </summary>
    public static void Config(CheckBox chk)
    {
        ConfigurarBase(chk, CheckBoxStyle.ModernBox);
    }

    /// <summary>
    /// Configura el CheckBox como un interruptor deslizante moderno (toggle switch).
    /// </summary>
    public static void ConfigSwitch(CheckBox chk)
    {
        ConfigurarBase(chk, CheckBoxStyle.Switch);
    }

    private static void ConfigurarBase(CheckBox chk, CheckBoxStyle style)
    {
        // Habilita DoubleBuffered vía reflexión para eliminar cualquier parpadeo al repintar.
        typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(chk, true);

        chk.Cursor = Cursors.Hand;
        chk.Font = CheckFont;
        chk.ForeColor = TextoPrimario;
        chk.FlatStyle = FlatStyle.Standard;

        ControlState state = States.GetOrCreateValue(chk);
        state.Style = style;

        // Suscribir eventos interactivos una única vez por instancia
        chk.Paint -= Chk_Paint;
        chk.Paint += Chk_Paint;

        chk.MouseEnter -= Chk_MouseEnter;
        chk.MouseEnter += Chk_MouseEnter;

        chk.MouseLeave -= Chk_MouseLeave;
        chk.MouseLeave += Chk_MouseLeave;

        chk.MouseDown -= Chk_MouseDown;
        chk.MouseDown += Chk_MouseDown;

        chk.MouseUp -= Chk_MouseUp;
        chk.MouseUp += Chk_MouseUp;

        chk.CheckedChanged -= Chk_StateChanged;
        chk.CheckedChanged += Chk_StateChanged;

        chk.EnabledChanged -= Chk_StateChanged;
        chk.EnabledChanged += Chk_StateChanged;

        chk.Invalidate();
    }

    private static void Chk_MouseEnter(object? sender, EventArgs e)
    {
        if (sender is CheckBox chk && States.TryGetValue(chk, out var state))
        {
            state.IsHovered = true;
            chk.Invalidate();
        }
    }

    private static void Chk_MouseLeave(object? sender, EventArgs e)
    {
        if (sender is CheckBox chk && States.TryGetValue(chk, out var state))
        {
            state.IsHovered = false;
            state.IsPressed = false;
            chk.Invalidate();
        }
    }

    private static void Chk_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && sender is CheckBox chk && States.TryGetValue(chk, out var state))
        {
            state.IsPressed = true;
            chk.Invalidate();
        }
    }

    private static void Chk_MouseUp(object? sender, MouseEventArgs e)
    {
        if (sender is CheckBox chk && States.TryGetValue(chk, out var state))
        {
            state.IsPressed = false;
            chk.Invalidate();
        }
    }

    private static void Chk_StateChanged(object? sender, EventArgs e)
    {
        if (sender is CheckBox chk)
        {
            chk.Invalidate();
        }
    }

    private static void Chk_Paint(object? sender, PaintEventArgs e)
    {
        if (sender is not CheckBox chk || !States.TryGetValue(chk, out var state))
            return;

        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        // Limpiar el fondo con el color del contenedor padre para evitar marcas residuales
        Color colorFondoPadre = chk.Parent?.BackColor ?? chk.BackColor;
        using (var brushFondo = new SolidBrush(colorFondoPadre))
        {
            g.FillRectangle(brushFondo, chk.ClientRectangle);
        }

        if (state.Style == CheckBoxStyle.Switch)
        {
            DibujarSwitch(g, chk, state);
        }
        else
        {
            DibujarModernBox(g, chk, state);
        }
    }

    private static void DibujarModernBox(Graphics g, CheckBox chk, ControlState state)
    {
        const int boxSize = 18;
        int boxX = 2;
        int boxY = Math.Max(0, (chk.ClientSize.Height - boxSize) / 2);
        RectangleF boxRect = new RectangleF(boxX, boxY, boxSize, boxSize);

        float cornerRadius = 4f;

        if (!chk.Enabled)
        {
            if (chk.Checked)
            {
                using var brush = new SolidBrush(ControlDisabled);
                using var path = CrearRectanguloRedondeado(boxRect, cornerRadius);
                g.FillPath(brush, path);
                DibujarTilde(g, boxX, boxY, Color.White);
            }
            else
            {
                using var brush = new SolidBrush(FondoDisabled);
                using var pen = new Pen(ControlDisabled, 1.5f);
                using var path = CrearRectanguloRedondeado(boxRect, cornerRadius);
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        }
        else if (chk.Checked)
        {
            Color colorFondo = state.IsPressed ? AccentBluePress : (state.IsHovered ? AccentBlueHover : AccentBlue);
            using var brush = new SolidBrush(colorFondo);
            using var path = CrearRectanguloRedondeado(boxRect, cornerRadius);
            g.FillPath(brush, path);

            DibujarTilde(g, boxX, boxY, Color.White);
        }
        else
        {
            Color colorBorde = state.IsHovered ? BordeHover : BordeReposo;
            using var brush = new SolidBrush(Color.White);
            using var pen = new Pen(colorBorde, 1.5f);
            using var path = CrearRectanguloRedondeado(boxRect, cornerRadius);
            g.FillPath(brush, path);
            g.DrawPath(pen, path);
        }

        // Texto adyacente
        int textX = boxX + boxSize + 8;
        Rectangle textRect = new Rectangle(textX, 0, Math.Max(0, chk.ClientSize.Width - textX), chk.ClientSize.Height);
        Color colorTexto = chk.Enabled ? TextoPrimario : ControlDisabled;

        TextRenderer.DrawText(
            g,
            chk.Text,
            chk.Font,
            textRect,
            colorTexto,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private static void DibujarSwitch(Graphics g, CheckBox chk, ControlState state)
    {
        const int switchWidth = 36;
        const int switchHeight = 20;
        int switchX = 2;
        int switchY = Math.Max(0, (chk.ClientSize.Height - switchHeight) / 2);
        RectangleF switchRect = new RectangleF(switchX, switchY, switchWidth, switchHeight);

        float cornerRadius = switchHeight / 2f;

        Color fondoSwitch;
        if (!chk.Enabled)
        {
            fondoSwitch = FondoDisabled;
        }
        else if (chk.Checked)
        {
            fondoSwitch = state.IsPressed ? AccentBluePress : (state.IsHovered ? AccentBlueHover : AccentBlue);
        }
        else
        {
            fondoSwitch = state.IsHovered ? SwitchFondoOffHover : SwitchFondoOff;
        }

        using (var brush = new SolidBrush(fondoSwitch))
        using (var path = CrearRectanguloRedondeado(switchRect, cornerRadius))
        {
            g.FillPath(brush, path);

            if (!chk.Checked && chk.Enabled)
            {
                using var pen = new Pen(state.IsHovered ? BordeHover : BordeReposo, 1f);
                g.DrawPath(pen, path);
            }
        }

        // Perno / círculo deslizante (knob)
        const int knobSize = 14;
        const int margin = 3;
        float knobX = chk.Checked ? (switchX + switchWidth - knobSize - margin) : (switchX + margin);
        float knobY = switchY + margin;
        RectangleF knobRect = new RectangleF(knobX, knobY, knobSize, knobSize);

        Color knobColor = chk.Enabled ? Color.White : ControlDisabled;
        using (var brushKnob = new SolidBrush(knobColor))
        {
            g.FillEllipse(brushKnob, knobRect);
        }

        // Texto adyacente
        int textX = switchX + switchWidth + 8;
        Rectangle textRect = new Rectangle(textX, 0, Math.Max(0, chk.ClientSize.Width - textX), chk.ClientSize.Height);
        Color colorTexto = chk.Enabled ? TextoPrimario : ControlDisabled;

        TextRenderer.DrawText(
            g,
            chk.Text,
            chk.Font,
            textRect,
            colorTexto,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private static void DibujarTilde(Graphics g, int boxX, int boxY, Color color)
    {
        using var pen = new Pen(color, 2f)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };

        PointF[] puntosTilde =
        {
            new PointF(boxX + 4.5f, boxY + 9.5f),
            new PointF(boxX + 7.5f, boxY + 13.0f),
            new PointF(boxX + 13.5f, boxY + 5.5f)
        };

        g.DrawLines(pen, puntosTilde);
    }

    private static GraphicsPath CrearRectanguloRedondeado(RectangleF rect, float radio)
    {
        GraphicsPath path = new GraphicsPath();
        float diametro = radio * 2;

        path.AddArc(rect.X, rect.Y, diametro, diametro, 180, 90);
        path.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270, 90);
        path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90, 90);
        path.CloseFigure();

        return path;
    }
}
