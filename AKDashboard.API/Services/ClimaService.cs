using AKDashboard.API.Models;
using System.Text.Json;

namespace AKDashboard.API.Services
{
    public class ClimaService : IClimaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ClimaService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Clima> ObterClimaAsync(string cidade)
        {
            string apiKey = _config["OpenWeather:ApiKey"];
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade},br&appid={apiKey}&units=metric&lang=pt_br";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var clima = new Clima
            {
                Cidade = cidade,
                DataConsulta = DateTime.Now,
                Temperatura = root.GetProperty("main").GetProperty("temp").GetDouble(),
                TemperaturaMin = root.GetProperty("main").GetProperty("temp_min").GetDouble(),
                TemperaturaMax = root.GetProperty("main").GetProperty("temp_max").GetDouble(),
                Umidade = root.GetProperty("main").GetProperty("humidity").GetInt32()
            };

            return clima;
        }
    }
}
