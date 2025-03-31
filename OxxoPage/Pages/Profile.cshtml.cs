using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class Profile : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _dbContext;

        // Modelos utilizados
        public Usuarios Usuario { get; set; } = new();
        public Experiencia Experiencia { get; set; } = new();
        public List<Achievement> Achievements { get; set; } = new();
        public string Role { get; set; } = "Usuario estándar";

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
                var usuario = _dbContext.ObtenerDatosUsuario(nickname);

                if (usuario != null)
                {
                    Usuario = usuario;
                    Usuario.AboutMe ??= "Este usuario aún no ha escrito su biografía.";
                    Role = _dbContext.ObtenerRolUsuario(usuario.IdUsuario);

                    // Obtener logros y experiencia total
                    Achievements = _dbContext.ObtenerLogrosUsuario(nickname, out int totalXP);
                    CalcularExperiencia(totalXP);
                }
                else
                {
                    Usuario.Nickname = "Invitado";
                    Usuario.Fotografia = "default.png";
                    Usuario.AboutMe = "Perfil no disponible";
                }
            }
        }

        private void CalcularExperiencia(int totalXP)
        {
            Experiencia.Level = (totalXP / 100) + 1;
            Experiencia.CurrentXP = totalXP % 100;
            Experiencia.RequiredXP = 100 - Experiencia.CurrentXP;
            Experiencia.RequiredXPBar = 100;
        }
    }
}
