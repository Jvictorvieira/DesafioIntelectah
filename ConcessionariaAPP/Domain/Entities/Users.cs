namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using ConcessionariaAPP.Domain.Enum;
using Microsoft.AspNetCore.Identity;

public class Users : IdentityUser
{

    [MaxLength(50, ErrorMessage = "O nome de usuário não pode exceder 50 caracteres.")]
    [Display(Name = "Nome do Usuário")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    [Display(Name = "Senha")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Nível de Acesso")]
    public AccessLevel AccessLevel { get; set; } = AccessLevel.Admin;

    public Users(string id, string name, string password, AccessLevel accessLevel)
    {
        Id = id;
        UserName = name;
        Name = name;
        Password = password;
        AccessLevel = accessLevel;
    }
    public Users() : base() { }
}
