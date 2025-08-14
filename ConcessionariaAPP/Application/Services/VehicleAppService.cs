namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;



public class VehicleAppService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleAppService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    public async Task<Vehicles> CreateAsync(Vehicles entity)
    {
        return await _vehicleRepository.CreateAsync(entity);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _vehicleRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Vehicles>> GetAllAsync()
    {
        return await _vehicleRepository.GetAllAsync();
    }

    public async Task<Vehicles> GetByIdAsync(int id)
    {
        return await _vehicleRepository.GetByIdAsync(id);
    }
    public async Task<Vehicles> UpdateAsync(Vehicles entity)
    {
        return await _vehicleRepository.UpdateAsync(entity);
    }

}