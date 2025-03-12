using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace OxxoPage.Pages
{
    public class TablaPuntajesModel : PageModel
    {
        public List<Colaborador> Colaboradores { get; set; }
        
        public int TotalMedallas { get; set; } = 147;
        public decimal PorcentajeMeta { get; set; } = 81.6m;
        
        public int CapacitacionesDia { get; set; } = 13;
        public decimal PorcentajeMetaDiaria { get; set; } = 109.72m;
        
        public int MetaMes { get; set; } = 180;
        public int CursosFaltantes { get; set; } = 33;
        
        public void OnGet()
        {
            Colaboradores = new List<Colaborador>
            {
                new Colaborador { Posicion = 4, Nombre = "Rafael Pereira", ImagenUrl = "rafael.jpeg", Medallas = 20 },
                new Colaborador { Posicion = 5, Nombre = "Debora Carranza", ImagenUrl = "debora.jpeg", Medallas = 19 },
                new Colaborador { Posicion = 6, Nombre = "Alma Teresa", ImagenUrl = "alma.jpeg", Medallas = 16 },
                new Colaborador { Posicion = 7, Nombre = "Benito López", ImagenUrl = "benito.jpeg", Medallas = 12 },
                new Colaborador { Posicion = 8, Nombre = "Kai Cenat", ImagenUrl = "kai.jpeg", Medallas = 8 },
                new Colaborador { Posicion = 9, Nombre = "Dolores González", ImagenUrl = "dolores.jpeg", Medallas = 5 },
                new Colaborador { Posicion = 10, Nombre = "Elver Farías", ImagenUrl = "elver.jpeg", Medallas = 3 },
            };
            
            // Los 3 primeros lugares se muestran en el podio, no en la tabla
        }
    }

    public class Colaborador
    {
        public int Posicion { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public int Medallas { get; set; }
    }
}