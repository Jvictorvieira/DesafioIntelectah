using System.ComponentModel.DataAnnotations;
namespace ConcessionariaAPP.Models.UserViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O nome não pode exceder 50 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O Email informado não é válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirme a Senha")]
    [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}