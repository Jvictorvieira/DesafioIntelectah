using ConcessionariaAPP.Models;

namespace ConcessionariaAPP.Models.HomeViewModel;

public class HomeViewModel
{
    public HomeFilterViewModel filter { get; set; } = new();
    public SalesPerCarDealershipViewModel SalesPerCarDealership { get; set; } = new();
    public SalesPerClientViewModel SalesPerClient { get; set; } = new();
    public SalesPerManufacturerViewModel SalesPerManufacturer { get; set; } = new();
    public SalesPerMonthViewModel SalesPerMonth { get; set; } = new();
    public SalesPerVehicleTypeViewModel SalesPerVehicleType { get; set; } = new();
}

