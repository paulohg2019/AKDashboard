namespace AKDashboard.API.Models
{
    public class Clima
    {
        public int Id { get; set; }
        public string Cidade { get; set; } = string.Empty;
        public DateTime DataConsulta { get; set; }
        public double Temperatura { get; set; }
        public double TemperaturaMin { get; set; }
        public double TemperaturaMax { get; set; }
        public int Umidade { get; set; }
    }

}
