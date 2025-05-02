using DataGenerator.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataGenerator;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });

        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        var apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7272/api/patients";
        
        using var httpClient = CreateHttpClient();
        var patientGenerator = new PatientGenerator(
            httpClient,
            loggerFactory.CreateLogger<PatientGenerator>(),
            new PatientDataGenerator(),
            apiBaseUrl
        );

        await patientGenerator.GeneratePatientsAsync();
    }

    private static HttpClient CreateHttpClient()
    {
        return new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30),
            DefaultRequestHeaders = { { "User-Agent", "PatientDataGenerator/1.0" } }
        };
    }
}