
namespace ConcessionariaAPP.Application.Dto;

public class CarDealershipDto : BaseDto
{
    public int? CarDealershipId { get; set; }

    public required string Name { get; set; }

    public required string Address { get; set; }

    public required string City { get; set; }

    public required string State { get; set; }
    
    public required string AddressCode { get; set; }

    public required string Phone { get; set; }

    public required string Email { get; set; }

    public required int MaxVehicleCapacity { get; set; }

}