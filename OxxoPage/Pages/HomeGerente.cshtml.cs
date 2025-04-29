using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages;
//done PAGINA COMENTADA
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

    public void OnGet()
    {
        // Obtener el nickname del usuario de la sesión o asignar "Invitado" si no existe
        string nickname = _httpContextAccessor.HttpContext?.Session.GetString("Usuario") ?? "Invitado";
        Usuario = _dbContext.ObtenerDatosUsuario(nickname);

        int idGerente = _httpContextAccessor.HttpContext?.Session.GetInt32("IdGerente") ?? 1;
        // Console.WriteLine($"IdGerente: {idGerente}");
        AsesoresList = _dbContext.GetAsesoresDeGerente(idGerente);
    }
}
