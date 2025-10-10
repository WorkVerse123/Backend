using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ChartResponse
    {
        public ChartUserResponse UserStats { get; set; }
        public List<ChartJobResponse> JobStats { get; set; }
        public List<ChartPaymentResponse> PaymentStats { get; set; }
    }
    public class ChartUserResponse
    {
        public int TotalUsers { get; set; }
        public int TotalEmployers { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalStaffs { get; set; }
    }
    public class ChartJobResponse
    {
        public DateOnly Date { get; set; }
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int ClosedJobs { get; set; }
        public int Applications { get; set; }
    }
    public class ChartPaymentResponse
    {
        public DateOnly Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal EmployerPayments { get; set; }
        public decimal EmployeePayments { get; set; }
        public int TotalTransactions { get; set; }
        public int EmployerTransactions { get; set; }
        public int EmployeeTransactions { get; set; }
    }
}
