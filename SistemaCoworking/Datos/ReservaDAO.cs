using MySql.Data.MySqlClient;
using SistemaCoworking.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SistemaCoworking.Datos;

public class ReservaDAO
{
    public ReservaDAO() { }

    public DataTable ObtenerTodos()
    {
        DataTable reservas = new DataTable();

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            string sql =
                "SELECT id_reserva, fecha_inicio, fecha_fin, estado_reserva, total_reserva, cliente_nombre, dni, recurso_nombre, recurso_ubicacion, nombre_tipo_recurso " +
                "FROM vw_reservas_completas " +
                "ORDER BY fecha_inicio DESC ";

            using (MySqlDataAdapter adaptador =
                new MySqlDataAdapter(sql, conexion))
            {
                adaptador.Fill(reservas);
            }
        }

        return reservas;
    }

    public Reserva? ObtenerPorId(int id)
    {
        Reserva? r = null;

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            string sql =
                "SELECT id, id_cliente, id_recurso, fecha_inicio, fecha_fin, estado_reserva, total_reserva " +
                "FROM reservas " +
                "WHERE id = @id ";

            using (MySqlCommand comando =
                new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue(
                    "@id",
                    id
                );

                using (MySqlDataReader lector =
                    comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        r = new Reserva(
                            Convert.ToInt32(lector["id"]),
                            Convert.ToInt32(lector["id_cliente"]),
                            Convert.ToInt32(lector["id_recurso"]),
                            Convert.ToDateTime(lector["fecha_inicio"]),
                            Convert.ToDateTime(lector["fecha_fin"]),
                            Enum.Parse<Reserva.EstadoReserva>(lector["estado_reserva"].ToString()!),
                            Convert.ToDecimal(lector["total_reserva"])
                        );
                    }
                }
            }
        }

        return r;
    }

    public void Insertar(
        int idCliente,
        int idRecurso,
        DateTime fechaInicio,
        DateTime fechaFin,
        decimal total_reserva
    )
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "sp_alta_reserva",
                    conexion))
            {
                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.AddWithValue(
                    "p_id_cliente",
                    idCliente
                );

                comando.Parameters.AddWithValue(
                    "p_id_recurso",
                    idRecurso
                );

                comando.Parameters.AddWithValue(
                    "p_fecha_inicio",
                    fechaInicio
                );

                comando.Parameters.AddWithValue(
                    "p_fecha_fin",
                    fechaFin
                );

                comando.Parameters.AddWithValue(
                    "p_total_reserva",
                    total_reserva
                );

                comando.ExecuteNonQuery();
            }
        }
    }

    public void Modificar(Reserva reserva)
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "sp_modificar_reserva",
                    conexion))
            {
                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.AddWithValue(
                    "p_id_reserva",
                    reserva.Id
                );

                comando.Parameters.AddWithValue(
                    "p_id_recurso",
                    reserva.IdRecurso
                );

                comando.Parameters.AddWithValue(
                    "p_fecha_inicio",
                    reserva.FechaInicio
                );

                comando.Parameters.AddWithValue(
                    "p_fecha_fin",
                    reserva.FechaFin
                );

                comando.Parameters.AddWithValue(
                    "p_total_reserva",
                    reserva.TotalReserva
                );

                comando.ExecuteNonQuery();
            }
        }
    }

    public void Cancelar(int id)
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "sp_cancelar_reserva",
                    conexion))
            {
                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.AddWithValue(
                    "p_id_reserva",
                    id
                );

                comando.ExecuteNonQuery();
            }
        }
    }
}