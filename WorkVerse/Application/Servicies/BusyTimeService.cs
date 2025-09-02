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
    public class BusyTimeService : IBusyTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BusyTimeService> _logger;

        public BusyTimeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BusyTimeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BusyTimeDTOResponse>> GetByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var entities = await _unitOfWork.BusyTime.GetByEmployeeIdAsync(employeeId);

                if (entities == null || !entities.Any())
                {
                    return Enumerable.Empty<BusyTimeDTOResponse>();
                }

                return _mapper.Map<IEnumerable<BusyTimeDTOResponse>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving BusyTime with EmployeeId: {Id}", employeeId);
                throw;
            }
        }

        public async Task<IEnumerable<BusyTimeDTOResponse>> CreateBusyTimesAsync(int employeeId, BusyTimeDTORequest request)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }

                var busyTimeEntities = new List<BusyTime>();

                foreach (var bt in request.BusyTimes)
                {
                    // Map string dayOfWeek -> (0–6)
                    var dayNumber = bt.DayOfWeek switch
                    {
                        "Sunday" => 0,
                        "Monday" => 1,
                        "Tuesday" => 2,
                        "Wednesday" => 3,
                        "Thursday" => 4,
                        "Friday" => 5,
                        "Saturday" => 6,
                        _ => throw new ArgumentException($"Invalid dayOfWeek: {bt.DayOfWeek}")
                    };

                    var isOverlap = await _unitOfWork.BusyTime.ExistsOverlapAsync(employeeId, (byte)dayNumber, bt.StartTime, bt.EndTime,null);

                    if (isOverlap)
                    {
                        throw new InvalidOperationException(
                            $"Busy time overlaps with existing schedule on {bt.DayOfWeek} ({bt.StartTime}-{bt.EndTime})");
                    }

                    busyTimeEntities.Add(new BusyTime
                    {
                        EmployeeId = employeeId,
                        DayOfWeek = (byte)dayNumber,
                        StartTime = bt.StartTime,
                        EndTime = bt.EndTime
                    });
                }

                await _unitOfWork.BusyTime.AddRangeAsync(busyTimeEntities);
                await _unitOfWork.SaveChangesAsync();

                return busyTimeEntities.Select(b => new BusyTimeDTOResponse
                {
                    BusyTimeId = b.BusyTimeId,
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), b.DayOfWeek) ?? b.DayOfWeek.ToString(),
                    StartTime = b.StartTime,
                    EndTime = b.EndTime
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating busy times for employee {EmployeeId}", employeeId);
                throw;
            }
        }

        public async Task<IEnumerable<BusyTimeDTOResponse>> UpdateBusyTimesAsync(int employeeId, BusyTimeDTORequest requests)
        {
            try
            {
                // Check employee
                var employee = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");

                // Lấy tất cả busyTimes của employee trước
                var existingBusyTimes = (await _unitOfWork.BusyTime.GetByEmployeeIdAsync(employeeId)).ToList();

                foreach (var request in requests.BusyTimes)
                {
                    // Kiểm tra request có busyTimeId không
                    if (request.BusyTimeId == null)
                        throw new ArgumentException("BusyTimeId is required for update");

                    var busyTime = existingBusyTimes.FirstOrDefault(b => b.BusyTimeId == request.BusyTimeId);
                    if (busyTime == null)
                        throw new KeyNotFoundException(
                            $"BusyTime with ID {request.BusyTimeId} not found for Employee {employeeId}");
                    
                    // Map string dayOfWeek -> số (0-6)
                    var dayNumber = request.DayOfWeek switch
                    {
                        "Sunday" => 0,
                        "Monday" => 1,
                        "Tuesday" => 2,
                        "Wednesday" => 3,
                        "Thursday" => 4,
                        "Friday" => 5,
                        "Saturday" => 6,
                        _ => throw new ArgumentException($"Invalid dayOfWeek: {request.DayOfWeek}")
                    };

                    // Kiểm tra overlap với những busyTime khác (ngoại trừ chính nó)
                    var isOverlap = await _unitOfWork.BusyTime.ExistsOverlapAsync(
                        employeeId, (byte)dayNumber, request.StartTime, request.EndTime, excludeBusyTimeId: request.BusyTimeId.Value);

                    if (isOverlap)
                        throw new InvalidOperationException(
                            $"Busy time overlaps with existing schedule on {request.DayOfWeek} ({request.StartTime}-{request.EndTime})");

                    // Update entity
                    busyTime.DayOfWeek = (byte)dayNumber;
                    busyTime.StartTime = request.StartTime;
                    busyTime.EndTime = request.EndTime;

                    _unitOfWork.BusyTime.Update(busyTime);
                }

                // Save changes 1 lần   
                await _unitOfWork.SaveChangesAsync();

                // Trả về tất cả sau update
                var updatedBusyTimes = await _unitOfWork.BusyTime.GetByEmployeeIdAsync(employeeId);
                return updatedBusyTimes.Select(b => new BusyTimeDTOResponse
                {
                    BusyTimeId = b.BusyTimeId,
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), b.DayOfWeek) ?? b.DayOfWeek.ToString(),
                    StartTime = b.StartTime,
                    EndTime = b.EndTime
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating busy times for employee {EmployeeId}", employeeId);
                throw;
            }
        }

        public async Task<bool> DeleteBusyTimesAsync(int employeeId, int busyTimeId)
        {
            try
            {
            
                var employee = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");

              
                var busyTime = await _unitOfWork.BusyTime.GetByIdAsync(busyTimeId);
                if (busyTime == null || busyTime.EmployeeId != employeeId)
                    throw new KeyNotFoundException(
                        $"BusyTime with ID {busyTimeId} not found for Employee {employeeId}");

               
                _unitOfWork.BusyTime.HardRemove(busyTime);

              
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting busy time {BusyTimeId} for employee {EmployeeId}", busyTimeId, employeeId);
                throw;
            }
        }


    }
}
