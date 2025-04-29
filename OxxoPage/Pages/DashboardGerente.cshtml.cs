using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace OxxoPage.Pages;

public class DashboardGerente : PageModel
{
    public string URL_Embebido { get; set; } = "";
    private const string URL_Base = "https://lookerstudio.google.com/embed/u/0/reporting/3d68cc29-42f2-4647-87de-ff5f2a63c447/page/p_n9hh8o82dd";
    public void OnGet()
    {
        int? gerenteId = HttpContext.Session.GetInt32("IdGerente");
        string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

        this.URL_Embebido = URL_Base;

        if (tipoUsuario == "gerente" && gerenteId.HasValue && gerenteId.Value > 0)
        {
            // 1. Construir el string del Json
            string paramsJsonString = $"{{\"p_idgerente\":[{gerenteId.Value}]}}";

            // 2. Codificar el string JSON para la URL 
            string encodedParams = WebUtility.UrlEncode(paramsJsonString);

            // 3. Construir la URL final con ?params=
            this.URL_Embebido = $"{URL_Base}?params={encodedParams}";
            Console.WriteLine($"URL Embebido: {this.URL_Embebido}");
        }
    }
}

