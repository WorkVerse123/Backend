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
    public class JobCategoryRepository : GenericRepository<JobCategory>, IJobCategoryRepository
    {
        public JobCategoryRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }

        // lấy tất cả thể loại ngành nghề
        public async Task<IEnumerable<JobCategory>> GetAllJobCategoriesAsync()
        {
            var result = await _dbSet.ToListAsync();
            return result;
        }
    }
}
