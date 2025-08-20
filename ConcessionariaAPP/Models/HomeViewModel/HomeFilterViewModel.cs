using ConcessionariaAPP.Models;

namespace ConcessionariaAPP.Models.HomeViewModel;

public class HomeFilterViewModel : FilterViewModel
{
    public int VehicleTypeId { get; set; }

    public int CarDealershipId { get; set; }

    public int ManufacturerId { get; set; }

    public int ClientId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

}

