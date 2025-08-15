using System.Threading.Tasks;
using ConcessionariaAPP.Infrastructure;
using Microsoft.AspNetCore.Identity;


public static class RolesSeedConfiguration
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { Roles.Admin, Roles.Manager, Roles.Seller };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}