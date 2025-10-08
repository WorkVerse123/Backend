using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IPaymentService
    {
        Task<PaymentDTOResponse> GetById(int id);
        Task<PaymentDTOResponse> AddAsync(PaymentDTORequest paymentDto);
        Task<PaymentDTOResponse> UpdateAsync(PaymentDTORequest paymentDto);
    }
}
