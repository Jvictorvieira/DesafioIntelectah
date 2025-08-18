namespace ConcessionariaAPP.Models.HomeViewModel;

public class ChartViewModel
{
    public Dictionary<string, decimal> Data { get; set; } = new();
    
    public List<string> Labels { get; set; } = new();
}