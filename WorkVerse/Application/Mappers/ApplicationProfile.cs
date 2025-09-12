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
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Domain.Entities.Application, JobApplicationItemDTO>()
            .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
            .ForMember(dest => dest.JobLocation, opt => opt.MapFrom(src => src.Job.Location))
            .ForMember(dest => dest.JobCategory, opt => opt.MapFrom(
                src => src.Job.JobCategoryMappings
                    .Select(m => m.Category.CategoryName)
                    .ToList()
            ))
            .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.Status));

            CreateMap<Domain.Entities.Application, JobApplicationDetailsDTOResponse>()
            .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => src.AppliedAt))
            .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.JobId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Domain.Entities.Application, JobApplicationSummaryDTO>()
           .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
           .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => src.Employee.FullName))
           .ForMember(dest => dest.EmployeeGender, opt => opt.MapFrom(src => src.Employee.Gender))
           .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => src.AppliedAt));
        }
    }
}
