

namespace ConcessionariaAPP.Application.Dto;

public class ManufacturerDto : BaseDto
{
    public int? ManufacturerId { get; set; }

    public required string Name { get; set; }

    public required string Country { get; set; }

    public int FundationYear { get; set; }

    public required string WebSite { get; set; }
}