using ConcessionariaAPP.Domain.Enum;

namespace ConcessionariaAPP.Infrastructure;

public static class Roles
{
    public const string Admin = nameof(AccessLevel.Admin);
    public const string Manager = nameof(AccessLevel.Manager);
    public const string Seller = nameof(AccessLevel.Seller);
}