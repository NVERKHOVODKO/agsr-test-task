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
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Patient.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateWebApplicationBuilder(args);
        var app = BuildApplication(builder);
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var provider = app.Services.GetRequiredService<Microsoft.AspNetCore.Mvc.ApiExplorer.IApiDescriptionGroupCollectionProvider>();
            var swaggerGen = app.Services.GetRequiredService<Swashbuckle.AspNetCore.Swagger.ISwaggerProvider>();
            var swagger = swaggerGen.GetSwagger("v1");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "swagger.json");
            using var fileStream = File.Create(filePath);
            using var writer = new StreamWriter(fileStream);
            var json = System.Text.Json.JsonSerializer.Serialize(swagger, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            writer.Write(json);
        });
        app.UseSwagger();
        app.UseSwaggerUI();
        app.Run();
    }

    private static WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogging(builder);
        ConfigureServices(builder);
        ConfigureSwagger(builder);

        return builder;
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        const string logsDirectory = "Logs";
        Directory.CreateDirectory(logsDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                new CompactJsonFormatter(),
                Path.Combine(logsDirectory, $"information-{DateTime.UtcNow:yyyy-MM-dd}.log"),
                rollingInterval: RollingInterval.Day,
                shared: true)
            .WriteTo.File(
                new CompactJsonFormatter(),
                Path.Combine(logsDirectory, $"error-{DateTime.UtcNow:yyyy-MM-dd}.log"),
                LogEventLevel.Error,
                rollingInterval: RollingInterval.Day,
                shared:true)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Services.AddDbContext<DataBaseContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty));

        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        builder.Services.AddScoped<IRepository<Core.Models.Patient>, PatientRepository>();
        builder.Services.AddScoped<IPatientService, PatientService>();
        builder.Services.AddScoped<IDataHelper, DataHelper>();
        builder.Services.AddAutoMapper(typeof(PatientProfile));
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
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
            IncludeXmlComments(options);
            options.EnableAnnotations();
        });
    }

    private static void IncludeXmlComments(SwaggerGenOptions options)
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    }

    private static WebApplication BuildApplication(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        app.Urls.Add("http://0.0.0.0:7272");

        app.UseMiddleware<ExceptionMiddleware>();
        EnsureDatabaseCreated(app);
        ConfigureMiddlewarePipeline(app);

        return app;
    }

    private static void EnsureDatabaseCreated(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
        dbContext.Database.EnsureCreated();
    }

    private static void ConfigureMiddlewarePipeline(WebApplication app)
    {
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
    }
}