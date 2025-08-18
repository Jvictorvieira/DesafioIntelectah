using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class ManufacturerRepository(AppDbContext context) : IManufacturerRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Manufacturers> CreateAsync(Manufacturers entity)
    {
        var entry = await _context.Manufacturers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var manufacturer = await _context.Manufacturers.FindAsync(id)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");

        manufacturer.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Manufacturers>> GetAllAsync()
    {
        return await _context.Manufacturers
            .AsNoTracking()
            .Where(m => !m.IsDeleted)
            .ToListAsync();
    }

    public async Task<Manufacturers> GetByIdAsync(int id)
    {
        return await _context.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ManufacturerId == id)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }

    public async Task<Manufacturers> GetByNameAsync(string name)
    {
        return await _context.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Name == name)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }

    public async Task<Manufacturers> UpdateAsync(Manufacturers entity)
    {
        var existing = await _context.Manufacturers.FindAsync(entity.ManufacturerId)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");

        existing.Name = entity.Name;
        existing.Country = entity.Country;
        existing.FundationYear = entity.FundationYear;
        existing.WebSite = entity.WebSite;

        await _context.SaveChangesAsync();
        return existing;
    }
}
