using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using Application.DTOs.Request;
using Application.Interfaces.IServicies;
using Infrastructure.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class PayOSService : IThirdPaymentService
    {
        private readonly PayOS _payOS;
        private readonly PayOSSettings _settings;
        private readonly IPaymentService _paymentService;

        public PayOSService(IConfiguration config, IPaymentService paymentService)
        {
            _settings = config.GetSection("PayOS").Get<PayOSSettings>();
            _payOS = new PayOS(_settings.ClientId, _settings.ApiKey, _settings.ChecksumKey);
            _paymentService = paymentService;
        }

        public async Task<string> CreatePaymentLink(string userId, SubscriptionPlanDTORequest plan)
        {
            int id = int.Parse(userId);


            var paymentDto = new PaymentDTORequest
            {
                UserId = id,
                PlanId = plan.PlanId,
                Amount = plan.Price,
                PaymentMethod = "PayOS",
                PaymentDate = DateTime.UtcNow,
                Status = "Pending"
            };

            var payment = await _paymentService.AddAsync(paymentDto);
            var orderCode = payment.PaymentId;
            var description = $"UserId:{userId} | PlanId:{plan.PlanId} | Thanh toán gói {plan.PlanName}";

            var items = new List<ItemData>
            {
                new ItemData(plan.PlanName, 1, (int)plan.Price)
            }; 

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)plan.Price,
                description: plan.Description ?? $"Thanh toán gói {plan.PlanName}",
                items: items,
                cancelUrl: _settings.CancelUrl,
                returnUrl: _settings.ReturnUrl
            );

            var result = await _payOS.createPaymentLink(paymentData);

            return result.checkoutUrl;
        }


        /// <summary>
        /// Xác thực chữ ký (signature) trong webhook từ payOS
        /// </summary>
        public async Task<PaymentDTORequest> VerifyWebhookDataAsync(string jsonBody)
        {
            if (string.IsNullOrWhiteSpace(jsonBody))
                throw new ArgumentException("Webhook body is empty");

            try
            {
                var body = JsonConvert.DeserializeObject<WebhookType>(jsonBody)
                           ?? throw new InvalidDataException("Invalid webhook JSON");

                var verified = _payOS.verifyPaymentWebhookData(body); 

                var desc = verified.description ?? string.Empty;

                var userIdMatch = Regex.Match(desc, @"UserId:(\d+)");
                var planIdMatch = Regex.Match(desc, @"PlanId:(\d+)");

                var userId = userIdMatch.Success ? int.Parse(userIdMatch.Groups[1].Value) : 0;
                var planId = planIdMatch.Success ? int.Parse(planIdMatch.Groups[1].Value) : 0;

                return new PaymentDTORequest
                {
                    PaymentId = (int)verified.orderCode,
                    UserId = userId,
                    PlanId = planId,
                    Amount = verified.amount,
                    PaymentMethod = "PayOS",
                    PaymentDate = DateTime.Parse(verified.transactionDateTime),
                    Status = verified.code == "PAYMENT_SUCCESS" ? "Completed" : "Failed"
                };
            }
            catch (JsonException)
            {
                throw new InvalidDataException("Cannot parse webhook JSON");
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid signature — request not trusted. Details: {ex.Message}");
            }
        }


    }
}
