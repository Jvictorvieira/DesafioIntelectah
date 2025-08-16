using ConcessionariaAPP.Domain.Enum;
using System.ComponentModel.DataAnnotations;


namespace ConcessionariaAPP.Models.VehicleViewModel;

public class VehicleViewModel : BaseViewModel

{
    public int? VehicleId { get; set; }

    [MaxLength(100, ErrorMessage = "O modelo do veículo não pode exceder 100 caracteres.")]
    [Display(Name = "Nome do modelo do veículo")]
    public string? Model { get; set; }

    [Display(Name = "Ano de fabricação do veículo")]
    public int? ManufacturingYear { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo.")]
    [Display(Name = "Preço do veículo")]
    public decimal Price { get; set; }

    public int ManufacturerId { get; set; }

    public string? ManufacturerName { get; set; }

    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Display(Name = "Tipo de Veículo")]
    public VehiclesTypes VehicleType { get; set; }
}