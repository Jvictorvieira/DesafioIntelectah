using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Models.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Infrastructure;

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

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
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

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = new Users
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Seller); // Default role for new users
            if (roleResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in roleResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

}