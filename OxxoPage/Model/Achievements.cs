namespace OxxoPage.Model
{
    public class Achievement
    {
        public string Nombre { get; set; }       // Nombre del logro
        public DateTime Fecha { get; set; }      // Fecha de obtención
        public int Experiencia { get; set; }     // XP que otorga el logro
        public string Icono { get; set; }        // Imagen del logro

        public Achievement(string nombre, DateTime fecha, int experiencia, string icono)
        {
            Nombre = nombre;
            Fecha = fecha;
            Experiencia = experiencia;
            Icono = string.IsNullOrEmpty(icono) ? "default-medal.png" : icono;
        }
    }
}
