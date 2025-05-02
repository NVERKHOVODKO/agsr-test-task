using Patient.Core.DTOs;

namespace DataGenerator.Helpers.Interfaces;

public interface IPatientDataGenerator
{
    CreatePatientDto GeneratePatient();
}