using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Domain.Entities;

namespace ConcessionariaAPP.Domain.Interfaces;

public interface IManufactureRepository : IGenericCrudRepository<Manufacturers>
{
    Task<Manufacturers> GetByNameAsync(string name);
}