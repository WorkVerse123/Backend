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

            // Dùng Task.WhenAll để chạy song song các CountAsync() (không block)
            var totalUsersTask = _dbContext.Users.CountAsync();
            var totalEmployeesTask = _dbContext.Users.CountAsync(u => u.Role.RoleName == "employee");
            var totalEmployersTask = _dbContext.Users.CountAsync(u => u.Role.RoleName == "employer");
            var totalJobsTask = _dbContext.Jobs.CountAsync();
            var totalApplicationsTask = _dbContext.Applications.CountAsync();
            var totalReportsTask = _dbContext.Reports.CountAsync();
            var totalFeedbacksTask = _dbContext.Feedbacks.CountAsync();

            await Task.WhenAll(totalUsersTask, totalEmployeesTask, totalEmployersTask,
                               totalJobsTask, totalApplicationsTask, totalReportsTask, totalFeedbacksTask);

            return new StatsResponse
            {
                TotalUsers = totalUsersTask.Result,
                TotalEmployees = totalEmployeesTask.Result,
                TotalEmployers = totalEmployersTask.Result,
                TotalJobs = totalJobsTask.Result,
                TotalApplications = totalApplicationsTask.Result,
                TotalReports = totalReportsTask.Result,
                TotalFeedbacks = totalFeedbacksTask.Result
            };
        }

        // 1️⃣ Tổng user (không cần theo ngày)
        public async Task<ChartUserResponse> GetUserChartAsync()
        {
            var totalUsers = await _dbContext.Users.CountAsync();
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
                .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
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
