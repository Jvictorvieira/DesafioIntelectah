using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class ManufacturerRepository(AppDbContext context) : GenericCrudRepository<Manufacturers>(context), IManufacturerRepository
{
    private readonly AppDbContext _context = context;

    
    public async Task<Manufacturers> GetByNameAsync(string name)
    {
        return await _context.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Name == name)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }

}
