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

namespace Infrastructure.Repositories
{
    public class EmployeeProfileRepository : GenericRepository<EmployeeProfile>, IEmployeeProfileRepository
    {
        public EmployeeProfileRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }

        // Lấy profile theo EmployeeId
        public async Task<EmployeeProfile?> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbSet.Include(c => c.User).FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
            return result;
        }


        // Lấy profile theo UserId
        public async Task<EmployeeProfile?> GetByUserIdAsync(int userId)
        {
            var result = await _dbSet.Include(c => c.User).FirstOrDefaultAsync(u => u.UserId == userId);
            return result;
        }
        // Kiểm tra profile tồn tại  theo employeeId
        public async Task<bool> ExistsByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.AnyAsync(j => j.EmployeeId == employeeId);
        }

        // Lấy tất cả Employee public
        public async Task<IEnumerable<EmployeeProfile>> GetAllPublicEmployeeAsync()
        {
            var result = await _dbSet.ToListAsync();
            return result;
        }
        // Đếm tất cả employee
        public async Task<int> CountAllEmployeeAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<IEnumerable<EmployeeProfile>> SearchEmployeeByAIResult(EmployeeQuery employeeQuery)
        {
            var employees = _dbSet.AsQueryable();

            // ========================
            // 1. Hard filter
            // ========================
            if (employeeQuery != null)
            {
                // Gender (hard filter)
                if (employeeQuery.Gender != null && employeeQuery.Gender.Any())
                {
                    var gendersNormalized = employeeQuery.Gender
                        .Where(g => !string.IsNullOrWhiteSpace(g))
                        .Select(g => g.Trim().ToLower())
                        .ToList();

                    employees = employees.Where(e =>
                        e.Gender != null &&
                        gendersNormalized.Contains(e.Gender.Trim().ToLower()));
                }
                if (employeeQuery.Address != null && employeeQuery.Address.Any())
                {
                    var addressesNormalized = employeeQuery.Address.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().ToLower()).ToList();
                    employees = employees.Where(e => e.Address != null && addressesNormalized.Any(addr => e.Address.ToLower().Contains(addr)));
                }
            }

            // ========================
            // 2. Soft ranking
            // ========================
            var employeeList = await employees.ToListAsync();

            var ranked = employeeList
                .Select(e => new
                {
                    Employee = e,
                    Score =
                        // FullName
                        (employeeQuery?.FullName != null && employeeQuery.FullName.Any() &&
                         GenericMatchAI.MatchAnyField(e.FullName, employeeQuery.FullName) ? 1 : 0)

                        // Address
                        + (employeeQuery?.Address != null && employeeQuery.Address.Any() &&
                           GenericMatchAI.MatchAnyField(e.Address, employeeQuery.Address) ? 1 : 0)

                        // Skills
                        + (employeeQuery?.Skills != null && employeeQuery.Skills.Any() &&
                           GenericMatchAI.MatchAnyField(e.Skills, employeeQuery.Skills) ? 1 : 0)

                        // Education
                        + (employeeQuery?.Education != null && employeeQuery.Education.Any() &&
                           GenericMatchAI.MatchAnyField(e.Education, employeeQuery.Education) ? 1 : 0)

                        // WorkExperience
                        + (employeeQuery?.WorkExperience != null && employeeQuery.WorkExperience.Any() &&
                           GenericMatchAI.MatchAnyField(e.WorkExperience, employeeQuery.WorkExperience) ? 1 : 0)
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Employee);

            return ranked;
        }
    }
}
