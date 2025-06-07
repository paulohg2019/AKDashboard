let graficoTempInstance = null;
let graficoUmidadeInstance = null;

async function buscarClima() {
    const cidade = document.getElementById('cidade').value;
    const response = await fetch(`https://localhost:7201/api/clima/${cidade}`);

    const data = await response.json();
    carregarEstatisticas(data);
    carregarGraficos();
}

async function carregarGraficos() {
    const response = await fetch('https://localhost:7201/api/clima/historico');
    const historico = await response.json();

    const labels = historico.map(c => new Date(c.dataConsulta).toLocaleTimeString());
    const temps = historico.map(c => c.temperatura);
    const umidades = historico.map(c => c.umidade);

    const ctxTemp = document.getElementById('graficoTemp').getContext('2d');
    if (graficoTempInstance) {
        graficoTempInstance.destroy();
    }
    graficoTempInstance = new Chart(ctxTemp, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Temperatura (\u00B0C)',
                data: temps,
                borderColor: 'orange',
                fill: false
            }]
        }
    });

    const ctxUmidade = document.getElementById('graficoUmidade').getContext('2d');
    if (graficoUmidadeInstance) {
        graficoUmidadeInstance.destroy();
    }
    graficoUmidadeInstance = new Chart(ctxUmidade, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Umidade (%)',
                data: umidades,
                backgroundColor: 'lightblue'
            }]
        }
    });
}

function carregarEstatisticas(dados) {
    const container = document.getElementById('estatisticas-conteudo');
    container.innerHTML = `
  <p><strong>Cidade:</strong> ${dados.cidade}</p>
  <p><strong>Temperatura:</strong> ${dados.temperatura} \u00B0C</p>
  <p><strong>Min:</strong> ${dados.temperaturaMin} \u00B0C</p>
  <p><strong>Max:</strong> ${dados.temperaturaMax} \u00B0C</p>
  <p><strong>Umidade:</strong> ${dados.umidade}%</p>
  <p><strong>Data:</strong> ${new Date(dados.dataConsulta).toLocaleString()}</p>
`;
}
