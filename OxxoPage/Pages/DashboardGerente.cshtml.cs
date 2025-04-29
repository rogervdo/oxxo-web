using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace OxxoPage.Pages;

public class DashboardGerente : PageModel
{
    public string URL_Embed { get; set; } = "";
    private const string URL_Base = "https://lookerstudio.google.com/embed/u/0/reporting/3d68cc29-42f2-4647-87de-ff5f2a63c447/page/p_n9hh8o82dd";
    public void OnGet()
    {
        int? gerenteId = HttpContext.Session.GetInt32("IdGerente");
        string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

        this.URL_Embed = URL_Base;

        if (tipoUsuario == "gerente" && gerenteId.HasValue && gerenteId.Value > 0)
        {
            // String JSON
            string paramsJsonString = $"{{\"p_idgerente\":[{gerenteId.Value}]}}";
            // Codificar string
            string encodedParams = WebUtility.UrlEncode(paramsJsonString);
            // Construir url final, insertando parametros
            this.URL_Embed = $"{URL_Base}?params={encodedParams}";
        }
    }
}

