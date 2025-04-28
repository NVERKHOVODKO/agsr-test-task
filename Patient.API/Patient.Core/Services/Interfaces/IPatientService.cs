using Patient.Core.DTOs;

namespace Patient.Core.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<GetPatientDto>> GetAllAsync();

    Task<GetPatientDto?> GetByIdAsync(Guid id);

    Task<GetPatientDto> CreateAsync(CreatePatientDto patient);

    Task UpdateAsync(Guid id, UpdatePatientDto patient);

    Task DeleteAsync(Guid id);
}