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
    public class SubscriptionPlanProfile : Profile
    {
        public SubscriptionPlanProfile()
        {
            CreateMap<SubscriptionPlan, SubscriptionPlanDTOResponse>();
            CreateMap<SubscriptionPlan, PlanPaymentDTORespone>();
        }
    }
}
