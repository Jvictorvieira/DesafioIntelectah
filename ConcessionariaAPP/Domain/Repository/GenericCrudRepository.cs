using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class GenericCrudRepository<T>(AppDbContext context) : IGenericCrudRepository<T> where T : class
{
    private readonly AppDbContext _context = context;

    public async Task<T> Create(T entity)
    {
        var entry =  await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id)
            ?? throw new KeyNotFoundException("Entidade não encontrada.");
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public IQueryable<T> GetAll(bool includeDeleted = false)
    {
        var query = _context.Set<T>().AsNoTracking();

        if (!includeDeleted)
        {
            query = query.Where(s => !EF.Property<bool>(s, "IsDeleted"));
        }

        return query;
    }

    public async Task<T> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id)
            ?? throw new KeyNotFoundException("Entidade não encontrada.");
    }



    public async Task<T> Update(T entity)
    {
        var existingEntity = await _context.Set<T>().FindAsync(entity)
            ?? throw new KeyNotFoundException("Entidade não encontrada.");

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return existingEntity;
    }

    
}

