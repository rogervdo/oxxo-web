using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class ProfileViewGerente : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _dbContext;

        // Modelos utilizados
        public Usuarios UsuarioAsesor { get; set; } = new();
        public Experiencia Experiencia { get; set; } = new();
        public List<Achievement> Achievements { get; set; } = new();
        public string Role { get; set; } = "UsuarioAsesor estándar";

        public ProfileViewGerente(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = new DataBaseContext();
        }

        public void OnGet()
        {

            HttpContext.Session.SetString("UsuarioAsesor", "carlito");
            string nickname = _httpContextAccessor.HttpContext?.Session.GetString("UsuarioAsesor") ?? string.Empty;
            Console.WriteLine(nickname);
            Console.WriteLine(id);

            if (!string.IsNullOrEmpty(id))
            {
                var usuario = _dbContext.ObtenerDatosUsuario(id);

                if (usuario != null)
                {
                    UsuarioAsesor = usuario;
                    UsuarioAsesor.AboutMe ??= "Este usuario aún no ha escrito su biografía.";
                    Role = _dbContext.ObtenerRolUsuario(usuario.IdUsuario);

                    if (Role == "gerente")
                    {
                        Role = "Gerente de Plaza";
                    }
                    else if (Role == "asesor")
                    {
                        Role = "Asesor de Tienda";
                    }

                    UsuarioAsesor.Fotografia = _dbContext.ObtenerFotoDePerfil(id);
                    // Obtener logros y experiencia total
                    Achievements = _dbContext.ObtenerLogrosUsuario(id, out int totalXP);
                    CalcularExperiencia(totalXP);
                }
                else
                {
                    UsuarioAsesor.Nickname = "Invitado";
                    UsuarioAsesor.Fotografia = "default.png";
                    UsuarioAsesor.AboutMe = "Perfil no disponible";
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
