using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Datos;

/// <summary>
/// Los errores de la base NO se capturan aca: el DAO lanza la excepcion y la captura la`r`n    /// capa de presentacion (el modal o el form), que es la que decide como mostrarla.`r`n    /// Por eso esta capa no depende de WinForms.`r`n/// Los de escritura devuelven false para que el modal que pidio guardar sepa
/// que fallo y siga abierto: el usuario puede corregir y reintentar sin
/// perder lo que escribio.
/// </summary>
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

    /// <summary>
    /// Lista de recursos con su tipo concreto (Sala, PuestoTrabajo u
    /// OficinaPrivada).
    ///
    /// Es lo que necesita el modal de Reservas: el total sale de
    /// Recurso.PrecioDia, y ese precio depende de los datos del tipo
    /// (proyector, monitores, metros), que solo estan en recursos_detalle.
    ///
    /// El listado de la grilla sigue usando ObtenerTodos(), que devuelve la
    /// vista plana: ahi la columna del tipo no se puede bindear, porque
    /// Recurso no tiene el nombre del tipo, solo su IdTipoRecurso.
    /// </summary>
    public List<Recurso> ObtenerTodosComoLista()
    {
        List<Recurso> recursos = new List<Recurso>();

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
                "ORDER BY r.id ";

            using (MySqlCommand comando =
                new MySqlCommand(sql, conexion))
            using (MySqlDataReader lector =
                comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    Recurso? recurso = RecursoFactory.Crear(
                        Convert.ToString(lector["nombre_tipo_recurso"])!,
                        Convert.ToInt32(lector["id"]),
                        Convert.ToInt32(lector["id_tipo_recurso"]),
                        Convert.ToString(lector["nombre"])!,
                        Convert.ToString(lector["ubicacion"])!,
                        Convert.ToDecimal(lector["costo"]),
                        Enum.Parse<Recurso.EstadoRecurso>(Convert.ToString(lector["estado"])!),
                        LeerInt(lector["capacidad_personas"]),
                        LeerBool(lector["tiene_proyector"]),
                        LeerInt(lector["numero_monitor"]),
                        LeerDecimal(lector["metros_cuadrados"])
                    );

                    // La fabrica devuelve null si le falta el dato obligatorio
                    // del tipo. Un recurso sin su fila de detalle no se
                    // muestra: es preferible que no aparezca a que rompa el
                    // combo.
                    if (recurso is not null)
                        recursos.Add(recurso);
                }
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
                        r = RecursoFactory.Crear(
                            Convert.ToString(lector["nombre_tipo_recurso"])!,
                            Convert.ToInt32(lector["id"]),
                            Convert.ToInt32(lector["id_tipo_recurso"]),
                            Convert.ToString(lector["nombre"])!,
                            Convert.ToString(lector["ubicacion"])!,
                            Convert.ToDecimal(lector["costo"]),
                            Enum.Parse<Recurso.EstadoRecurso>(Convert.ToString(lector["estado"])!),
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

                AgregarParametrosDetalle(comando, recurso.Detalle);

                comando.ExecuteNonQuery();
            }
        }

    }

    public void Modificar(Recurso recurso)
    {
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

                AgregarParametrosDetalle(comando, recurso.Detalle);

                comando.ExecuteNonQuery();
            }
        }

    }

    /// <summary>
    /// Agrega los parametros del detalle del tipo. El DAO no sabe que clases
    /// concretas existen: solo escribe lo que el recurso declara, y las
    /// columnas que no aplican al tipo van en NULL.
    /// </summary>
    private static void AgregarParametrosDetalle(
        MySqlCommand comando,
        DetalleTipo detalle)
    {
        comando.Parameters.AddWithValue(
            "p_capacidad_personas",
            (object?)detalle.CapacidadPersonas ?? DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "p_tiene_proyector",
            (object?)detalle.TieneProyector ?? DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "p_numero_monitor",
            (object?)detalle.NumeroMonitor ?? DBNull.Value
        );

        // El dominio maneja los metros como decimal y la columna es entera.
        // La conversion es de la capa de datos, y es exacta porque la fabrica
        // no deja pasar valores con decimales.
        comando.Parameters.AddWithValue(
            "p_metros_cuadrados",
            detalle.MetrosCuadrados.HasValue
                ? Convert.ToInt32(detalle.MetrosCuadrados.Value)
                : (object)DBNull.Value
        );
    }

    // Los datos especificos del tipo llegan como null cuando la columna no
    // aplica a ese tipo. No se convierten a 0: la fabrica necesita distinguir
    // "no aplica" de "vale cero".
    private static int? LeerInt(object valor) => valor is DBNull ? null : Convert.ToInt32(valor);

    private static bool? LeerBool(object valor) => valor is DBNull ? null : Convert.ToBoolean(valor);

    private static decimal? LeerDecimal(object valor) => valor is DBNull ? null : Convert.ToDecimal(valor);
}
