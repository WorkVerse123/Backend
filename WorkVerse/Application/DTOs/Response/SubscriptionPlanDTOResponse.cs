using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class SubscriptionPlanDTOResponse
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; } = null!;
        public int Type { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? Features { get; set; }
    }
}
