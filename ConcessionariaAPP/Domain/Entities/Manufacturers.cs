namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Manufacturers : BaseEntity
{
    [Key]
    public int ManufacturerId { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(50)]
    public required string Country { get; set; }

    public int FundationYear { get; set; }

    [MaxLength(255)]
    public string? WebSite { get; set; }

    public List<Vehicles> Vehicles { get; set; } = [];

    public Manufacturers(int manufacturerId, string name, string country, int fundationYear, string? webSite = null)
    {
        ManufacturerId = manufacturerId;
        Name = name;
        Country = country;
        FundationYear = fundationYear;
        WebSite = webSite;
    }
    public Manufacturers() { }
}