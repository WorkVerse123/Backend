using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IBookmarkRepository : IGenericRepository<Bookmark>
    {
        Task<IEnumerable<Bookmark>> GetByEmployeeIdAsync(int employeeId);

        Task<Bookmark> FindByEmployeeAndJobAsync(int employeeId, int jobId);

        Task<Bookmark> GetByIdAsync(int bookmarkId);

        Task<int> CountBookmarkJobsByEmployeeIdAsync(int employeeId);

    }
}
