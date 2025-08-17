using ConcessionariaAPP.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ConcessionariaAPP.Models.VehicleViewModel;

public class VehicleViewModel : BaseViewModel

{
    public int? VehicleId { get; set; }

    [MaxLength(100, ErrorMessage = "O modelo do veículo não pode exceder 100 caracteres.")]
    [Display(Name = "Nome do modelo do veículo")]
    [Required(ErrorMessage = "O nome do modelo do veículo é obrigatório.")]
    public string? Model { get; set; }

    [Display(Name = "Ano de fabricação do veículo")]
    [Range(1900, int.MaxValue, ErrorMessage = "O ano de fabricação deve ser maior ou igual a 1900.")]
    public int? ManufacturingYear { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo.")]
    [Display(Name = "Preço do veículo")]
    [Required(ErrorMessage = "O preço do veículo é obrigatório.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "O fabricante é obrigatório.")]
    [Display(Name = "Fabricante do veículo")]
    public int ManufacturerId { get; set; }

    public string? ManufacturerName { get; set; }

    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "O tipo de veículo é obrigatório.")]
    [Display(Name = "Tipo de Veículo")]
    public VehiclesTypes VehicleType { get; set; }

    public string? VehicleTypeName { get; set; }

}