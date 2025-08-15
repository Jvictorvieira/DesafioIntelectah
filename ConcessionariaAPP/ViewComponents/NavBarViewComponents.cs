using Microsoft.AspNetCore.Identity;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Enum;
using ConcessionariaAPP.Models.UserViewModel;
using Microsoft.AspNetCore.Mvc;

public class NavbarUserViewComponent : ViewComponent
{
    private readonly UserManager<Users> _userManager;

    public NavbarUserViewComponent(UserManager<Users> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var userViewModel = new LoggedUserViewModel()
        {
            UserName = user?.Name ?? "Visitante",
            AccessLevel = user?.AccessLevel ?? AccessLevel.Seller
        };
        return View(userViewModel);
    }
}