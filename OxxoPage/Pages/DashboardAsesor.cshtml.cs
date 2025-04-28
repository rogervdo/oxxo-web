using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace OxxoPage.Pages;

public class Dashboard : PageModel
{
    public string URL_Embebido { get; set; } = "";
    private const string URL_Base = "https://lookerstudio.google.com/embed/reporting/5de498e5-43dc-4bd6-8437-01cbd94ec0cd/page/p_8jvxmorodd";
    public void OnGet()
    {
        int? asesorId = HttpContext.Session.GetInt32("IdAsesor");
        string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

        this.URL_Embebido = URL_Base; 

        if (tipoUsuario == "asesor" && asesorId.HasValue && asesorId.Value > 0)
        {
            // 1. Construir el string del Json
            string paramsJsonString = $"{{\"idasesor\":[{asesorId.Value}]}}";

            // 2. Codificar el string JSON para la URL 
            string encodedParams = WebUtility.UrlEncode(paramsJsonString);

            // 3. Construir la URL final con ?params=
            this.URL_Embebido = $"{URL_Base}?params={encodedParams}";
        }
    }
}
