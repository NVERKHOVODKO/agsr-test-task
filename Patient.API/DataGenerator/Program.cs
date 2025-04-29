using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataGenerator;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });

        var logger = loggerFactory.CreateLogger<PatientGenerator>();
        
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "PatientDataGenerator/1.0");
        
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
        
        var apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7272/api/patients";

        var patientGenerator = new PatientGenerator(httpClient, logger, apiBaseUrl);

        await patientGenerator.GeneratePatientsAsync();
    }
}