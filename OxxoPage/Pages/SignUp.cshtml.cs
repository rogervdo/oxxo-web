using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OxxoPage.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public RegistroUsuario User { get; set; } = new RegistroUsuario();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Si hay errores, vuelve a mostrar la página con los mensajes de error
            }

            // Aquí puedes agregar la lógica para guardar en la base de datos
            // dbContext.Usuarios.Add(User);
            // dbContext.SaveChanges();

            return RedirectToPage("Login"); // Redirige al login después del registro
        }
    }

    public class RegistroUsuario
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public string Correo { get; set; } = "";

        [Required(ErrorMessage = "El número de nómina es obligatorio")]
        public string Nomina { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contrasena { get; set; } = "";

        [Required(ErrorMessage = "Debes confirmar la contraseña")]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; } = "";
    }
}
