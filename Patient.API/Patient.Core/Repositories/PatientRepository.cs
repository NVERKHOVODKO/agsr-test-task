using Microsoft.EntityFrameworkCore;
using Patient.Core.DataBase;
using Patient.Core.Repositories.Interfaces;

namespace Patient.Core.Repositories;
 
public class PatientRepository : IRepository<Models.Patient>
{
    private readonly DataBaseContext _context;

    public PatientRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Core.Models.Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Include(p => p.Name)
            .ThenInclude(p => p.Given)
            .ToListAsync();
    }

    public async Task<Models.Patient?> GetByIdAsync(Guid id)
    {
        return await _context.Patients
            .Include(p => p.Name)
            .ThenInclude(p => p.Given)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Core.Models.Patient? patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, Core.Models.Patient patient)
    {
        _context.Entry(patient).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var patient = await GetByIdAsync(id);
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExists(Guid id)
    {
        return await _context.Patients.AnyAsync(p => p.Id == id);
    }
}