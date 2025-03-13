using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class Home : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acceso al contexto HTTP (para obtener la sesión)
        private readonly DataBaseContext _dbContext;  // Instancia para acceder a la base de datos

        // Propiedades para almacenar el nickname y la foto de perfil del usuario
        public string Nickname { get; set; }
        public string FotoPerfil { get; set; }

        // Constructor que inyecta dependencias para acceso a la sesión y la base de datos
        public Home(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;  // Asigna el IHttpContextAccessor a la propiedad
            _dbContext = new DataBaseContext();  // Crea una nueva instancia de DataBaseContext para interactuar con la base de datos
        }

        // Método que se ejecuta al cargar la página
        public void OnGet()
        {
            // Intenta obtener el nickname del usuario desde la sesión, o asigna "Invitado" si no está disponible
            Nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario") ?? "Invitado";  

            // Si el usuario está autenticado, obtiene la foto de perfil desde la base de datos
            if (Nickname != "Invitado")
            {
                FotoPerfil = ObtenerFotoDePerfil(Nickname);  // Llama a la función que obtiene la foto de perfil
            }
            else
            {
                // Si no hay un usuario en sesión, asigna una imagen por defecto
                FotoPerfil = "default.png"; 
            }
        }

        // Función que obtiene la foto de perfil desde la base de datos usando el nickname
        private string ObtenerFotoDePerfil(string nickname)
        {
            string foto = "default.png"; // Valor por defecto si no se encuentra foto en la base de datos
            using (var conexion = new MySqlConnection(_dbContext.ConnectionString))  // Establece la conexión con la base de datos
            {
                try
                {
                    conexion.Open();  // Abre la conexión con la base de datos
                    // Consulta SQL que obtiene la fotografía del usuario según el nickname
                    string query = "SELECT fotografia FROM usuarios WHERE nickname = @nickname";
                    using (var cmd = new MySqlCommand(query, conexion))  // Prepara el comando SQL
                    {
                        cmd.Parameters.AddWithValue("@nickname", nickname);  // Agrega el parámetro del nickname a la consulta
                        object result = cmd.ExecuteScalar();  // Ejecuta la consulta y obtiene el valor de la foto
                        if (result != null && !string.IsNullOrEmpty(result.ToString()))  // Verifica si el resultado es válido
                        {
                            foto = result.ToString();  // Asigna la foto obtenida a la variable foto
                        }
                    }
                }
                catch (Exception ex)  // Maneja cualquier error de la conexión o consulta
                {
                    Console.WriteLine("Error al obtener la foto de perfil: " + ex.Message);  // Imprime el error en la consola
                }
            }
            return foto;  // Retorna la foto (o "default.png" si no se encontró una foto válida)
        }
    }
}
