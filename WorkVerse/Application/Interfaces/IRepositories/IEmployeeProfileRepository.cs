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
       
        Task<EmployeeProfile?> GetByEmployeeIdAsync(int employeeId);
        Task<EmployeeProfile?> GetByUserIdAsync(int userId);
        Task<bool> ExistsByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<EmployeeProfile>> GetAllPublicEmployeeAsync();
        Task<int> CountAllEmployeeAsync();

    }

}
