using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace OxxoPage.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string? Usuario { get; set; }

        [BindProperty]
        public string? Contrasena { get; set; }

        public string? MensajeError { get; set; }

        public void OnGet()
        {
            // Limpiar mensajes al cargar la página
            MensajeError = "";
        }
        

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Contrasena))
            {
                MensajeError = "Por favor, completa todos los campos.";
                return Page();
            }

            // Simulación de credenciales correctas (puedes cambiar esto con una base de datos)
            if (Usuario == "1" && Contrasena == "1")
            {
                // Guardamos el usuario en la sesión
                HttpContext.Session.SetString("Usuario", Usuario);

                // Redirigir a otra página después del login exitoso
                return RedirectToPage("/Home");
            }
            else
            {
                MensajeError = "Usuario o contraseña incorrectos.";
                return Page();
            }
        }
    }
}

