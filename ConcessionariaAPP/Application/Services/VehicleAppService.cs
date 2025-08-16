namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Domain.Entities;
using AutoMapper;

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
                throw new InvalidOperationException("Veículo está excluído (lógico).");

            _mapper.Map(dto, entity);

            var updated = await _vehicleRepository.UpdateAsync(entity);
            return _mapper.Map<VehicleDto>(updated);
        }
        else
        {
            throw new ArgumentException("O objeto não contém um ID válido para atualização.", nameof(dto.VehicleId));
        }
    }
    
    private static void Validate(VehicleDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }
        if (isUpdate && dto.VehicleId <= 0)
        {
            throw new ArgumentException("Id inválido para atualização.", nameof(dto.VehicleId));
        }

        if (string.IsNullOrWhiteSpace(dto.Model))
        {
            throw new ArgumentException("Nome do Modelo é obrigatório.", nameof(dto.Model));
        }

    }

}