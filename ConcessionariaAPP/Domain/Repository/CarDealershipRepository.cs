using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class CarDealershipRepository(AppDbContext context) : ICarDealershipRepository
{
    private readonly AppDbContext _context = context;

    public async Task<CarDealership> CreateAsync(CarDealership entity)
    {
        var entry = await _context.CarDealerships.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var CarDealership = await _context.CarDealerships.FindAsync(id)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");

        CarDealership.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CarDealership>> GetAllAsync()
    {
        return await _context.CarDealerships
            .AsNoTracking()
            .Where(m => !m.IsDeleted)
            .ToListAsync();
    }

    public async Task<CarDealership> GetByIdAsync(int id)
    {
        return await _context.CarDealerships
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CarDealershipId == id)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }

    public async Task<CarDealership> GetByNameAsync(string name)
    {
        return await _context.CarDealerships
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Name == name)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }

    public async Task<CarDealership> UpdateAsync(CarDealership entity)
    {
        var existing = await _context.CarDealerships.FindAsync(entity.CarDealershipId)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");

        existing.Name = entity.Name;
        existing.Address = entity.Address;
        existing.City = entity.City;
        existing.State = entity.State;
        existing.Phone = entity.Phone;
        existing.Email = entity.Email;
        existing.MaxVehicleCapacity = entity.MaxVehicleCapacity;

        await _context.SaveChangesAsync();
        return existing;
    }
}