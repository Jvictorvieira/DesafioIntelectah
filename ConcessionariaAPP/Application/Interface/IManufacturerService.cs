namespace ConcessionariaAPP.Application.Interfaces;

using ConcessionariaAPP.Application.Dto;

public interface IManufacturerService : IGenericCrudInterface<ManufacturerDto>
{
    Task<bool> ExistsByNameAsync(string name, int id = 0);
}