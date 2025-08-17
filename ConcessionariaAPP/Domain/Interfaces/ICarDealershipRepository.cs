using ConcessionariaAPP.Domain.Entities;

namespace ConcessionariaAPP.Domain.Interfaces;

public interface ICarDealershipRepository : IGenericCrudRepository<CarDealership>
{
    Task<CarDealership> GetByNameAsync(string name);
}