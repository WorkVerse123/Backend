using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int? HandledBy { get; set; }
        public DateTime? HandledAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual User? Handler { get; set; }
    }
}
