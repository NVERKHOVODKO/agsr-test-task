using DataGenerator.Data;
using DataGenerator.Helpers.Interfaces;
using DataGenerator.Models;
using Patient.Core.DTOs;
using Patient.Core.Enums;

namespace DataGenerator.Helpers;

public class PatientDataGenerator : IPatientDataGenerator
{
    private readonly Random _random = new();
    private const double ActivePatientProbability = 0.8;
    
    public CreatePatientDto GeneratePatient()
    {
        return new CreatePatientDto
        {
            Name = GenerateName(),
            Gender = GetRandomGender(),
            BirthDate = GetRandomBirthDate(),
            Active = _random.NextDouble() <= ActivePatientProbability
        };
    }

    private PatientNameDto GenerateName() => new()
    {
        Use = GetRandomNameUse(),
        Family = PatientData.LastNames[_random.Next(PatientData.LastNames.Length)],
        Given = GetRandomGivenNames()
    };

    private string? GetRandomNameUse() => 
        _random.Next(2) == 0 ? null : PatientData.NameUses[_random.Next(PatientData.NameUses.Length)];

    private Gender GetRandomGender() => 
        Enum.GetValues<Gender>()[_random.Next(Enum.GetValues<Gender>().Length)];

    private DateTime GetRandomBirthDate()
    {
        var years = _random.Next(GeneratorConfig.MinAgeYears, GeneratorConfig.MaxAgeYears + 1);
        var days = _random.Next(-365, 365);
        return DateTime.Now.AddYears(-years).AddDays(days);
    }

    private List<string> GetRandomGivenNames()
    {
        var nameCount = _random.Next(1, 4);
        var names = new HashSet<string>();
        while (names.Count < nameCount)
            names.Add(PatientData.FirstNames[_random.Next(PatientData.FirstNames.Length)]);
        return names.ToList();
    }
}