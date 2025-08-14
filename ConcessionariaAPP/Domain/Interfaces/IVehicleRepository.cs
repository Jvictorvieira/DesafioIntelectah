namespace ConcessionariaAPP.Domain.Interfaces;

using System.Threading.Tasks;

using ConcessionariaAPP.Domain.Entities;

public interface IVehicleRepository : IGenericCrudRepository<Vehicles>
{
    Task<Vehicles> GetByModelAsync(string model);
}