using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class SaleRepository(AppDbContext context) : ISaleRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Sales> CreateAsync(Sales entity)
    {
        var entry = await _context.Sales.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var Sale = await _context.Sales.FindAsync(id)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");
        Sale.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Sales>> GetAllAsync()
    {
        return await _context.Sales
            .AsNoTracking()
            .Include(v => v.Client)
            .Include(v => v.Vehicle)
            .Include(v => v.CarDealership)
            .Where(v => !v.IsDeleted)
            .ToListAsync();
    }

    public async Task<Sales> GetByIdAsync(int id)
    {
        return await _context.Sales
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.SaleId == id)
            ?? throw new KeyNotFoundException("Venda não encontrada.");
    }

    

    public async Task<Sales> UpdateAsync(Sales entity)
    {
        var existingSale = await _context.Sales.FindAsync(entity.SaleId)
            ?? throw new KeyNotFoundException("Venda não encontrada.");

        existingSale.ClientId = entity.ClientId;
        existingSale.VehicleId = entity.VehicleId;
        existingSale.CarDealershipId = entity.CarDealershipId;

        await _context.SaveChangesAsync();
        return existingSale;
    }
}
