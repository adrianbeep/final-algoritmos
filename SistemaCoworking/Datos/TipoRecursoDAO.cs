using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Datos;

public class TipoRecursoDAO
{
    public TipoRecursoDAO() { }

    public List<TipoRecurso> ObtenerTodos()
    {
        List<TipoRecurso> tipos= new List<TipoRecurso>();

        using (MySqlConnection conexion = ConexionBD.CrearConexion())
        {
            conexion.Open();
            string sql =
                "SELECT id, nombre_tipo_recurso , descripcion " +
                "FROM tipos_recursos " +
                "ORDER BY id ";
            using (MySqlCommand comando = new MySqlCommand(sql, conexion))
            {
                using (MySqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        TipoRecurso tr = new TipoRecurso(
                                Convert.ToInt32(lector["id"]),
                                Convert.ToString(lector["nombre_tipo_recurso"]),
                                Convert.ToString(lector["descripcion"])
                            );
                        tipos.Add(tr);
                    }
                }
            }
        }
        return tipos;
    }
}