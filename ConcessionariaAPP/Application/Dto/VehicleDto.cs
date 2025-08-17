using ConcessionariaAPP.Domain.Enum;

namespace ConcessionariaAPP.Application.Dto;

public class VehicleDto : BaseDto
{
    public int? VehicleId { get; set; }

    public required string Model { get; set; }

    public int ManufacturingYear { get; set; }

    public decimal Price { get; set; }

    public int ManufacturerId { get; set; }
    public string? ManufacturerName { get; set; }

    public string? Description { get; set; }
    public VehiclesTypes VehicleType { get; set; }
}