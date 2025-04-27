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

    public async Task<IEnumerable<Models.Patient>> GetAllAsync()
    {
        return await _patientRepository.GetAllAsync();
    }

    public async Task<Models.Patient?> GetByIdAsync(Guid id)
    {
        return await _patientRepository.GetByIdAsync(id);
    }

    public async Task<Models.Patient> CreateAsync(Models.Patient patient)
    {
        if (string.IsNullOrEmpty(patient.Name.Family))
            throw new BadRequestException("Family name is required.");

        if (patient.BirthDate == default)
            throw new BadRequestException("Birth date is required.");

        patient.Id = Guid.NewGuid();
        patient.Name.Id = Guid.NewGuid();

        await _patientRepository.AddAsync(patient);
        return patient;
    }

    public async Task UpdateAsync(Guid id, Models.Patient patient)
    {
        if (id != patient.Id)
            throw new BadRequestException("ID in URL does not match patient ID.");

        if (!await IsExistsAsync(id))
            throw new NotFoundException($"Patient with ID {id} not found.");

        await _patientRepository.UpdateAsync(id, patient);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await IsExistsAsync(id))
            throw new NotFoundException($"Patient with ID {id} not found.");

        await _patientRepository.DeleteAsync(id);
    }

    private async Task<bool> IsExistsAsync(Guid id)
    {
        return await _patientRepository.IsExists(id);
    }
}