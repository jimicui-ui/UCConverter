using Microsoft.Extensions.FileProviders;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using UCConverter.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure UnitsSettings path
var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
var unitsSettingsPath = Path.Combine(solutionRoot, "UnitsSettings");

// If running from bin/Debug, adjust path
if (!Directory.Exists(unitsSettingsPath))
{
    // Try alternative path
    unitsSettingsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "UnitsSettings");
}

// If still not found, use a relative path from the solution
if (!Directory.Exists(unitsSettingsPath))
{
    unitsSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "UnitsSettings");
}

// Register Infrastructure services
builder.Services.AddSingleton<IUnitRepository>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<JsonUnitRepository>>();
    var repository = new JsonUnitRepository(unitsSettingsPath, logger);
    repository.Initialize(); // Load all JSON files at startup
    return repository;
});

// Register Domain services
builder.Services.AddScoped<IConversionService, ConversionService>();

// Register Application services
builder.Services.AddScoped<UCConverter.Application.Interfaces.IUnitConverterService, UnitConverterService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }
