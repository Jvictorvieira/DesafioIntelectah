namespace ConcessionariaAPP.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Manufacturers : BaseEntity
{
    [Key]
    public int ManufacturerId { get; set; }

    [MaxLength(100, ErrorMessage = "O nome do fabricante não pode exceder 100 caracteres.")]
    [Display(Name = "Nome do Fabricante")]
    public string Name { get; set; }

    [MaxLength(50, ErrorMessage = "O país de origem não pode exceder 50 caracteres.")]
    [Display(Name = "País de Origem")]
    public string Country { get; set; }

    [Display(Name = "Ano de Fundação")]
    public int FundationYear { get; set; }

    [MaxLength(255, ErrorMessage = "O site não pode exceder 255 caracteres.")]
    public string? WebSite { get; set; }

    public List<Vehicles> Vehicles { get; set; } = [];

    public Manufacturers(int manufacturerId, string name, string country, int fundationYear, string? webSite = null)
    {
        ManufacturerId = manufacturerId;
        Name = name;
        Country = country;
        FundationYear = fundationYear;
        WebSite = webSite;
    }
}