namespace OxxoPage.Model
{
    public class Experiencia
    {
        public int Level { get; set; } = 1;
        public int CurrentXP { get; set; } = 0;
        public int RequiredXP { get; set; } = 100;
        public int RequiredXPBar { get; set; } = 100;

        public Experiencia(int level, int currentXP, int requiredXP, int requiredXPBar)
        {
            Level = level;
            CurrentXP = currentXP;
            RequiredXP = requiredXP;
            RequiredXPBar = requiredXPBar;
        }

        public Experiencia() {}
    }
}
