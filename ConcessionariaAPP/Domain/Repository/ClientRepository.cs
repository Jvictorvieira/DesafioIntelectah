using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class ClientRepository(AppDbContext context) : GenericCrudRepository<Clients>(context), IClientRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Clients> GetByCpfAsync(string cpf)
    {
        return await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Cpf == cpf)
            ?? throw new KeyNotFoundException("Fabricante não encontrado.");
    }
}