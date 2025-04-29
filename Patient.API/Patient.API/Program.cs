using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Patient.Core.DataBase;
using Patient.Core.Helpers;
using Patient.Core.Helpers.Interfaces;
using Patient.Core.Middlewares;
using Patient.Core.Profiles;
using Patient.Core.Repositories;
using Patient.Core.Repositories.Interfaces;
using Patient.Core.Services;
using Patient.Core.Services.Interfaces;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Patient.API;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });
        builder.Services.AddDbContext<DataBaseContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
        });

        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        
        builder.Services.AddScoped<IRepository<Core.Models.Patient>, PatientRepository>();
        builder.Services.AddScoped<IPatientService, PatientService>();
        builder.Services.AddAutoMapper(typeof(PatientProfile));
        builder.Services.AddScoped<IDataHelper, DataHelper>();
        
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
                    Name = "Nikita Verkhovodko",
                    Email = "mikita.verkhavodka@gmail.com"
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
    }
}