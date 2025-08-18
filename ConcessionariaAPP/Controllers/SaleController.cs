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
using Microsoft.AspNetCore.Mvc.Rendering;
using ConcessionariaAPP.Application.Excepetions;

namespace ConcessionariaAPP.Controllers;


[Authorize(Roles = "Admin, Manager, Seller")]
public class SaleController(ISaleService SaleService,
                            IClientService ClientService,
                            IVehicleService VehicleService,
                            ICarDealershipService CarDealershipService,
                            IManufacturerService ManufacturerService,
                            IMapper Mapper) : BaseController
{

    private readonly ISaleService _saleService = SaleService;
    private readonly IClientService _clientService = ClientService;
    private readonly IVehicleService _vehicleService = VehicleService;
    private readonly ICarDealershipService _carDealershipService = CarDealershipService;
    private readonly IManufacturerService _manufacturerService = ManufacturerService;
    private readonly IMapper _mapper = Mapper;

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = new SaleTableViewModel();
        var Sales = await _saleService.GetAllAsync();
        model.Rows = _mapper.Map<List<SaleViewModel>>(Sales);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SaleViewModel model)
    {
        ViewData["Action"] = "Create";

        if (!ModelState.IsValid)
        {
            await LoadSelectsAsync(model.ClientId, model.VehicleId, model.CarDealershipId);
            return PartialView("_Form", model);
        }

        try
        {
            var dto = _mapper.Map<SaleDto>(model);
            await _saleService.CreateAsync(dto);
            return Json(new { success = true, message = "Cadastro realizado com sucesso!", url = Url.Action("GetTableData", "Sale") });
        }
        catch (AppValidationException ex)
        {
            HandleException(ex);
            await LoadSelectsAsync(model.ClientId, model.VehicleId, model.CarDealershipId);
            return PartialView("_Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        ViewData["Action"] = "Create";
        await LoadSelectsAsync();
        return PartialView("_Form", new SaleViewModel());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Action"] = "Edit";
        var Sale = await _saleService.GetByIdAsync(id);
        if (Sale == null)
        {
            return NotFound();
        }
        await LoadSelectsAsync(Sale.ClientId, Sale.VehicleId, Sale.CarDealershipId);
        var model = _mapper.Map<SaleViewModel>(Sale);
        return PartialView("_Form", model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(SaleViewModel model)
    {
        ViewData["Action"] = "Edit";

        if (!ModelState.IsValid)
        {
            await LoadSelectsAsync(model.ClientId, model.VehicleId, model.CarDealershipId);
            return PartialView("_Form", model);
        }
        try
        {
            var dto = _mapper.Map<SaleDto>(model);
            await _saleService.UpdateAsync(dto);
            return Json(new { success = true, message = "Cadastro atualizado com sucesso!", url = Url.Action("GetTableData", "Sale") });
        }
        catch (AppValidationException ex)
        {
            HandleException(ex);
            await LoadSelectsAsync(model.ClientId, model.VehicleId, model.CarDealershipId);
            return PartialView("_Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var Sale = await _saleService.GetByIdAsync(id);
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
            await _saleService.DeleteAsync(id);
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
        var Sales = await _saleService.GetAllAsync();
        if (Sales != null)
        {
            model.Rows = _mapper.Map<List<SaleViewModel>>(Sales);
        }
        return PartialView("_Table", model);
    }

    private async Task LoadSelectsAsync(int? clientId = null, int? vehicleId = null, int? carDealershipId = null)
    {

        var clients = await _clientService.GetAllAsync();
        ViewBag.Clients = new SelectList(clients, "ClientId", "Name", clientId);

        var vehicles = await _vehicleService.GetAllAsync();

        // Cria lista de veículos agrupados
        var vehicleSelectList = vehicles.Select(v => new SelectListItem
        {
            Value = v.VehicleId.ToString(),
            Text = v.Model, // ou v.Model, conforme seu ViewModel
            Group = new SelectListGroup { Name = v.ManufacturerName ?? "Desconhecido" },
            Selected = vehicleId.HasValue && v.VehicleId == vehicleId.Value
        }).ToList();

        ViewBag.Vehicles = vehicleSelectList;

        var carDealerships = await _carDealershipService.GetAllAsync();
        ViewBag.CarDealerships = new SelectList(carDealerships, "CarDealershipId", "Name", carDealershipId);

    }
    
    
}