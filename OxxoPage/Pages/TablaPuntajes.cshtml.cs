using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using OxxoPage.Model;

namespace OxxoPage.Pages
{
    public class TablaPuntajesModel : PageModel
    {
        private readonly DataBaseContext _dbContext;

        public List<Usuarios> TodosUsuarios { get; set; }
        public List<Usuarios> UsuariosTabla { get; set; } // Los que aparecen en la tabla (posiciones 4+)
        public List<Usuarios> Podio { get; set; } // Los tres primeros lugares

        // Métricas del dashboard
        public int TotalLogros { get; set; }
        public decimal PorcentajeMeta { get; set; }

        public int CapacitacionesDia { get; set; }
        public decimal PorcentajeMetaDiaria { get; set; }

        public int MetaMes { get; set; }
        public int LogrosFaltantes { get; set; }

        public TablaPuntajesModel()
        {
            _dbContext = new DataBaseContext();
        }

        public void OnGet()
        {
            // Obtener usuarios con medallas (limitado a 10)
            TodosUsuarios = _dbContext.GetUsuariosConMedallas();

            // Separar los 3 primeros para el podio y el resto para la tabla
            Podio = TodosUsuarios.Count >= 3
                ? TodosUsuarios.GetRange(0, 3)
                : TodosUsuarios;

            UsuariosTabla = TodosUsuarios.Count > 3
                ? TodosUsuarios.GetRange(3, Math.Min(7, TodosUsuarios.Count - 3)) // Solo hasta completar 10 en total
                : new List<Usuarios>();

            // Obtener métricas del dashboard
            var metricas = _dbContext.GetDashboardMetricas();

            // Asignar métricas a propiedades
            TotalLogros = metricas.TotalLogros;
            PorcentajeMeta = metricas.PorcentajeMeta;
            CapacitacionesDia = metricas.CapacitacionesDia;
            PorcentajeMetaDiaria = metricas.PorcentajeMetaDiaria;
            MetaMes = metricas.MetaMensual;
            LogrosFaltantes = metricas.LogrosFaltantes;
        }
    }
}