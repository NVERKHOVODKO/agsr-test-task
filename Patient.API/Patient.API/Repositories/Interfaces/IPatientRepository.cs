namespace Patient.API.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<IEnumerable<Models.Patient>> GetAllPatientsAsync();

    Task<Models.Patient> GetPatientByIdAsync(Guid id);

    Task AddPatientAsync(Models.Patient patient);

    Task UpdatePatientAsync(Guid id, Models.Patient patient);

    Task DeletePatientAsync(Guid id);

    Task<bool> PatientExists(Guid id);
}