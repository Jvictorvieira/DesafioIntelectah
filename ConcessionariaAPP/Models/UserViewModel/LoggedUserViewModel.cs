using ConcessionariaAPP.Domain.Enum;

namespace ConcessionariaAPP.Models.UserViewModel;

public class LoggedUserViewModel
{
    public string? UserName { get; set; }
    public AccessLevel AccessLevel { get; set; }
}
