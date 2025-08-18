using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcessionariaAPP.Models.SaleViewModel;

public class SaleViewModel : BaseViewModel
{
    public int? SaleId { get; set; }

    [Display(Name = "Veículo")]
    [Required(ErrorMessage = "O veículo é obrigatório.")]
    public int VehicleId { get; set; }

    public string? VehicleModel { get; set; }

    [Required(ErrorMessage = "O cliente é obrigatório.")]
    [Display(Name = "Cliente")]
    public int ClientId { get; set; }
    
    public string? ClientName { get; set; }
    
    [Display(Name = "Concessionária")]
    [Required(ErrorMessage = "A concessionária é obrigatória.")]
    public int CarDealershipId { get; set; }

    public string? CarDealershipName { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Preço de Venda")]
    [Required(ErrorMessage = "O preço de venda é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser um valor positivo.")]
    public decimal SalePrice { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data da Venda")]
    [Required(ErrorMessage = "A data da venda é obrigatória.")]
    public DateTime SaleDate { get; set; } = DateTime.Now;

    [Display(Name = "Protocolo de Venda")]
    [MaxLength(20)]
    public string? SaleProtocol { get; set; }
}