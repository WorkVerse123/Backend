using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class PaymentDTOResponse
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } = null!;
        public UserPaymentRespone User { get; set; } = null!;
        public PlanPaymentDTORespone Plan { get; set; } = null!;
    }
}
