using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConcessionariaAPP.Models;
using Microsoft.AspNetCore.Authorization;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Models.HomeViewModel;
using ConcessionariaAPP.Domain.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConcessionariaAPP.Controllers;

[Authorize(Roles = "Admin, Manager, Seller")]
public class HomeController(
    ILogger<HomeController> logger,
    ISaleService saleService,
    IClientService clientService,
    IVehicleService vehicleService,
    ICarDealershipService carDealershipService,
    IManufacturerService manufacturerService) : BaseController
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly ISaleService _saleService = saleService;
    private readonly IClientService _clientService = clientService;
    private readonly IVehicleService _vehicleService = vehicleService;
    private readonly ICarDealershipService _carDealershipService = carDealershipService;
    private readonly IManufacturerService _manufacturerService = manufacturerService;
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel();
        LoadDashboardData(viewModel);
        await LoadDependencies((HomeFilterViewModel)viewModel.filter);
        return View(viewModel);
    }
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Filter(HomeViewModel viewModel)
    {
        var filteredViewModel = LoadDashboardData(viewModel);

        return PartialView("_Dashboard", filteredViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private  HomeViewModel LoadDashboardData(HomeViewModel viewModel)
    {
        var homeViewModel = new HomeViewModel();

        homeViewModel = _saleService.LoadCharts(homeViewModel);

        return homeViewModel;
    }

    private async Task LoadDependencies(HomeFilterViewModel viewModel)
    {
        var clients = await _clientService.GetAll();
        ViewBag.Clients = new SelectList(clients, "ClientId", "Name", viewModel.ClientId);

        ViewBag.VehicleTypes = new SelectList(Enum.GetValues(typeof(VehiclesTypes)).Cast<VehiclesTypes>().Select(v => new
        {
            Value = v,
            Text = GetDisplayName(v)
        }), "Value", "Text", viewModel.VehicleTypeId);

        var carDealerships = await _carDealershipService.GetAll();
        ViewBag.CarDealerships = new SelectList(carDealerships, "CarDealershipId", "Name", viewModel.CarDealershipId);

        var manufacturers = await _manufacturerService.GetAll();
        ViewBag.Manufacturers = new SelectList(manufacturers, "ManufacturerId", "Name", viewModel.ManufacturerId);
    }

    
}
