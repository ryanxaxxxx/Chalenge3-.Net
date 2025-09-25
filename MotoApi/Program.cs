using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Oracle EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger/OpenAPI configurado com t�tulo, vers�o e descri��o
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Moto API",
        Version = "v1",
        Description = "API RESTful para gerenciar Motos e Usu�rios"
    });

    // Caminho do arquivo XML gerado pelo .NET para habilitar os summaries
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Middleware Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto API v1");
        // Mantendo o padr�o /swagger
        // c.RoutePrefix = string.Empty;  <-- N�O usamos para evitar 404 na raiz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
