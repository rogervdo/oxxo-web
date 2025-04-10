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

        public void OnPost()
        {
            string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario");
            if (!string.IsNullOrEmpty(nickname))
            {
                var usuario = _dbContext.ObtenerDatosUsuario(nickname);

                if (usuario != null)
                {
                    // Cargar los datos del usuario
                    Usuario = usuario;
                    Usuario.AboutMe ??= "Este usuario aún no ha escrito su biografía.";
                    Role = _dbContext.ObtenerRolUsuario(usuario.IdUsuario);

                    // Obtener logros y experiencia total
                    Achievements = _dbContext.ObtenerLogrosUsuario(nickname, out int totalXP);
                    CalcularExperiencia(totalXP);

                    // Verificar si se ha enviado el campo de AboutMe desde el formulario
                    string aboutMeInput = Request.Form["aboutMeInput"];
                    if (!string.IsNullOrEmpty(aboutMeInput) && aboutMeInput != Usuario.AboutMe)
                    {
                        // Actualizar en la base de datos
                        _dbContext.ActualizarAboutMe(Usuario.IdUsuario, aboutMeInput);
                        // Actualizar el valor en el modelo
                        Usuario.AboutMe = aboutMeInput;
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateUserPhoto(int userId, string newImageName)
        {
            try
            {
                // Call the method to update the photo in the database
                ActualizarFotografia(userId, newImageName);

                // Return a success response
                return Ok(new { message = "Profile picture updated successfully!" });
            }
            catch (Exception ex)
            {
                // If an error occurs, return a bad request with the error message
                return BadRequest(new { message = "Error updating profile picture", error = ex.Message });
            }
        }

        private void CalcularExperiencia(int totalXP)
        {
            Experiencia.Level = (totalXP / 100) + 1;
            Experiencia.CurrentXP = totalXP % 100;
            Experiencia.RequiredXP = 100 - Experiencia.CurrentXP;
            Experiencia.RequiredXPBar = 100;
        }

    }
}





