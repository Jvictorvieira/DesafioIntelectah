namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Domain.Entities;
using AutoMapper;
using ConcessionariaAPP.Application.Excepetions;
using Microsoft.EntityFrameworkCore;

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

        var created = await _vehicleRepository.Create(entity);
        return _mapper.Map<VehicleDto>(created);
    }

    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _vehicleRepository.Delete(id);
    }

    public async Task<IEnumerable<VehicleDto>> GetAll()
    {
        var list = await _vehicleRepository.GetAll().Select(v =>new VehicleDto
        {
            VehicleId = v.VehicleId,
            Model = v.Model,
            ManufacturerName = v.Manufacturer.Name,
            ManufacturingYear = v.ManufacturingYear,
            Price = v.Price
        }).ToListAsync();
        return list;
    }

    public async Task<VehicleDto> GetByIdAsync(int id)
    {
        var entity = await _vehicleRepository.GetById(id);
        return _mapper.Map<VehicleDto>(entity);
    }
    public async Task<VehicleDto> UpdateAsync(VehicleDto dto)
    {
        Validate(dto, isUpdate: true);
        if (dto.VehicleId.HasValue)
        {
            var entity = await _vehicleRepository.GetById(dto.VehicleId.Value);
            if (entity.IsDeleted)
                throw new AppValidationException(nameof(dto.VehicleId), "Veículo está excluído (lógico).");

            _mapper.Map(dto, entity);

            var updated = await _vehicleRepository.Update(entity);
            return _mapper.Map<VehicleDto>(updated);
        }
        else
        {
            throw new AppValidationException(nameof(dto.VehicleId), "O objeto não contém um ID válido para atualização.");
        }
    }
    
    private static void Validate(VehicleDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new AppValidationException(nameof(dto.VehicleId), "O objeto não pode ser nulo.");
        }
        if (isUpdate && dto.VehicleId <= 0)
        {
            throw new AppValidationException(nameof(dto.VehicleId), "Id inválido para atualização.");
        }

        if (string.IsNullOrWhiteSpace(dto.Model))
        {
             throw new AppValidationException(nameof(dto.Model), "Nome do Modelo é obrigatório.");
        }

    }

}