namespace SistemaCoworking.Herramientas;

/// <summary>
/// Que caracteres se pueden ingresar en un campo.
///
/// El filtro de caracteres y la validacion de dominio son cosas distintas:
/// el filtro evita que se pueda escribir algo que no corresponde (no reporta
/// error, simplemente la tecla no entra), y la validacion del modelo revisa
/// que el valor tenga sentido (costo mayor a cero, metros enteros).
/// </summary>
public enum TipoEntrada
{
    /// <summary>Cualquier caracter.</summary>
    Texto,

    /// <summary>Solo letras y espacios. Para nombres y apellidos.</summary>
    SoloLetras,

    /// <summary>
    /// Letras, digitos, espacio y los signos de uso habitual ( . - _ @ + / ).
    /// Para ubicaciones, correos y nombres que llevan numeros.
    /// </summary>
    LetrasYNumeros,

    /// <summary>Solo digitos. Enteros sin signo.</summary>
    SoloNumeros,

    /// <summary>Digitos y separador decimal. Sin signo.</summary>
    NumericoConDecimales,

    /// <summary>Digitos, separador decimal y signo negativo.</summary>
    NumericoConNegativos
}
