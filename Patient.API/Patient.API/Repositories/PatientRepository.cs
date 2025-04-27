using Microsoft.EntityFrameworkCore;
using Patient.API.DataBase;
using Patient.API.Repositories.Interfaces;

namespace Patient.API.Repositories;

/*[AutoInterface]*/
public class PatientRepository : IPatientRepository
{
    private readonly DataBaseContext _context;

    public PatientRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Models.Patient>> GetAllPatientsAsync()
    {
        return await _context.Patients
            .Include(p => p.Name)
            .ToListAsync();
    }

    public async Task<Models.Patient> GetPatientByIdAsync(Guid id)
    {
        return await _context.Patients
            .Include(p => p.Name)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPatientAsync(Models.Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePatientAsync(Guid id, Models.Patient patient)
    {
        _context.Entry(patient).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletePatientAsync(Guid id)
    {
        var patient = await GetPatientByIdAsync(id);
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> PatientExists(Guid id)
    {
        return await _context.Patients.AnyAsync(p => p.Id == id);
    }
}