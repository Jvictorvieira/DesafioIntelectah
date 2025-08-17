using System.ComponentModel.DataAnnotations;

namespace ConcessionariaAPP.Models.CarDealershipViewModel;

public class CarDealershipViewModel : BaseViewModel
{
    public int? CarDealershipId { get; set; }

    [MaxLength(100, ErrorMessage = "O nome da concessionária não pode exceder 100 caracteres.")]
    [Required(ErrorMessage = "O nome da concessionária é obrigatório.")]
    [Display(Name = "Nome da Concessionária")]
    public string? Name { get; set; }

    [MaxLength(255, ErrorMessage = "O endereço não pode exceder 255 caracteres.")]
    [Required(ErrorMessage = "O endereço da concessionária é obrigatório.")]
    [Display(Name = "Endereço")]
    public string? Address { get; set; }

    [MaxLength(50, ErrorMessage = "A cidade não pode exceder 50 caracteres.")]
    [Display(Name = "Cidade")]
    public string? City { get; set; }

    [MaxLength(50, ErrorMessage = "O estado não pode exceder 50 caracteres.")]
    [Display(Name = "Estado")]
    public string? State { get; set; }

    [MaxLength(10, ErrorMessage = "O CEP não pode exceder 10 caracteres.")]
    [Display(Name = "CEP")]
    public string? AddressCode { get; set; }

    [MaxLength(15, ErrorMessage = "O telefone não pode exceder 15 caracteres.")]
    [Display(Name = "Telefone")]
    public string? Phone { get; set; }

    [MaxLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
    [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
    [Required(ErrorMessage = "O email da concessionária é obrigatório.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A capacidade máxima de veículos é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A capacidade máxima de veículos deve ser maior que zero.")]
    [Display(Name = "Capacidade Máxima de Veículos")]
    public int MaxVehicleCapacity { get; set; }
}