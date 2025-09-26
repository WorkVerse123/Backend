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
    public class EmployerProfileRepository : GenericRepository<EmployerProfile>, IEmployerProfileRepository
    {
        public EmployerProfileRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }
        // Kiểm tra tồn tại theo EmployerId
        public async Task<bool> ExistsByEmployerIdAsync(int employerId)
        {
            return await _dbSet.AnyAsync(j => j.EmployerId == employerId);
        }
        // Lấy tất cả employer
        public async Task<IEnumerable<EmployerProfile>> GetAllEmployersAsync()
        {
            var result = await _dbSet
                 .Include(j => j.EmployerType)
                .ToListAsync();
            return result;
        }

        // Lấy chi tiết employer theo Id
        public async Task<EmployerProfile?> GetByIdAsync(int employerId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(c => c.EmployerId == employerId);
            return result;  
        }

        // Lấy employer theo UserId
        public async Task<EmployerProfile?> GetByUserIdAsync(int userId)
        {
            var result = await _dbSet.Include(c => c.User).FirstOrDefaultAsync(u => u.UserId == userId);
            return result;
        }

        // Đếm tất cả employer
        public async Task<int> CountAllEmployersAsync()
        {
            return await _dbSet.CountAsync();
        }
        // Kiểm tra tồn tại employer theo UserId
        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _dbSet.AnyAsync(j => j.UserId == userId);
        }

        // Tìm các công ty theo kết quả AI (ưu tiên filter cứng trước, mềm sau)
        public async Task<IEnumerable<EmployerProfile>> SearchEmployerByAIResult(EmployerQuery employerQuery)
        {
            var employers = _dbSet
                .Include(e => e.EmployerType)
                .AsQueryable();

            // ========================
            // 1. Hard filter (chỉ filter những cái "must have")
            // ========================
            if (employerQuery.EmployerTypes != null && employerQuery.EmployerTypes.Any())
            {
                employers = employers.Where(e =>
                    e.EmployerType != null &&
                    employerQuery.EmployerTypes.Contains(e.EmployerType.EmployerTypeName));


            }
            if (employerQuery.Address != null && employerQuery.Address.Any())
            {
                var addressesNormalized = employerQuery.Address.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().ToLower()).ToList();
                employers = employers.Where(e => e.Address != null && addressesNormalized.Any(addr => e.Address.ToLower().Contains(addr)));
            }
            // Load ra memory (không hard filter Address/CompanyName để không mất dữ liệu)
            var employerList = await employers.ToListAsync();

            // ========================
            // 2. Soft ranking (match mờ)
            // ========================
            var ranked = employerList.Select(e => new
            {
                Employer = e,
                Score =
                    (employerQuery.CompanyNames != null &&
                     GenericMatchAI.MatchAnyField(e.CompanyName, employerQuery.CompanyNames) ? 1 : 0) +

                    (employerQuery.Address != null &&
                     GenericMatchAI.MatchAnyField(e.Address, employerQuery.Address) ? 1 : 0)
            })
            .OrderByDescending(x => x.Score)
            .Select(x => x.Employer);

            return ranked;
        }

    }
}
