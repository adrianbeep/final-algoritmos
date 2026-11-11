using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Datos;
/*
 * El DAO Data Access Object sirve para hacer consultas o
 * ejecucion de StoredProcedures, etc sin que la interfaz grafica o
 * el dominio (entidades con logica propia)
 * tengan que tener conocimiento de la base de datos que los persiste
 *
 * Los errores de la base NO se capturan aca: el DAO lanza la excepcion y
 * la captura la capa de presentacion (el modal o el form), que es la que
 * decide como mostrarla. Por eso esta capa no depende de WinForms.
 */
public class ClienteDAO
{
    public ClienteDAO() { }

    /// <summary>
    /// Listado plano de clientes para la grilla.
    ///
    /// Es lo que usa el DataGridView: las columnas se bindean a los nombres
    /// de las columnas de la tabla, no a las propiedades del modelo. Igual que
    /// el listado de recursos.
    /// </summary>
    public DataTable ObtenerTodos()
    {
        DataTable clientes = new DataTable();

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            string sql =
                "SELECT id, nombre, apellido, dni, telefono, email " +
                "FROM clientes " +
                "ORDER BY id ";

            using (MySqlDataAdapter adaptador =
                new MySqlDataAdapter(
                    sql,
                    conexion))
            {
                adaptador.Fill(clientes);
            }
        }

        return clientes;
    }

    /// <summary>
    /// Lista de clientes.
    ///
    /// Es lo que necesita el modal de Reservas para armar el combo: el combo
    /// muestra la descripcion del cliente y guarda el Id.
    ///
    /// El listado de la grilla sigue usando ObtenerTodos(), que devuelve la
    /// tabla plana.
    /// </summary>
    public List<Cliente> ObtenerTodosComoLista()
    {
        List<Cliente> clientes = new List<Cliente>();

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            string sql =
                "SELECT id, nombre, apellido, dni, telefono, email " +
                "FROM clientes " +
                "ORDER BY id ";

            using (MySqlCommand comando =
                new MySqlCommand(sql, conexion))
            {
                using (MySqlDataReader lector =
                    comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Cliente c =
                            new Cliente(
                                Convert.ToInt32(lector["id"]),
                                Convert.ToString(lector["nombre"])!,
                                Convert.ToString(lector["apellido"])!,
                                Convert.ToInt32(lector["dni"]),
                                Convert.ToString(lector["telefono"])!,
                                Convert.ToString(lector["email"])!
                            );

                        clientes.Add(c);
                    }
                }
            }
        }

        return clientes;
    }

    public Cliente? ObtenerPorId(int id)
    {
        Cliente? c = null;

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            string sql =
                "SELECT id, nombre, apellido, dni, telefono, email " +
                "FROM clientes " +
                "WHERE id = @id "; //@id indica que es parametro

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
                        c = new Cliente(
                            Convert.ToInt32(lector["id"]),
                            Convert.ToString(lector["nombre"])!,
                            Convert.ToString(lector["apellido"])!,
                            Convert.ToInt32(lector["dni"])!,
                            Convert.ToString(lector["telefono"])!,
                            Convert.ToString(lector["email"])!
                        );
                    }
                }
            }
        }

        return c;
    }

    /// <summary>
    /// Da de alta un cliente.
    ///
    /// El Id lo asigna la base: en el alta la entidad llega con Id = 0 y el
    /// procedimiento almacenado no lo recibe. El DAO lo ignora.
    /// </summary>
    public void Insertar(Cliente cliente)
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "sp_alta_cliente",
                    conexion))
            {
                //OBLIGATORIO
                comando.CommandType =
                    System.Data.CommandType.StoredProcedure;


                //En general hago que los parametros
                //coincidan con los del StoredProcedure de sql
                comando.Parameters.AddWithValue(
                    "p_nombre",
                    cliente.Nombre
                );

                comando.Parameters.AddWithValue(
                    "p_apellido",
                    cliente.Apellido
                );

                comando.Parameters.AddWithValue(
                    "p_dni",
                    cliente.Dni
                );

                comando.Parameters.AddWithValue(
                    "p_telefono",
                    cliente.Telefono
                );

                comando.Parameters.AddWithValue(
                    "p_email",
                    cliente.Email
                );

                //Siempre en operaciones
                //que hagan INSERT, UPDATE, DELETE
                comando.ExecuteNonQuery();
            }
        }

    }

    public void Modificar(Cliente cliente)
    {
        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "sp_modificar_cliente",
                    conexion))
            {
                //OBLIGATORIO
                comando.CommandType =
                    System.Data.CommandType.StoredProcedure;


                //En general hago que los parametros
                //coincidan con los del StoredProcedure de sql
                comando.Parameters.AddWithValue(
                    "p_id",
                    cliente.Id
                );

                comando.Parameters.AddWithValue(
                    "p_nombre",
                    cliente.Nombre
                );

                comando.Parameters.AddWithValue(
                    "p_apellido",
                    cliente.Apellido
                );

                comando.Parameters.AddWithValue(
                    "p_dni",
                    cliente.Dni
                );

                comando.Parameters.AddWithValue(
                    "p_telefono",
                    cliente.Telefono
                );

                comando.Parameters.AddWithValue(
                    "p_email",
                    cliente.Email
                );

                //Siempre en operaciones
                //que hagan INSERT, UPDATE, DELETE
                comando.ExecuteNonQuery();
            }
        }

    }
}
