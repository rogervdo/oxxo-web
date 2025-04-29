using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;
using System.Net;


namespace OxxoPage.Pages
{
    public class ProfileViewGerente : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; } // id que realmente es nickname (SE OBTIENE DEL URL)
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _dbContext;

        // Modelos utilizados
        public Usuarios UsuarioAsesor { get; set; } = new();
        public Experiencia Experiencia { get; set; } = new();
        public List<Achievement> Achievements { get; set; } = new();
        public string Role { get; set; } = "UsuarioAsesor estándar";


        public string URL_EmbebidoG { get; set; } = "";
        private const string URL_BaseG = "https://lookerstudio.google.com/embed/reporting/5de498e5-43dc-4bd6-8437-01cbd94ec0cd/page/p_8jvxmorodd";

        public ProfileViewGerente(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = new DataBaseContext();
        }

        public void OnGet()
        {
            // Console.WriteLine(id);
            if (!string.IsNullOrEmpty(id))
            {
                UsuarioAsesor = _dbContext.ObtenerDatosUsuario(id); // Obtener datos del usuario por id (nickname)

                if (UsuarioAsesor != null)
                {
                    Role = _dbContext.ObtenerRolUsuario(UsuarioAsesor.IdUsuario); // Obtener rol del usuario para descripcion de titulo
                    if (Role == "gerente") Role = "Gerente de Plaza";
                    else if (Role == "asesor") Role = "Asesor de Tienda";

                    //// UsuarioAsesor.Fotografia = _dbContext.ObtenerFotoDePerfil(id);
                    // Obtener logros y experiencia total
                    Achievements = _dbContext.ObtenerLogrosUsuario(id, out int totalXP);
                    CalcularExperiencia(totalXP);
                }
                else
                {
                    UsuarioAsesor.Nickname = "Invitado";
                    // UsuarioAsesor.Fotografia = "default.png";
                    // UsuarioAsesor.AboutMe = "Perfil no disponible";
                }

                // Console.WriteLine($"IdUsuario = {UsuarioAsesor.IdUsuario}");
                int? asesorId = _dbContext.ObtenerUnicoDatoTabla(UsuarioAsesor.IdUsuario, "asesores", "id_usuario", "id_asesor") as int?;

                if (asesorId == null)
                {
                    asesorId = 1;
                }

                // Console.WriteLine($"Id Asesor: {asesorId}");
                this.URL_EmbebidoG = URL_BaseG;

                string paramsJsonString = $"{{\"idasesor\":[{asesorId}]}}";
                string encodedParams = WebUtility.UrlEncode(paramsJsonString);
                if (asesorId.HasValue && asesorId.Value > 0)
                {
                    // 3. Construir la URL final con ?params=
                    this.URL_EmbebidoG = $"{URL_BaseG}?params={encodedParams}";
                }
                else
                {
                    // Si no hay id de asesor, no se agrega el parámetro a la URL
                    this.URL_EmbebidoG = URL_BaseG;
                }
                this.URL_EmbebidoG = $"{URL_BaseG}?params={encodedParams}";
                // Console.WriteLine($"URL Embebido Gerente: {this.URL_EmbebidoG}");
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
