using ConcessionariaAPP.Domain.Entities;

namespace ConcessionariaAPP.Domain.Interfaces;

public interface IVehicleRepository : IGenericCrudRepository<Vehicles>
{
    Task<Vehicles> GetByNameAsync(string name);
}