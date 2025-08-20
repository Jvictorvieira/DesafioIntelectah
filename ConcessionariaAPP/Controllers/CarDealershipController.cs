using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Models;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Models.CarDealershipViewModel;
using AutoMapper;
using ConcessionariaAPP.Application.Dto;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ConcessionariaAPP.Application.Excepetions;

namespace ConcessionariaAPP.Controllers;


[Authorize(Roles = "Admin")]
public class CarDealershipController(ICarDealershipService CarDealershipService, IMapper mapper) : BaseController
{

    private readonly ICarDealershipService _CarDealershipService = CarDealershipService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = new CarDealershipTableViewModel();
        var CarDealerships = await _CarDealershipService.GetAll();
        model.Rows = _mapper.Map<List<CarDealershipViewModel>>(CarDealerships);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CarDealershipViewModel model)
    {
        ViewData["Action"] = "Create";
        
            if (!ModelState.IsValid)
                return PartialView("_Form", model);
        

        try
        {
            var dto = _mapper.Map<CarDealershipDto>(model);
            await _CarDealershipService.CreateAsync(dto);
            return Json(new { success = true, message = "Cadastro realizado com sucesso!", url = Url.Action("GetTableData", "CarDealership") });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            return PartialView("_Form", model);
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Action"] = "Create";
        return PartialView("_Form", new CarDealershipViewModel());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Action"] = "Edit";
        var CarDealership = await _CarDealershipService.GetByIdAsync(id);
        if (CarDealership == null)
        {
            return NotFound();
        }
        var model = _mapper.Map<CarDealershipViewModel>(CarDealership);
        return PartialView("_Form", model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(CarDealershipViewModel model)
    {
        ViewData["Action"] = "Edit";
        
            if (!ModelState.IsValid)
                
            return PartialView("_Form", model);
        

        try
        {
            var dto = _mapper.Map<CarDealershipDto>(model);
            await _CarDealershipService.UpdateAsync(dto);
            return Json(new { success = true, message = "Cadastro atualizado com sucesso!", url = Url.Action("GetTableData", "CarDealership") });
        }
        catch (AppValidationException ex)
        {
            HandleException(ex);
            return PartialView("_Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var CarDealership = await _CarDealershipService.GetByIdAsync(id);
        if (CarDealership == null)
        {
            return NotFound();
        }

        var model = _mapper.Map<CarDealershipViewModel>(CarDealership);
        return PartialView("_FormDelete", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _CarDealershipService.DeleteAsync(id);
            return Json(new { success = true, message = "Cadastro excluído com sucesso!", url = Url.Action("GetTableData", "CarDealership") });
        }
        catch (AppValidationException ex)
        {
            HandleException(ex);
            return PartialView("_FormDelete", new CarDealershipViewModel { CarDealershipId = id });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetTableData()
    {
        var model = new CarDealershipTableViewModel();
        var CarDealerships = await _CarDealershipService.GetAll();
        if (CarDealerships != null)
        {
            model.Rows = _mapper.Map<List<CarDealershipViewModel>>(CarDealerships);
        }
        return PartialView("_Table", model);
    }
}