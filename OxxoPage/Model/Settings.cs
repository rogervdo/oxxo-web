namespace OxxoPage.Model
{
    public class Settings
    {
        public string Nickname { get; set; }           // Actual
        public string? Contrasena { get; set; }         // Nueva
        public DateTime? FechaNacimiento { get; set; }  // Fecha de nacimiento
        public int? Genero { get; set; }                // Género (1: Masculino, 2: Femenino, 3: No binario)

        public Settings(string nickname, string contrasena, DateTime fechaNacimiento, int genero)
        {
            Nickname = nickname;
            Contrasena = contrasena;
            FechaNacimiento = fechaNacimiento;
            Genero = genero;
        }

        public Settings() { }
    }
}
