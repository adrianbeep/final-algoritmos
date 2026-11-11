using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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

    /// <summary>
    /// Como se ve el cliente: nombre, apellido y DNI. Es lo que muestran
    /// los combos y los listados: con el nombre solo no se distingue a dos
    /// clientes que se llaman igual.
    ///
    /// Es un dato derivado, hecho para mostrar: no va en el JSON de la
    /// exportacion, que lleva los datos del cliente.
    /// </summary>
    [JsonIgnore]
    public string Descripcion => $"{Nombre} {Apellido} - DNI {Dni}";

    /// <summary>
    /// El cliente se muestra con su nombre, su apellido y su DNI.
    /// </summary>
    public override string ToString() => Descripcion;

    public static bool ValidarNombre(string nombre, out string? mensajeError)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            mensajeError = "Ingrese un nombre.";
            return false;
        }

        if (nombre.Length > 100)
        {
            mensajeError = "Ingrese un nombre menor a 100 caracteres";
            return false;
        }

        mensajeError = null;
        return true;
    }

    public static bool ValidarApellido(string apellido, out string? mensajeError)
    {
        if (string.IsNullOrEmpty(apellido))
        {
            mensajeError = "Ingrese un apellido.";
            return false;
        }

        if (apellido.Length > 100)
        {
            mensajeError = "Ingrese un apellido menor a 100 caracteres";
            return false;
        }

        mensajeError = null;
        return true;
    }

    public static bool ValidarDni(string dni, out string? mensajeError)
    {
        if (string.IsNullOrEmpty(dni))
        {
            mensajeError = "Ingrese un dni.";
            return false;
        }

        if (dni.Length > 18)
        {
            mensajeError = "Ingrese un dni menor a 18 caracteres.";
            return false;
        }

        if (!int.TryParse(dni, out int resultado))
        {
            mensajeError = "Ingrese un dni con caracteres numericos.";
            return false;
        }

        mensajeError = null;
        return true;
    }

    public static bool ValidarTelefono(string telefono, out string? mensajeError)
    {
        if (string.IsNullOrEmpty(telefono))
        {
            mensajeError = "Ingrese un telefono.";
            return false;
        }

        if (telefono.Length > 30)
        {
            mensajeError = "Ingrese un telefono menor a 30 caracteres.";
            return false;
        }

        mensajeError = null;
        return true;
    }

    public static bool ValidarEmail(string email, out string? mensajeError)
    {
        if (string.IsNullOrEmpty(email))
        {
            mensajeError = "Ingrese un email.";
            return false;
        }

        if (email.Length > 50)
        {
            mensajeError = "Ingrese un email menor a 50 caracteres.";
            return false;
        }

        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            mensajeError = "Formato de email invalido.";
            return false;
        }

        mensajeError = null;
        return true;
    }
}