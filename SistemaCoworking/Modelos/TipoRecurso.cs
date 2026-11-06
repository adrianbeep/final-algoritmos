using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class TipoRecurso
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public TipoRecurso(int id, string nombre, string descripcion)
    {
        Id = id;
        Nombre = nombre;
        Descripcion = descripcion;
    }

    /// <summary>
    /// Nombre del tipo. Es lo que se muestra en los combos y en los listados,
    /// para que no aparezca el nombre de la clase.
    /// </summary>
    public override string ToString() => Nombre;
}