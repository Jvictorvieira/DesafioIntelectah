using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class ClientRepository(AppDbContext context) : IClientRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Clients> CreateAsync(Clients entity)
    {
        var entry = await _context.Clients.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var Client = await _context.Clients.FindAsync(id)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");

        Client.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Clients>> GetAllAsync()
    {
        return await _context.Clients
            .AsNoTracking()
            .Where(m => !m.IsDeleted)
            .ToListAsync();
    }

    public async Task<Clients> GetByIdAsync(int id)
    {
        return await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ClientId == id)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }

    public async Task<Clients> GetByNameAsync(string name)
    {
        return await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Name == name && !m.IsDeleted)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }

    public async Task<Clients> UpdateAsync(Clients entity)
    {
        var existing = await _context.Clients.FindAsync(entity.ClientId)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
            
        await _context.SaveChangesAsync();
        return existing;
    }
}