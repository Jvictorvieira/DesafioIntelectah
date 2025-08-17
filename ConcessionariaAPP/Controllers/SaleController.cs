using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Models;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Models.SaleViewModel;
using AutoMapper;
using ConcessionariaAPP.Application.Dto;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace ConcessionariaAPP.Controllers;


[Authorize(Roles = "Admin, Manager, Seller")]
public class SaleController(ISaleService SaleService, IMapper mapper) : Controller
{

    private readonly ISaleService _SaleService = SaleService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = new SaleTableViewModel();
        var Sales = await _SaleService.GetAllAsync();
        model.Rows = _mapper.Map<List<SaleViewModel>>(Sales);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SaleViewModel model)
    {
        ViewData["Action"] = "Create";
        
            if (!ModelState.IsValid)
                return PartialView("_Form", model);
        

        try
        {
            var dto = _mapper.Map<SaleDto>(model);
            await _SaleService.CreateAsync(dto);
            return Json(new { success = true, message = "Cadastro realizado com sucesso!", url = Url.Action("GetTableData", "Sale") });
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
        return PartialView("_Form", new SaleViewModel());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Action"] = "Edit";
        var Sale = await _SaleService.GetByIdAsync(id);
        if (Sale == null)
        {
            return NotFound();
        }
        var model = _mapper.Map<SaleViewModel>(Sale);
        return PartialView("_Form", model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(SaleViewModel model)
    {
        ViewData["Action"] = "Edit";
        
            if (!ModelState.IsValid)
            {
                return PartialView("_Form", model);
            }
        

        try
        {
            var dto = _mapper.Map<SaleDto>(model);
            await _SaleService.UpdateAsync(dto);
            return Json(new { success = true, message = "Cadastro atualizado com sucesso!", url = Url.Action("GetTableData", "Sale") });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            return PartialView("_Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var Sale = await _SaleService.GetByIdAsync(id);
        if (Sale == null)
        {
            return NotFound();
        }

        var model = _mapper.Map<SaleViewModel>(Sale);
        return PartialView("_FormDelete", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _SaleService.DeleteAsync(id);
            return Json(new { success = true, message = "Cadastro excluído com sucesso!", url = Url.Action("GetTableData", "Sale") });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            return PartialView("_FormDelete", new SaleViewModel { SaleId = id });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetTableData()
    {
        var model = new SaleTableViewModel();
        var Sales = await _SaleService.GetAllAsync();
        if (Sales != null)
        {
            model.Rows = _mapper.Map<List<SaleViewModel>>(Sales);
        }
        return PartialView("_Table", model);
    }
}