using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // danh mục ngành nghề
    public class JobCategoryListDTOResponse
    {
        public List<JobCategoryItemDTO> JobCategories { get; set; } = new();
    }
    public class JobCategoryItemDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

    }

}
