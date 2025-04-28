namespace Patient.API.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<IEnumerable<Models.Patient>> GetAllAsync();

    Task<Models.Patient?> GetByIdAsync(Guid id);

    Task AddAsync(Models.Patient? patient);

    Task UpdateAsync(Guid id, Models.Patient patient);

    Task DeleteAsync(Guid id);

    Task<bool> IsExists(Guid id);
}