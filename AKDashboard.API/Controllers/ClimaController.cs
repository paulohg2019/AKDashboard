using AKDashboard.API.Database;
using AKDashboard.API.Models;
using AKDashboard.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AKDashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClimaController : ControllerBase
    {
        private readonly IClimaService _climaService;
        private readonly ClimaDbContext _context;

        public ClimaController(IClimaService climaService, ClimaDbContext context)
        {
            _climaService = climaService;
            _context = context;
        }


        [HttpGet("{cidade}")]
        public async Task<ActionResult<Clima>> GetClima(string cidade)
        {
            try
            {
                var clima = await _climaService.ObterClimaAsync(cidade);


                _context.Climas.Add(clima);
                await _context.SaveChangesAsync();

                return Ok(clima);
            }
            catch (HttpRequestException e)
            {
                return BadRequest($"Erro ao obter dados do clima: {e.Message}");
            }
        }


        [HttpGet("historico")]
        public ActionResult<IEnumerable<Clima>> GetHistorico()
        {
            var dados = _context.Climas
                .OrderByDescending(c => c.DataConsulta)
                .ToList();

            return Ok(dados);
        }
    }
}
