using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class UserSubscription
    {
        public int UserSubscriptionId { get; set; }
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual SubscriptionPlan Plan { get; set; } = null!;
    }
}
