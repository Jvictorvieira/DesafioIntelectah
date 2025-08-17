
namespace ConcessionariaAPP.Models.SaleViewModel
{
    public class SaleTableViewModel : TableViewModel<SaleViewModel, SaleFilterViewModel>
    {
        public SaleTableViewModel()
        {
            Columns =
            [
                "Id", "Veículo", "Cliente", "Concessionária", "Preço de Venda", "Data da Venda", "Protocolo de Venda"
            ];
            Keys =
            [
                "SaleId","VehicleModel","ClientName","CarDealershipName","SalePrice","SaleDate","SaleProtocol"
            ];
        }
    }
}