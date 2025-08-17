
namespace ConcessionariaAPP.Models.ClientViewModel      
{
    public class ClientTableViewModel : TableViewModel<ClientViewModel, ClientFilterViewModel>
    {
        public ClientTableViewModel()
        {
            Columns =
            [
                "Id", "Nome", "CPF", "Email", "Telefone"
            ];
            Keys =
            [
                "ClientId", "Name", "Cpf", "Email", "Phone"
            ];
        }
    }
}