namespace ConcessionariaAPP.Domain.Entities;

using ConcessionariaAPP.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Vehicles : BaseEntity
{
    [Key]
    public int VehicleId { get; set; }

    [MaxLength(100, ErrorMessage = "O modelo do veículo não pode exceder 100 caracteres.")]
    [Display(Name = "Nome do modelo do veículo")]
    public string Model { get; set; }

    [Display(Name = "Ano de fabricação do veículo")]
    public int ManufacturingYear { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo.")]
    [Display(Name = "Preço do veículo")]
    public decimal Price { get; set; }

    [ForeignKey("Manufacturers")]
    public int ManufacturerId { get; set; }

    [Display(Name = "Fabricantes")]
    public List<Manufacturers> Manufacturers { get; set; } = [];

    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Display(Name = "Tipo de Veículo")]
    public VehiclesTypes VehicleType { get; set; }

    public List<Sales> Sales { get; set; } = [];

    public Vehicles(int vehicleId, string model, int manufacturingYear, decimal price, int manufacturerId, string? description, VehiclesTypes vehicleType)
    {
        VehicleId = vehicleId;
        Model = model;
        ManufacturingYear = manufacturingYear;
        Price = price;
        ManufacturerId = manufacturerId;
        Description = description;
        VehicleType = vehicleType;
    }

}
