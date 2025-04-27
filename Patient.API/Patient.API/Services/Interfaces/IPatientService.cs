namespace Patient.API.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<Models.Patient>> GetAllAsync();

    Task<Models.Patient?> GetByIdAsync(Guid id);

    Task<Models.Patient> CreateAsync(Models.Patient patient);

    Task UpdateAsync(Guid id, Models.Patient patient);

    Task DeleteAsync(Guid id);
}