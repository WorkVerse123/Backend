using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class BusyTimeValidationHelper
    {
        private static readonly string[] AllowedDays =
            { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        public static (bool IsValid, string ErrorMessage) ValidateBusyTimeRequest(BusyTimeDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            if (request.BusyTimes == null || request.BusyTimes.Count == 0)
                return (false, "At least one busy time is required.");

            foreach (var busyTime in request.BusyTimes)
            {
                // DayOfWeek
                if (string.IsNullOrWhiteSpace(busyTime.DayOfWeek))
                    return (false, "DayOfWeek is required.");
                if (!AllowedDays.Contains(busyTime.DayOfWeek))
                    return (false, $"DayOfWeek must be one of: {string.Join(", ", AllowedDays)}.");

                // StartTime & EndTime
                if (busyTime.StartTime >= busyTime.EndTime)
                    return (false, "BusyTime: StartTime must be earlier than EndTime (no overnight times allowed).");

            }

            // Check overlap trong cùng 1 ngày
            foreach (var group in request.BusyTimes.GroupBy(b => b.DayOfWeek))
            {
                var times = group.OrderBy(t => t.StartTime).ToList();
                for (int i = 0; i < times.Count - 1; i++)
                {
                    if (times[i].EndTime > times[i + 1].StartTime)
                        return (false, $"Overlapping busy times found for {group.Key}.");
                }
            }

            return (true, string.Empty);
        }
    }

}
