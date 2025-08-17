namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Application.Dto;
using AutoMapper;

public class CarDealershipAppService : ICarDealershipService
{
    private readonly ICarDealershipRepository _CarDealershipRepository;
    private readonly IMapper _mapper;
    public CarDealershipAppService(ICarDealershipRepository CarDealershipRepository, IMapper mapper)
    {
        _CarDealershipRepository = CarDealershipRepository;
        _mapper = mapper;
    }

    public async Task<CarDealershipDto> CreateAsync(CarDealershipDto dto)
    {
        Validate(dto, isUpdate: false);
        var entity = _mapper.Map<CarDealership>(dto);

        entity.IsDeleted = false;

        if (await ExistsByNameAsync(dto.Name))
        { 
            throw new InvalidOperationException("O nome do fabricante já está em uso.");
        }

        var created = await _CarDealershipRepository.CreateAsync(entity);
        return _mapper.Map<CarDealershipDto>(created);
    }

    public async Task<CarDealershipDto> UpdateAsync(CarDealershipDto dto)
    {
        Validate(dto, isUpdate: true);

        if (await ExistsByNameAsync(dto.Name, dto.CarDealershipId ?? 0))
        {
            throw new InvalidOperationException("O nome do fabricante já está em uso.");
        }

        var entity = _mapper.Map<CarDealership>(dto);
        var updated = await _CarDealershipRepository.UpdateAsync(entity);
        return _mapper.Map<CarDealershipDto>(updated);
    }

    public async Task<CarDealershipDto> GetByIdAsync(int id)
    {
        var entity = await _CarDealershipRepository.GetByIdAsync(id);
        return _mapper.Map<CarDealershipDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id inválido para exclusão.", nameof(id));
        }
        return await _CarDealershipRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CarDealershipDto>> GetAllAsync()
    {
        var list = await _CarDealershipRepository.GetAllAsync();
        return [.. list.Select(e => _mapper.Map<CarDealershipDto>(e))];
    }

    private static void Validate(CarDealershipDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }
        if (isUpdate && dto.CarDealershipId <= 0)
        {
            throw new ArgumentException("Id inválido para atualização.", nameof(dto.CarDealershipId));
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
                var CarDealership = await _CarDealershipRepository.GetByNameAsync(name.Trim());
                if (CarDealership != null && CarDealership.CarDealershipId == id)
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