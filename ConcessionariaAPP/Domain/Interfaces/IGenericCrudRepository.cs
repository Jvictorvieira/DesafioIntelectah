namespace ConcessionariaAPP.Domain.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGenericCrudRepository<T> where T : class
{
    IQueryable<T> GetAll(bool includeDeleted = false);
    Task<T> GetById(int id);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<bool> Delete(int id);
}