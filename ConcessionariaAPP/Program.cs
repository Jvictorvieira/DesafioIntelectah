using ConcessionariaAPP.Infrastructure; // seu AppDbContext
using ConcessionariaAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Dependency Injection
AppInjectionConfiguration.Configure(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (builder.Configuration.GetValue("RUN_DEMO_SEED", true))
    { 
        await DemoDataSeeder.SeedAsync(app.Services, new DemoSeedOptions
        {
            Manufacturers = 8,
            VehiclesPerManufacturer = 8,
            CarDealerships = 5,
            Clients = 600,
            MonthsBack = 18,
            AvgSalesPerMonth = 250
        });
     }

    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger"; // Set Swagger UI at the app's root
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Seed roles
RolesSeedConfiguration.SeedRoles(app.Services).GetAwaiter().GetResult(); // Seed roles

var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

if (!isDocker && !app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapIdentityApi<Users>(); // Map Identity API endpoints for Users
app.Run();
