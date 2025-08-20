using System.Net.NetworkInformation;
using AutoMapper;
using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Application.Excepetions;
using ConcessionariaAPP.Application.Services;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Domain.Enum;
using ConcessionariaAPP.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ConcessionariaAppTest.Application.Service
{
    public class SaleAppServiceTest
    {
        private readonly Mock<DbSet<Sales>> _mockSet;

        private readonly Mock<DbSet<Vehicles>> _mockVehicleSet;
        private readonly Mock<DbSet<Manufacturers>> _mockManufacturerSet;
        private readonly Mock<DbContext> _mockContext;
        private readonly SaleAppService _service;
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly Mock<IVehicleRepository> _mockVehicleRepository;
        private readonly Mapper _mapper;


        public SaleAppServiceTest()
        {
            _mockSet = new Mock<DbSet<Sales>>();
            _mockVehicleSet = new Mock<DbSet<Vehicles>>();
            _mockContext = new Mock<DbContext>();
            _mockManufacturerSet = new Mock<DbSet<Manufacturers>>();
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sales, SaleDto>().ReverseMap();
                cfg.CreateMap<Vehicles, VehicleDto>().ReverseMap();
            }));
            _mockSaleRepository = new Mock<ISaleRepository>();
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _service = new SaleAppService(_mockSaleRepository.Object, _mockVehicleRepository.Object, _mapper);
        }

        [Fact]
        public async Task CreateAsync_sale_equal_vehicle_price()
        {
            // Arrange
            var saleDto = new SaleDto { VehicleId = 1, SalePrice = 15000, ClientId = 1, CarDealershipId = 1 };
            var manufacturer = new Manufacturers { ManufacturerId = 1, Name = "Volkswagen", Country = "Germany" };
            var vehicle = new Vehicles { VehicleId = 1, Price = 15000, Model = "Fusca", ManufacturerId = 1, Manufacturer = manufacturer, VehicleType = VehiclesTypes.Car };

            _mockVehicleRepository.Setup(r => r.GetById(1))
                .ReturnsAsync(vehicle);

            _mockSaleRepository.Setup(r => r.Create(It.IsAny<Sales>()))
                .ReturnsAsync(new Sales(
                    saleId: 1,
                    vehicleId: 1,
                    clientId: saleDto.ClientId,
                    carDealershipId: saleDto.CarDealershipId,
                    salePrice: saleDto.SalePrice,
                    saleDate: DateTime.Now,
                    saleProtocol: "01234567890123456789"
                ));

            var service = new SaleAppService(_mockSaleRepository.Object, _mockVehicleRepository.Object, _mapper);

            // Act
            var result = await service.CreateAsync(saleDto);

            // Assert
            result.SalePrice.Should().Be(15000);
            result.VehicleId.Should().Be(1);
        }
        
        [Fact]
        public async Task CreateAsync_expection_vehicle_price()
        {
            // Arrange
            var saleDto = new SaleDto { VehicleId = 1, SalePrice = 15000, ClientId = 1, CarDealershipId = 1 };
            var manufacturer = new Manufacturers { ManufacturerId = 1, Name= "Volkswagen", Country = "Germany" };
            var vehicle = new Vehicles { VehicleId = 1, Price = 10000, Model = "Fusca", ManufacturerId = 1, Manufacturer = manufacturer, VehicleType = VehiclesTypes.Car };
            var priceValue = "R$ " + vehicle.Price.ToString("N2");
            var message = "O valor máximo de venda deste veículo é de " + priceValue; 
            _mockVehicleRepository.Setup(r => r.GetById(1))
                .ReturnsAsync(vehicle);
            var service = new SaleAppService(_mockSaleRepository.Object, _mockVehicleRepository.Object, _mapper);

            Func<Task> act = async () => await service.CreateAsync(saleDto);
            // Act
            await act.Should().ThrowAsync<AppValidationException>()
                .WithMessage(message);
        }
    }
}