using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages;

public class Home : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _dbContext;

        public string Nickname { get; set; }
        public string FotoPerfil { get; set; }

        public Home(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = new DataBaseContext();
        }

        public void OnGet()
        {
            Nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario") ?? "Invitado";

            if (Nickname != "Invitado")
            {
                FotoPerfil = ObtenerFotoDePerfil(Nickname);
            }
            else
            {
                FotoPerfil = "default.png"; // Imagen por defecto si no hay usuario en sesión
            }
        }

        private string ObtenerFotoDePerfil(string nickname)
        {
            string foto = "default.png"; // Valor por defecto en caso de error
            using (var conexion = new MySqlConnection(_dbContext.ConnectionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT fotografia FROM usuarios WHERE nickname = @nickname";
                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nickname", nickname);
                        object result = cmd.ExecuteScalar();
                        if (result != null && !string.IsNullOrEmpty(result.ToString()))
                        {
                            foto = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener la foto de perfil: " + ex.Message);
                }
            }
            return foto;
    }
}

