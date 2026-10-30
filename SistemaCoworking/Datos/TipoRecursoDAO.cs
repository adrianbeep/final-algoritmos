using SistemaCoworking.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SistemaCoworking.Datos;

public class TipoRecursoDAO
{
    public TipoRecursoDAO() { }

    public List<TipoRecurso> ObtenerTodos()
    {
        List<TipoRecurso> tRecursos = new List<TipoRecurso>();

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            string sql =
                "SELECT id, nombre_tipo_recurso, descripcion " +
                "FROM tipo_recursos " +
                "ORDER BY id ";


            using (MySqlCommand comando =
                new MySqlCommand(sql, conexion))
            {
                using (MySqlDataReader lector = 
                    comando.ExecuteReader())
                {
                    TipoRecurso tipoRecurso;
                    while (lector.Read())
                    {
                        tipoRecurso = 
                            new TipoRecurso(
                                Convert.ToInt32(lector["id"]),
                                Convert.ToString(lector["nombre_tipo_recurso"])!,
                                Convert.ToString(lector["descripcion"])!
                            );

                        tRecursos.Add(tipoRecurso);
                    }
                }
            }
        }

        return tRecursos;
    }
}