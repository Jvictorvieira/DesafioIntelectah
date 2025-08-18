using System.ComponentModel.DataAnnotations;

namespace ConcessionariaAPP.Domain.Enum;

public enum VehiclesTypes
{

    [DisplayAttribute(Name = "Carro")]
    Car = 1,
    [DisplayAttribute(Name = "Moto")]
    Motorcycle,
    [DisplayAttribute(Name = "Caminhão")]
    Truck,
    [DisplayAttribute(Name = "Ônibus")]
    Bus,
    [DisplayAttribute(Name = "Avião")]
    Airplane,
    [DisplayAttribute(Name = "Navio")]
    Ship,
    [DisplayAttribute(Name = "Bicicleta")]
    Bicycle,
}