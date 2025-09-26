using Application.Interfaces.IReposervices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<ITransaction?> GetCurrentTransactionAsync();

        IEmployeeProfileRepository EmployeeProfile { get; }
        IBusyTimeRepository BusyTime { get; }
        IBookmarkRepository Bookmark { get; }
        IJobRepository Job { get; }
        IApplicationRepository Application { get; }
        IJobCategoryRepository JobCategory { get; }
        IReviewRepository Review { get; }
        IEmployerProfileRepository EmployerProfile { get; }
        IUserRepository User { get; }
        IEmployerTypeRepository EmployerType { get; }

        IReportRepository Report { get; }
        IRoleRepository Role { get; }
        IFeedbackRepository Feedback { get; }

        IBlogRepository Blog { get; }   
        INotificationRepository Notification { get; }
        IStaffProfileRepository StaffProfile { get; }

    }
}
