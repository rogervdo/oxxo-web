using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class Settings : PageModel
    {
        [BindProperty]
        public Settings Usuario { get; set; }
        public string Nickname { get; private set; }
        public string Contrasena { get; private set; }

        private readonly DataBaseContext _db;

        public Settings()
        {
            _db = new DataBaseContext();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Solo pasamos el nickname y la nueva contraseña
            bool actualizado = _db.ActualizarUsuario(Usuario.Nickname, Usuario.Contrasena);

            if (actualizado)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError("", "Error al actualizar el usuario.");
                return Page();
            }
        }
    }
}