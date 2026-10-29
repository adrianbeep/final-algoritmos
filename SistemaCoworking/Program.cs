using SistemaCoworking.Datos;

namespace SistemaCoworking;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        try
        {
            //Valido la configuracion una sola vez, antes de abrir cualquier form
            ConexionBD.ObtenerCadenaConexion();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "No se pudo leer la configuracion de la base de datos.\n\n" +
                "Revisa que exista el archivo appsettings.json en:\n" +
                $"{AppContext.BaseDirectory}\n\n" +
                ex.Message,
                "Error de configuracion",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        Application.Run(new FrmPrincipal());
    }
}