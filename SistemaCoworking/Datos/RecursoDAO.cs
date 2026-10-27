using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Datos;

public class RecursoDAO
{
    public RecursoDAO() { }
    public List<Recurso> ObtenerTodos()
    {
        List<Recurso> recursos = new List<Recurso>();
        using (MySqlConnection conexion = ConexionBD.CrearConexion())
        {
            conexion.Open();
            string sql =
                "SELECT id, id_tipo_recurso,nombre_tipo_recurso, descripcion " +
                "FROM recursos " +
                "ORDER BY id ";
            using (MySqlCommand comando = new MySqlCommand(sql, conexion))
            {
                using (MySqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Recurso r = new Recurso(
                                Convert.ToInt32(lector["id"]),
                                Convert.ToString(lector["nombre"]),
                                Convert.ToString(lector["descripcion"]),
                                Convert.ToInt32(lector["id_tipo_recurso"])
                            );
                        recursos.Add(r);
                    }
                }
            }
        }
        return recursos;
    }

}