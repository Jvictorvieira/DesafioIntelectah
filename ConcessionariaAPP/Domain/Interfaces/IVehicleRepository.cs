namespace ConcessionariaAPP.Domain.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

using ConcessionariaAPP.Domain.Entities;


public interface IVehicleRepository : IGenericCrudRepository<Vehicles>
{
    Task<IEnumerable<Vehicles>> GetByModelAsync(string model);
    Task<bool> CheckExistenceAsync(string model);
}