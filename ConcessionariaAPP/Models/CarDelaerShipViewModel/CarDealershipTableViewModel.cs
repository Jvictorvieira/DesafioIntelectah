
namespace ConcessionariaAPP.Models.CarDealershipViewModel
{
    public class CarDealershipTableViewModel : TableViewModel<CarDealershipViewModel, CarDealershipFilterViewModel>
    {
        public CarDealershipTableViewModel()
        {
            Columns =
            [
                "Id", "Nome", "Endereço", "Cidade", "Estado", "CEP", "Telefone", "Email", "Capacidade Máxima de Veículos"
            ];
            Keys =
            [
                "CarDealershipId", "Name", "Address", "City", "State", "AddressCode", "Phone", "Email", "MaxVehicleCapacity"
            ];
        }
    }
}