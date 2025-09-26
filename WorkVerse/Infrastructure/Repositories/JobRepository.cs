using Application.DTOs.Request;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }
        // Lấy 1 job theo Id
        public async Task<Job> GetByIdAsync(int jobId)
        {
            var result = await _dbSet
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(u => u.JobId == jobId);
            return result;
        }
        // Lấy tất cả job
        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            var result = await _dbSet
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .OrderByDescending(u => u.IsPriority)
                .ThenByDescending(c => c.CreatedAt).ToListAsync();

            return result;
        }
        // Kiểm tra tồn tại job theo Id
        public async Task<bool> ExistsByJobIdAsync(int jobId)
        {
            return await _dbSet.AnyAsync(j => j.JobId == jobId);
        }

        // Đếm tất cả job
        public async Task<int> CountAllJobsAsync()
        {
            return await _dbSet.CountAsync();
        }

        // Đếm job mới trong range time
        public async Task<int> CountNewJobsAsync(TimeSpan range)
        {
            var fromDate = DateTime.Now.Subtract(range);
            return await _dbSet
                .Where(j => j.CreatedAt >= fromDate)
                .CountAsync();
        }

        // Lấy danh sách tất cả job theo EmployerId
        public async Task<IEnumerable<Job>> GetJobsByEmployerIdAsync(int employerId)
        {
            var result = await _dbSet
                .Where(j => j.EmployerId == employerId)
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .OrderByDescending(c => c.CreatedAt).ToListAsync();
            return result;
        }
        // Lay title cua job bang jobId
        public async Task<string?> GetJobTitleByIdAsync(int id)
        {
            return await _dbSet
                .Where(j => j.JobId == id)
                .Select(j => j.Title)
                .FirstOrDefaultAsync();
        }

        // Tìm danh sách công việc theo kết quả AI (ưu tiên cứng trước, mềm sau)
        public async Task<IEnumerable<Job>> SearchJobByAIResult(JobQuery jobQuery)
        {
            var jobs = _dbSet
                .Include(j => j.Shifts)
                .Include(j => j.JobCategoryMappings)
                    .ThenInclude(m => m.Category)
                .AsQueryable();

            // ========================
            // 1. Hard filter (chỉ giữ cái bắt buộc)
            // ========================
            if (jobQuery.Categories != null && jobQuery.Categories.Any())
            {
                var categoriesNormalized = jobQuery.Categories
                    .Select(c => c.Trim().ToLower())
                    .ToList();

                jobs = jobs.Where(j =>
                    j.JobCategoryMappings != null &&
                    j.JobCategoryMappings.Any(m =>
                        m.Category != null &&
                        categoriesNormalized.Contains(m.Category.CategoryName.Trim().ToLower())));
            }
            if (jobQuery.Location != null && jobQuery.Location.Any())
            {
                var locationNormalized = jobQuery.Location.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().ToLower()).ToList();
                jobs = jobs.Where(e => e.Location != null && locationNormalized.Any(addr => e.Location.ToLower().Contains(addr)));
            }
            if (jobQuery.SalaryMin.HasValue)
                jobs = jobs.Where(j => j.SalaryMax >= jobQuery.SalaryMin.Value);

            if (jobQuery.SalaryMax.HasValue)
                jobs = jobs.Where(j => j.SalaryMin <= jobQuery.SalaryMax.Value);

            if (jobQuery.JobTime != null && jobQuery.JobTime.Any())
            {
                var jobTimesNormalized = jobQuery.JobTime
                    .Select(t => t.Trim().ToLower())
                    .ToList();

                jobs = jobs.Where(j =>
                    j.JobTime != null &&
                    jobTimesNormalized.Contains(j.JobTime.Trim().ToLower()));
            }
            // Filter theo ngày
            if (jobQuery.DaysOfWeek != null && jobQuery.DaysOfWeek.Any())
            {
                jobs = jobs.Where(j => j.Shifts.Any(s => jobQuery.DaysOfWeek.Contains(s.DayOfWeek)));
            }

            // Filter theo giờ bắt đầu/kết thúc
            if (jobQuery.StartTime.HasValue)
            {
                jobs = jobs.Where(j => j.Shifts.Any(s => s.StartTime >= jobQuery.StartTime));
            }
            if (jobQuery.EndTime.HasValue)
            {
                jobs = jobs.Where(j => j.Shifts.Any(s => s.EndTime <= jobQuery.EndTime));
            }

            // ========================
            // 2. Ranking mềm
            // ========================
            var jobList = await jobs.ToListAsync();

            var ranked = jobList
                .Select(j => new
                {
                    Job = j,
                    Score =
                        // Match Title (mềm)
                        (jobQuery.Title != null && jobQuery.Title.Any() &&
                         GenericMatchAI.MatchAnyField(j.Title, jobQuery.Title) ? 1 : 0) +

                        // Match Location (mềm)
                        (jobQuery.Location != null && jobQuery.Location.Any() &&
                         GenericMatchAI.MatchAnyField(j.Location, jobQuery.Location) ? 1 : 0)
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Job)
                .OrderByDescending(x => x.IsPriority); // Ưu tiên job "hot"

            return ranked;
        }



        public async Task<IEnumerable<Job>> SearchJobByEmployerAIResult(JobQuery jobQuery, EmployerQuery employerQuery)
        {
            var jobs = _dbSet.Include(j => j.Shifts)
                .Include(j => j.Employer)
                    .ThenInclude(e => e.EmployerType)
                .Include(j => j.JobCategoryMappings)
                    .ThenInclude(m => m.Category)
                .AsQueryable();

            // =========================
            // 0. Hard filter: Employer
            // =========================
            if (employerQuery != null)
            {
                // Lọc theo EmployerType
                if (employerQuery.EmployerTypes != null && employerQuery.EmployerTypes.Any())
                {
                    var employerTypesNormalized = employerQuery.EmployerTypes
                        .Where(e => !string.IsNullOrWhiteSpace(e))
                        .Select(e => e.Trim().ToLower())
                        .ToList();

                    jobs = jobs.Where(j =>
                        j.Employer != null &&
                        j.Employer.EmployerType != null &&
                        employerTypesNormalized.Contains(j.Employer.EmployerType.EmployerTypeName.Trim().ToLower()));
                }

            }
            if (employerQuery.Address != null && employerQuery.Address.Any())
            {
                var addressesNormalized = employerQuery.Address.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().ToLower()).ToList();
                jobs = jobs.Where(e => e.Employer.Address != null && addressesNormalized.Any(addr => e.Employer.Address.ToLower().Contains(addr)));
            }
            // =========================
            // 1. Hard filter: Job
            // =========================
            if (jobQuery != null)
            {
                if (jobQuery.Categories != null && jobQuery.Categories.Any())
                {
                    var categoriesNormalized = jobQuery.Categories
                        .Select(c => c.Trim().ToLower())
                        .ToList();

                    jobs = jobs.Where(j =>
                        j.JobCategoryMappings != null &&
                        j.JobCategoryMappings.Any(m =>
                            m.Category != null &&
                            categoriesNormalized.Contains(m.Category.CategoryName.Trim().ToLower())));
                }
                if (jobQuery.Location != null && jobQuery.Location.Any())
                {
                    var locationNormalized = jobQuery.Location.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().ToLower()).ToList();
                    jobs = jobs.Where(e => e.Location != null && locationNormalized.Any(addr => e.Location.ToLower().Contains(addr)));
                }
                if (jobQuery.SalaryMin.HasValue)
                    jobs = jobs.Where(j => j.SalaryMax >= jobQuery.SalaryMin.Value);

                if (jobQuery.SalaryMax.HasValue)
                    jobs = jobs.Where(j => j.SalaryMin <= jobQuery.SalaryMax.Value);

                if (jobQuery.JobTime != null && jobQuery.JobTime.Any())
                {
                    var jobTimesNormalized = jobQuery.JobTime
                        .Select(t => t.Trim().ToLower())
                        .ToList();

                    jobs = jobs.Where(j =>
                        j.JobTime != null &&
                        jobTimesNormalized.Contains(j.JobTime.Trim().ToLower()));
                }

                // Filter theo ngày
                if (jobQuery.DaysOfWeek != null && jobQuery.DaysOfWeek.Any())
                {
                    jobs = jobs.Where(j => j.Shifts.Any(s => jobQuery.DaysOfWeek.Contains(s.DayOfWeek)));
                }

                // Filter theo giờ bắt đầu/kết thúc
                if (jobQuery.StartTime.HasValue)
                {
                    jobs = jobs.Where(j => j.Shifts.Any(s => s.StartTime >= jobQuery.StartTime));
                }
                if (jobQuery.EndTime.HasValue)
                {
                    jobs = jobs.Where(j => j.Shifts.Any(s => s.EndTime <= jobQuery.EndTime));
                }

            }

            // =========================
            // 2. Lấy list ra rồi ranking "mềm"
            // =========================
            var jobList = await jobs.ToListAsync();

            var ranked = jobList
                .Select(j => new
                {
                    Job = j,
                    Score =
                        // Soft: Title
                        (jobQuery?.Title != null && jobQuery.Title.Any() &&
                         GenericMatchAI.MatchAnyField(j.Title, jobQuery.Title) ? 1 : 0)

                        // Soft: Job.Location
                        + (jobQuery?.Location != null && jobQuery.Location.Any() &&
                           GenericMatchAI.MatchAnyField(j.Location, jobQuery.Location) ? 1 : 0)

                        // Soft: Employer.CompanyNames
                        + (employerQuery?.CompanyNames != null && employerQuery.CompanyNames.Any() &&
                           j.Employer != null &&
                           GenericMatchAI.MatchAnyField(j.Employer.CompanyName, employerQuery.CompanyNames) ? 1 : 0)

                        // Soft: Employer.Address
                        + (employerQuery?.Address != null && employerQuery.Address.Any() &&
                           j.Employer != null &&
                           GenericMatchAI.MatchAnyField(j.Employer.Address, employerQuery.Address) ? 1 : 0)
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Job)
                .OrderByDescending(x => x.IsPriority);

            return ranked;
        }

        public Task<int> CountJobApply(int jobId)
        {
            return _context.Applications
                .Where(ja => ja.JobId == jobId)
                .CountAsync();
        }

        public Task<bool> AddJobCategory(int jobId, List<int> categoryIds)
        {
            var mappings = categoryIds.Select(catId => new JobCategoryMapping
            {
                JobId = jobId,
                CategoryId = catId
            }).ToList();
            return _context.JobCategoryMappings
                .AddRangeAsync(mappings)
                .ContinueWith(t => t.IsCompletedSuccessfully);
        }

        public async Task<bool> RemoveJobCategory(int jobId)
        {
            return await _context.JobCategoryMappings
                .Where(m => m.JobId == jobId)
                .ExecuteDeleteAsync()
                .ContinueWith(t => t.Result > 0);
        }
    }
}
