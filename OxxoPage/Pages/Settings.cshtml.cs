using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class SettingsModel : PageModel
    {

        [BindProperty]
        public Settings Usuario { get; set; }

        private readonly DataBaseContext _db;

        public SettingsModel()
        {
            _db = new DataBaseContext();
        }

        // Manejador para actualizar la contraseña
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Solo pasamos el nickname, la nueva contraseña, la fecha de nacimiento y el género
            bool actualizado = _db.ActualizarUsuario(Usuario.Nickname, Usuario.Contrasena, Usuario.FechaNacimiento, Usuario.Genero);

            if (actualizado)
            {
                return RedirectToPage("/Settings");
            }
            else
            {
                ModelState.AddModelError("", "Error al actualizar el usuario.");
                return Page();
            }
        }
    }
}