namespace ConcessionariaAPP.Models.VehicleViewModel;

public class VehicleTableViewModel : TableViewModel<VehicleViewModel, VehicleFilterViewModel>
{
    public VehicleTableViewModel()
    {
        Columns = ["Id", "Modelo","Tipo de Veículo", "Ano de Fabricação", "Preço", "Fabricante"];
        Keys = ["VehicleId", "Model", "VehicleTypeName", "ManufacturingYear", "Price", "ManufacturerName"];
    }
}