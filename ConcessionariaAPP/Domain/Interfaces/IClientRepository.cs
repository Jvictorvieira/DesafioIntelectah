using ConcessionariaAPP.Domain.Entities;

namespace ConcessionariaAPP.Domain.Interfaces;

public interface IClientRepository : IGenericCrudRepository<Clients>
{
    Task<Clients> GetByNameAsync(string name);
}