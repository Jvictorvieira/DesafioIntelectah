namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Application.Excepetions;
using AutoMapper;
using ConcessionariaAPP.Models.HomeViewModel;
using ConcessionariaAPP.Domain.Enum;
using ConcessionariaAPP.Domain.Repository;
using Microsoft.EntityFrameworkCore;

public class SaleAppService(ISaleRepository SaleRepository, IVehicleRepository VehicleRepository, IMapper mapper) : ISaleService
{
    private readonly ISaleRepository _SaleRepository = SaleRepository;
    private readonly IVehicleRepository _VehicleRepository = VehicleRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<SaleDto> CreateAsync(SaleDto dto)
    {
        Validate(dto, isUpdate: false);
        var entity = _mapper.Map<Sales>(dto);

        entity.IsDeleted = false;

        entity.SaleProtocol = GenerateProtocol();

        await CheckVehiclePriceAsync(entity.VehicleId, entity.SalePrice);

        var created = await _SaleRepository.Create(entity);
        return _mapper.Map<SaleDto>(created);
    }

    public async Task<SaleDto> UpdateAsync(SaleDto dto)
    {
        Validate(dto, isUpdate: true);

        var entity = _mapper.Map<Sales>(dto);
        var updated = await _SaleRepository.Update(entity);

        await CheckVehiclePriceAsync(entity.VehicleId, entity.SalePrice);

        return _mapper.Map<SaleDto>(updated);
    }

    public async Task<SaleDto> GetByIdAsync(int id)
    {
        var entity = await _SaleRepository.GetById(id);
        return _mapper.Map<SaleDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new AppValidationException(nameof(SaleDto.SaleId), "Id inválido para exclusão.");
        }
        return await _SaleRepository.Delete(id);
    }

    public async Task<IEnumerable<SaleDto>> GetAll()
    {
        var list = await _SaleRepository.GetAll().ToListAsync();
        return [.. list.Select(e => _mapper.Map<SaleDto>(e))];
    }

    private static void Validate(SaleDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new AppValidationException(nameof(dto), "Objeto não pode ser nulo.");
        }
        if (isUpdate && dto.SaleId <= 0)
        {
            throw new AppValidationException(nameof(dto.SaleId), "Id inválido para atualização.");
        }

    }

    private static string GenerateProtocol()
    {
        var random = new Random();
        return new string([.. Enumerable.Repeat("0123456789", 20).Select(s => s[random.Next(s.Length)])]);
    }

    public HomeViewModel LoadCharts(HomeViewModel viewModel)
    {

        var allSales = _SaleRepository.GetAll();

        var filter = viewModel.filter;

        if (filter.CarDealershipId != 0)
        {
            allSales = allSales.Where(s => s.CarDealershipId == filter.CarDealershipId);
        }
        if (filter.VehicleTypeId != 0)
        {
            allSales = allSales.Where(s => s.Vehicle.VehicleType == (VehiclesTypes)filter.VehicleTypeId);
        }
        if (filter.ManufacturerId != 0)
        {
            allSales = allSales.Where(s => s.Vehicle.ManufacturerId == filter.ManufacturerId);
        }
        if (filter.ClientId != 0)
        {
            allSales = allSales.Where(s => s.ClientId == filter.ClientId);
        }
        if (filter.StartDate.HasValue)
        {
            allSales = allSales.Where(s => s.SaleDate >= filter.StartDate.Value);
        }
        if (filter.EndDate.HasValue)
        {
            allSales = allSales.Where(s => s.SaleDate <= filter.EndDate.Value);
        }

        viewModel.SalesPerCarDealership = GetSalesByCarDealership(allSales);
        viewModel.SalesPerVehicleType = GetSalesByVehicleType(allSales);
        viewModel.SalesPerClient = GetSalesByClient(allSales);
        viewModel.SalesPerManufacturer = GetSalesByManufacturer(allSales);
        viewModel.SalesPerMonth = GetSalesByMonth(allSales);

        return viewModel;
    }

    public SalesPerVehicleTypeViewModel GetSalesByVehicleType(IQueryable<Sales> allSales)
    {

        var data = allSales.Select(s => new { s.SalePrice, VehicleType = s.Vehicle.VehicleType.ToString() })
                            .GroupBy(x => x.VehicleType)
                            .ToDictionary(g => g.Key, g => g.Sum(x => x.SalePrice));
        var labels = data.Keys.ToList();

        return new SalesPerVehicleTypeViewModel
        {
            Labels = labels,
            Data = data
        };
    }

    public SalesPerClientViewModel GetSalesByClient(IQueryable<Sales> allSales)
    {
        var data = allSales.Select(s => new { s.SalePrice, s.Client.Name })
                                .GroupBy(x => x.Name)
                                .ToDictionary(g => g.Key, g => g.Sum(x => x.SalePrice));

        var labels = data.Keys.ToList();
        return new SalesPerClientViewModel
        {
            Labels = labels,
            Data = data
        };
    }

    public SalesPerManufacturerViewModel GetSalesByManufacturer(IQueryable<Sales> allSales)
    {
        var data = allSales.Select(s => new { s.SalePrice, s.Vehicle.Manufacturer.Name })
                                .GroupBy(x => x.Name)
                                .ToDictionary(g => g.Key, g => g.Sum(x => x.SalePrice));

        var labels = data.Keys.ToList();
        return new SalesPerManufacturerViewModel
        {
            Labels = labels,
            Data = data
        };
    }

    public SalesPerMonthViewModel GetSalesByMonth(IQueryable<Sales> allSales)
    {
        var month = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var data = allSales.Select(s => new { s.SalePrice, Month = month[s.SaleDate.Month - 1] })
                            .GroupBy(x => x.Month)
                            .ToDictionary(g => g.Key, g => g.Sum(x => x.SalePrice));


        var labels = data.Keys.ToList();
        
        return new SalesPerMonthViewModel
        {
            Labels = labels,
            Data = data
        };
    }

    public SalesPerCarDealershipViewModel GetSalesByCarDealership(IQueryable<Sales> allSales)
    {
        var data = allSales.Select(s => new { s.SalePrice, CarDealership = s.CarDealership.Name })
                            .GroupBy(x => x.CarDealership)
                            .ToDictionary(g => g.Key, g => g.Sum(x => x.SalePrice));

        var labels = data.Keys.ToList();
        return new SalesPerCarDealershipViewModel
        {
            Labels = labels,
            Data = data
        };
    }

    private async Task CheckVehiclePriceAsync(int vehicleId, decimal salePrice)
    {
        var vehicle = await _VehicleRepository.GetById(vehicleId) ?? throw new AppValidationException(nameof(SaleDto.VehicleId), "Veículo não encontrado.");
        if (vehicle.Price < salePrice)
        {
            var priceValue = "R$ " + vehicle.Price.ToString("N2");
            throw new AppValidationException(nameof(SaleDto.SalePrice), "O valor máximo de venda deste veículo é de " + priceValue);
        }

    }
    
}