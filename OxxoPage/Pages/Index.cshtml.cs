using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model; // Asegúrate de importar el contexto de la BD

namespace OxxoPage.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string? Usuario { get; set; }

        [BindProperty]
        public string? Contrasena { get; set; }

        public string? MensajeError { get; set; }

        public string? TipoUsuario { get; set; }

        public int? IdUsuario { get; set; }

        private readonly DataBaseContext _db;

        public IndexModel()
        {
            _db = new DataBaseContext(); // Inicializa la conexión a la BD
        }

        public void OnGet()
        {
            MensajeError = "";
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Contrasena))
            {
                MensajeError = "Por favor, completa todos los campos.";
                return Page();
            }

            TipoUsuario = _db.ObtenerRolUsuarioNickname(Usuario);
            IdUsuario = _db.ObtenerUnicoDatoUsuario(Usuario, "nickname", "id_usuario") as int?;

            // Verifica si el usuario existe en la base de datos
            if (_db.LoginUser(Usuario, Contrasena))
            {
                HttpContext.Session.SetString("Usuario", Usuario); // Guarda la sesión
                HttpContext.Session.SetString("TipoUsuario", TipoUsuario);
                HttpContext.Session.SetInt32("IdUsuario", IdUsuario ?? 0);
                if (TipoUsuario == "gerente")
                {
                    return RedirectToPage("/HomeGerente"); // Redirige a la página principal
                }
                return RedirectToPage("/Home"); // Redirige a la página principal
            }
            else
            {
                MensajeError = "Usuario o contraseña incorrectos.";
                return Page();
            }
        }
    }
}
