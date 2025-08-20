namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Clients : BaseEntity
{
    [Key]
    public int ClientId { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(11)]
    public required string Cpf { get; set; }

    [MaxLength(15)]
    [Display(Name = "Telefone")]
    public required string Phone { get; set; }

    public List<Sales> Sales { get; set; } = [];

    public Clients(int clientId, string name, string cpf, string phone)
    {
        ClientId = clientId;
        Name = name;
        Cpf = cpf;
        Phone = phone;
    }

    public Clients() { }
}