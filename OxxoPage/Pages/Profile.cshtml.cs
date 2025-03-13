using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;


namespace OxxoPage.Pages;
    public class Profile : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _dbContext;

        public string UserName { get; set; }
        public string Biography { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Role { get; set; }
        public int Level { get; set; } = 1;  // 🔹 Se calculará después
        public int CurrentXP { get; set; } = 0;
        public int RequiredXP { get; set; } = 100;
        public int RequiredXPBar { get; set; } = 100; // 🔹 Se ajustará luego
        public List<Achievement> Achievements { get; set; } = new(); // 🔹 Se llenará con los datos reales

        public Profile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = new DataBaseContext();
        }

        public void OnGet()
        {
            string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario");

            if (!string.IsNullOrEmpty(nickname))
            {
                // Obtener datos del usuario
                var usuario = _dbContext.ObtenerDatosUsuario(nickname);
                if (usuario != null)
                {
                    UserName = $"{usuario.Nombre} {usuario.ApellidoPaterno} {usuario.ApellidoMaterno}".Trim();
                    ProfileImageUrl = usuario.Fotografia;
                    Biography = usuario.AboutMe;

                    // Obtener el rol del usuario
                    Role = _dbContext.ObtenerRolUsuario(usuario.IdUsuario);

                    // 🔹 Obtener logros desde `instancias_logro` y calcular XP total
                    Achievements = _dbContext.ObtenerLogrosUsuario(nickname, out int totalXP);
                    CurrentXP = totalXP;
                    CalcularNivelUsuario();
                    
                }
                else
                {
                    UserName = "Invitado";
                    ProfileImageUrl = "default.png";
                    Biography = "Este usuario aún no ha escrito su biografía.";
                    Role = "Usuario estándar";
                }
            }
        }

        private void CalcularNivelUsuario()
        {
            // Calcula el nivel basado en la experiencia total (cada 100 XP sube de nivel)
            Level = (CurrentXP / 100) + 1;

            // Calcula el XP restante para la barra de progreso
            CurrentXP = CurrentXP % 100;

            // XP necesario para el siguiente nivel
            RequiredXP = 100 - CurrentXP; // Esto indica cuánto le falta para el siguiente nivel
        }

    }

