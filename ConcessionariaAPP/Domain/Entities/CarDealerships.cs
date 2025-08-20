namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class CarDealership : BaseEntity
{
    [Key]
    public int CarDealershipId { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(255)]
    public required string Address { get; set; }

    [MaxLength(50)]
    public required string City { get; set; }

    [MaxLength(50)]
    [Display(Name = "Estado")]
    public required string State { get; set; }

    [MaxLength(10)]
    public required string AddressCode { get; set; }

    [MaxLength(15)]
    public required string Phone { get; set; }

    [MaxLength(100)]
    public required string Email { get; set; }

    public int MaxVehicleCapacity { get; set; }

    public List<Sales> Sales { get; set; } = [];

    public CarDealership(int carDealershipId, string name, string address, string city, string state, string addressCode, string phone, string email, int maxVehicleCapacity)
    {
        CarDealershipId = carDealershipId;
        Name = name;
        Address = address;
        City = city;
        State = state;
        AddressCode = addressCode;
        Phone = phone;
        Email = email;
        MaxVehicleCapacity = maxVehicleCapacity;
    }

    public CarDealership() { }
}