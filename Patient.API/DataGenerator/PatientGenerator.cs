using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Patient.Core.DTOs;
using Patient.Core.Enums;

namespace DataGenerator;

public class PatientGenerator
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PatientGenerator> _logger;
    private readonly Random _random = new();
    private const string ApiBaseUrl = "https://localhost:7272/api/patients";
    private const int DelayBetweenRequestsMs = 100;
    private const int MinAgeYears = 18;
    private const int MaxAgeYears = 90;
    private const double ActivePatientProbability = 0.8;
    private const int PatientCount = 100;

    private static readonly string[] FirstNames = 
    {
        "James", "Mary", "John", "Patricia", "Robert", "Jennifer",
        "Michael", "Linda", "William", "Elizabeth", "David", "Barbara",
        "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah",
        "Charles", "Karen", "Christopher", "Nancy", "Daniel", "Lisa",
        "Matthew", "Betty", "Anthony", "Margaret", "Donald", "Sandra"
    };

    private static readonly string[] LastNames = 
    {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller",
        "Davis", "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson",
        "Taylor", "Thomas", "Hernandez", "Moore", "Martin", "Jackson",
        "Thompson", "White", "Lopez", "Lee", "Gonzalez", "Harris",
        "Clark", "Lewis", "Robinson", "Walker", "Perez", "Hall"
    };

    private static readonly string[] NameUses = 
    { 
        "official", "usual", "nickname", "anonymous", "old", "maiden" 
    };

    public PatientGenerator(HttpClient httpClient, ILogger<PatientGenerator> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
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
        return _random.Next(2) == 0 ? null : NameUses[_random.Next(NameUses.Length)];
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
            givenNames.Add(FirstNames[_random.Next(FirstNames.Length)]);
        }
        
        return givenNames.ToList();
    }

    private string GetRandomLastName() => LastNames[_random.Next(LastNames.Length)];

    private async Task<HttpResponseMessage> CreatePatientAsync(CreatePatientDto patient)
    {
        using var content = JsonContent.Create(patient);
        
        return await _httpClient.PostAsync(ApiBaseUrl, content);
    }
}