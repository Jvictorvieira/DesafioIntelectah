namespace ConcessionariaAPP.Models.VehicleViewModel;

public class VehicleTableViewModel : TableViewModel<VehicleViewModel, VehicleFilterViewModel>
{
    public VehicleTableViewModel()
    {
        Columns = ["Id", "Modelo", "Ano de Fabricação", "Preço", "Fabricante"];
        Keys = ["VehicleId", "Model", "ManufacturingYear", "Price", "Manufacturer"];
    }
}