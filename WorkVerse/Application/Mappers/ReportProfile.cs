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
    public class ReportProfile : Profile
    {
        public ReportProfile() {
            CreateMap<Report, ReportDetailsDTOResponse>();

            CreateMap<SubmitReportDTORequest, Report>()
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "pending"))
             .ForMember(dest => dest.ReportedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Report, ReportDTORespone>();
            CreateMap<ReportDTORequest, Report>();
        }
    }
}
