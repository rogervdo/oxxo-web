using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages;

public class HomeGerente : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataBaseContext _dbContext;

    // Propiedades para almacenar el nickname y la foto de perfil del usuario
    public Usuarios Usuario { get; set; }

    public List<Asesores> AsesoresList { get; set; }

    // Constructor que inyecta dependencias para acceso a la sesión y la base de datos
    public HomeGerente(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = new DataBaseContext();
    }

    // Método que se ejecuta al cargar la página
    public void OnGet()
    {
        string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario") ?? "Invitado";

        Usuario = new Usuarios
        {
            Nickname = nickname,
            Fotografia = nickname != "Invitado"
                ? _dbContext.ObtenerFotoDePerfil(nickname) : "default.png"
        };

        AsesoresList = _dbContext.GetAsesoresDeGerente(1);
    }

    public void SetUsuarioAsesorSession(string nickname)
    {
        HttpContext.Session.SetString("UsuarioAsesor", nickname);
        Console.WriteLine(nickname);
    }
}
