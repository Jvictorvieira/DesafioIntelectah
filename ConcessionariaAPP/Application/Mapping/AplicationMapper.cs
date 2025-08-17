using AutoMapper;
using ConcessionariaAPP.Application.Dto;

using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Models.ManufacturerViewModel;
using ConcessionariaAPP.Models.VehicleViewModel;
// seus DTOs de Request/Response

namespace ConcessionariaAPP.Application.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<Vehicles, VehicleDto>()
            .ForMember(dest => dest.ManufacturerIds, opt => opt.MapFrom(src => src.Manufacturers.Select(m => m.ManufacturerId).ToList()))
            .ForMember(dest => dest.ManufacturerNames, opt => opt.MapFrom(src => src.Manufacturers.Select(m => m.Name).ToList()));

            
            CreateMap<VehicleDto, Vehicles>()
                .ForMember(dest => dest.Manufacturers, opt => opt.Ignore()); // Relacione manualmente no serviço/repositório


            CreateMap<VehicleDto, VehicleViewModel>()
                .ForMember(dest => dest.ManufacturerIds, opt => opt.MapFrom(src => src.ManufacturerIds))
                .ForMember(dest => dest.ManufacturerNames, opt => opt.MapFrom(src => src.ManufacturerNames))
                .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(src => src.ManufacturerNames != null && src.ManufacturerNames.Count > 0
                    ? string.Join(", ", src.ManufacturerNames)
                    : "Nenhum fabricante selecionado"))
                .ReverseMap();

            CreateMap<Manufacturers, ManufacturerDto>().ReverseMap();

            CreateMap<ManufacturerDto, ManufacturerViewModel>().ReverseMap();

        }
    }
}

