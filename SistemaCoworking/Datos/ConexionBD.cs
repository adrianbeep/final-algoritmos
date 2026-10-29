using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Datos;

public static class ConexionBD
{
    private static IConfiguration? configuracion = null;

    public static MySqlConnection CrearConexion()
    {
        return new MySqlConnection(ObtenerCadenaConexion());
    }

    public static string ObtenerCadenaConexion()
    {
        if (configuracion == null)
        {
            configuracion = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }

        string? cadena = configuracion.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(cadena))
        {
            throw new InvalidOperationException(
                "No se encontro la clave ConnectionStrings:Default en appsettings.json.");
        }

        return cadena;
    }
}