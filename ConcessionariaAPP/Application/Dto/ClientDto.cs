

namespace ConcessionariaAPP.Application.Dto;

public class ClientDto : BaseDto
{
    public int? ClientId { get; set; }

    public required string Name { get; set; }

    public required string Cpf { get; set; }

    public required string Phone { get; set; }
}