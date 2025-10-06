using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using Application.DTOs.Request;
using Application.Interfaces.IServicies;
using Infrastructure.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class PayOSService : IThirdPaymentService
    {
        private readonly PayOS _payOS;
        private readonly PayOSSettings _settings;

        public PayOSService(IConfiguration config)
        {
            _settings = config.GetSection("PayOS").Get<PayOSSettings>();
            _payOS = new PayOS(_settings.ClientId, _settings.ApiKey, _settings.ChecksumKey);
        }

        public async Task<string> CreatePaymentLink(SubscriptionPlanDTORequest plan)
        {
            var items = new List<ItemData> { new ItemData(plan.PlanName, 1, (int)plan.Price) };

            var paymentData = new PaymentData(
                orderCode: plan.PlanId,
                amount: (int)plan.Price,
                description: plan.Description,
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
        public WebhookData VerifyWebhookData(string jsonBody)
        {
            if (string.IsNullOrWhiteSpace(jsonBody))
                throw new ArgumentException("Webhook body is empty");

            try
            {
                var body = JsonConvert.DeserializeObject<WebhookType>(jsonBody)
                           ?? throw new InvalidDataException("Invalid webhook JSON");

                return _payOS.verifyPaymentWebhookData(body);
            }
            catch (JsonException)
            {
                throw new InvalidDataException("Cannot parse webhook JSON");
            }
            catch
            {
                throw new Exception("Invalid signature — request not trusted.");
            }
        }

    }
}
