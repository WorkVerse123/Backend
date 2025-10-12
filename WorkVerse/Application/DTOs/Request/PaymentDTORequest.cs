using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class PaymentDTORequest
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }
    public class PaymentFilterDTORequest
    {
        public int? UserId { get; set; }
        public int? PlanId { get; set; }
        public int? Type { get; set; } // 1 = Employee, 2 = Employer
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
