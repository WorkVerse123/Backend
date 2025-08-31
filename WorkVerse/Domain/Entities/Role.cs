using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!; // Admin, Staff, Employer, Employee

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
