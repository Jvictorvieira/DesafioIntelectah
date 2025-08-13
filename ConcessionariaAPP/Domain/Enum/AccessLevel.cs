using System.ComponentModel.DataAnnotations;

namespace ConcessionariaAPP.Domain.Enum;

public enum AccessLevel
{
    [DisplayAttribute(Name = "Administrador")]
    Admin,
    [DisplayAttribute(Name = "Gerente")]
    Manager,
    [DisplayAttribute(Name = "Vendedor")]
    Seller,
    [DisplayAttribute(Name = "Cliente")]
    Client // Cliente?
}