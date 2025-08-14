using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Models.UserViewModel;
using Microsoft.AspNetCore.Authorization;

namespace ConcessionariaAPP.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;

    public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View(model);
        }
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

}