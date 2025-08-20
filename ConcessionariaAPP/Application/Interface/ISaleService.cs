namespace ConcessionariaAPP.Application.Interfaces;

using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Enum;
using ConcessionariaAPP.Models.HomeViewModel;

public interface ISaleService : IGenericCrudInterface<SaleDto>
{
    SalesPerCarDealershipViewModel GetSalesByCarDealership(IQueryable<Sales> allSales);
    SalesPerVehicleTypeViewModel GetSalesByVehicleType(IQueryable<Sales> allSales);
    SalesPerClientViewModel GetSalesByClient(IQueryable<Sales> allSales);
    SalesPerManufacturerViewModel GetSalesByManufacturer(IQueryable<Sales> allSales);
    SalesPerMonthViewModel GetSalesByMonth(IQueryable<Sales> allSales);

    HomeViewModel LoadCharts(HomeViewModel viewModel);
}