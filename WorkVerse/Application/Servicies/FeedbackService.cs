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
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FeedbackService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FeedbackItemDTOResponse> CreateFeedbackAsync(SendFeedbackDTORequest request)
        {
            try
            {

                var user = await _unitOfWork.User.ExistByIdAsync(request.UserId);
                if (!user)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} not found");
                }

                var feedback = _mapper.Map<Feedback>(request);

                await _unitOfWork.Feedback.AddAsync(feedback);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<FeedbackItemDTOResponse>(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating feedback with user{request.UserId}");
                throw;
            }
        }

        public async Task<FeedbackListDTOResponse> GetFeedbackListAsync(int pageNumber, int pageSize)
        {
            var query = (await _unitOfWork.Feedback.GetAllFeedbackDetailsAsync())
                            .AsQueryable()
                            .OrderByDescending(f => f.CreatedAt);

            var totalRecords = query.Count();

            var pagedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new List<FeedbackItemDetailsDTOResponse>();

            foreach (var feedback in pagedData)
            {
                // Lấy fullname của user (employee/employer)
                string userFullName = feedback.User.EmployeeProfile?.FullName
                                    ?? feedback.User.EmployerProfile?.CompanyName
                                    ?? "(Unknown User)";

                // Lấy fullname của handler (staff) nếu có
                string? handlerFullName = feedback.Handler?.StaffProfile?.FullName;

                result.Add(new FeedbackItemDetailsDTOResponse
                {
                    FeedbackId = feedback.FeedbackId,
                    Content = feedback.Content,
                    CreatedAt = feedback.CreatedAt,
                    Status = feedback.HandledAt.HasValue ? "handled" : "pending",
                    User = new FeedbackUserDTOResponse
                    {
                        UserId = feedback.User.UserId,
                        FullName = userFullName
                    },
                    Handler = feedback.Handler == null ? null : new FeedbackHandlerDTOResponse
                    {
                        UserId = feedback.Handler.UserId,
                        FullName = handlerFullName ?? "(Unknown Staff)"
                    },
                    HandledAt = feedback.HandledAt
                });
            }

            return new FeedbackListDTOResponse
            {
                Paging = new PaginatedResponse
                {
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                },
                Feedbacks = result
            };
        }

        public async Task<bool> UpdateFeedbackHandleAsync(int feedbackId, UpdateFeedbackHandlerDTORequest request)
        {
            try
            {
                var feedback = await _unitOfWork.Feedback.GetFeedbackByIdAsync(feedbackId);
                if (feedback == null)
                {
                    throw new KeyNotFoundException($"Feedback with ID {feedbackId} not found.");
                }
                var user = await _unitOfWork.User.ExistByIdAsync(request.HandleBy);
                if (!user)
                {
                    throw new KeyNotFoundException($"User with ID {request.HandleBy} not found.");
                }
                feedback.HandledBy = request.HandleBy;
                feedback.HandledAt = DateTime.Now;
                _unitOfWork.Feedback.Update(feedback);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing status of Feedback with ID: {Id}", feedbackId);
                throw;
            }

        }
    }
}
