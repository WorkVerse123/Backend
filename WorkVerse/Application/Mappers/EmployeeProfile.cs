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
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Create
            CreateMap<EmployeeProfileDTOPostRequest, EmployeeProfile>();

            // Update
            CreateMap<EmployeeProfileDTOPutRequest, EmployeeProfile>();

            // Response
            CreateMap<EmployeeProfile, EmployeeProfileDTOResponse>();

        }
    }
}
