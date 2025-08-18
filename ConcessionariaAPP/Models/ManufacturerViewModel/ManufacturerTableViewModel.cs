
namespace ConcessionariaAPP.Models.ManufacturerViewModel
{
    public class ManufacturerTableViewModel : TableViewModel<ManufacturerViewModel, ManufacturerFilterViewModel>
    {
        public ManufacturerTableViewModel()
        {
            Columns =
            [
                "Id",
                "Nome do Fabricante",
                "País",
                "Ano de Fundação",
                "WebSite",
                "Ativo"
            ];
            Keys =
            [
                "ManufacturerId",
                "Name",
                "Country",
                "FundationYear",
                "WebSite",
                "IsDeleted"
            ];
        }
    }
}