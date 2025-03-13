using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class Profile : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;  // Acceso al contexto HTTP (para obtener la sesión)
        private readonly DataBaseContext _dbContext;  // Instancia para acceder a la base de datos

        // Propiedades públicas para almacenar los datos del usuario
        public string UserName { get; set; }  // Nombre completo del usuario
        public string Biography { get; set; }  // Biografía del usuario
        public string ProfileImageUrl { get; set; }  // URL de la imagen de perfil
        public string Role { get; set; }  // Rol del usuario (Asesor, Gerente, Usuario estándar)
        public int Level { get; set; } = 1;  // Nivel del usuario (se calculará más adelante)
        public int CurrentXP { get; set; } = 0;  // Experiencia actual del usuario
        public int RequiredXP { get; set; } = 100;  // XP necesario para subir de nivel
        public int RequiredXPBar { get; set; } = 100;  // XP necesario para el siguiente nivel (ajustado más tarde)
        public List<Achievement> Achievements { get; set; } = new();  // Lista de logros del usuario (se llenará con datos reales)

        // Constructor que inyecta dependencias para acceso a la sesión y la base de datos
        public Profile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;  // Asigna el IHttpContextAccessor a la propiedad
            _dbContext = new DataBaseContext();  // Crea una nueva instancia de DataBaseContext para interactuar con la base de datos
        }

        // Método que se ejecuta cuando se carga la página
        public void OnGet()
        {
            // Obtiene el nickname del usuario desde la sesión (o "Invitado" si no hay un usuario autenticado)
            string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario");

            if (!string.IsNullOrEmpty(nickname))
            {
                // Si el usuario está autenticado, obtiene sus datos
                var usuario = _dbContext.ObtenerDatosUsuario(nickname);  // Llama a la función que obtiene los datos del usuario

                if (usuario != null)
                {
                    // Asigna el nombre completo y la biografía del usuario
                    UserName = $"{usuario.Nombre} {usuario.ApellidoPaterno} {usuario.ApellidoMaterno}".Trim();
                    ProfileImageUrl = usuario.Fotografia;  // Asigna la foto de perfil del usuario
                    Biography = usuario.AboutMe;  // Asigna la biografía del usuario

                    // Obtiene el rol del usuario (Asesor, Gerente, Usuario estándar)
                    Role = _dbContext.ObtenerRolUsuario(usuario.IdUsuario);

                    // 🔹 Obtiene los logros del usuario y calcula su XP total
                    Achievements = _dbContext.ObtenerLogrosUsuario(nickname, out int totalXP);
                    CurrentXP = totalXP;  // Asigna la experiencia total del usuario

                    // Calcula el nivel del usuario en función de la experiencia obtenida
                    CalcularNivelUsuario();
                }
                else
                {
                    // Si no se encuentra el usuario, asigna valores predeterminados
                    UserName = "Invitado";
                    ProfileImageUrl = "default.png";  // Imagen por defecto si no hay usuario en sesión
                    Biography = "Este usuario aún no ha escrito su biografía.";
                    Role = "Usuario estándar";  // Rol predeterminado si no hay rol asignado
                }
            }
        }

        // Método privado que calcula el nivel del usuario en base a su experiencia
        private void CalcularNivelUsuario()
        {
            // Calcula el nivel en función de la experiencia acumulada (cada 100 XP sube un nivel)
            Level = (CurrentXP / 100) + 1;  // Obtiene el nivel actual

            // Calcula la experiencia restante para mostrar en la barra de progreso
            CurrentXP = CurrentXP % 100;  // XP restante para el nivel actual

            // Calcula el XP necesario para el siguiente nivel
            RequiredXP = 100 - CurrentXP;  // Esto indica cuánto le falta para el siguiente nivel
        }
    }
}
