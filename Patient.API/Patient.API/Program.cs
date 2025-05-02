using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;
using AutoMapper;
using Patient.Core.DAL.Context;
using Patient.Core.DAL.Repositories;
using Patient.Core.DAL.Repositories.Interfaces;
using Patient.Core.Helpers;
using Patient.Core.Mappings;
using Patient.Core.Services;
using Patient.Core.Services.Interfaces;
using Serilog.Formatting.Compact;

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
builder.Services.AddScoped<IRepository<Patient.Core.DAL.Models.Patient>, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<DataHelper>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(PatientProfile));

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

app.Run();