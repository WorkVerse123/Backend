using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IUserService
    {
        Task<bool> UpdateStatusAsync(int userId, string newStatus);
    }
}
