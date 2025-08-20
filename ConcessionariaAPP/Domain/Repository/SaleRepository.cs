using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConcessionariaAPP.Domain.Repository;

public class SaleRepository(AppDbContext context): GenericCrudRepository<Sales>(context), ISaleRepository
{
    private readonly AppDbContext _context = context;



}


