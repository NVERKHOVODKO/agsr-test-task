using Patient.API.Exceptions;
using Patient.API.Repositories.Interfaces;
using Patient.API.Services.Interfaces;

namespace Patient.API.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<Models.Patient>> GetAllPatientsAsync()
    {
        return await _patientRepository.GetAllPatientsAsync();
    }

    public async Task<Models.Patient?> GetPatientByIdAsync(Guid id)
    {
        return await _patientRepository.GetPatientByIdAsync(id);
    }

    public async Task<Models.Patient> CreatePatientAsync(Models.Patient patient)
    {
        if (string.IsNullOrEmpty(patient.Name.Family))
        {
            throw new BadRequestException("Family name is required.");
        }

        if (patient.BirthDate == default)
        {
            throw new BadRequestException("Birth date is required.");
        }

        patient.Id = Guid.NewGuid();
        patient.Name.Id = Guid.NewGuid();

        await _patientRepository.AddPatientAsync(patient);
        return patient;
    }

    public async Task UpdatePatientAsync(Guid id, Models.Patient patient)
    {
        if (id != patient.Id)
        {
            throw new BadRequestException("ID in URL does not match patient ID.");
        }

        if (!await PatientExistsAsync(id))
        {
            throw new NotFoundException($"Patient with ID {id} not found.");
        }

        await _patientRepository.UpdatePatientAsync(id, patient);
    }

    public async Task DeletePatientAsync(Guid id)
    {
        if (!await PatientExistsAsync(id))
        {
            throw new NotFoundException($"Patient with ID {id} not found.");
        }

        await _patientRepository.DeletePatientAsync(id);
    }

    public async Task<bool> PatientExistsAsync(Guid id)
    {
        return await _patientRepository.PatientExists(id);
    }
}