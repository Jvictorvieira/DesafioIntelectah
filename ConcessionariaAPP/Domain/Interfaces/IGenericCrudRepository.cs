namespace ConcessionariaAPP.Domain.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGenericCrudRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}