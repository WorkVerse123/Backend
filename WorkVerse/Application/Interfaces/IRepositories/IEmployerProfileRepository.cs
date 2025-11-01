using Application.DTOs.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IEmployerProfileRepository : IGenericRepository<EmployerProfile>
    {

        Task<EmployerProfile?> GetByIdAsync(int employerId);
        Task<EmployerProfile?> GetByUserIdAsync(int userId);
        Task<bool> ExistsByEmployerIdAsync(int employerId);
        Task<IEnumerable<EmployerProfile>> GetAllEmployersAsync();
        Task<int> CountAllEmployersAsync();
        Task<bool> ExistsByUserIdAsync(int userId);
        Task<IEnumerable<EmployerProfile>> SearchEmployerByAIResult(EmployerQuery jobQuery);
        Task<bool> ExistsByContactPhoneAsync(string contactPhone, int? excludeId = null);
        Task<bool> ExistsByContactEmailAsync(string contactEmail, int? excludeId = null);
        Task<bool> UpdatePriority(int userId, bool isPriority);

    }
}

