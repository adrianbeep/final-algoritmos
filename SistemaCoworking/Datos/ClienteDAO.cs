using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Datos;
/*
 * El DAO Data Access Object sirve para hacer consultas o
 * ejecucion de StoredProcedures, etc sin que la interfaz grafica o 
 * el dominio (entidades con logica propia)
 * tengan que tener conocimiento de la base de datos que los persiste
 */
public class ClienteDAO
{
    public ClienteDAO() { }

    public List<Cliente> ObtenerTodos()
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
                                Convert.ToString(lector["nombre"]),
                                Convert.ToString(lector["apellido"]),
                                Convert.ToInt32(lector["dni"]),
                                Convert.ToString(lector["telefono"]),
                                Convert.ToString(lector["email"])
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
                            Convert.ToString(lector["nombre"]),
                            Convert.ToString(lector["apellido"]),
                            Convert.ToInt32(lector["dni"]),
                            Convert.ToString(lector["telefono"]),
                            Convert.ToString(lector["email"])
                        );
                    }
                }
            }
        }

        return c;
    }

    public void Insertar(
        string nombre,
        string apellido,
        int dni,
        string telefono,
        string email)
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
                    nombre
                );

                comando.Parameters.AddWithValue(
                    "p_apellido",
                    apellido
                );

                comando.Parameters.AddWithValue(
                    "p_dni",
                    dni
                );

                comando.Parameters.AddWithValue(
                    "p_telefono",
                    telefono
                );

                comando.Parameters.AddWithValue(
                    "p_email",
                    email
                );

                //Siempre en operaciones
                //que hagan INSERT, UPDATE, DELETE
                comando.ExecuteNonQuery();
            }
        }
    }

    public void Modificar(
        int id,
        string nombre,
        string apellido,
        int dni,
        string telefono,
        string email)
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
                    id
                );

                comando.Parameters.AddWithValue(
                    "p_nombre",
                    nombre
                );

                comando.Parameters.AddWithValue(
                    "p_apellido",
                    apellido
                );

                comando.Parameters.AddWithValue(
                    "p_dni",
                    dni
                );

                comando.Parameters.AddWithValue(
                    "p_telefono",
                    telefono
                );

                comando.Parameters.AddWithValue(
                    "p_email",
                    email
                );

                //Siempre en operaciones
                //que hagan INSERT, UPDATE, DELETE
                comando.ExecuteNonQuery();
            }
        }
    }
}