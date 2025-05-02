using Patient.Core.DAL.Models;

namespace Patient.Core.DAL.Repositories.Interfaces;

public interface IRepository<T> where T : BaseModel
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T model);

    Task UpdateAsync(T model);

    Task DeleteAsync(Guid id);

    IQueryable<Models.Patient> GetQueryable();

    Task<bool> IsExists(Guid id);
}