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
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobSummaryDTO>()
            .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.JobId))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.JobCategory, opt => opt.MapFrom(
                src => src.JobCategoryMappings
                    .Select(m => m.Category.CategoryName)
                    .ToList()
            ))
            .ForMember(dest => dest.JobLocation, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.JobSalaryMin, opt => opt.MapFrom(src => src.SalaryMin))
            .ForMember(dest => dest.JobSalaryMax, opt => opt.MapFrom(src => src.SalaryMax))
            .ForMember(dest => dest.JobTime, opt => opt.MapFrom(src => src.JobTime))
            .ForMember(dest => dest.JobCreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.JobExpiredAt, opt => opt.MapFrom(src => src.ExpiredAt))
            .ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.Status));


            CreateMap<Job, JobDetailsDTOResponse>()
            .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.JobId))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.JobCategory, opt => opt.MapFrom(
                src => src.JobCategoryMappings
                    .Select(m => m.Category.CategoryName)
                    .ToList()
            ))
            .ForMember(dest => dest.JobDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.JobRequirements, opt => opt.MapFrom(src => src.Requirements))
            .ForMember(dest => dest.JobLocation, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.JobSalaryMin, opt => opt.MapFrom(src => src.SalaryMin))
            .ForMember(dest => dest.JobSalaryMax, opt => opt.MapFrom(src => src.SalaryMax))
            .ForMember(dest => dest.JobTime, opt => opt.MapFrom(src => src.JobTime))
            .ForMember(dest => dest.JobCreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.JobExpiredAt, opt => opt.MapFrom(src => src.ExpiredAt))
            .ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.Status));

            CreateMap<JobDTORequest, Job>();
        }
    }
}


