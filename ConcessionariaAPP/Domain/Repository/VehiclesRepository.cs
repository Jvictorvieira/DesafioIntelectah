using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class VehiclesRepository(AppDbContext context) : IVehicleRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Vehicles> CreateAsync(Vehicles entity)
    {
        var entry = await _context.Vehicles.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");
        vehicle.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Vehicles>> GetAllAsync()
    {
        return await _context.Vehicles
            .AsNoTracking()
            .Include(v => v.Manufacturer)
            .Where(v => !v.IsDeleted)
            .ToListAsync();
    }

    public async Task<Vehicles> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.VehicleId == id)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");
    }

    public async Task<Vehicles> GetByNameAsync(string modelName)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Model == modelName && !v.IsDeleted)
            ?? throw new KeyNotFoundException("Veículo com o modelo especificado não encontrado.");
    }

    public async Task<Vehicles> UpdateAsync(Vehicles entity)
    {
        var existingVehicle = await _context.Vehicles.FindAsync(entity.VehicleId)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");

        existingVehicle.Model = entity.Model; // mantém o que você já atualizava
        await _context.SaveChangesAsync();
        return existingVehicle;
    }
}
