using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServicies;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PaymentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentDTOResponse> AddAsync(PaymentDTORequest paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            await _unitOfWork.Payment.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            var resultDto = _mapper.Map<PaymentDTOResponse>(payment);
            return resultDto;
        }

        public async Task<PaymentDTOResponse> GetById(int id)
        {
            var payment = await _unitOfWork.Payment.GetPaymentAsync(id);
            if (payment == null)
            {
                _logger.LogWarning($"Payment with id {id} not found.");
                return null;
            }
            var paymentDto = _mapper.Map<PaymentDTOResponse>(payment);
            return paymentDto;
        }
        public async Task<PaymentDTOResponse> UpdateAsync(PaymentDTORequest paymentRequest)
        {
            var payment = await _unitOfWork.Payment.GetPaymentAsync(paymentRequest.PaymentId);
            if (payment == null)
            {
                _logger.LogWarning($"Payment with id {paymentRequest.PaymentId} not found for update.");
                return null;
            }

            _mapper.Map(paymentRequest, payment);

            _unitOfWork.Payment.Update(payment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PaymentDTOResponse>(payment);
        }

    }
}
