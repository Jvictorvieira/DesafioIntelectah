namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Application.Dto;
using AutoMapper;
using ConcessionariaAPP.Application.Excepetions;

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

        if (await ExistsByCpfAsync(dto.Cpf))
        {
            throw new AppValidationException().Add(nameof(dto.Cpf), "O CPF do cliente já está em uso.");
        }
        entity.Cpf = removeMask(entity.Cpf);
        var created = await _ClientRepository.CreateAsync(entity);
        var mapped = _mapper.Map<ClientDto>(created);
        mapped.Cpf = formatCpf(mapped.Cpf);
        return mapped;
    }

    public async Task<ClientDto> UpdateAsync(ClientDto dto)
    {
        Validate(dto, isUpdate: true);

        if (await ExistsByCpfAsync(dto.Cpf, dto.ClientId ?? 0))
        {
            throw new AppValidationException().Add(nameof(dto.Cpf), "O CPF do cliente já está em uso.");
        }

        var entity = _mapper.Map<Clients>(dto);
        entity.Cpf = removeMask(entity.Cpf);
        var updated = await _ClientRepository.UpdateAsync(entity);
        var mapped = _mapper.Map<ClientDto>(updated);
        mapped.Cpf = formatCpf(mapped.Cpf);
        return mapped;
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
            throw new AppValidationException().Add(nameof(ClientDto.ClientId), "Id inválido para exclusão.");
        }
        return await _ClientRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ClientDto>> GetAllAsync()
    {
        var list = await _ClientRepository.GetAllAsync();
        var mapped = list.Select(e => _mapper.Map<ClientDto>(e)).ToList();
        mapped.ForEach(e => e.Cpf = formatCpf(e.Cpf));
        return mapped;
    }

    private static void Validate(ClientDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new AppValidationException().Add(nameof(dto), "Cliente não existe.");
        }
        if (isUpdate && dto.ClientId <= 0)
        {
            throw new AppValidationException().Add(nameof(dto.ClientId), "Id inválido para atualização.");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new AppValidationException().Add(nameof(dto.Name), "Nome do Cliente é obrigatório.");
        }
    }

    public async Task<bool> ExistsByCpfAsync(string cpf, int id = 0)
    {
        try
        {
            var Client = await _ClientRepository.GetByCpfAsync(cpf.Trim());
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

    private string removeMask(string value)
    {
        return value.Replace(".", "").Replace("-", "").Replace(" ", "");
    }

    private string formatCpf(string value)
    {
        value = removeMask(value);
        if (value.Length != 11) return value;
        return $"{value.Substring(0, 3)}.{value.Substring(3, 3)}.{value.Substring(6, 3)}-{value.Substring(9, 2)}";
    }
}