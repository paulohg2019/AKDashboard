using AKDashboard.API.Models;

namespace AKDashboard.API.Services
{
    public interface IClimaService
    {
        Task<Clima> ObterClimaAsync(string cidade);
    }
}
