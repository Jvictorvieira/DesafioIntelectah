namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Domain.Entities;
using AutoMapper;
using ConcessionariaAPP.Application.Excepetions;

public class VehicleAppService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;
    public VehicleAppService(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }
    public async Task<VehicleDto> CreateAsync(VehicleDto dto)
    {
        Validate(dto, isUpdate: false);

        var entity = _mapper.Map<Vehicles>(dto);
        entity.IsDeleted = false; 

        var created = await _vehicleRepository.CreateAsync(entity);
        return _mapper.Map<VehicleDto>(created);
    }

    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _vehicleRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<VehicleDto>> GetAllAsync()
    {
        var list = await _vehicleRepository.GetAllAsync();
        return [.. list.Select(e => _mapper.Map<VehicleDto>(e))];
    }

    public async Task<VehicleDto> GetByIdAsync(int id)
    {
        var entity = await _vehicleRepository.GetByIdAsync(id);
        return _mapper.Map<VehicleDto>(entity);
    }
    public async Task<VehicleDto> UpdateAsync(VehicleDto dto)
    {
        Validate(dto, isUpdate: true);
        if (dto.VehicleId.HasValue)
        {
            var entity = await _vehicleRepository.GetByIdAsync(dto.VehicleId.Value);
            if (entity.IsDeleted)
                throw new AppValidationException().Add(nameof(dto.VehicleId), "Veículo está excluído (lógico).");

            _mapper.Map(dto, entity);

            var updated = await _vehicleRepository.UpdateAsync(entity);
            return _mapper.Map<VehicleDto>(updated);
        }
        else
        {
            throw new AppValidationException().Add(nameof(dto.VehicleId), "O objeto não contém um ID válido para atualização.");
        }
    }
    
    private static void Validate(VehicleDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new AppValidationException().Add(nameof(dto.VehicleId), "O objeto não pode ser nulo.");
        }
        if (isUpdate && dto.VehicleId <= 0)
        {
            throw new AppValidationException().Add(nameof(dto.VehicleId), "Id inválido para atualização.");
        }

        if (string.IsNullOrWhiteSpace(dto.Model))
        {
             throw new AppValidationException().Add(nameof(dto.Model), "Nome do Modelo é obrigatório.");
        }

    }

}