namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;



public class VehicleAppService(IVehicleRepository repo) : IVehicleService
{
    private readonly IVehicleRepository _repo = repo;

    public async Task LogicalDeleteAsync(int id)
    {
        try
        {

            var vehicle = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Veículo não encontrado.");
            vehicle.IsDeleted = true;
            await _repo.UpdateAsync(vehicle);
        } catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Erro ao deletar veículo: {ex.Message}");
        }
    }

    public async Task<Vehicles> CreateAsync(Vehicles entity)
    {
        try
        {
            var newEntity = await _repo.CreateAsync(entity);
            return entity;
        } catch (Exception ex)
        {
            throw new Exception($"Erro ao criar veículo: {ex.Message}");
        }
    }
    public async Task<IEnumerable<Vehicles>> GetAllAsync()
    {
        try
        {
            return await _repo.GetAllAsync();
        } catch (Exception ex)
        {
            throw new Exception($"Erro ao obter veículos: {ex.Message}");
        }
    }
    public async Task<Vehicles> GetByIdAsync(int id)
    {
        try
        {
            return await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Veículo não encontrado.");
        } catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Erro ao obter veículo: {ex.Message}");
        }
    }
    public async Task<Vehicles> UpdateAsync(Vehicles entity)
    {
        try
        {
            var existingVehicle = await _repo.GetByIdAsync(entity.VehicleId) ?? throw new KeyNotFoundException("Veículo não encontrado.");
            existingVehicle.Model = entity.Model;
            return await _repo.UpdateAsync(existingVehicle);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Erro ao atualizar veículo: {ex.Message}");
        }
    } 
}