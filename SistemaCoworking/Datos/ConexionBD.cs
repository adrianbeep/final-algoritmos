using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Datos;

public static class ConexionBD
{
    private static string cadenaConexion =
        "server=localhost;" +
        "port=3306;" +
        "database=coworking;" +
        "uid=root;" +
        "pwd=;";

    public static MySqlConnection CrearConexion()
    {
        return new MySqlConnection(cadenaConexion);
    }
}