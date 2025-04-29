using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OxxoPage.Model;

namespace OxxoPage.Pages
{

    public class Games : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _dbContext;

        public int SelectedGameId { get; set; }
        public GameInfo SelectedGameInfo { get; set; }

        public Games(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = new DataBaseContext();
        }

        //on post result de hacer click en un boton, recibe valor = ID juego 
        public IActionResult OnPost(int gameId)
        {
            // Aquí conectas a la base de datos, llamas metodo con id 
            SelectedGameInfo = _dbContext.GetGameInfo(gameId);

            if (SelectedGameInfo == null)
            {
                ModelState.AddModelError("", "No se encontró la información del juego.");
            }
            //guarda el id 
            SelectedGameId = gameId;
            return Page(); //recarga pagina con datos
        }


    }
}

