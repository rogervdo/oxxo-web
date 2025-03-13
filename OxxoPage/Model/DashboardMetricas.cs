public class DashboardMetricas
{
    public int TotalLogros { get; set; }
    public int MetaMensual { get; set; }
    public decimal PorcentajeMeta { get; set; }
    public int LogrosFaltantes { get; set; }
    
    public int CapacitacionesDia { get; set; }
    public int MetaDiaria { get; set; }  // Asegúrate que este sea tipo int
    public decimal PorcentajeMetaDiaria { get; set; }
}