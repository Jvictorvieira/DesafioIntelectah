namespace ConcessionariaAPP.Application.Interfaces;

using ConcessionariaAPP.Application.Dto;

public interface IClientService : IGenericCrudInterface<ClientDto>
{
    Task<bool> ExistsByNameAsync(string name, int id = 0);
}