using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class JobCategoryMapping
    {
        public int JobId { get; set; }
        public int CategoryId { get; set; }

        public virtual Job Job { get; set; } = null!;
        public virtual JobCategory Category { get; set; } = null!;
    }
}
