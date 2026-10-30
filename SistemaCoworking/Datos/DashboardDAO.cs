using System;
using MySql.Data.MySqlClient;

namespace SistemaCoworking.Datos;

public class DashboardDAO
{
    public DashboardDAO() { }

    public int ObtenerTotalClientes()
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "SELECT fn_total_clientes()",
                    conexion))
            {
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }
    }

    public int ObtenerReservasPorFinalizar()
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "SELECT fn_reservas_por_finalizar()",
                    conexion))
            {
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }
    }

    public int ObtenerReservasHoy()
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "SELECT fn_reservas_hoy()",
                    conexion))
            {
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }
    }

    public int ObtenerRecursosDisponibles()
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "SELECT fn_recursos_disponibles()",
                    conexion))
            {
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }
    }

    public int ObtenerRecursosReservadosHoy()
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "SELECT fn_recursos_reservados_hoy()",
                    conexion))
            {
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }
    }
}
