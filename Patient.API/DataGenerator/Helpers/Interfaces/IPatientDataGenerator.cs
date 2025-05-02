using Patient.Core.Patient;

namespace DataGenerator.Helpers.Interfaces;

public interface IPatientDataGenerator
{
    CreatePatientDto GeneratePatient();
}