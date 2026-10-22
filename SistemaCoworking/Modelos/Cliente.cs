using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCoworking.Modelos;

public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public int Dni { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }

    public Cliente(int id, string nombre, string apellido, int dni, string telefono, string email)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
        Telefono = telefono;
        Email = email;
    }
}