using System.ComponentModel.DataAnnotations;
using ConcessionariaAPP.Models.Attributes;

namespace ConcessionariaAPP.Models.ClientViewModel;

public class ClientViewModel : BaseViewModel
{
    public int? ClientId { get; set; }
    [MaxLength(100, ErrorMessage = "O nome do cliente não pode exceder 100 caracteres.")]
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [Display(Name = "Nome do Cliente")]
    public string? Name { get; set; }

    [MaxLength(14, ErrorMessage = "O CPF não pode exceder 14 caracteres.")]
    [Required(ErrorMessage = "O CPF do cliente é obrigatório.")]
    [Display(Name = "CPF")]
    [Cpf(ErrorMessage = "CPF inválido.")]
    public string? Cpf { get; set; }

    [MaxLength(15, ErrorMessage = "O telefone não pode exceder 15 caracteres.")]
    [Required(ErrorMessage = "O telefone do cliente é obrigatório.")]
    [Display(Name = "Telefone")]
    public string? Phone { get; set; }
}