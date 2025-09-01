using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class JobCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<JobCategoryMapping> JobCategoryMappings { get; set; } = new List<JobCategoryMapping>();
    }
}
