using System.ComponentModel.DataAnnotations;

namespace ConcessionariaAPP.Models.ManufacturerViewModel;

public class ManufacturerViewModel : BaseViewModel
{
    public int? ManufacturerId { get; set; }

    [MaxLength(100, ErrorMessage = "O nome do fabricante não pode exceder 100 caracteres.")]
    [Display(Name = "Nome do Fabricante")]
    [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
    public string? Name { get; set; }

    [MaxLength(50, ErrorMessage = "O país de origem não pode exceder 50 caracteres.")]
    [Display(Name = "País de Origem")]
    [Required(ErrorMessage = "O país de origem é obrigatório.")]
    public string? Country { get; set; }

    [Display(Name = "Ano de Fundação")]
    [Range(1961, 2100, ErrorMessage = "O ano de fundação deve ser maior que 1960 e menor que o ano atual.")]
    public int FundationYear { get; set; }

    [MaxLength(255, ErrorMessage = "O site não pode exceder 255 caracteres.")]
    public string? WebSite { get; set; }
}