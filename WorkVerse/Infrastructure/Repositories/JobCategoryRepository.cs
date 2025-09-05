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
        private readonly WorkVerseDBContext _dbContext;
        public JobCategoryRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<JobCategory>> GetAllAsync()
        {
            var result = await _dbContext.JobCategories.ToListAsync();
            return result;
        }
    }
}
