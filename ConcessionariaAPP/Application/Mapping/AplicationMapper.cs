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
            // Entidade -> DTO de saída
            CreateMap<Vehicles, VehicleDto>()
                .ForMember(opt => opt.ManufacturerName, ent => ent.Ignore());

            // DTO de criação -> Entidade
            CreateMap<VehicleDto, Vehicles>()
                .ForMember(opt => opt.VehicleId, ent => ent.Ignore())
                .ForMember(opt => opt.Manufacturers, ent => ent.Ignore());

            CreateMap<VehicleDto, VehicleViewModel>().ReverseMap();

            CreateMap<Manufacturers, ManufacturerDto>().ReverseMap();

            CreateMap<ManufacturerDto, ManufacturerViewModel>().ReverseMap();
            CreateMap<Manufacturers, ManufacturerDto>().ReverseMap();

            CreateMap<ManufacturerDto, ManufacturerViewModel>().ReverseMap();
        }
    }
}

