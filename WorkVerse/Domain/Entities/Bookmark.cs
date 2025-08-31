using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Bookmark
    {
        public int BookmarkId { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public DateTime SavedAt { get; set; }

        public virtual EmployeeProfile Employee { get; set; } = null!;
        public virtual Job Job { get; set; } = null!;
    }
}
