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
        public string? ImagePath { get; set; } //path de imagen guardada

        [TempData]
        public string StatusMessage { get; set; }


        public EditProfile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = new DataBaseContext();
        }

        public void OnGet()
        {
            string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario") ?? "Invitado";
            if (!string.IsNullOrEmpty(nickname))
            {
                var usuario = _dbContext.ObtenerDatosUsuario(nickname);

                if (Usuario != null)
                {
                    // Cargar los datos del usuario
                    Usuario = usuario;
                    Usuario.AboutMe ??= "Este usuario aún no ha escrito su biografía.";


                    Role = _dbContext.ObtenerRolUsuario(Usuario.IdUsuario);
                    if (Role == "gerente") Role = "Gerente de Plaza";
                    else if (Role == "asesor") Role = "Asesor de Tienda";

                    // Obtener logros y experiencia total
                    Achievements = _dbContext.ObtenerLogrosUsuario(nickname, out int totalXP);
                    CalcularExperiencia(totalXP);

                }
            }
        }

        //modifico onPost para ambas opciones (seleccionar / upload imagenes)
        public async Task<IActionResult> OnPostAsync()
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

                    //funcionalidad upload 
                    var uploadedFile = Request.Form.Files["uploadedImage"];
                    if (uploadedFile != null && uploadedFile.Length > 0)
                    //se crea nombre unico al archivo y en que carpeta
                    {
                        string uniqueFileName = $"{Guid.NewGuid()}_{uploadedFile.FileName}";
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img");
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }
                        //se actuliza nombre de img
                        bool updated = _dbContext.ActualizarFotografia(Usuario.IdUsuario, uniqueFileName);
                        if (updated)
                        {
                            Usuario.Fotografia = uniqueFileName;
                            StatusMessage = "¡Tu foto de perfil ha sido actualizada!";
                        }
                        else
                        {
                            StatusMessage = "Hubo un error al actualizar la foto de perfil.";
                        }
                    }
                    else //en caso de no subir nada se verfica si se selecciono
                    {

                        string selectedImage = Request.Form["selectedImage"];
                        //si hay una select y es diferente a la actual, se actualiza
                        if (!string.IsNullOrEmpty(selectedImage) && selectedImage != Usuario.Fotografia)
                        {
                            bool updated = _dbContext.ActualizarFotografia(Usuario.IdUsuario, selectedImage);
                            if (updated)
                            {
                                Usuario.Fotografia = selectedImage;
                                StatusMessage = "¡Tu foto de perfil ha sido actualizada!";
                            }
                            else
                            {
                                StatusMessage = "Hubo un error al actualizar la foto de perfil.";
                            }
                        }
                    }

                    string aboutMeInput = Request.Form["aboutMeInput"];
                    if (!string.IsNullOrEmpty(aboutMeInput) && aboutMeInput != Usuario.AboutMe)
                    {
                        _dbContext.ActualizarAboutMe(Usuario.IdUsuario, aboutMeInput);
                        Usuario.AboutMe = aboutMeInput;
                        StatusMessage = "¡Tu biografía ha sido actualizada!";
                    }

                    return RedirectToPage();
                }
            }
            return Page();
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





