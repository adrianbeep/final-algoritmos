using System.Drawing;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Paleta de los formularios modales.
///
/// Vive en un solo archivo a proposito: antes cada control tenia su color
/// escrito en su propio lugar (el modal, los campos, los combos) y cambiar el
/// aspecto de un modal era cambiar el color en tres o cuatro sitios.
///
/// Los colores NO son los mismos que usa el DataGridView. El grid es blanco
/// (255,255,255) con filas alternas (248,248,248), y un modal blanco se
/// mezclaba con la grilla que tiene debajo: no se sabia donde terminaba el
/// formulario y empezaba la grilla. El modal usa una escala de grises frios
/// para leerse como otra capa.
///
/// - <see cref="FondoModal"/> es el lienzo del modal.
/// - <see cref="FondoCampo"/> es la superficie de los controles que son caja
///   (combos y selectores de fecha).
/// - Los campos de texto NO son caja: son una etiqueta, una linea y el texto,
///   asi que comparten el fondo del modal y se distinguen por la linea.
/// </summary>
public static class Paleta
{
    /// <summary>Lienzo del modal. Gris perla frio, claramente no blanco.</summary>
    public static readonly Color FondoModal = Color.FromArgb(226, 230, 236);

    /// <summary>Superficie de los controles que son caja: combos y fechas.</summary>
    public static readonly Color FondoCampo = Color.FromArgb(250, 251, 253);

    /// <summary>Linea de abajo del campo de texto, en reposo.</summary>
    public static readonly Color BordeReposo = Color.FromArgb(163, 172, 186);

    /// <summary>Linea de abajo del campo de texto, con el foco puesto.</summary>
    public static readonly Color BordeFoco = Color.FromArgb(0, 120, 215);

    /// <summary>Linea de abajo del campo de texto, con error de validacion.</summary>
    public static readonly Color BordeError = Color.FromArgb(198, 40, 40);

    /// <summary>Color del texto que escribe el usuario.</summary>
    public static readonly Color Texto = Color.FromArgb(38, 44, 54);

    /// <summary>Color de las etiquetas de campo.</summary>
    public static readonly Color TextoEtiqueta = Color.FromArgb(86, 96, 112);

    /// <summary>Color del texto de ayuda (placeholder) de los campos.</summary>
    public static readonly Color Placeholder = Color.FromArgb(150, 158, 172);

    /// <summary>Color del titulo del modal.</summary>
    public static readonly Color Titulo = Color.FromArgb(0, 99, 200);

    /// <summary>Color de los textos secundarios ("Crear", "Editar").</summary>
    public static readonly Color Secundario = Color.FromArgb(112, 120, 133);

    /// <summary>Color de los valores destacados, como el precio calculado.</summary>
    public static readonly Color Exito = Color.FromArgb(15, 110, 60);
}
