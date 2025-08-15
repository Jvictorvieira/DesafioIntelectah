using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Repository;
using ConcessionariaAPP.Application.Services;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Infrastructure;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;


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
        services.AddControllersWithViews(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });

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
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Roles.Admin, Roles.Manager, Roles.Seller)
                .Build();

            options.AddPolicy("Admin", policy =>
                policy.RequireRole(Roles.Admin));
            options.AddPolicy("Manager", policy =>
                policy.RequireRole(Roles.Manager));
            options.AddPolicy("Seller", policy =>
                policy.RequireRole(Roles.Seller));
        });

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

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        });

        //services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
    }
    

}