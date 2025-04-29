using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages;

public class Home : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataBaseContext _dbContext;

    // Propiedades para almacenar el nickname y la foto de perfil del usuario
    public Usuarios Usuario { get; set; }

    // Constructor que inyecta dependencias para acceso a la sesión y la base de datos
    public Home(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = new DataBaseContext();
    }

    // Método que se ejecuta al cargar la página
    public void OnGet()
    {
        // Obtener el nickname del usuario de la sesión o asignar "Invitado" si no existe
        string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario") ?? "Invitado";
        Usuario = _dbContext.ObtenerDatosUsuario(nickname);
    }
}
