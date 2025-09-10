using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<Review> GetByIdAsync(int reviewId);

        Task<IEnumerable<Review>> GetByJobIdAsync(int jobId);

        Task<Review> GetByJobIdEmployeeIdAsync(int jobId, int employeeId);

        Task<bool> ExistsAsync(int jobId, int employeeId);


    }
}
