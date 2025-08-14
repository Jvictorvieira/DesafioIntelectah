namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class Sales : BaseEntity
{
    [Key]
    public int SaleId { get; set; }

    [ForeignKey("Veiculos")]
    public int VehicleId { get; set; }

    public Vehicles Vehicle { get; set; } = null!;

    [ForeignKey("Clientes")]
    public int ClientId { get; set; }

    public Clients Client { get; set; } = null!;

    [ForeignKey("CarDealership")]
    public int CarDealershipId { get; set; } 

    public CarDealership CarDealership { get; set; } = null!;


    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Preço de Venda")]
    public decimal SalePrice { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data da Venda")]
    public DateTime SaleDate { get; set; }

    [Display(Name = "Protocolo de Venda")]
    [MaxLength(20)]
    public string SaleProtocol { get; set; }

    public Sales(int saleId, int vehicleId, int clientId, int carDealershipId, decimal salePrice, DateTime saleDate, string saleProtocol)
    {
        SaleId = saleId;
        VehicleId = vehicleId;
        ClientId = clientId;
        CarDealershipId = carDealershipId;

        SalePrice = salePrice;
        SaleDate = saleDate;
        SaleProtocol = saleProtocol;
    }
}