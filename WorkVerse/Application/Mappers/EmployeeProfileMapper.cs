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
    public class EmployeeProfileMapper : Profile
    {
        public EmployeeProfileMapper()
        {
            // Create, Update
            CreateMap<EmployeeProfileDTORequest, EmployeeProfile>();

            // Response
            CreateMap<EmployeeProfile, EmployeeProfileDTOResponse>();

            CreateMap<EmployeeProfile, CandidateItemDTO>()
            .ForMember(dest => dest.EmployeeLocation,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.EmployeeEducation,
                opt => opt.MapFrom(src => src.Education));


        }
    }
}
