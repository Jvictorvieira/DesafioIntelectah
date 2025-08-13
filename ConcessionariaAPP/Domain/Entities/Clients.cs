namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Clients : BaseEntity
{
    [Key]
    public int ClientId { get; set; }

    [MaxLength(100, ErrorMessage = "O nome do cliente não pode exceder 100 caracteres.")]
    [Required]
    [Display(Name = "Nome do Cliente")]
    public string Name { get; set; }

    [MaxLength(11, ErrorMessage = "O CPF não pode exceder 11 caracteres.")]
    public string Cpf { get; set; }

    [MaxLength(15, ErrorMessage = "O telefone não pode exceder 15 caracteres.")]
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