using MySqlConnector;
using System;
using System.Windows.Forms;

namespace SistemaCoworking.Herramientas;

/// <summary>
/// Traduce un error de la base a un mensaje que se pueda mostrar al usuario.
///
/// Los procedimientos almacenados levantan las reglas de negocio con
/// SIGNAL SQLSTATE '45000'. El conector (MySqlConnector) expone ese SQLSTATE
/// en la excepcion, asi que un error de negocio se reconoce por ahi y su
/// mensaje es el del dominio, que se muestra tal cual.
///
/// El codigo 1644 (ER_SIGNAL_EXCEPTION) queda como segunda via: identifica
/// el mismo tipo de error aunque el SQLSTATE no venga.
///
/// Cualquier otro error es tecnico: se muestra un mensaje generico con el
/// detalle tecnico, para no filtrar SQL al usuario pero tampoco ocultar el
/// error.
/// </summary>
public static class ErroresDeBaseDeDatos
{
    /// <summary>
    /// ER_SIGNAL_EXCEPTION: el codigo con el que MySQL y MariaDB reportan un
    /// SIGNAL. Es lo que distingue una regla de negocio de un error tecnico.
    /// </summary>
    private const int CodigoDeErrorDeNegocio = 1644;

    /// <summary>
    /// Muestra el error y devuelve true si fue un error de NEGOCIO (una regla
    /// del dominio) y false si fue un error TECNICO de la base.
    /// </summary>
    public static bool Mostrar(Exception ex)
    {
        if (ex is MySqlException my && EsErrorDeNegocio(my))
        {
            MessageBox.Show(
                my.Message,
                "No se pudo completar la operacion",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return true;
        }

        MessageBox.Show(
            "Ocurrio un error al trabajar con la base de datos. Intente de nuevo."
                + Environment.NewLine + Environment.NewLine + ex.Message,
            "Error de base de datos",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

        return false;
    }

    private static bool EsErrorDeNegocio(MySqlException ex)
    {
        // El SQLSTATE es lo que pide la consigna. El codigo 1644 cubre el
        // caso en que el servidor no lo mande.
        return ex.SqlState == "45000" || ex.Number == CodigoDeErrorDeNegocio;
    }
}
