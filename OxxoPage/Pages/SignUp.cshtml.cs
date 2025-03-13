using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using OxxoPage.Model; 

namespace OxxoPage.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public RegistroUsuario Usuario { get; set; } = new RegistroUsuario();

         private readonly DataBaseContext _db;

        public SignUpModel()
        {
            _db = new DataBaseContext();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Si hay errores, vuelve a mostrar la página con los mensajes de error
            }

            bool registrado = _db.SignUp(Usuario.Nombre, Usuario.Nickname, Usuario.Contrasena, Usuario.Correo);

            if (registrado)
            {
                return RedirectToPage("/Index"); // Redirige al login después del registro
            }
            else
            {
                ModelState.AddModelError("", "Error al registrar usuario. Inténtalo de nuevo.");
                return Page();
            }
        }
    }

    public class RegistroUsuario
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string Correo { get; set; } = "";

        [Required(ErrorMessage = "El nickname es obligatorio")]
        public string Nickname { get; set; } = ""; // Cambié "Nomina" por "Nickname"

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contrasena { get; set; } = "";

        [Required(ErrorMessage = "Debes confirmar la contraseña")]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; } = "";
    }
}
