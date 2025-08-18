using ConcessionariaAPP.Domain.Enum;

namespace ConcessionariaAPP.Models.HomeViewModel;

public class SalesPerVehicleTypeViewModel : ChartViewModel
{
    public SalesPerVehicleTypeViewModel()
    {
        Labels = [];
        Data = [];
    }
}