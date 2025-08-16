namespace ConcessionariaAPP.Application.Dto;

public class BaseDto
{
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}