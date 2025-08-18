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
        var viewModel = new HomeFilterViewModel();
        await LoadDashboardData(viewModel);
        await LoadDependencies(viewModel);
        return View(viewModel);
    }
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Filter(HomeFilterViewModel viewModel)
    {
        var filteredViewModel = await LoadDashboardData(viewModel);

        return PartialView("_Dashboard", filteredViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<HomeFilterViewModel> LoadDashboardData(HomeFilterViewModel viewModel)
    {
        var salesByCarDealership = await _saleService.GetSalesByCarDealershipAsync(viewModel.CarDealershipId);
        var salesByVehicleType = await _saleService.GetSalesByVehicleTypeAsync((VehiclesTypes)viewModel.VehicleTypeId);
        var salesByClient = await _saleService.GetSalesByClientAsync(viewModel.ClientId);
        var salesByManufacturer = await _saleService.GetSalesByManufacturerAsync(viewModel.ManufacturerId);
        var salesByMonth = await _saleService.GetSalesByMonthAsync(DateTime.Now.Year);

        // Map the data to the view model
        viewModel.SalesPerCarDealership = salesByCarDealership;
        viewModel.SalesPerVehicleType = salesByVehicleType;
        viewModel.SalesPerClient = salesByClient;
        viewModel.SalesPerManufacturer = salesByManufacturer;
        viewModel.SalesPerMonth = salesByMonth;

        return viewModel;
    }

    private async Task LoadDependencies(HomeFilterViewModel viewModel)
    {
        var clients = await _clientService.GetAllAsync();
        ViewBag.Clients = new SelectList(clients, "ClientId", "Name", viewModel.ClientId);

        ViewBag.VehicleTypes = new SelectList(Enum.GetValues(typeof(VehiclesTypes)).Cast<VehiclesTypes>().Select(v => new
        {
            Value = v,
            Text = GetDisplayName(v)
        }), "Value", "Text", viewModel.VehicleTypeId);

        var carDealerships = await _carDealershipService.GetAllAsync();
        ViewBag.CarDealerships = new SelectList(carDealerships, "CarDealershipId", "Name", viewModel.CarDealershipId);

        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = new SelectList(manufacturers, "ManufacturerId", "Name", viewModel.ManufacturerId);
    }

    
}
