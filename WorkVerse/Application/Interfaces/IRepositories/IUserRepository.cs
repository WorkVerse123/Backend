using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(string email);
        Task<bool> ExistByIdAsync(int userId);
        Task<string?> GetUserFullNameByIdAsync(int id);
        Task<bool> UpdatePasswordAsynce(int userId, string newPasswordHash);
        Task<bool> IsPremiumAsync(int userId);
        Task<bool> UpdateStatusAsync(int userId, string newStatus);
        Task<bool> UpdateUserByEmployerId(int employerId, string newPhone, string newEmail);

        Task<bool> ExistsByUserPhoneAsync(string phone);
    }
}
    