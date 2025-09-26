using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace Application.DTOs.Email
{
	public class OtpEntry
	{
		public string HashedOtp { get; set; } = null!;
		public DateTime Expiry { get; set; }
		public int FailedAttempts { get; set; } = 0;
		public DateTime LastSent { get; set; }
		public int SentCount { get; set; } = 0;
		public DateTime? LockedUntil { get; set; }

		public bool IsLocked => LockedUntil != null && LockedUntil > DateTime.UtcNow;

		// Config
		public static int MaxFailedAttempts = 5; // Sai OTP tối đa 5 lần
		public static TimeSpan LockDuration = TimeSpan.FromMinutes(5); // Khóa sau khi nhập sai nhiều
		public static TimeSpan MinSendInterval = TimeSpan.FromMinutes(3); // Khoảng cách tối thiểu giữa 2 lần gửi
		public static int MaxSendsPerHour = 5; // Giới hạn 5 lần gửi/giờ
	}

	public static class OtpCache
	{
		private static readonly ConcurrentDictionary<string, OtpEntry> _cache = new();

		private static string GetKey(string email, string purpose) => $"{purpose}:{email}";

		public static (bool Success, string? ErrorMessage) SaveOtp(string email, string otp, TimeSpan validFor, string purpose = "Default")
		{
			var key = GetKey(email, purpose);

			if (_cache.TryGetValue(key, out var existing))
			{
				// Check spam interval
				if (DateTime.UtcNow - existing.LastSent < OtpEntry.MinSendInterval)
				{
					return (false, $"Bạn vừa yêu cầu OTP, vui lòng thử lại sau {OtpEntry.MinSendInterval.TotalMinutes} phút.");
				}

				// Check max sends per hour
				if (existing.SentCount >= OtpEntry.MaxSendsPerHour &&
					DateTime.UtcNow - existing.LastSent < TimeSpan.FromHours(1))
				{
					return (false, "Bạn đã vượt quá số lần gửi OTP cho phép trong 1 giờ. Vui lòng thử lại sau.");
				}
			}

			var entry = new OtpEntry
			{
				HashedOtp = BCrypt.Net.BCrypt.HashPassword(otp),
				Expiry = DateTime.UtcNow.Add(validFor),
				FailedAttempts = 0,
				LastSent = DateTime.UtcNow,
				SentCount = (existing?.SentCount ?? 0) + 1,
				LockedUntil = null
			};

			_cache[key] = entry;

			return (true, null);
		}

		public enum OtpVerifyResult
		{
			Success,
			Invalid,
			Expired,
			Locked
		}

		public static OtpVerifyResult VerifyOtp(string email, string otp, string purpose = "Default")
		{
			var key = GetKey(email, purpose);

			if (!_cache.TryGetValue(key, out var entry))
				return OtpVerifyResult.Invalid;

			if (entry.IsLocked)
				return OtpVerifyResult.Locked;

			if (entry.Expiry < DateTime.UtcNow)
			{
				_cache.TryRemove(key, out _);
				return OtpVerifyResult.Expired;
			}

			var isValid = BCrypt.Net.BCrypt.Verify(otp, entry.HashedOtp);
			if (isValid)
			{
				_cache.TryRemove(key, out _);
				return OtpVerifyResult.Success;
			}
			else
			{
				entry.FailedAttempts++;
				if (entry.FailedAttempts >= OtpEntry.MaxFailedAttempts)
				{
					entry.LockedUntil = DateTime.UtcNow.Add(OtpEntry.LockDuration);
				}
				return OtpVerifyResult.Invalid;
			}
		}


		public static void Remove(string email, string purpose = "Default")
		{
			var key = GetKey(email, purpose);
			_cache.TryRemove(key, out _);
		}
	}


}


