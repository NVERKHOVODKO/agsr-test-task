using Patient.Core.Enums;
using Patient.Core.Patient;

namespace Patient.Core.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<GetPatientDto>> GetAllAsync();

    Task<GetPatientDto?> GetByIdAsync(Guid id);

    Task<GetPatientDto> CreateAsync(CreatePatientDto patient);

    Task<Status> UpdateAsync(Guid id, UpdatePatientDto patient);

    Task DeleteAsync(Guid id);

    Task<IEnumerable<GetPatientDto>> SearchByBirthDateAsync(string fhirDateParam);
}