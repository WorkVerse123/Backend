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
    public class FeedbackProfile:Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackItemDTOResponse>();

            CreateMap<SendFeedbackDTORequest, Feedback>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.HandledBy, opt => opt.Ignore())
            .ForMember(dest => dest.HandledAt, opt => opt.Ignore());

            CreateMap<FeedbackDTORequest, Feedback>();
            CreateMap<Feedback, FeedbackDTOResponse>();
        }
    }
}
