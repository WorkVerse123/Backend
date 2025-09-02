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
    public class BusyTimeProfile : Profile
    {
        public BusyTimeProfile()
        {
            CreateMap<BusyTime, BusyTimeDTOResponse>()
                .ForMember(dest => dest.DayOfWeek,
                           opt => opt.MapFrom(src =>
                               Enum.GetName(typeof(DayOfWeek), src.DayOfWeek)
                               ?? src.DayOfWeek.ToString()));
        }
    }
}
