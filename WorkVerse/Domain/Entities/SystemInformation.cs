using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class SystemInformation
    {
        public int InfoId { get; set; }
        public string PageKey { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Language { get; set; } = null!;
        public DateTime UpdatedAt { get; set; }
    }
}
