using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using OxxoPage.Model;

namespace OxxoPage.Pages
{
    // Modelo para mostrar la tabla de puntajes y el podio
    public class TablaPuntajesModel : PageModel
    {
        // Contexto de base de datos para acceder a los datos
        private readonly DataBaseContext _dbContext;

        // Lista completa de usuarios con sus logros
        public List<Usuarios> TodosUsuarios { get; set; }
        
        // Usuarios que aparecen en la tabla (posiciones 4 en adelante)
        public List<Usuarios> UsuariosTabla { get; set; }
        
        // Los tres primeros lugares para mostrar en el podio
        public List<Usuarios> Podio { get; set; }

        // Métricas del dashboard
        public DashboardMetricas Metricas { get; set; }
        // Constructor que inicializa el contexto de la base de datos
        public TablaPuntajesModel()
        {
            _dbContext = new DataBaseContext();
        }

        // Método que se ejecuta cuando se carga la página
        public void OnGet()
        {
            // Obtengo los usuarios con sus medallas (limitado a 10)
            TodosUsuarios = _dbContext.GetUsuariosConMedallas();

            // Separo los 3 primeros para el podio y el resto para la tabla
            Podio = TodosUsuarios.Count >= 3 ? TodosUsuarios.GetRange(0, 3) : TodosUsuarios;

            UsuariosTabla = TodosUsuarios.Count > 3
                ? TodosUsuarios.GetRange(3, Math.Min(7, TodosUsuarios.Count - 3)) // Solo hasta completar 10 en total
                : new List<Usuarios>();

            // Obtengo las métricas del dashboard desde la base de datos
            Metricas = _dbContext.GetDashboardMetricas();
        }
    }
}