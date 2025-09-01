using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IEmployeeProfileRepository : IGenericRepository<EmployeeProfile>
    {
        // Employee profile
        Task<EmployeeProfile?> GetByIdAsync(int employeeId);                     // GET /employees/{id}

    }

}
