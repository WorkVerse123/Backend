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
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Blog, BlogItemDTOResponse>()
                .ForMember(dest => dest.Excerpt,
                    opt => opt.MapFrom(src =>
                        src.Content.Length > 100
                            ? src.Content.Substring(0, 100) + "..."
                            : src.Content));

            CreateMap<User, AuthorBlogDTOResponse>()
          .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.UserId))
          .ForMember(dest => dest.fullName, opt => opt.MapFrom(src => src.StaffProfile.FullName));

            CreateMap<Blog, BlogDetailsDTOResponse>();

            CreateMap<CreateBlogDTORequest, Blog>()
     .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => GenerateSlug(src.Title)))
     .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
     .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) 
     .ForMember(dest => dest.Author, opt => opt.Ignore());
            CreateMap<UpdateBlogDTORequest, Blog>()
    .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => GenerateSlug(src.Title)))
    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));
        }
        private static string GenerateSlug(string title)
        {
            return title
                .ToLower()
                .Replace(" ", "-")
                .Replace(".", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace(";", "");
        }

    }
}
