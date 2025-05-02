using Microsoft.EntityFrameworkCore;
using Patient.Core.DAL.Context;
using Patient.Core.DAL.Repositories.Interfaces;

namespace Patient.Core.DAL.Repositories;
 
public class PatientRepository : IRepository<Models.Patient>
{
    private readonly DataBaseContext _context;

    public PatientRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Models.Patient>> GetAllAsync()
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

    public async Task AddAsync(Models.Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Models.Patient patient)
    {
        _context.Entry(patient).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var patient = await GetByIdAsync(id);
        if (patient != null) 
            _context.Patients.Remove(patient);
        
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExists(Guid id) => await _context.Patients.AnyAsync(p => p.Id == id);

    public IQueryable<Models.Patient> GetQueryable() => _context.Patients.AsQueryable();
}