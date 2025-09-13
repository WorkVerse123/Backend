using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<bool> ExistsByUserAndTargetAsync(int userId, string targetType, int targetId);
        Task<IEnumerable<Report>> GetReportsAsync();

        Task<Report> GetReportByIdAsync(int reportId);
    }
}
