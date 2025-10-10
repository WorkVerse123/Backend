using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly WorkVerseDBContext _dbContext;
        public AdminRepository(WorkVerseDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<StatsResponse> GetSystemStatsAsync()
        {
            var totalUsers = await _dbContext.Users
            .CountAsync(u => u.Role.RoleName == "employee" || u.Role.RoleName == "employer");
            var totalEmployees = await _dbContext.Users.CountAsync(u => u.Role.RoleName == "employee");
            var totalEmployers = await _dbContext.Users.CountAsync(u => u.Role.RoleName == "employer");
            var totalJobs = await _dbContext.Jobs.CountAsync();
            var totalApplications = await _dbContext.Applications.CountAsync();
            var totalReports = await _dbContext.Reports.CountAsync();
            var totalFeedbacks = await _dbContext.Feedbacks.CountAsync();

            return new StatsResponse
            {
                TotalUsers = totalUsers,
                TotalEmployees = totalEmployees,
                TotalEmployers = totalEmployers,
                TotalJobs = totalJobs,
                TotalApplications = totalApplications,
                TotalReports = totalReports,
                TotalFeedbacks = totalFeedbacks
            };
        }


        // 1️⃣ Tổng user (không cần theo ngày)
        public async Task<ChartUserResponse> GetUserChartAsync()
        {
            var totalUsers = await _dbContext.Users
            .CountAsync(u => u.Role.RoleName == "employee" || u.Role.RoleName == "employer" || u.Role.RoleName == "staff");
            var totalEmployers = await _dbContext.Users.CountAsync(u => u.Role.RoleName == "employer");
            var totalEmployees = await _dbContext.Users.CountAsync(u => u.Role.RoleName == "employee");
            var totalStaffs = await _dbContext.Users.CountAsync(u => u.Role.RoleName == "staff");

            return new ChartUserResponse
            {
                TotalUsers = totalUsers,
                TotalEmployers = totalEmployers,
                TotalEmployees = totalEmployees,
                TotalStaffs = totalStaffs
            };
        }

        // 2️⃣ Job theo ngày
        public async Task<List<ChartJobResponse>> GetJobChartAsync(DateOnly startDate, DateOnly endDate)
        {
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = endDate.ToDateTime(TimeOnly.MaxValue);

            return await _dbContext.Jobs
                .Where(j => j.CreatedAt >= start && j.CreatedAt <= end)
                .GroupBy(j => j.CreatedAt.Date)
                .Select(g => new ChartJobResponse
                {
                    Date = DateOnly.FromDateTime(g.Key),
                    TotalJobs = g.Count(),
                    ActiveJobs = g.Count(x => x.Status == "active"),
                    ClosedJobs = g.Count(x => x.Status == "closed"),
                    Applications = _dbContext.Applications.Count(a => a.JobId != null && a.AppliedAt.Date == g.Key)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        // 3️⃣ Payment theo ngày
        public async Task<List<ChartPaymentResponse>> GetPaymentChartAsync(DateOnly startDate, DateOnly endDate)
        {
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = endDate.ToDateTime(TimeOnly.MaxValue);

            return await _dbContext.Payments
                .Where(p => p.PaymentDate >= start && p.PaymentDate <= end && p.Status == "completed") 
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => new ChartPaymentResponse
                {
                    Date = DateOnly.FromDateTime(g.Key),
                    TotalRevenue = g.Sum(x => x.Amount),
                    EmployerPayments = g.Where(x => x.User.Role.RoleName == "employer").Sum(x => x.Amount),
                    EmployeePayments = g.Where(x => x.User.Role.RoleName == "employee").Sum(x => x.Amount),
                    TotalTransactions = g.Count(),
                    EmployerTransactions = g.Count(x => x.User.Role.RoleName == "employer"),
                    EmployeeTransactions = g.Count(x => x.User.Role.RoleName == "employee")
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }
    }
}
