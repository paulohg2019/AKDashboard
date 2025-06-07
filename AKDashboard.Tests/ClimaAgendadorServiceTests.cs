using AKDashboard.API.Database;
using AKDashboard.API.Models;
using AKDashboard.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AKDashboard.Tests
{
    public class ClimaAgendadorServiceTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveSalvarClimaParaCadaCidade()
        {
            // Dados simulados
            var dadosFake = new Clima
            {
                Cidade = "Curitiba",
                Temperatura = 20,
                TemperaturaMin = 18,
                TemperaturaMax = 22,
                Umidade = 80,
                DataConsulta = DateTime.Now
            };

            // ClimaService mockado
            var climaServiceMock = new Mock<IClimaService>();
            climaServiceMock
                .Setup(s => s.ObterClimaAsync(It.IsAny<string>()))
                .ReturnsAsync(dadosFake);

            var loggerMock = new Mock<ILogger<ClimaAgendadorService>>();

            // DbContext em memoria 
            var options = new DbContextOptionsBuilder<ClimaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ClimaDbContext(options);

            // ServiceProvider mockado
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(s => s.GetService(typeof(IClimaService))).Returns(climaServiceMock.Object);
            serviceProviderMock.Setup(s => s.GetService(typeof(ClimaDbContext))).Returns(context);

            // Mock do IServiceScope
            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

            // Mock do IServiceScopeFactory
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

            // Cria a instância do serviço com scopeFactory
            var service = new ClimaAgendadorService(scopeFactoryMock.Object, loggerMock.Object);

            var cts = new CancellationTokenSource();
            await service.StartAsync(cts.Token);

            // Verificações
            Assert.True(await context.Climas.CountAsync() >= 1);
            climaServiceMock.Verify(x => x.ObterClimaAsync(It.IsAny<string>()), Times.AtLeastOnce());
        }
    }
}