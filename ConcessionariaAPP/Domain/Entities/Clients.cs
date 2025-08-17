namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Clients : BaseEntity
{
    [Key]
    public int ClientId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(11)]
    public string Cpf { get; set; }

    [MaxLength(15)]
    [Display(Name = "Telefone")]
    public string Phone { get; set; }

    public List<Sales> Sales { get; set; } = [];

    public Clients(int clientId, string name, string cpf, string phone)
    {
        ClientId = clientId;
        Name = name;
        Cpf = cpf;
        Phone = phone;
    }
}