namespace OxxoPage.Model
{
    public class Settings
    {
        public string Nickname { get; set; }
        public string? Contrasena { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Genero { get; set; } // 1: Masculino, 2: Femenino, 3: No binario

        public Settings() { }
    }
}
