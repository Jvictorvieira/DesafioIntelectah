namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Application.Dto;
using AutoMapper;

public class ManufacturerAppService : IManufacturerService
{
    private readonly IManufacturerRepository _ManufacturerRepository;
    private readonly IMapper _mapper;
    public ManufacturerAppService(IManufacturerRepository ManufacturerRepository, IMapper mapper)
    {
        _ManufacturerRepository = ManufacturerRepository;
        _mapper = mapper;
    }

    public async Task<ManufacturerDto> CreateAsync(ManufacturerDto dto)
    {
        Validate(dto, isUpdate: false);
        var entity = _mapper.Map<Manufacturers>(dto);

        entity.IsDeleted = false;

        if (await ExistsByNameAsync(dto.Name))
        { 
            throw new InvalidOperationException("Modelo já cadastrado.");
        }

        var created = await _ManufacturerRepository.CreateAsync(entity);
        return _mapper.Map<ManufacturerDto>(created);
    }

    public async Task<ManufacturerDto> UpdateAsync(ManufacturerDto dto)
    {
        Validate(dto, isUpdate: true);

        if (await ExistsByNameAsync(dto.Name, dto.ManufacturerId ?? 0))
        {
            throw new InvalidOperationException("Modelo já cadastrado.");
        }

        var entity = _mapper.Map<Manufacturers>(dto);
        var updated = await _ManufacturerRepository.UpdateAsync(entity);
        return _mapper.Map<ManufacturerDto>(updated);
    }

    public async Task<ManufacturerDto> GetByIdAsync(int id)
    {
        var entity = await _ManufacturerRepository.GetByIdAsync(id);
        return _mapper.Map<ManufacturerDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id inválido para exclusão.", nameof(id));
        }
        return await _ManufacturerRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ManufacturerDto>> GetAllAsync()
    {
        var list = await _ManufacturerRepository.GetAllAsync();
        return [.. list.Select(e => _mapper.Map<ManufacturerDto>(e))];
    }

    private static void Validate(ManufacturerDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }
        if (isUpdate && dto.ManufacturerId <= 0)
        {
            throw new ArgumentException("Id inválido para atualização.", nameof(dto.ManufacturerId));
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
                var Manufacturer = await _ManufacturerRepository.GetByNameAsync(name.Trim());
                if (Manufacturer != null && Manufacturer.ManufacturerId == id)
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