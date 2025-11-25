using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using UCConverter.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add localization support - resources are in Application layer
builder.Services.AddLocalization(options => 
{
    options.ResourcesPath = "Resources";
});
builder.Services.Configure<Microsoft.AspNetCore.Builder.RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "zh", "en-US", "zh-CN" };
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new Microsoft.AspNetCore.Localization.QueryStringRequestCultureProvider());
    options.RequestCultureProviders.Add(new Microsoft.AspNetCore.Localization.AcceptLanguageHeaderRequestCultureProvider());
});

// Register localization service
builder.Services.AddScoped<ILocalizationService>(sp =>
{
    var localizer = sp.GetRequiredService<IStringLocalizer<UCConverter.Application.Resources.SharedResources>>();
    return new LocalizationService(localizer);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Unit Converter API",
        Version = "v1",
        Description = @"Open-source Unit Converter API with support for multiple unit categories (Length, Weight, Temperature, Volume, Area, Time, Speed). 
        
Built with Clean Architecture and SOLID principles.

## Features
- Support for multiple unit categories
- SI base unit conversions
- Formula-based conversions (e.g., temperature)
- Localization support (English, Chinese)
- Comprehensive error handling

## Getting Started
1. Get all available categories: `GET /api/categories`
2. Get units for a category: `GET /api/categories/{name}/units`
3. Perform conversion: `POST /api/convert`

## Examples
See the examples section for each endpoint to see real-world usage scenarios.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Unit Converter Project",
            Url = new Uri("https://github.com/jimicui-ui/UCConverter")
        }
    });

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Include XML comments from Application layer
    var applicationXmlFile = "UCConverter.Application.xml";
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
    if (File.Exists(applicationXmlPath))
    {
        options.IncludeXmlComments(applicationXmlPath);
    }

    // Enable example schemas
    options.EnableAnnotations();
    options.UseInlineDefinitionsForEnums();
    
    // Custom schema IDs to avoid conflicts
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

// Add CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // React default ports
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Unit Converter API v1");
    options.RoutePrefix = "swagger"; // Accessible at /swagger
    
    // Enhanced Swagger UI configuration
    options.DisplayRequestDuration();
    options.EnableDeepLinking();
    options.EnableFilter();
    options.EnableTryItOutByDefault();
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    options.DefaultModelsExpandDepth(2);
    options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
    options.ShowExtensions();
    options.EnableValidator();
    options.SupportedSubmitMethods(new[] { 
        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get, 
        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Post,
        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Put,
        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Delete 
    });
});

app.UseCors("AllowFrontend");

// Use localization middleware
var localizationOptions = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Builder.RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }
