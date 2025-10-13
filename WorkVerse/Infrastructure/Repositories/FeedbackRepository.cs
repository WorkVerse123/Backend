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
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(WorkVerseDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbackDetailsAsync()
        {
            return await _dbSet
                .Include(f => f.User)
                    .ThenInclude(u => u.EmployeeProfile)
                .Include(f => f.User)
                    .ThenInclude(u => u.EmployerProfile)
                .Include(f => f.Handler)
                    .ThenInclude(h => h.StaffProfile)
                .ToListAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
           return await _dbSet.FirstOrDefaultAsync(c => c.FeedbackId == feedbackId);  
        }
    }
}
