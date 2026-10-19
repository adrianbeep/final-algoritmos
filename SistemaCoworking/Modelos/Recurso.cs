using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public abstract class Recurso
{
    public int id { get; set; }
    public int IdTipoRecurso { get; set; }
    public string Nombre { get; set; }
    
}