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

        //GET para cargar los datos actuales
        public IActionResult OnGet()
        {
            string nickname = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrWhiteSpace(nickname))
            {
                return RedirectToPage("/Index");
            }

            Usuario = _db.ObtenerUsuario(nickname);

            if (Usuario == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
            }

            return Page();
        }


        //POST para actualizar los datos
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool actualizado = _db.ActualizarUsuario(Usuario.Nickname, Usuario.Contrasena, Usuario.FechaNacimiento, Usuario.Genero);

            if (actualizado)
            {
                return RedirectToPage("/Settings", new { nickname = Usuario.Nickname });
            }
            else
            {
                ModelState.AddModelError("", "Error al actualizar el usuario.");
                return Page();
            }
        }
    }
}
