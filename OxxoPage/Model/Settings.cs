namespace OxxoPage.Model
{
    public class Settings
    {

        public string Nickname { get; set; }
        public string Contrasena { get; set; }
        public  DateTime Fecha_Nacimiento{get; set;}

        public Settings(string nickname, string contrasena, DateTime fecha_nacimiento)
        {
            Nickname = nickname;
            Contrasena = contrasena;
            Fecha_Nacimiento = fecha_nacimiento;
        }

        public Settings() { }
    }
}

