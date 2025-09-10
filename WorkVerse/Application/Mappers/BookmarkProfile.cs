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
    public class BookmarkProfile : Profile
    {
        public BookmarkProfile()
        {
            CreateMap<Bookmark, BookmarkItemDTO>()
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
            .ForMember(dest => dest.JobLocation, opt => opt.MapFrom(src => src.Job.Location))
            .ForMember(dest => dest.JobCategory, opt => opt.MapFrom(
                src => src.Job.JobCategoryMappings
                    .Select(m => m.Category.CategoryName)
                    .ToList()
            ));
        }
    }
}
