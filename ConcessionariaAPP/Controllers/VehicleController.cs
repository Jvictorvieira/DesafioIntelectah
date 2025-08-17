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
using ConcessionariaAPP.Domain.Enum;

namespace ConcessionariaAPP.Controllers;


[Authorize(Roles = "Admin, Manager")]
public class VehicleController(IVehicleService vehicleService, IManufacturerService manufacturerService, IMapper mapper) : Controller
{

    private readonly IVehicleService _vehicleService = vehicleService;
    private readonly IManufacturerService _manufacturerService = manufacturerService;
    private readonly IMapper _mapper = mapper;
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = new VehicleTableViewModel();
        await LoadTable(model);

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        ViewData["Action"] = "Create";
        var model = new VehicleViewModel();
        await LoadSelects();
        return PartialView("_Form", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            if (viewModel.ManufacturingYear > DateTime.Now.Year)
            {
                ModelState.AddModelError(nameof(viewModel.ManufacturingYear), "O ano de fabricação não pode ser maior que o ano atual.");
            }
            await LoadSelects();
            return PartialView("_Form", viewModel);
        }
        try
        {
            var dto = _mapper.Map<VehicleDto>(viewModel);
            await _vehicleService.CreateAsync(dto);
            return Json(new
            {
                success = true,
                message = "Cadastro realizado com sucesso!",
                url = Url.Action("GetTableData", "Vehicle")
            });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            await LoadSelects();
            return PartialView("_Form", viewModel);
        }
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Action"] = "Edit";
        var entity = await _vehicleService.GetByIdAsync(id);
        await LoadSelects(entity?.ManufacturerId, (int?)entity?.VehicleType);
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
        ViewData["Action"] = "Edit";
        if (!ModelState.IsValid)
        {
            if (viewModel.ManufacturingYear > DateTime.Now.Year)
            {
                ModelState.AddModelError(nameof(viewModel.ManufacturingYear), "O ano de fabricação não pode ser maior que o ano atual.");
            }
            await LoadSelects(viewModel.ManufacturerId, (int?)viewModel.VehicleType);
            return PartialView("_Form", viewModel);
        }

        try
        {
            var dto = _mapper.Map<VehicleDto>(viewModel);
            await _vehicleService.UpdateAsync(dto);
            return Json(new { success = true, message = "Atualização realizada com sucesso!", url = Url.Action("GetTableData", "Vehicle") });
        }
        catch (InvalidOperationException ex)
        {
            foreach (var msg in ex.Message.Split(';'))
                ModelState.AddModelError(string.Empty, msg.Trim());
            await LoadSelects(viewModel.ManufacturerId, (int?)viewModel.VehicleType);
            return PartialView("_Form", viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTableData()
    {
        var model = new VehicleTableViewModel();
        await LoadTable(model);
        return PartialView("_Table", model);
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
            return Json(new
            {
                success = true,
                message = "Veículo excluído com sucesso!",
                url = Url.Action("GetTableData", "Vehicle")
            });
        }
        return Json(new { success = false, message = "Erro ao excluir veículo." });
    }

    private async Task LoadSelects(int? manufacturerId = null, int? vehicleTypeId = null)
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = new SelectList(manufacturers, "ManufacturerId", "Name", manufacturerId);
        ViewBag.VehicleTypes = new SelectList(Enum.GetValues(typeof(VehiclesTypes)).Cast<VehiclesTypes>().Select(v => new
        {
            Value = v,
            Text = GetDisplayName(v)
        }), "Value", "Text", vehicleTypeId);
    }

    public string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                        .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                        .FirstOrDefault();
        return attr?.Name ?? value.ToString();
    }

    private async Task LoadTable(VehicleTableViewModel model)
    {
        var vehicles = await _vehicleService.GetAllAsync();

        var rows = _mapper.Map<List<VehicleViewModel>>(vehicles);
        foreach (var row in rows)
        {
            row.VehicleTypeName = GetDisplayName(row.VehicleType);
        }
        model.Rows = rows;
    }
}
