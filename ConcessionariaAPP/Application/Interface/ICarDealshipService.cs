namespace ConcessionariaAPP.Application.Interfaces;

using ConcessionariaAPP.Application.Dto;

public interface ICarDealershipService : IGenericCrudInterface<CarDealershipDto>
{
    Task<bool> ExistsByNameAsync(string name, int id = 0);
}