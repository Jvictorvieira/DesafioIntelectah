using AutoMapper;
using ConcessionariaAPP.Application.Dto;

using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Models.ManufacturerViewModel;
using ConcessionariaAPP.Models.SaleViewModel;
using ConcessionariaAPP.Models.VehicleViewModel;
using ConcessionariaAPP.Models.CarDealershipViewModel;
using ConcessionariaAPP.Models.ClientViewModel;


namespace ConcessionariaAPP.Application.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<Vehicles, VehicleDto>()
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.Name));

            CreateMap<VehicleDto, Vehicles>()
                .ForMember(dest => dest.Manufacturer, opt => opt.Ignore()); 


            CreateMap<VehicleDto, VehicleViewModel>()
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.ManufacturerName))
                .ReverseMap();

            CreateMap<Manufacturers, ManufacturerDto>().ReverseMap();

            CreateMap<ManufacturerDto, ManufacturerViewModel>().ReverseMap();

            CreateMap<SaleDto, SaleViewModel>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.ClientName))
                .ForMember(dest => dest.CarDealershipName, opt => opt.MapFrom(src => src.CarDealershipName))
                .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.VehicleModel))
                .ReverseMap();

            CreateMap<Sales, SaleDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
                .ForMember(dest => dest.CarDealershipName, opt => opt.MapFrom(src => src.CarDealership.Name))
                .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model));
            
            CreateMap<SaleDto, Sales>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.CarDealership, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore());

            CreateMap<ClientDto, ClientViewModel>().ReverseMap();

            CreateMap<Clients, ClientDto>().ReverseMap();

            CreateMap<CarDealership, CarDealershipDto>().ReverseMap();

            CreateMap<CarDealershipDto, CarDealershipViewModel>().ReverseMap();

        }
    }
}

