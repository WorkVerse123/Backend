using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(WorkVerseDBContext context) : base(context)
        {
        }
        public async Task<bool> ExistsByUserAndTargetAsync(int userId, string targetType, int targetId)
        {
            return await _dbSet.AnyAsync(r =>
                r.UserId == userId &&
                r.TargetType == targetType &&
                r.TargetId == targetId);
        }
        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ReportId == reportId);
        }
        public async Task<IEnumerable<Report>> GetReportsAsync()
        {
            return await _dbSet.Include(c => c.User).ToListAsync();
        }

    }
}
