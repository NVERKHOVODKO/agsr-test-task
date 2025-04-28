namespace Patient.Core.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<IEnumerable<Core.Models.Patient>> GetAllAsync();

    Task<Core.Models.Patient?> GetByIdAsync(Guid id);

    Task AddAsync(Core.Models.Patient? patient);

    Task UpdateAsync(Guid id, Core.Models.Patient patient);

    Task DeleteAsync(Guid id);

    Task<bool> IsExists(Guid id);
}