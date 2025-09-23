using Application.Interfaces.IServicies;
using GenerativeAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class AIService : IAIService
    {
        private readonly GoogleAi _googleAi;
        private readonly GenerativeModel _geminiModel;
        private readonly string _employeePrompt;
        private readonly string _employerPrompt;
        public AIService(string apiKey)
        {
            _googleAi = new GoogleAi(apiKey);
            _geminiModel = _googleAi.CreateGenerativeModel("gemini-2.0-flash"); // chọn model bạn muốn dùng
                                                                                // Đọc prompt từ file .txt
            var employeePromptPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "EmployeePrompt.txt");
            var employerPromptPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "EmployerPrompt.txt");
            _employeePrompt = File.ReadAllText(employeePromptPath);
            _employerPrompt = File.ReadAllText(employerPromptPath);

        }

        public async Task<string> ExtractPromptEmployeeAsync(string userQuery)
        {
            var prompt = $"{_employeePrompt}\n\nUser: {userQuery}";

            var response = await _geminiModel.GenerateContentAsync(prompt);

            return response.Text;
        }

        public async Task<string> ExtractPromptEmployerAsync(string userQuery)
        {
            var prompt = $"{_employerPrompt}\n\nUser: {userQuery}";

            var response = await _geminiModel.GenerateContentAsync(prompt);

            return response.Text;
        }
    }
}
