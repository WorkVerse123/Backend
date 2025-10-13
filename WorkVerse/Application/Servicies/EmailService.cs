using Application.DTOs.Email;
using Application.Interfaces.IServicies;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _settings;

		public EmailService(IOptions<EmailSettings> options)
		{
			_settings = options.Value;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string body)
		{
			using var smtpClient = new SmtpClient(_settings.SmtpServer)
			{
				Port = _settings.SmtpPort,
				Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
				EnableSsl = true
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_settings.SenderEmail),
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			};

			mailMessage.To.Add(toEmail);

			await smtpClient.SendMailAsync(mailMessage);
		}

	}
}
