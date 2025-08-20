using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Models.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Infrastructure;
using ConcessionariaAPP.Application.Excepetions;

namespace ConcessionariaAPP.Controllers;

public class BaseController : Controller
{
    protected void HandleException(AppValidationException ex)
    {
        var dataField = ex.Data["Field"] as string;
        var errorMessage = ex.Data["ErrorMessage"] as string;
        this.ModelState.AddModelError(dataField ?? string.Empty, errorMessage ?? ex.Message);
    }
    
    protected string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                        .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                        .FirstOrDefault();
        return attr?.Name ?? value.ToString();
    }
}