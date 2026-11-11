using MySqlConnector;
using SistemaCoworking.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SistemaCoworking.Datos;

/// <summary>
/// Los errores de la base NO se capturan aca: el DAO lanza la excepcion y la
/// captura la capa de presentacion (el modal o el form), que es la que decide
/// como mostrarla. Por eso esta capa no depende de WinForms.
///
/// Los metodos de escritura devuelven void: la excepcion es lo que le avisa a
/// la presentacion que la operacion fallo, asi el modal que pidio guardar sigue
/// abierto y el usuario puede corregir y reintentar sin perder lo que escribio.
/// </summary>
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

    /// <summary>
    /// Si el recurso ya tiene una reserva confirmada que se superpone con el
    /// periodo indicado.
    ///
    /// La regla de superposicion vive en la funcion fn_recurso_ocupado, que es
    /// la misma que usan los procedimientos almacenados: la base y la
    /// aplicacion no pueden discrepar.
    ///
    /// idReservaExcluida sirve para editar una reserva sin que se superponga
    /// consigo misma: se pasa el id de la reserva que se esta editando.
    /// </summary>
    public bool RecursoOcupado(int idRecurso, DateTime fechaInicio, DateTime fechaFin, int idReservaExcluida = 0)
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "SELECT fn_recurso_ocupado(@id_recurso, @fecha_inicio, @fecha_fin, @id_reserva_excluida)",
                    conexion))
            {
                comando.Parameters.AddWithValue(
                    "@id_recurso",
                    idRecurso
                );

                comando.Parameters.AddWithValue(
                    "@fecha_inicio",
                    fechaInicio
                );

                comando.Parameters.AddWithValue(
                    "@fecha_fin",
                    fechaFin
                );

                comando.Parameters.AddWithValue(
                    "@id_reserva_excluida",
                    idReservaExcluida
                );

                return Convert.ToInt32(comando.ExecuteScalar()) == 1;
            }
        }
    }

    /// <summary>
    /// Da de alta una reserva.
    ///
    /// El TotalReserva lo calcula el modelo antes de guardar: el DAO no lo
    /// recalcula, escribe el que trae la entidad. El Id tambien lo asigna la
    /// base y no se pasa.
    /// </summary>
    public void Insertar(Reserva reserva)
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
                    reserva.IdCliente
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
