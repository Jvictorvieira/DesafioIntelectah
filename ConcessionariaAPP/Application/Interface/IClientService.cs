namespace ConcessionariaAPP.Application.Interfaces;

using ConcessionariaAPP.Application.Dto;

public interface IClientService : IGenericCrudInterface<ClientDto>
{
    Task<bool> ExistsByCpfAsync(string cpf, int id = 0);
}