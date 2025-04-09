namespace OxxoPage.Model
{
    public class Settings
    {

        public string Nickname { get; set; }           // Actual
        public string Contrasena { get; set; }         // Nueva

        public Settings(string nickname, string contrasena)
        {
            Nickname = nickname;
            Contrasena = contrasena;
        }

        public Settings() { }
    }
}

