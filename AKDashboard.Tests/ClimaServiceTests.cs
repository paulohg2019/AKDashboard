using AKDashboard.API.Services;
using Moq;
using Moq.Protected;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace AKDashboard.Tests
{
    public class ClimaServiceTests
    {
        [Fact]
        public async Task ObterClimaPorCidade_DeveRetornarObjetoValido()
        {
            // Arrange
            var cidade = "Curitiba";
            var fakeJson = @"{
                ""main"": {
                    ""temp"": 22,
                    ""temp_min"": 20,
                    ""temp_max"": 24,
                    ""humidity"": 80
                }
            }";

            var handler = new Mock<HttpMessageHandler>();
            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeJson)
                });

            var client = new HttpClient(handler.Object);

            // Mock IConfiguration com chave fictícia
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["OpenWeatherMap:ApiKey"]).Returns("fake-api-key");

            // Instancia o serviço com HttpClient e configuração mockada
            var service = new ClimaService(client, mockConfig.Object);

            // Act
            var resultado = await service.ObterClimaAsync(cidade);

            // Assert
            Assert.Equal("Curitiba", resultado.Cidade);
            Assert.Equal(22, resultado.Temperatura);
            Assert.Equal(20, resultado.TemperaturaMin);
            Assert.Equal(24, resultado.TemperaturaMax);
            Assert.Equal(80, resultado.Umidade);
        }
    }
}
