using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Patient.Core.DataBase;
using Patient.Core.Helpers;
using Patient.Core.Middlewares;
using Patient.Core.Profiles;
using Patient.Core.Repositories;
using Patient.Core.Services;
using Serilog;
using System.Text.Json.Serialization;
using AutoMapper;
using Patient.Core.Helpers.Interfaces;
using Patient.Core.Repositories.Interfaces;
using Patient.Core.Services.Interfaces;
using Serilog.Formatting.Compact;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Host.UseSerilog((context, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        new CompactJsonFormatter(),
        Path.Combine("Logs", "log-.log"),
        rollingInterval: RollingInterval.Day,
        shared: true));

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Configure database
builder.Services.AddDbContext<DataBaseContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException()));

// Register application services
builder.Services.AddScoped<IRepository<Patient.Core.Models.Patient>, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDataHelper, DataHelper>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(PatientProfile));

// Configure routing
builder.Services.Configure<RouteOptions>(options => 
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Patient API",
        Version = "v1",
        Description = "API для управления пациентами",
        Contact = new OpenApiContact
        {
            Name = "Nikita Verkhovodko",
            Email = "mikita.verkhavodka@gmail.com"
        }
    });
    
    var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.EnableAnnotations();
});

var app = builder.Build();

// Configure middleware pipeline
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
    dbContext.Database.EnsureCreated();
}

// Generate swagger.json file
app.Lifetime.ApplicationStarted.Register(() =>
{
    var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
    var swaggerDoc = swaggerProvider.GetSwagger("v1");

    using var stream = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "swagger.json"));
    var json = JsonSerializer.Serialize(swaggerDoc, new JsonSerializerOptions
    {
        WriteIndented = true
    });
});

app.Run();