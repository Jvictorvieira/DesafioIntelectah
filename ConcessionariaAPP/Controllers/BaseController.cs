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
        foreach (var (field, messages) in ex.Errors)
        {
            var key = field;
            foreach (var msg in messages)
                this.ModelState.AddModelError(key, msg);
        }
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