namespace ConcessionariaAPP.Domain.Entities;

using ConcessionariaAPP.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Vehicles : BaseEntity
{
    [Key]
    public int VehicleId { get; set; }

    [MaxLength(100)]
    public string Model { get; set; }

    public int ManufacturingYear { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int ManufacturerId { get; set; }

    public List<Manufacturers> Manufacturers { get; set; } = [];

    public string? Description { get; set; }

    public VehiclesTypes VehicleType { get; set; }

    public List<Sales> Sales { get; set; } = [];

    public Vehicles(int vehicleId, string model, int manufacturingYear, decimal price, int manufacturerId, string? description, VehiclesTypes vehicleType)
    {
        VehicleId = vehicleId;
        Model = model;
        ManufacturingYear = manufacturingYear;
        Price = price;
        ManufacturerId = manufacturerId;
        Description = description;
        VehicleType = vehicleType;
    }

}
