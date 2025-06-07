using AKDashboard.API.Database;
using AKDashboard.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<ClimaDbContext>(opt =>
    opt.UseInMemoryDatabase("ClimaDb"));


builder.Services.AddHttpClient<IClimaService, ClimaService>();
builder.Services.AddHostedService<ClimaAgendadorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseCors();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseDefaultFiles(); // Procura por index.html, etc.
app.UseStaticFiles();  // Habilita o uso de wwwroot


app.UseAuthorization();

app.MapControllers();


app.Run();
