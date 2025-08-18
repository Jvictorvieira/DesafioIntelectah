
namespace ConcessionariaAPP.Models.ClientViewModel      
{
    public class ClientTableViewModel : TableViewModel<ClientViewModel, ClientFilterViewModel>
    {
        public ClientTableViewModel()
        {
            Columns =
            [
                "Id", "Nome", "CPF", "Telefone"
            ];
            Keys =
            [
                "ClientId", "Name", "Cpf", "Phone"
            ];
        }
    }
}