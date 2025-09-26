using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class EmployerProfileMapper : Profile
    {
        public EmployerProfileMapper() {
            CreateMap<EmployerProfile, CompanyItemDTO>()
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.EmployerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.WebsiteUrl))
            .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.LogoUrl))
            .ForMember(dest => dest.Industry,
           opt => opt.MapFrom(src => src.EmployerType.EmployerTypeName));

            CreateMap<EmployerProfileDTORequest, EmployerProfile>()
           .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
           .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
           .ForMember(dest => dest.EmployerTypeId, opt => opt.MapFrom(src => src.EmployerType))
           .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.WebsiteUrl))
           .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl))
           .ForMember(dest => dest.DateEstablish, opt => opt.MapFrom(src => src.DateEstablished))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.EmployerType, opt => opt.Ignore());

            CreateMap<EmployerProfile, EmployerProfileDTOResponse>()
            .ForMember(dest => dest.EmployerTypeName, opt => opt.MapFrom(src => src.EmployerType.EmployerTypeName));

            CreateMap<EmployerProfile, EmployerInformationDTO>();

        }
    }
}
