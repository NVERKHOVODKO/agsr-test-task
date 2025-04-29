using Patient.Core.Models;

namespace Patient.Core.Repositories.Interfaces;

public interface IRepository<T> where T : BaseModel
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(Guid id);

    Task AddAsync(T? model);

    Task UpdateAsync(Guid id, T model);

    Task DeleteAsync(Guid id);

    Task<bool> IsExists(Guid id);
}