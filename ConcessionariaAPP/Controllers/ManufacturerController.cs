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
        
        if (!ModelState.IsValid)
            return View("Index",model);
        if (model.FundationYear > DateTime.Now.Year)
        {
            ModelState.AddModelError(nameof(model.FundationYear), "O ano de fundação não pode ser maior que o ano atual.");
            return View("Index",model);
        }
        
        try
        {
            var dto = _mapper.Map<ManufacturerDto>(model);
            await _ManufacturerService.CreateAsync(dto);
            TempData["SuccessMessage"] = "Cadastro realizado com sucesso!";
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            var messages = ex.Message.Split(';');
            foreach (var msg in messages)
            {
                ModelState.AddModelError(string.Empty, msg.Trim());
            }
            return View("Index",model);
        }
    }
}