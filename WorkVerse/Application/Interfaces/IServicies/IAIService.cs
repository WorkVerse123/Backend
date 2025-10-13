using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IAIService
    {
        Task<string> ExtractPromptEmployeeAsync(string userQuery);
        Task<string> ExtractPromptEmployerAsync(string userQuery);
    }
}
