using System;
namespace OxxoPage.Model
{
public class Usuarios
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; }
    public string ApellidoMaterno { get; set; }
    public string ApellidoPaterno { get; set; }
    public string Telefono { get; set; }
    public string Fotografia { get; set; }
    public string CodigoPostal { get; set; }
    public string NumNomina { get; set; }
    public int IdEstado { get; set; }
    public int IdCiudad { get; set; }
    public string Colonia { get; set; }
    public string Municipio { get; set; }
    public string NumCasa { get; set; }
    public string Calle { get; set; }
    public string Contrasena { get; set; }
    public string CorreoElectronico { get; set; }
    public string Nickname { get; set; }
    public string AboutMe { get; set; }

    // Constructor para Login
    public Usuarios (string nickname, string contrasena)
    {
        Nickname = nickname;
        Contrasena = contrasena;
    }

    // Constructor para Sign In 
    public Usuarios(string nombre, string contrasena, string nickname)
    {
        Nombre = nombre;
        Contrasena = contrasena;
        Nickname = nickname;
    }

    // Constructor vacío para inicializar el objeto sin parámetros
    public Usuarios() { }
}

}