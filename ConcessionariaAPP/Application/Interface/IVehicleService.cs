namespace ConcessionariaAPP.Application.Interfaces;

using ConcessionariaAPP.Domain.Entities;
using System.Threading.Tasks;

public interface IVehicleService : IGenericCrudInterface<Vehicles>
{
    Task LogicalDeleteAsync(int id);
}