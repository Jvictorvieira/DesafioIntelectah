namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Application.Dto;
using AutoMapper;
using ConcessionariaAPP.Application.Excepetions;

public class CarDealershipAppService(ICarDealershipRepository CarDealershipRepository, IMapper mapper) : ICarDealershipService
{
    private readonly ICarDealershipRepository _CarDealershipRepository = CarDealershipRepository;
    private readonly IMapper _mapper = mapper;


    public async Task<CarDealershipDto> CreateAsync(CarDealershipDto dto)
    {
        Validate(dto, isUpdate: false);
        var entity = _mapper.Map<CarDealership>(dto);

        entity.IsDeleted = false;

        if (await ExistsByNameAsync(dto.Name))
        {
            throw new AppValidationException().Add(nameof(CarDealershipDto.Name),"O nome do fabricante já está em uso." );
        }

        var created = await _CarDealershipRepository.CreateAsync(entity);
        return _mapper.Map<CarDealershipDto>(created);
    }

    public async Task<CarDealershipDto> UpdateAsync(CarDealershipDto dto)
    {
        Validate(dto, isUpdate: true);

        if (await ExistsByNameAsync(dto.Name, dto.CarDealershipId ?? 0))
        {
            throw new AppValidationException().Add(nameof(CarDealershipDto.Name),"O nome da concessionária já está em uso.");
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
            throw new AppValidationException().Add(nameof(CarDealershipDto.CarDealershipId),"Id inválido para exclusão.");
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
            throw new AppValidationException().Add(nameof(dto),"Concessionária não existe.");
        }
        if (isUpdate && dto.CarDealershipId <= 0)
        {
            throw new AppValidationException().Add(nameof(dto.CarDealershipId),"Id inválido para atualização.");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new AppValidationException().Add(nameof(dto.Name),"Nome da Concessionária é obrigatório.");
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