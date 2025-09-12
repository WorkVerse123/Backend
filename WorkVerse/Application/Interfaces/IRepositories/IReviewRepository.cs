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

        Task<Review> GetByJobAndCandidateAsync(int jobId, int employeeId);

        Task<bool> ExistsByJobAndCandidateAsync(int jobId, int employeeId);


    }
}
