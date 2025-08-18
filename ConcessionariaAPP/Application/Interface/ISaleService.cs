namespace ConcessionariaAPP.Application.Interfaces;

using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Domain.Enum;
using ConcessionariaAPP.Models.HomeViewModel;

public interface ISaleService : IGenericCrudInterface<SaleDto>
{
    Task<SalesPerCarDealershipViewModel> GetSalesByCarDealershipAsync(int? carDealershipId);
    Task<SalesPerVehicleTypeViewModel> GetSalesByVehicleTypeAsync(VehiclesTypes? vehicleTypeId);
    Task<SalesPerClientViewModel> GetSalesByClientAsync(int? clientId);
    Task<SalesPerManufacturerViewModel> GetSalesByManufacturerAsync(int? manufacturerId);
    Task<SalesPerMonthViewModel> GetSalesByMonthAsync(int year);
}