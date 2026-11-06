using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

public static class InputConfiguration
{
    private static readonly Color BordeReposo = Color.FromArgb(200, 200, 200);
    private static readonly Color BordeFocus = Color.FromArgb(0, 120, 215);
    private static readonly Color BordeError = Color.FromArgb(198, 40, 40);
    private static readonly Color PlaceholderColor = Color.FromArgb(150, 150, 150);
    private static readonly Color TextoPrimario = Color.FromArgb(40, 40, 40);
    private static readonly Color LabelColor = Color.FromArgb(80, 80, 80);
    private static readonly Font InputFont = new Font("Segoe UI", 9.5F);
    private static readonly Font LabelFont = new Font("Segoe UI", 9F);

    /// <summary>
    /// Configura el estilo visual uniforme de las etiquetas de campo (labels):
    /// fuente Segoe UI 9pt, color gris suave (#505050 / 80, 80, 80) y fondo transparente.
    /// </summary>
    public static void ConfigLabel(Label lbl)
    {
        lbl.BackColor = Color.Transparent;
        lbl.ForeColor = LabelColor;
        lbl.Font = LabelFont;
    }

    // Signos que se aceptan en los campos alfanumericos: los que aparecen
    // en un correo, en una ubicacion o en un telefono.
    private const string SignosAlfanumericos = ".-_@+/,";

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
    /// Si el caracter se puede escribir en el campo, segun el tipo de entrada
    /// y lo que ya hay tipeado.
    ///
    /// El filtro no reporta errores: si el caracter no corresponde, la tecla
    /// simplemente no entra. El error de un valor invalido lo reporta la
    /// validacion del modelo, al guardar.
    ///
    /// En los campos numericos el unico separador que se admite es el
    /// decimal de la cultura. El otro separador, el de miles, no se acepta: en
    /// es-AR "1.500" son mil quinientos, no uno con cinco.
    /// </summary>
    public static bool EsEntradaValida(char caracter, string textoActual, TipoEntrada tipo)
    {
        if (tipo == TipoEntrada.Texto)
            return true;

        if (EsTipoNumerico(tipo))
        {
            if (EsSigno(caracter))
            {
                // El signo solo al principio del campo, y solo en los tipos que lo admiten.
                return tipo == TipoEntrada.NumericoConNegativos && textoActual.Length == 0;
            }

            if (EsSeparadorDecimal(caracter))
            {
                if (tipo == TipoEntrada.SoloNumeros)
                    return false;

                // Solo un separador decimal.
                return !textoActual.Any(c => EsSeparadorDecimal(c));
            }

            return EsDigito(caracter);
        }

        return EsCaracterPermitido(caracter, tipo);
    }

    /// <summary>
    /// Saca del texto los caracteres que no corresponden al tipo de entrada.
    ///
    /// Se usa para el texto pegado: pegar no dispara KeyPress, asi que lo que
    /// entra por el portapapeles hay que revisarlo aparte. Usa la misma regla
    /// que el filtrado por tecla, para que las dos caminos den el mismo texto.
    /// </summary>
    public static string Sanear(string texto, TipoEntrada tipo)
    {
        if (tipo == TipoEntrada.Texto || string.IsNullOrEmpty(texto))
            return texto;

        StringBuilder salida = new StringBuilder();

        foreach (char c in texto)
        {
            if (char.IsControl(c))
                continue;

            if (!EsEntradaValida(c, salida.ToString(), tipo))
                continue;

            salida.Append(c);
        }

        return salida.ToString();
    }

    private static bool EsTipoNumerico(TipoEntrada tipo)
    {
        return tipo == TipoEntrada.SoloNumeros
            || tipo == TipoEntrada.NumericoConDecimales
            || tipo == TipoEntrada.NumericoConNegativos;
    }

    private static bool EsCaracterPermitido(char c, TipoEntrada tipo)
    {
        return tipo switch
        {
            TipoEntrada.SoloLetras => char.IsLetter(c) || c == ' ',
            TipoEntrada.LetrasYNumeros => char.IsLetter(c) || EsDigito(c) || c == ' ' || SignosAlfanumericos.Contains(c),
            _ => true
        };
    }

    // Solo digitos ASCII: el valor va a una columna numerica de MySQL.
    private static bool EsDigito(char c) => c >= '0' && c <= '9';

    // El separador decimal es el de la cultura: en es-AR es la coma, y el
    // punto es el separador de miles.
    private static bool EsSeparadorDecimal(char c)
    {
        return c == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
    }

    private static bool EsSigno(char c) => c == '-';
}
