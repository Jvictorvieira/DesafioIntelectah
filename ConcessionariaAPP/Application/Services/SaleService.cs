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

        var created = await _SaleRepository.CreateAsync(entity);
        return _mapper.Map<SaleDto>(created);
    }

    public async Task<SaleDto> UpdateAsync(SaleDto dto)
    {
        Validate(dto, isUpdate: true);

        var entity = _mapper.Map<Sales>(dto);
        var updated = await _SaleRepository.UpdateAsync(entity);

        await CheckVehiclePriceAsync(entity.VehicleId, entity.SalePrice);

        return _mapper.Map<SaleDto>(updated);
    }

    public async Task<SaleDto> GetByIdAsync(int id)
    {
        var entity = await _SaleRepository.GetByIdAsync(id);
        return _mapper.Map<SaleDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new AppValidationException().Add(nameof(SaleDto.SaleId), "Id inválido para exclusão.");
        }
        return await _SaleRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<SaleDto>> GetAllAsync()
    {
        var list = await _SaleRepository.GetAllAsync();
        return [.. list.Select(e => _mapper.Map<SaleDto>(e))];
    }

    private static void Validate(SaleDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new AppValidationException().Add(nameof(dto), "Objeto não pode ser nulo.");
        }
        if (isUpdate && dto.SaleId <= 0)
        {
            throw new AppValidationException().Add(nameof(dto.SaleId), "Id inválido para atualização.");
        }

    }

    private static string GenerateProtocol()
    {
        var random = new Random();
        return new string([.. Enumerable.Repeat("0123456789", 20).Select(s => s[random.Next(s.Length)])]);
    }

    public async Task<SalesPerCarDealershipViewModel> GetSalesByCarDealershipAsync(int? carDealershipId)
    {

        var allSales = await _SaleRepository.GetAllAsync();

        if (carDealershipId != 0)
        {
            allSales = allSales.Where(s => s.CarDealershipId == carDealershipId);
        }

        var data = allSales.Select(s => new { s.SalePrice, s.CarDealership.Name })
                                .GroupBy(x => x.Name)
                                .ToDictionary(g => g.Key, g => g.Sum(x => x.SalePrice));
        var labels = data.Keys.ToList();
        return new SalesPerCarDealershipViewModel
        {
            Labels = labels,
            Data = data
        };
    }

    public async Task<SalesPerVehicleTypeViewModel> GetSalesByVehicleTypeAsync(VehiclesTypes? vehicleTypeId)
    {
        var allSales = await _SaleRepository.GetAllAsync();

        if (vehicleTypeId != 0)
        {
            allSales = allSales.Where(s => s.Vehicle.VehicleType == vehicleTypeId);
        }
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

    public async Task<SalesPerClientViewModel> GetSalesByClientAsync(int? clientId)
    {
        var allSales = await _SaleRepository.GetAllAsync();

        if (clientId != 0)
        {
            allSales = allSales.Where(s => s.ClientId == clientId);
        }

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

    public async Task<SalesPerManufacturerViewModel> GetSalesByManufacturerAsync(int? manufacturerId)
    {
        var allSales = await _SaleRepository.GetAllAsync();

        if (manufacturerId != 0)
        {
            allSales = allSales.Where(s => s.Vehicle.ManufacturerId == manufacturerId);
        }

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

    public async Task<SalesPerMonthViewModel> GetSalesByMonthAsync(int year)
    {
        var allSales = await _SaleRepository.GetAllAsync();

        var data = allSales.Where(s => s.SaleDate.Year == year)
                                .Select(s => new { s.SalePrice, Month = s.SaleDate.ToString("MMM/yyyy") })
                                .GroupBy(x => x.Month)
                                .ToDictionary(g => g.Key, g => g.Sum(x => x.SalePrice));

        var labels = data.Keys.ToList();
        return new SalesPerMonthViewModel
        {
            Labels = labels,
            Data = data
        };
    }
    
    private async Task CheckVehiclePriceAsync(int vehicleId, decimal salePrice)
    {
        var vehicle = await _VehicleRepository.GetByIdAsync(vehicleId) ?? throw new AppValidationException().Add(nameof(SaleDto.VehicleId), "Veículo não encontrado.");
        if (vehicle.Price < salePrice)
        {
            var priceValue = "R$ " + vehicle.Price.ToString("N2");
            throw new AppValidationException().Add(nameof(SaleDto.SalePrice), "O valor máximo de venda deste veículo é de " + priceValue);
        }
        
    }
}