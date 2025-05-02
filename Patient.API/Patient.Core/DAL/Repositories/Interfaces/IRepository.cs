using Patient.Core.DAL.Models;

namespace Patient.Core.DAL.Repositories.Interfaces;

public interface IRepository<T> where T : BaseModel
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T model);

    Task UpdateAsync(T model);

    Task DeleteAsync(Guid id);

    Task<bool> IsExists(Guid id);

    Task<List<Models.Patient>> SearchByBirthDateAsync(DateTime? startDate, DateTime? endDate, string prefix);
}