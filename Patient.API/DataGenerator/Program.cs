using System.Net.Http.Json;
using Patient.Core.DTOs;
using Patient.Core.Enums;

namespace DataGenerator;

internal static class Program
{
    private static readonly HttpClient _httpClient = new();
    private static readonly Random _random = new();
    private const string ApiBaseUrl = "https://localhost:7272/api/Patients";
    private const int PatientCount = 100;
    private const int DelayBetweenRequestsMs = 100;
    private const int MinAgeYears = 18;
    private const int MaxAgeYears = 90;
    private const double ActivePatientProbability = 0.8;

    public static async Task Main(string[] args)
    {
        try
        {
            ConfigureHttpClient();
            
            Console.WriteLine($"Starting patient data generation for {PatientCount} patients...");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var createdCount = await GeneratePatientsAsync();
            stopwatch.Stop();

            Console.WriteLine($"Successfully created {createdCount}/{PatientCount} patients in {stopwatch.Elapsed.TotalSeconds:0.00} seconds.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            Environment.ExitCode = 1;
        }
        finally
        {
            _httpClient.Dispose();
        }
    }

    private static void ConfigureHttpClient()
    {
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "PatientDataGenerator/1.0");
    }

    private static async Task<int> GeneratePatientsAsync()
    {
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
                    LogProgress(i, patient);
                }
                else
                {
                    Console.WriteLine($"Failed to create patient {i}. Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP error creating patient {i}: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error creating patient {i}: {ex.Message}");
            }

            if (i < PatientCount)
            {
                await Task.Delay(DelayBetweenRequestsMs);
            }
        }

        return successCount;
    }

    private static void LogProgress(int currentIndex, CreatePatientDto patient)
    {
        var givenNames = string.Join(" ", patient.Name.Given);
        Console.WriteLine($"Created {currentIndex}/{PatientCount}: {patient.Name.Family}, {givenNames} " + 
                         $"(Gender: {patient.Gender}, Age: {GetAge(patient.BirthDate):0})");
    }

    private static int GetAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    private static CreatePatientDto GenerateRandomPatient()
    {
        return new CreatePatientDto
        {
            Name = GenerateRandomName(),
            Gender = GetRandomGender(),
            BirthDate = GetRandomBirthDate(),
            Active = _random.NextDouble() <= ActivePatientProbability
        };
    }

    private static PatientNameDto GenerateRandomName()
    {
        return new PatientNameDto
        {
            Use = GetRandomNameUse(),
            Family = GetRandomLastName(),
            Given = GetRandomGivenNames()
        };
    }

    private static string? GetRandomNameUse()
    {
        var nameUses = new[] { "official", "usual", "nickname", "anonymous", "old", "maiden" };
        return _random.Next(2) == 0 ? null : nameUses[_random.Next(nameUses.Length)];
    }

    private static Gender GetRandomGender()
    {
        var genders = Enum.GetValues<Gender>();
        return genders[_random.Next(genders.Length)];
    }

    private static DateTime GetRandomBirthDate()
    {
        var years = _random.Next(MinAgeYears, MaxAgeYears + 1);
        var days = _random.Next(-365, 365);
        return DateTime.Now.AddYears(-years).AddDays(days);
    }

    private static List<string> GetRandomGivenNames()
    {
        var firstNames = new[] 
        {
            "James", "Mary", "John", "Patricia", "Robert", "Jennifer",
            "Michael", "Linda", "William", "Elizabeth", "David", "Barbara",
            "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah",
            "Charles", "Karen", "Christopher", "Nancy", "Daniel", "Lisa",
            "Matthew", "Betty", "Anthony", "Margaret", "Donald", "Sandra"
        };
        
        var nameCount = _random.Next(1, 4);
        var givenNames = new HashSet<string>(nameCount);
        
        while (givenNames.Count < nameCount)
        {
            givenNames.Add(firstNames[_random.Next(firstNames.Length)]);
        }
        
        return givenNames.ToList();
    }

    private static string GetRandomLastName()
    {
        var lastNames = new[] 
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller",
            "Davis", "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson",
            "Taylor", "Thomas", "Hernandez", "Moore", "Martin", "Jackson",
            "Thompson", "White", "Lopez", "Lee", "Gonzalez", "Harris",
            "Clark", "Lewis", "Robinson", "Walker", "Perez", "Hall"
        };
        
        return lastNames[_random.Next(lastNames.Length)];
    }

    private static async Task<HttpResponseMessage> CreatePatientAsync(CreatePatientDto patient)
    {
        using var content = JsonContent.Create(patient);
        return await _httpClient.PostAsync(ApiBaseUrl, content);
    }
}