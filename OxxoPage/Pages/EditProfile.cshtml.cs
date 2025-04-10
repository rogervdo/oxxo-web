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


                    // PARA VANIA!!! METODO DE ACTUALIZAR FOTOGRAFIA EN BD
                    // bool ActualidazoCheck = _dbContext.ActualizarFotografia(nickname, "victor.jpg");
                    // Console.WriteLine($"ActualidazoCheck: {ActualidazoCheck}");

                    // Verificar si se ha enviado el campo de AboutMe desde el formulario

                }
            }
        }

        //post para update de AboutMe
        // public IActionResult OnPost()
        // {
        //     string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario");

        //     if (!string.IsNullOrEmpty(nickname))
        //     {
        //         var usuario = _dbContext.ObtenerDatosUsuario(nickname);

        //         if (usuario != null)
        //         {
        //             // Cargar los datos del usuario
        //             Usuario = usuario;
        //             Usuario.AboutMe ??= "Este usuario aún no ha escrito su biografía.";

        //             // Verificar si se ha enviado el campo de AboutMe desde el formulario
        //             string aboutMeInput = Request.Form["aboutMeInput"];
        //             if (!string.IsNullOrEmpty(aboutMeInput) && aboutMeInput != Usuario.AboutMe)
        //             {
        //                 // Actualizar en la base de datos
        //                 _dbContext.ActualizarAboutMe(Usuario.IdUsuario, aboutMeInput);
        //                 Usuario.AboutMe = aboutMeInput;
        //             }
        //             //como se sobreescribian los datos se guarda about me y vuelve a pag
        //             //carga todo los datos correctos 
        //             return RedirectToPage();
        //         }
        //     }
        //     return Page();
        // }

        public IActionResult OnPost()
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

                    // Recuperar la imagen seleccionada desde el formulario
                    string selectedImage = Request.Form["selectedImage"];

                    // Si se ha seleccionado una imagen, actualizar la fotografía en la base de datos
                    if (!string.IsNullOrEmpty(selectedImage) && selectedImage != Usuario.Fotografia)
                    {
                        // Actualizar en la base de datos
                        bool updated = _dbContext.ActualizarFotografia(Usuario.IdUsuario, selectedImage);
                        if (updated)
                        {
                            Usuario.Fotografia = selectedImage; // Actualizar el modelo en memoria
                            StatusMessage = "¡Tu foto de perfil ha sido actualizada!";
                        }
                        else
                        {
                            StatusMessage = "Hubo un error al actualizar la foto de perfil.";
                        }
                    }

                    // Verificar si se ha enviado el campo de AboutMe desde el formulario
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





