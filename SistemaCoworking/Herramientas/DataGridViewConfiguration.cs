using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Herramientas;

public static class DataGridViewConfiguration
{
    private static readonly Color HeaderDark = Color.FromArgb(32, 32, 32);
    private static readonly Color TextPrimary = Color.FromArgb(40, 40, 40);
    private static readonly Color BorderLight = Color.FromArgb(230, 230, 230);
    private static readonly Color RowAlt = Color.FromArgb(248, 248, 248);
    private static readonly Color SelectionBlue = Color.FromArgb(229, 243, 255);

    private static readonly Color AccentBlue = Color.FromArgb(0, 120, 215);
    private static readonly Color AccentBluePressed = Color.FromArgb(0, 95, 145);
    private static readonly Color WarningOrange = Color.FromArgb(255, 159, 28);
    private static readonly Color WarningOrangePress = Color.FromArgb(210, 130, 20);

    public static void ConfigDGV(DataGridView dgv)
    {
        // Layout
        dgv.Dock = DockStyle.Fill;
        dgv.BorderStyle = BorderStyle.None;
        dgv.BackgroundColor = Color.White;
        dgv.GridColor = BorderLight;
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.RowHeadersVisible = false;

        dgv.AllowUserToResizeRows = false;
        dgv.AllowUserToResizeColumns = false;
        dgv.AutoGenerateColumns = false;
        dgv.AllowUserToAddRows = false;

        // Headers
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgv.ColumnHeadersHeight = 40;
        dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = HeaderDark,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 10),
            Padding = new Padding(12, 10, 12, 10),
            SelectionBackColor = HeaderDark,
            SelectionForeColor = Color.White,
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };

        // Celdas
        dgv.DefaultCellStyle = new DataGridViewCellStyle
        {
            Font = new Font("Segoe UI", 9),
            BackColor = Color.White,
            ForeColor = TextPrimary,
            SelectionBackColor = SelectionBlue,
            SelectionForeColor = TextPrimary,
            Padding = new Padding(12, 8, 12, 8)
        };
        dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = RowAlt };
        dgv.RowTemplate.Height = 38;

        // Columnas
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Botones de acción
        StyleActionColumns(dgv);

        // Pintar Botones
        dgv.CellPainting += Dgv_CellPainting;
    }

    private static void StyleActionColumns(DataGridView dgv)
    {
        foreach (DataGridViewColumn col in dgv.Columns)
        {
            if (col is not DataGridViewButtonColumn btn) continue;

            btn.FlatStyle = FlatStyle.Flat;
            btn.UseColumnTextForButtonValue = true;

            if (string.IsNullOrEmpty(btn.Text))
            {
                var n = col.Name.ToLower();
                btn.Text = n.Contains("editar") ? "Editar"
                         : n.Contains("cancelar") ? "Cancelar"
                         : n.Contains("ver") ? "Ver"
                         : "Acción";
            }

            bool esCancelar = col.Name.Contains("Cancelar", StringComparison.OrdinalIgnoreCase)
                           || col.Name.Contains("Cancel", StringComparison.OrdinalIgnoreCase);

            btn.DefaultCellStyle.BackColor = esCancelar ? WarningOrange : AccentBlue;
            btn.DefaultCellStyle.SelectionBackColor = esCancelar ? WarningOrangePress : AccentBluePressed;
            btn.DefaultCellStyle.ForeColor = Color.White;
            btn.DefaultCellStyle.SelectionForeColor = Color.White;
            btn.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9);
            btn.DefaultCellStyle.Padding = new Padding(6, 6, 6, 6);
        }
    }

    private static void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (!(sender is DataGridView dgv)) return;
        if (!(dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn btn)) return;

        // Padding con color fijo (no heredar del row)
        // #F5F5F8 contrasta con blanco de filas normales Y con #F8F8F8 de filas alternas
        using (var paddingBrush = new SolidBrush(Color.FromArgb(245, 245, 248)))
            e.Graphics.FillRectangle(paddingBrush, e.CellBounds);

        // Rectángulo interno del botón
        var btnRect = e.CellBounds;
        btnRect.Inflate(-10, -6);

        // Colores según la columna (Cancelar = naranja, Editar = azul)
        bool esCancelar = btn.Name.Contains("Cancelar", StringComparison.OrdinalIgnoreCase)
                       || btn.Name.Contains("Cancel", StringComparison.OrdinalIgnoreCase);

        var bgColor = esCancelar ? WarningOrange : AccentBlue;
        var borderColor = esCancelar ? WarningOrangePress : AccentBluePressed;

        // Pintar el botón
        using (var bg = new SolidBrush(bgColor))
            e.Graphics.FillRectangle(bg, btnRect);

        using (var border = new Pen(borderColor))
            e.Graphics.DrawRectangle(border, btnRect);

        var text = e.FormattedValue?.ToString() ?? "";
        TextRenderer.DrawText(
            e.Graphics, text,
            new Font("Segoe UI Semibold", 9),
            btnRect, Color.White,
            TextFormatFlags.HorizontalCenter
              | TextFormatFlags.VerticalCenter
              | TextFormatFlags.SingleLine);

        e.Handled = true;
    }
}