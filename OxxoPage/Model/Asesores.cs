using System;
namespace OxxoPage.Model
{
    public class Asesores
    {
        public int IdUsuario { get; set; }
        public int IdAsesor { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
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
        public string Nickname { get; set; }
        public string AboutMe { get; set; }
        public int Posicion { get; set; }
        public int Medallas { get; set; }

        public int NumOxxos { get; set; }
        // Constructor para Login

        // Constructor vacío para inicializar el objeto sin parámetros
        public Asesores() { }
    }

}