namespace ConcessionariaAPP.Models;

public class BaseViewModel
{
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}