using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OxxoPage.Pages;

public class Profile : PageModel
{
    public string UserName { get; set; } = "Juan Pérez";
    public string Role { get; set; } = "Asesor de Tienda";
    public int Level { get; set; } = 2;
    public int CurrentXP { get; set; } = 5200;
    public int RequiredXP { get; set; } = 6000;
    public double Progress => (double)CurrentXP / RequiredXP * 100;
    public string Biography { get; set; } = "Texto de biografía...";
    public string ProfileImageUrl { get; set; } = "~/img/perfil.jpg";
    
    public List<Achievement> Achievements { get; set; } = new()
    {
        new Achievement("Certificación Oro en Administración de Tienda", new DateTime(2025, 2, 20), 5, 5),
        new Achievement("Mantenimiento", new DateTime(2025, 2, 18), 5, 10),
        new Achievement("Bronce en Planometría", new DateTime(2025, 2, 17), 8, 10),
        new Achievement("Plata en Reabastecimiento", new DateTime(2025, 2, 14), 9, 10),
        new Achievement("Oro en Drive-Thru", new DateTime(2025, 2, 10), 10, 10),
    };
}

public class Achievement
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public int TotalCorrect { get; set; }

    public Achievement(string title, DateTime date, int score, int totalCorrect)
    {
        Title = title;
        Date = date;
        Score = score;
        TotalCorrect = totalCorrect;
    }
}

