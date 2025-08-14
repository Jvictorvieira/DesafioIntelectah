using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Repository;
using ConcessionariaAPP.Application.Services;
using ConcessionariaAPP.Application.Interfaces;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;


namespace ConcessionariaAPP.Infrastructure;

public static class AppInjectionConfiguration
{
    public static void Configure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureAuthentication();
        services.ConfigureDbContext(configuration);
        services.ConfigureRepositories();
        services.ConfigureServices();
        services.AddControllers();
        services.AddControllersWithViews();
        services.ConfigureSwagger();
    }
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehiclesRepository>();
    }
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleAppService>();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddIdentity<Users, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        //services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
    }
    

}