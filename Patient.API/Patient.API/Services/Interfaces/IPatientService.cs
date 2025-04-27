namespace Patient.API.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<Models.Patient>> GetAllPatientsAsync();

    Task<Models.Patient?> GetPatientByIdAsync(Guid id);

    Task<Models.Patient> CreatePatientAsync(Models.Patient patient);

    Task UpdatePatientAsync(Guid id, Models.Patient patient);

    Task DeletePatientAsync(Guid id);
}