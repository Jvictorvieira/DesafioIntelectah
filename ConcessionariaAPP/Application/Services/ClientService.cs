namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Application.Dto;
using AutoMapper;

public class ClientAppService : IClientService
{
    private readonly IClientRepository _ClientRepository;
    private readonly IMapper _mapper;
    public ClientAppService(IClientRepository ClientRepository, IMapper mapper)
    {
        _ClientRepository = ClientRepository;
        _mapper = mapper;
    }

    public async Task<ClientDto> CreateAsync(ClientDto dto)
    {
        Validate(dto, isUpdate: false);
        var entity = _mapper.Map<Clients>(dto);

        entity.IsDeleted = false;

        if (await ExistsByNameAsync(dto.Name))
        { 
            throw new InvalidOperationException("O nome do fabricante já está em uso.");
        }

        var created = await _ClientRepository.CreateAsync(entity);
        return _mapper.Map<ClientDto>(created);
    }

    public async Task<ClientDto> UpdateAsync(ClientDto dto)
    {
        Validate(dto, isUpdate: true);

        if (await ExistsByNameAsync(dto.Name, dto.ClientId ?? 0))
        {
            throw new InvalidOperationException("O nome do fabricante já está em uso.");
        }

        var entity = _mapper.Map<Clients>(dto);
        var updated = await _ClientRepository.UpdateAsync(entity);
        return _mapper.Map<ClientDto>(updated);
    }

    public async Task<ClientDto> GetByIdAsync(int id)
    {
        var entity = await _ClientRepository.GetByIdAsync(id);
        return _mapper.Map<ClientDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id inválido para exclusão.", nameof(id));
        }
        return await _ClientRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ClientDto>> GetAllAsync()
    {
        var list = await _ClientRepository.GetAllAsync();
        return [.. list.Select(e => _mapper.Map<ClientDto>(e))];
    }

    private static void Validate(ClientDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }
        if (isUpdate && dto.ClientId <= 0)
        {
            throw new ArgumentException("Id inválido para atualização.", nameof(dto.ClientId));
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Nome do Modelo é obrigatório.", nameof(dto.Name));
        }
    }
    
    public async Task<bool> ExistsByNameAsync(string name, int id = 0)
        {
            try
            {
                var Client = await _ClientRepository.GetByNameAsync(name.Trim());
                if (Client != null && Client.ClientId == id)
                {
                    return false;       
                }
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

}