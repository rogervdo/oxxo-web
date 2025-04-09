using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class EditProfile : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _dbContext;

        //Modelos utilizados 
        public Usuarios Usuario { get; set; } = new();
        public Experiencia Experiencia { get; set; } = new();
        public List<Achievement> Achievements { get; set; } = new();
        public string Role { get; set; } = "Usuario estándar";
        [TempData]
        public string StatusMessage { get; set; }


        
        public EditProfile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = new DataBaseContext();
        }

        public void OnGet(){
            string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario");
            
            if (!string.IsNullOrEmpty(nickname))
            {
                var usuario = _dbContext.ObtenerDatosUsuario(nickname);

                if (usuario != null)
                {  
                    Usuario = usuario;
                    Usuario.AboutMe ??= "Este usuario aún no ha escrito su biografía.";
                    Role = _dbContext.ObtenerRolUsuario(usuario.IdUsuario);

                    // Obtener logros y experiencia total
                    Achievements = _dbContext.ObtenerLogrosUsuario(nickname, out int totalXP);
                    CalcularExperiencia(totalXP);
                }
                else
                {
                    Usuario.Nickname = "Invitado";
                    Usuario.Fotografia = "default.png";
                    Usuario.AboutMe = "Perfil no disponible";
                }
            }
        }

        
        private void CalcularExperiencia(int totalXP)
        {
            Experiencia.Level = (totalXP / 100) + 1;
            Experiencia.CurrentXP = totalXP % 100;
            Experiencia.RequiredXP = 100 - Experiencia.CurrentXP;
            Experiencia.RequiredXPBar = 100;
        }

        // public IActionResult OnPost(string aboutMeInput)
        // {
        //     string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario");
        //     if (string.IsNullOrEmpty(nickname))
        //     {
        //         StatusMessage = "Error: Sesión no válida para guardar cambios.";
        //         return RedirectToPage();
        //     }

        //     //encontrar en db datos usuario
        //     var userToUpdate = _dbContext.ObtenerDatosUsuario(nickname);

        //     //check
        //     if (userToUpdate == null)
        //     {
        //         StatusMessage = "Error: No se pudo encontrar el usuario para actualizar.";
        //         return RedirectToPage(); 
        //     }
        //     // Now you can safely update the fetched user object
        //     userToUpdate.AboutMe = aboutMeInput?.Trim(); // Trim whitespace

        //     try
        //     {
        //         // --- FIX 2: This method needs to EXIST in DataBaseContext.cs ---
        //         bool success = _dbContext.ActualizarUsuario(userToUpdate);

        //         if (success)
        //         {
        //             StatusMessage = "Tu información 'Acerca de mí' ha sido actualizada.";
        //         }
        //         else
        //         {
        //             StatusMessage = "Error: No se pudo guardar la información en la base de datos.";
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception (replace Console.WriteLine with a real logger in production)
        //         Console.WriteLine($"Error updating AboutMe for user {nickname}: {ex.ToString()}"); // Log full exception
        //         StatusMessage = "Error: Ocurrió un problema técnico al guardar los cambios.";
        //     }

        //     // Redirect back to the OnGet handler (PRG Pattern)
        //     return RedirectToPage();
        // }

    }

}



