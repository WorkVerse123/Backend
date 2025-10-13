using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public static class GenericMatchAI
    {
        private static List<string> Tokenize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new List<string>();

            return input
                .ToLower()
                .Split(new[] { ' ', ',', '.', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        /// <summary>
        /// Match chung cho bất kỳ field dạng string + query list
        /// </summary>
        public static bool MatchAnyField(string fieldValue, List<string>? queryList, int minCommonTokens = 2)
        {
            if (string.IsNullOrWhiteSpace(fieldValue) || queryList == null || !queryList.Any())
                return false;

            var fieldTokens = Tokenize(fieldValue);

            foreach (var query in queryList)
            {
                var queryTokens = Tokenize(query);

                // Đếm số token chung
                int common = queryTokens.Count(q => fieldTokens.Contains(q));

                if (common >= minCommonTokens)
                    return true;
            }

            return false;
        }

    }
}
