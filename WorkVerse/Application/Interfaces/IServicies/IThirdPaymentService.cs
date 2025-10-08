using Application.DTOs.Request;
using OpenAI.Graders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IThirdPaymentService
    {
        Task<string> CreatePaymentLink(string userId, SubscriptionPlanDTORequest plan);
        Task<PaymentDTORequest> VerifyWebhookDataAsync(string jsonBody);
    }
}
