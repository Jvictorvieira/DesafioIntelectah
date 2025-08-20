using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConcessionariaAPP.Infrastructure;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Enum; // VehiclesTypes

namespace ConcessionariaAPP.Infrastructure;

public sealed class DemoSeedOptions
{
    public int Manufacturers { get; init; } = 6;
    public int VehiclesPerManufacturer { get; init; } = 6;
    public int CarDealerships { get; init; } = 4;
    public int Clients { get; init; } = 300;
    public int MonthsBack { get; init; } = 12;
    public int AvgSalesPerMonth { get; init; } = 220;
}

public static class DemoDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services, DemoSeedOptions? options = null)
    {
        options ??= new DemoSeedOptions();
        var rnd = new Random(12345);

        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;
        var db = sp.GetRequiredService<AppDbContext>();
        
        // garante schema
        await db.Database.MigrateAsync();

        // Se já tem vendas suficientes, não semeia novamente
        if (await db.Sales.AsNoTracking().CountAsync() >= options.AvgSalesPerMonth * 3)
            return;

        // -------- Manufacturers
        if (!await db.Manufacturers.AnyAsync())
        {
            var names = new[]
            {
                "Aurora Motors","Veloce Auto","Atlas Vehicles","Brava Cars",
                "Orion Automotive","Nexa Mobility","Pulsar Motors","Clássica Auto"
            };

            var mans = Enumerable.Range(0, options.Manufacturers)
                .Select(i =>
                {
                    var name = names[i % names.Length];
                    var country = new[] { "BR", "US", "DE", "JP", "IT", "FR" }[rnd.Next(6)];
                    var year = rnd.Next(1950, 2018);
                    var site = $"https://{Slug(name)}.com";
                    return new Manufacturers(){ Name = name, Country = country, FundationYear = year, WebSite = site };
                }).ToList();

            db.Manufacturers.AddRange(mans);
            await db.SaveChangesAsync();
        }

        // recarrega com IDs
        var manufacturers = await db.Manufacturers.AsNoTracking().ToListAsync();

        // -------- CarDealerships
        if (!await db.CarDealerships.AnyAsync())
        {
            var cities = new[] { "São Paulo", "Rio de Janeiro", "Belo Horizonte", "Curitiba", "Porto Alegre", "Salvador" };
            var dealers = Enumerable.Range(0, options.CarDealerships).Select(i =>
            {
                var city = cities[i % cities.Length];
                var name = $"Concessionária {city}";
                var state = new[] { "SP", "RJ", "MG", "PR", "RS", "BA" }[i % 6];
                var phone = $"11 9{rnd.Next(1000, 9999)}-{rnd.Next(1000, 9999)}";
                var email = $"{Slug(name)}@exemplo.com";
                return new CarDealership() { Name = name, Address = $"Av. Principal, {rnd.Next(50, 999)}", City = city, State = state, AddressCode = $"{rnd.Next(10000, 99999)}-{rnd.Next(100, 999)}", Phone = phone, Email = email, MaxVehicleCapacity = rnd.Next(50, 300) };
            }).ToList();

            db.CarDealerships.AddRange(dealers);
            await db.SaveChangesAsync();
        }

        // -------- Vehicles
        if (!await db.Vehicles.AnyAsync())
        {
            var models = new[] { "Falcon", "Artemis", "Raptor", "Eclipse", "Comet", "Strada", "Vortex", "Nimbus" };
            var types = Enum.GetValues<VehiclesTypes>();

            var vehicles = new List<Vehicles>();
            foreach (var m in manufacturers)
            {
                for (int i = 0; i < options.VehiclesPerManufacturer; i++)
                {
                    var model = $"{models[(i + m.ManufacturerId) % models.Length]} {rnd.Next(100, 999)}";
                    var year = rnd.Next(2008, DateTime.UtcNow.Year + 1);
                    var price = Math.Round(45000m + (decimal)rnd.NextDouble() * 180000m, 2);
                    var vtype = (VehiclesTypes)types.GetValue(rnd.Next(types.Length))!;

                    var v = new Vehicles()
                    {
                        Model = model,
                        ManufacturingYear = year,
                        Price = price,
                        Manufacturer = m,
                        VehicleType = vtype
                    };
                    vehicles.Add(v);
                }
            }
            db.Vehicles.AddRange(vehicles);
            db.AttachRange(vehicles.Select(v => v.Manufacturer)); // attach manufacturers to vehicles
            await db.SaveChangesAsync();
        }

        var vehiclesAll = await db.Vehicles.AsNoTracking().ToListAsync();

        // -------- Clients
        if (!await db.Clients.AnyAsync())
        {
            var clients = Enumerable.Range(0, options.Clients).Select(i =>
            {
                var name = FakeName(rnd);
                var cpf = GenerateValidCpf(rnd);
                var phone = $"11 9{rnd.Next(1000, 9999)}-{rnd.Next(1000, 9999)}";
                return new Clients() { Phone = phone, Cpf = cpf, Name = name };
            }).ToList();

            db.Clients.AddRange(clients);
            await db.SaveChangesAsync();
        }

        var clientsAll = await db.Clients.AsNoTracking().ToListAsync();
        var dealersAll = await db.CarDealerships.AsNoTracking().ToListAsync();

        // -------- Sales (distribuídas por mês)
        var now = DateTime.UtcNow;
        var salesBuffer = new List<Sales>();

        for (int m = 0; m < options.MonthsBack; m++)
        {
            var monthRef = new DateTime(now.Year, now.Month, 1).AddMonths(-m);
            var days = DateTime.DaysInMonth(monthRef.Year, monthRef.Month);

            // sazonalidade leve
            var target = (int)(options.AvgSalesPerMonth * (0.85 + rnd.NextDouble() * 0.4));

            for (int i = 0; i < target; i++)
            {
                var v = vehiclesAll[rnd.Next(vehiclesAll.Count)];
                var c = clientsAll[rnd.Next(clientsAll.Count)];
                var d = dealersAll[rnd.Next(dealersAll.Count)];

                var day = rnd.Next(1, days + 1);
                var date = new DateTime(monthRef.Year, monthRef.Month, day, rnd.Next(9, 19), rnd.Next(0, 60), 0, DateTimeKind.Utc);

                // preço com pequena variação sobre o preço do veículo
                var price = Math.Round(v.Price * (decimal)(0.9 + rnd.NextDouble() * 0.25), 2);

                var protocol = GenerateProtocol();
                salesBuffer.Add(new Sales(0, v.VehicleId, c.ClientId, d.CarDealershipId, price, date, protocol));
            }

            // flush em lotes para poupar memória
            if (salesBuffer.Count >= 4000)
            {
                db.Sales.AddRange(salesBuffer);
                await db.SaveChangesAsync();
                salesBuffer.Clear();
            }
        }

        if (salesBuffer.Count > 0)
        {
            db.Sales.AddRange(salesBuffer);
            await db.SaveChangesAsync();
        }
    }

    // ---------- helpers ----------
    private static string Slug(string s)
        => new string((s ?? "").ToLowerInvariant().Where(ch => char.IsLetterOrDigit(ch) || ch == ' ').Select(ch => ch == ' ' ? '-' : ch).ToArray());

    private static string FakeName(Random rnd)
    {
        var first = new[] { "Ana", "Bruno", "Carla", "Diego", "Eva", "Felipe", "Gabriela", "Hugo", "Isabela", "João", "Karen", "Lucas", "Marina", "Nina", "Otávio", "Paula", "Rafael", "Sofia", "Tiago", "Vivian" };
        var last = new[] { "Silva", "Santos", "Oliveira", "Pereira", "Lima", "Ferreira", "Almeida", "Costa", "Souza", "Gomes" };
        return $"{first[rnd.Next(first.Length)]} {last[rnd.Next(last.Length)]}";
    }

    // CPF válido (apenas dígitos)
    private static string GenerateValidCpf(Random rnd)
    {
        int[] digits = new int[11];
        for (int i = 0; i < 9; i++) digits[i] = rnd.Next(0, 10);

        int Calc(int len)
        {
            int sum = 0, w = len + 1;
            for (int i = 0; i < len; i++) sum += digits[i] * w--;
            int mod = sum % 11; return mod < 2 ? 0 : 11 - mod;
        }

        digits[9] = Calc(9);
        digits[10] = Calc(10);

        return string.Concat(digits.Select(d => d.ToString(CultureInfo.InvariantCulture)));
    }
    
    private static string GenerateProtocol()
    {
        var random = new Random();
        return new string([.. Enumerable.Repeat("0123456789", 20).Select(s => s[random.Next(s.Length)])]);
    }
}
