using ConcessionariaAPP.Models;

namespace ConcessionariaAPP.Models.HomeViewModel;

public class HomeFilterViewModel : BaseViewModel
{
    public int VehicleTypeId { get; set; }

    public int CarDealershipId { get; set; }

    public int ManufacturerId { get; set; }

    public int ClientId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public SalesPerCarDealershipViewModel SalesPerCarDealership { get; set; } = new();
    public SalesPerClientViewModel SalesPerClient { get; set; } = new();
    public SalesPerManufacturerViewModel SalesPerManufacturer { get; set; } = new();
    public SalesPerMonthViewModel SalesPerMonth { get; set; } = new();
    public SalesPerVehicleTypeViewModel SalesPerVehicleType { get; set; } = new();
}

