namespace ConcessionariaAPP.Domain.Repository;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class VehiclesRepository(AppDbContext context) : IVehicleRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var vehicle = await _context.Vehicles.FindAsync(id) ?? throw new KeyNotFoundException("Veículo não encontrado.");
            vehicle.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Erro ao deletar veículo: {ex.Message}");
        }
    }

    public async Task<Vehicles> CreateAsync(Vehicles entity)
    {
        try
        {
            var newEntity = await _context.Vehicles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return newEntity.Entity;
        } catch (Exception ex)
        {
            throw new Exception($"Erro ao criar veículo: {ex.Message}");
        }
    }
    public async Task<IEnumerable<Vehicles>> GetAllAsync()
    {
        try
        {
            return await _context.Vehicles.SelectMany(c => !c.IsDeleted ? new List<Vehicles> { c } : new List<Vehicles>()).ToArrayAsync();
        } catch (Exception ex)
        {
            throw new Exception($"Erro ao obter veículos: {ex.Message}");
        }
    }
    public async Task<Vehicles> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Vehicles.FindAsync(id) ?? throw new KeyNotFoundException("Veículo não encontrado.");
        } catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Erro ao obter veículo: {ex.Message}");
        }
    }
    public async Task<Vehicles> UpdateAsync(Vehicles entity)
    {
        try
        {
            var existingVehicle = await _context.Vehicles.FindAsync(entity.VehicleId) ?? throw new KeyNotFoundException("Veículo não encontrado.");
            existingVehicle.Model = entity.Model;
            await _context.SaveChangesAsync();
            return existingVehicle;
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Erro ao atualizar veículo: {ex.Message}");
        }
    }
    
    public async Task<Vehicles> GetByNameAsync(string modelName)
    {
        try
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.Model == modelName && !v.IsDeleted) 
                   ?? throw new KeyNotFoundException("Veículo com o modelo especificado não encontrado.");
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Erro ao obter veículo por modelo: {ex.Message}");
        }
    }
}