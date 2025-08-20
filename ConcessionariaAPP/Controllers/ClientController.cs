using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Models;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Models.ClientViewModel;
using AutoMapper;
using ConcessionariaAPP.Application.Dto;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ConcessionariaAPP.Application.Excepetions;

namespace ConcessionariaAPP.Controllers;


[Authorize(Roles = "Admin, Manager")]
public class ClientController(IClientService ClientService, IMapper mapper) : BaseController
{

    private readonly IClientService _ClientService = ClientService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = new ClientTableViewModel();
        var Clients = await _ClientService.GetAll();
        model.Rows = _mapper.Map<List<ClientViewModel>>(Clients);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClientViewModel model)
    {
        ViewData["Action"] = "Create";
        
            if (!ModelState.IsValid)
                return PartialView("_Form", model);
        

        try
        {
            var dto = _mapper.Map<ClientDto>(model);
            await _ClientService.CreateAsync(dto);
            return Json(new { success = true, message = "Cadastro realizado com sucesso!", url = Url.Action("GetTableData", "Client") });
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
        return PartialView("_Form", new ClientViewModel());
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Action"] = "Edit";
        var Client = await _ClientService.GetByIdAsync(id);
        if (Client == null)
        {
            return NotFound();
        }
        var model = _mapper.Map<ClientViewModel>(Client);
        return PartialView("_Form", model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ClientViewModel model)
    {
        ViewData["Action"] = "Edit";
        
            if (!ModelState.IsValid)
                
            return PartialView("_Form", model);
        

        try
        {
            var dto = _mapper.Map<ClientDto>(model);
            await _ClientService.UpdateAsync(dto);
            return Json(new { success = true, message = "Cadastro atualizado com sucesso!", url = Url.Action("GetTableData", "Client") });
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
        var Client = await _ClientService.GetByIdAsync(id);
        if (Client == null)
        {
            return NotFound();
        }

        var model = _mapper.Map<ClientViewModel>(Client);
        return PartialView("_FormDelete", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _ClientService.DeleteAsync(id);
            return Json(new { success = true, message = "Cadastro excluído com sucesso!", url = Url.Action("GetTableData", "Client") });
        }
        catch (AppValidationException ex)
        {
            HandleException(ex);
            return PartialView("_FormDelete", new ClientViewModel { ClientId = id });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetTableData()
    {
        var model = new ClientTableViewModel();
        var Clients = await _ClientService.GetAll();
        if (Clients != null)
        {
            model.Rows = _mapper.Map<List<ClientViewModel>>(Clients);
        }
        return PartialView("_Table", model);
    }
}