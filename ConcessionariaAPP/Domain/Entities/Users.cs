namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using ConcessionariaAPP.Domain.Enum;


public class Users : BaseEntity
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(50, ErrorMessage = "O nome de usuário não pode exceder 50 caracteres.")]
    [Display(Name = "Nome de Usuário")]
    public string Name { get; set; }

    [MaxLength(255)]
    [Display(Name = "Senha")]
    public string Password { get; set; }

    [MaxLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
    [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
    public string Email { get; set; }

    [Display(Name = "Nível de Acesso")]
    public AccessLevel AccessLevel { get; set; }

    public List<Sales> Sales { get; set; } = [];

    public Users(int userId, string name, string password, string email, AccessLevel accessLevel)
    {
        UserId = userId;
        Name = name;
        Password = password;
        Email = email;
        AccessLevel = accessLevel;
    }
}
