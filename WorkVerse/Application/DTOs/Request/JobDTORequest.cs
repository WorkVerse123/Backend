using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class JobDTORequest
    {
        public int JobId { get; set; } //
        public int EmployerId { get; set; } //
        public string Title { get; set; } = null!; //
        public List<int> CategoryIds { get; set; } = new(); //
        public string Description { get; set; } = null!; //
        public string? Requirements { get; set; } //
        public string Location { get; set; } = null!; //
        public decimal SalaryMin { get; set; } //
        public decimal SalaryMax { get; set; } //
        public string JobTime { get; set; } = null!; // 
        public bool IsPriority { get; set; } = false;
        public DateTime CreatedAt { get; set; } //
        public DateTime ExpiredAt { get; set; } //
        public string Status { get; set; } = null!; //
    }
	public class JobFilterRequest
	{
		[MaxLength(200)]
		public string? Search { get; set; }  // tìm trong Title, Description, Requirements, Location

		public List<int>? CategoryId { get; set; } = new List<int>();

		[Range(0, double.MaxValue)]
		public decimal? SalaryMin { get; set; }

		[Range(0, double.MaxValue)]
		public decimal? SalaryMax { get; set; }

		public JobTimeType? JobTime { get; set; }


	}

public enum JobTimeType
	{
		[Description("Full-time")]
		FullTime,
		[Description("Part-time")]
		PartTime
	}



}
