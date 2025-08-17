using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Models;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ConcessionariaAPP.Models.VehicleViewModel;
using ConcessionariaAPP.Application.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConcessionariaAPP.Controllers;


[Authorize(Roles = "Admin, Seller")]
public class VehicleController(IVehicleService vehicleService, IManufacturerService manufacturerService, IMapper mapper) : Controller
{

    private readonly IVehicleService _vehicleService = vehicleService;
    private readonly IManufacturerService _manufacturerService = manufacturerService;
    private readonly IMapper _mapper = mapper;
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = new VehicleTableViewModel();
        var Vehicle = await _vehicleService.GetAllAsync();

        model.Rows = _mapper.Map<List<VehicleViewModel>>(Vehicle);

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new VehicleViewModel();
        return PartialView("_Form", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_Form", viewModel);
        }
        try
        {
            var dto = _mapper.Map<VehicleDto>(viewModel);
            await _vehicleService.CreateAsync(dto);
            return Json(new { success = true, message = "Cadastro realizado com sucesso!", url = Url.Action("GetTableData", "Manufacturer") });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            return PartialView("_Form", viewModel);
        }
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _vehicleService.GetByIdAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        var model = _mapper.Map<VehicleViewModel>(entity);
        return PartialView("_Form", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(VehicleViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_Form", viewModel);
        }

        try
        {
            var dto = _mapper.Map<VehicleDto>(viewModel);
            await _vehicleService.UpdateAsync(dto);
            return Json(new { success = true, message = "Atualização realizada com sucesso!", url = Url.Action("GetTableData", "Manufacturer") });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            return PartialView("_Form", viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTableData()
    {
        var vehicles = await _vehicleService.GetAllAsync();
        var viewModels = _mapper.Map<List<VehicleViewModel>>(vehicles);
        return PartialView("_Table", viewModels);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle == null)
        {
            return NotFound();
        }

        var model = _mapper.Map<VehicleViewModel>(vehicle);
        return PartialView("_FormDelete", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _vehicleService.DeleteAsync(id);
        if (result)
        {
            return Json(new { success = true, message = "Veículo excluído com sucesso!" });
        }
        return Json(new { success = false, message = "Erro ao excluir veículo." });
    }

    private async Task LoadSelects()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = new SelectList(manufacturers, "Id", "Name");
    }
}