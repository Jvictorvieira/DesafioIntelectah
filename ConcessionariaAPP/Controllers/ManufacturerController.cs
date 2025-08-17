using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Models;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Models.ManufacturerViewModel;
using AutoMapper;
using ConcessionariaAPP.Application.Dto;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace ConcessionariaAPP.Controllers;


[Authorize(Roles = "Admin")]
public class ManufacturerController(IManufacturerService ManufacturerService, IMapper mapper) : Controller
{

    private readonly IManufacturerService _ManufacturerService = ManufacturerService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = new ManufacturerTableViewModel();
        var Manufacturers = await _ManufacturerService.GetAllAsync();
        model.Rows = _mapper.Map<List<ManufacturerViewModel>>(Manufacturers);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ManufacturerViewModel model)
    {

        if (!ModelState.IsValid || model.FundationYear > DateTime.Now.Year)
        {
            if (model.FundationYear > DateTime.Now.Year)
                ModelState.AddModelError(nameof(model.FundationYear), "O ano de fundação não pode ser maior que o ano atual.");

            if (!ModelState.IsValid)
                return PartialView("_Form", model);
        }

        try
        {
            var dto = _mapper.Map<ManufacturerDto>(model);
            await _ManufacturerService.CreateAsync(dto);
            return Json(new { success = true, message = "Cadastro realizado com sucesso!", url = Url.Action("GetTableData", "Manufacturer") });
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
        return PartialView("_Form", new ManufacturerViewModel());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var manufacturer = await _ManufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        ViewData["Action"] = "Edit";
        var model = _mapper.Map<ManufacturerViewModel>(manufacturer);
        return PartialView("_Form", model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ManufacturerViewModel model)
    {
        if (!ModelState.IsValid || model.FundationYear > DateTime.Now.Year)
        {
            if (model.FundationYear > DateTime.Now.Year)
                ModelState.AddModelError(nameof(model.FundationYear), "O ano de fundação não pode ser maior que o ano atual.");

            if (!ModelState.IsValid)
                ViewData["Action"] = "Edit";
            return PartialView("_Form", model);
        }

        try
        {
            var dto = _mapper.Map<ManufacturerDto>(model);
            await _ManufacturerService.UpdateAsync(dto);
            return Json(new { success = true, message = "Cadastro atualizado com sucesso!", url = Url.Action("GetTableData", "Manufacturer") });
        }
        catch (InvalidOperationException ex)
        {
            ViewData["Action"] = "Edit";
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            return PartialView("_Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var manufacturer = await _ManufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }

        var model = _mapper.Map<ManufacturerViewModel>(manufacturer);
        return PartialView("_FormDelete", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _ManufacturerService.DeleteAsync(id);
            return Json(new { success = true, message = "Cadastro excluído com sucesso!", url = Url.Action("GetTableData", "Manufacturer") });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            return PartialView("_FormDelete", new ManufacturerViewModel { ManufacturerId = id });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetTableData()
    {
        var model = new ManufacturerTableViewModel();
        var manufacturers = await _ManufacturerService.GetAllAsync();
        if (manufacturers != null)
        {
            model.Rows = _mapper.Map<List<ManufacturerViewModel>>(manufacturers);
        }
        return PartialView("_Table", model);
    }
}