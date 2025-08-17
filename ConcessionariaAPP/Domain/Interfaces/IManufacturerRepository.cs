using ConcessionariaAPP.Domain.Entities;

namespace ConcessionariaAPP.Domain.Interfaces;

public interface IManufacturerRepository : IGenericCrudRepository<Manufacturers>
{
    Task<Manufacturers> GetByNameAsync(string name);
}