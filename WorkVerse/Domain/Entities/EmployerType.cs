using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class EmployerType
    {
        public int EmployerTypeId { get; set; }
        public string EmployerTypeName { get; set; } = null!;

        public virtual ICollection<EmployerProfile> EmployerProfiles { get; set; } = new List<EmployerProfile>();
    }
}
