using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using Application.DTOs.Request;
using Application.Interfaces.IServicies;
using Infrastructure.Models;

namespace Infrastructure.Services
{
    public class PayOSService : IThirdPaymentService
    {
        private readonly PayOS _payOS;
        private readonly PayOSSettings _settings;

        public PayOSService(IOptions<PayOSSettings> options)
        {
            _settings = options.Value;
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
    }
}
