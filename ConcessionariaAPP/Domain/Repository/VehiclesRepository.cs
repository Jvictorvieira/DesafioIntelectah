using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class VehiclesRepository(AppDbContext context) : GenericCrudRepository<Vehicles>(context), IVehicleRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Vehicles> GetByNameAsync(string modelName)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Model == modelName)
            ?? throw new KeyNotFoundException("Veículo com o modelo especificado não encontrado.");
    }

}
