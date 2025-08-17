
namespace ConcessionariaAPP.Application.Dto;

public class SaleDto : BaseDto
{
    public int? SaleId { get; set; }

    public int VehicleId { get; set; }

    public string? VehicleModel { get; set; }


    public int ClientId { get; set; }

    public string? ClientName { get; set; }

    public int CarDealershipId { get; set; }

    public string? CarDealershipName { get; set; }

    public decimal SalePrice { get; set; }

    public DateTime SaleDate { get; set; }

    public string? SaleProtocol { get; set; }
}