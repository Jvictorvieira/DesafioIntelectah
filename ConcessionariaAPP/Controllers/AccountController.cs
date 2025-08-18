using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Models.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConcessionariaAPP.Domain.Enum;

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
        LoadSelects();
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        LoadSelects();
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = new Users
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name,
            AccessLevel = model.AccessLevel
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, GetRoles(user.AccessLevel)); // Default role for new users
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

    private void LoadSelects()
    {
        ViewBag.AccessLevels = new SelectList(Enum.GetValues(typeof(AccessLevel)).Cast<AccessLevel>().Select(v => new
        {
            Value = v,
            Text = GetDisplayName(v)
        }), "Value", "Text");
    }

    public string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                        .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                        .FirstOrDefault();
        return attr?.Name ?? value.ToString();
    }

    private static string GetRoles(AccessLevel accessLevel)
    {
        switch (accessLevel)
        {
            case AccessLevel.Admin:
                return Roles.Admin;
            case AccessLevel.Manager:
                return Roles.Manager;
            default:
                return Roles.Seller;
        }
    }



    

}