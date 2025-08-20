using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class CarDealershipRepository(AppDbContext context) : GenericCrudRepository<CarDealership>(context), ICarDealershipRepository
{
    private readonly AppDbContext _context = context;

    public async Task<CarDealership> GetByNameAsync(string name)
    {
        return await _context.CarDealerships
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Name == name)
            ?? throw new KeyNotFoundException("Concessionária não encontrada.");
    }

}