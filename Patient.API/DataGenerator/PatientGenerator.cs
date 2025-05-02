using System.Net.Http.Json;
using DataGenerator.Helpers;
using Microsoft.Extensions.Logging;
using Patient.Core.DTOs;
using Patient.Core.Enums;

namespace DataGenerator;

public class PatientGenerator
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiBaseUrl;
    private readonly ILogger<PatientGenerator> _logger;
    private readonly Random _random = new();
    private const int DelayBetweenRequestsMs = 100;
    private const int MinAgeYears = 18;
    private const int MaxAgeYears = 90;
    private const double ActivePatientProbability = 0.8;
    private const int PatientCount = 100;

    public PatientGenerator(HttpClient httpClient, ILogger<PatientGenerator> logger,
        PatientDataGenerator patientDataGenerator, string? apiBaseUrl)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiBaseUrl = apiBaseUrl;
    }

    public async Task<int> GeneratePatientsAsync()
    {
        _logger.LogInformation("Starting patient data generation for {PatientCount} patients", PatientCount);
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var successCount = 0;
        
        for (var i = 1; i <= PatientCount; i++)
        {
            try
            {
                var patient = GenerateRandomPatient();
                var response = await CreatePatientAsync(patient);

                if (response.IsSuccessStatusCode)
                {
                    successCount++;
                }
                else
                {
                    _logger.LogWarning("Failed to create patient {PatientNumber}. Status: {StatusCode}", 
                        i, response.StatusCode);
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error creating patient {PatientNumber}", i);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating patient {PatientNumber}", i);
            }

            if (i < PatientCount)
            {
                await Task.Delay(DelayBetweenRequestsMs);
            }
        }

        stopwatch.Stop();
        _logger.LogInformation(
            "Completed patient generation. Created {SuccessCount}/{TotalCount} patients in {ElapsedSeconds:0.00} seconds",
            successCount, PatientCount, stopwatch.Elapsed.TotalSeconds);

        return successCount;
    }

    private CreatePatientDto GenerateRandomPatient()
    {
        return new CreatePatientDto
        {
            Name = GenerateRandomName(),
            Gender = GetRandomGender(),
            BirthDate = GetRandomBirthDate(),
            Active = _random.NextDouble() <= ActivePatientProbability
        };
    }

    private PatientNameDto GenerateRandomName()
    {
        return new PatientNameDto
        {
            Use = GetRandomNameUse(),
            Family = GetRandomLastName(),
            Given = GetRandomGivenNames()
        };
    }

    private string? GetRandomNameUse()
    {
        return _random.Next(2) == 0 ? null : PatientData.NameUses[_random.Next(PatientData.NameUses.Length)];
    }

    private Gender GetRandomGender()
    {
        var genders = Enum.GetValues<Gender>();
        
        return genders[_random.Next(genders.Length)];
    }

    private DateTime GetRandomBirthDate()
    {
        var years = _random.Next(MinAgeYears, MaxAgeYears + 1);
        var days = _random.Next(-365, 365);
        
        return DateTime.Now.AddYears(-years).AddDays(days);
    }

    private List<string> GetRandomGivenNames()
    {
        var nameCount = _random.Next(1, 4);
        var givenNames = new HashSet<string>(nameCount);
        
        while (givenNames.Count < nameCount)
        {
            givenNames.Add(PatientData.FirstNames[_random.Next(PatientData.FirstNames.Length)]);
        }
        
        return givenNames.ToList();
    }

    private string GetRandomLastName() => PatientData.LastNames[_random.Next(PatientData.LastNames.Length)];

    private async Task<HttpResponseMessage> CreatePatientAsync(CreatePatientDto patient)
    {
        using var content = JsonContent.Create(patient);
        
        return await _httpClient.PostAsync(_apiBaseUrl, content);
    }
}