using System;
using System.Data;
using MySql.Data.MySqlClient;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Datos;

public class RecursoDAO
{
    public RecursoDAO() { }

    public DataTable ObtenerTodos()
    {
        DataTable recursos = new DataTable();

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            string sql =
                "SELECT id_recurso, recurso_nombre, recurso_ubicacion, estado, precio_dia, nombre_tipo_recurso " +
                "FROM vw_recursos_estado " +
                "ORDER BY id_recurso ";

            using (MySqlDataAdapter adaptador =
                new MySqlDataAdapter(
                    sql,
                    conexion))
            {
                adaptador.Fill(recursos);
            }
        }

        return recursos;
    }

    public Recurso? ObtenerPorId(int id)
    {
        Recurso? r = null;

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            string sql =
                "SELECT r.id, r.id_tipo_recurso, r.nombre, r.estado, r.ubicacion, r.costo, " +
                "tr.nombre_tipo_recurso, " +
                "rd.capacidad_personas, rd.tiene_proyector, rd.numero_monitor, rd.metros_cuadrados " +
                "FROM recursos r " +
                "INNER JOIN tipo_recursos tr ON tr.id = r.id_tipo_recurso " +
                "LEFT JOIN recursos_detalle rd ON rd.id_recurso = r.id " +
                "WHERE r.id = @id ";

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
                    if (lector.Read())
                    {
                        r = Construir(
                            Convert.ToString(lector["nombre_tipo_recurso"])!,
                            Convert.ToInt32(lector["id"]),
                            Convert.ToInt32(lector["id_tipo_recurso"]),
                            Convert.ToString(lector["nombre"])!,
                            Enum.Parse<Recurso.EstadoRecurso>(Convert.ToString(lector["estado"])!),
                            Convert.ToString(lector["ubicacion"])!,
                            Convert.ToDecimal(lector["costo"]),
                            LeerInt(lector["capacidad_personas"]),
                            LeerBool(lector["tiene_proyector"]),
                            LeerInt(lector["numero_monitor"]),
                            LeerDecimal(lector["metros_cuadrados"])
                        );
                    }
                }
            }
        }

            
        return r;
    }


    public void Insertar(Recurso recurso)
    {
        object capacidadPersonas = DBNull.Value;
        object tieneProyector = DBNull.Value;
        object numeroMonitor = DBNull.Value;
        object metrosCuadrados = DBNull.Value;

        switch (recurso)
        {
            case Sala sala:
                capacidadPersonas = sala.CapacidadPersonas;
                tieneProyector = sala.TieneProyector;
                break;

            case PuestoTrabajo puestoTrabajo:
                numeroMonitor = puestoTrabajo.NumeroMonitor;
                break;

            case OficinaPrivada oficinaPrivada:
                metrosCuadrados = Convert.ToInt32(oficinaPrivada.MetrosCuadrados);
                break;
        }

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "sp_alta_recurso",
                    conexion))
            {
                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.AddWithValue(
                    "p_id_tipo_recurso",
                    recurso.IdTipoRecurso
                );

                comando.Parameters.AddWithValue(
                    "p_nombre",
                    recurso.Nombre
                );

                comando.Parameters.AddWithValue(
                    "p_estado",
                    recurso.Estado.ToString()
                );

                comando.Parameters.AddWithValue(
                    "p_ubicacion",
                    recurso.Ubicacion
                );

                comando.Parameters.AddWithValue(
                    "p_costo",
                    recurso.Costo
                );

                // El precio diario no se pasa como dato: sale de CalcularPrecio(),
                // que es la unica fuente de verdad de la regla de precio por tipo.
                comando.Parameters.AddWithValue(
                    "p_precio_dia",
                    recurso.CalcularPrecio()
                );

                comando.Parameters.AddWithValue(
                    "p_capacidad_personas",
                    capacidadPersonas
                );

                comando.Parameters.AddWithValue(
                    "p_tiene_proyector",
                    tieneProyector
                );

                comando.Parameters.AddWithValue(
                    "p_numero_monitor",
                    numeroMonitor
                );

                comando.Parameters.AddWithValue(
                    "p_metros_cuadrados",
                    metrosCuadrados
                );

                comando.ExecuteNonQuery();
            }
        }
    }

    public void Modificar(Recurso recurso)
    {
        object capacidadPersonas = DBNull.Value;
        object tieneProyector = DBNull.Value;
        object numeroMonitor = DBNull.Value;
        object metrosCuadrados = DBNull.Value;

        switch (recurso)
        {
            case Sala sala:
                capacidadPersonas = sala.CapacidadPersonas;
                tieneProyector = sala.TieneProyector;
                break;

            case PuestoTrabajo puestoTrabajo:
                numeroMonitor = puestoTrabajo.NumeroMonitor;
                break;

            case OficinaPrivada oficinaPrivada:
                metrosCuadrados = Convert.ToInt32(oficinaPrivada.MetrosCuadrados);
                break;
        }

        using (MySqlConnection conexion =
            ConexionBD.CrearConexion())
        {
            conexion.Open();

            using (MySqlCommand comando =
                new MySqlCommand(
                    "sp_modificar_recurso",
                    conexion))
            {
                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.AddWithValue(
                    "p_id_recurso",
                    recurso.Id
                );

                comando.Parameters.AddWithValue(
                    "p_nombre",
                    recurso.Nombre
                );

                comando.Parameters.AddWithValue(
                    "p_estado",
                    recurso.Estado.ToString()
                );

                comando.Parameters.AddWithValue(
                    "p_ubicacion",
                    recurso.Ubicacion
                );

                comando.Parameters.AddWithValue(
                    "p_costo",
                    recurso.Costo
                );

                comando.Parameters.AddWithValue(
                    "p_precio_dia",
                    recurso.CalcularPrecio()
                );

                comando.Parameters.AddWithValue(
                    "p_capacidad_personas",
                    capacidadPersonas
                );

                comando.Parameters.AddWithValue(
                    "p_tiene_proyector",
                    tieneProyector
                );

                comando.Parameters.AddWithValue(
                    "p_numero_monitor",
                    numeroMonitor
                );

                comando.Parameters.AddWithValue(
                    "p_metros_cuadrados",
                    metrosCuadrados
                );

                comando.ExecuteNonQuery();
            }
        }
    }

    private static Recurso Construir(
        string tipoRecurso,
        int id, 
        int idTipoRecurso, 
        string nombre, 
        Recurso.EstadoRecurso estado,
        string ubicacion,
        decimal costo,
        int capacidadPersonas,
        bool tieneProyector,
        int numeroMonitor,
        decimal metrosCuadrados
    )
    {
        switch (tipoRecurso)
        {
            case "Sala":            return new Sala(id, idTipoRecurso, nombre, ubicacion, costo, capacidadPersonas, tieneProyector, estado);
            case "PuestoTrabajo":   return new PuestoTrabajo(id, idTipoRecurso, nombre, ubicacion, costo, numeroMonitor, estado);
            case "OficinaPrivada":  return new OficinaPrivada(id, idTipoRecurso, nombre, ubicacion, costo, metrosCuadrados, estado);
            default: throw new InvalidOperationException($"Tipo de recurso desconocido: '{tipoRecurso}'");
        }
    }

    private static int LeerInt(object valor) => valor is DBNull ? 0 : Convert.ToInt32(valor);

    private static bool LeerBool(object valor) => valor is DBNull ? false : Convert.ToBoolean(valor);

    private static decimal LeerDecimal(object valor) => valor is DBNull ? 0m : Convert.ToDecimal(valor);
}