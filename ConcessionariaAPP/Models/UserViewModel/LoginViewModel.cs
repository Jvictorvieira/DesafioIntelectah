using System.ComponentModel.DataAnnotations;
namespace ConcessionariaAPP.Models.UserViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O Email informado não é válido.")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    [Display(Name = "Senha")]
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Lembrar-me")]
    public bool RememberMe { get; set; } = false;
}
