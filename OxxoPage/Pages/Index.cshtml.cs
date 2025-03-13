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

            // ⚡ Verifica si el usuario existe en la base de datos
            if (_db.LoginUser(Usuario, Contrasena))
            {
                HttpContext.Session.SetString("Usuario", Usuario); // Guarda la sesión
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
