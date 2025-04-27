using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Patient.API.DataBase;
using Patient.API.Repositories;
using Patient.API.Repositories.Interfaces;
using System.Reflection;
using Patient.API.Middlewares;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
});

builder.Services.AddScoped<IPatientRepository, PatientRepository>();

#region Logs

var logsDirectory = Path.Combine("Logs");
if (!Directory.Exists(logsDirectory))
    Directory.CreateDirectory(logsDirectory);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(new CompactJsonFormatter(), $"Logs/information-{DateTime.UtcNow.ToShortDateString()}.log",
        rollingInterval: RollingInterval.Day, shared: true)
    .WriteTo.File(new CompactJsonFormatter(), $"Logs/error-{DateTime.UtcNow.ToShortDateString()}.log", LogEventLevel.Error,
        rollingInterval: RollingInterval.Day, shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Patient API",
        Version = "v1",
        Description = "API для управления пациентами",
        Contact = new OpenApiContact
        {
            Name = "KlarseQ",
            Email = "klarseq@gmail.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.EnableAnnotations();
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

app.Run();