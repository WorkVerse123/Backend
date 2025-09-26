using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class EmployerTypeDTOResponse
    {
        public int EmployerTypeId { get; set; }
        public string EmployerTypeName { get; set; } = null!;
    }
}
