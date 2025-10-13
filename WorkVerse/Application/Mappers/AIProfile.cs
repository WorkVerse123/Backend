using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;


namespace Application.Mappers
{
    public class AIProfile : Profile
    {
        public AIProfile()
        {
            CreateMap<Job, JobAIDTOResponse>()
            .ForMember(dest => dest.Categories,
                   opt => opt.MapFrom(src => src.JobCategoryMappings.Select(m => m.Category.CategoryName)))
            .ForMember(dest => dest.Shifts,
               opt => opt.MapFrom(src => src.Shifts
                                           .GroupBy(s => new { s.StartTime, s.EndTime })
                                           .Select(g => new ShiftDTO
                                           {
                                               DaysOfWeek = g.Select(s => s.DayOfWeek).ToList(),
                                               StartTime = g.Key.StartTime,
                                               EndTime = g.Key.EndTime
                                           })));

            CreateMap<Job, JobWithEmployerAIDTOResponse>()
                .ForMember(dest => dest.Categories,
                           opt => opt.MapFrom(src => src.JobCategoryMappings.Select(m => m.Category.CategoryName)))
                .ForMember(dest => dest.EmployerId, opt => opt.MapFrom(src => src.Employer.EmployerId))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Employer.CompanyName))
                .ForMember(dest => dest.EmployerType, opt => opt.MapFrom(src => src.Employer.EmployerType.EmployerTypeName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Employer.Address))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Employer.LogoUrl))
                .ForMember(dest => dest.Shifts,
               opt => opt.MapFrom(src => src.Shifts
                                           .GroupBy(s => new { s.StartTime, s.EndTime })
                                           .Select(g => new ShiftDTO
                                           {
                                               DaysOfWeek = g.Select(s => s.DayOfWeek).ToList(),
                                               StartTime = g.Key.StartTime,
                                               EndTime = g.Key.EndTime
                                           })));

            CreateMap<EmployerProfile, EmployerAIDTOResponse>()
                .ForMember(dest => dest.EmployerType, opt => opt.MapFrom(src => src.EmployerType.EmployerTypeName));


            CreateMap<EmployeeProfile, EmployeeAIDTOResponse>();
        }
    }
}
