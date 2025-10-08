using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class EmployeeProfile
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? WorkExperience { get; set; }
        public string? Mode { get; set; }
        public string? SearchName { get; set; }
        public bool IsPriority { get; set; } = false;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<BusyTime> BusyTimes { get; set; } = new List<BusyTime>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
