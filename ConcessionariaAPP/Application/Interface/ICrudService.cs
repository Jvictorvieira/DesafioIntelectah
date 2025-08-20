namespace ConcessionariaAPP.Application.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGenericCrudInterface<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T dto);
    Task<T> UpdateAsync(T dto);
    Task<bool> DeleteAsync(int id);
}