using DataGenerator.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using DataGenerator.Helpers.Interfaces;

namespace DataGenerator.Services;

public class PatientGenerator
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PatientGenerator> _logger;
    private readonly IPatientDataGenerator _dataGenerator;
    private readonly string _apiBaseUrl;
    private readonly GeneratorConfig _config;

    public PatientGenerator(
        HttpClient httpClient,
        ILogger<PatientGenerator> logger,
        IPatientDataGenerator dataGenerator,
        string apiBaseUrl,
        GeneratorConfig? config = null)
    {
        _httpClient = httpClient;
        _logger = logger;
        _dataGenerator = dataGenerator;
        _apiBaseUrl = apiBaseUrl;
        _config = config ?? new GeneratorConfig();
    }

    public async Task<int> GeneratePatientsAsync()
    {
        _logger.LogInformation("Starting patient data generation for {PatientCount} patients", _config.PatientCount);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var successCount = 0;

        for (var i = 1; i <= _config.PatientCount; i++)
        {
            try
            {
                var patient = _dataGenerator.GeneratePatient();
                var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, patient);

                if (response.IsSuccessStatusCode)
                {
                    successCount++;
                    _logger.LogDebug("Created patient {PatientNumber}", i);
                }
                else
                {
                    _logger.LogWarning("Failed to create patient {PatientNumber}. Status: {StatusCode}", 
                        i, response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient {PatientNumber}", i);
            }

            if (i < _config.PatientCount)
            {
                await Task.Delay(_config.DelayBetweenRequestsMs);
            }
        }

        stopwatch.Stop();
        _logger.LogInformation(
            "Completed patient generation. Created {SuccessCount}/{TotalCount} patients in {ElapsedSeconds:0.00} seconds",
            successCount, _config.PatientCount, stopwatch.Elapsed.TotalSeconds);

        return successCount;
    }
}