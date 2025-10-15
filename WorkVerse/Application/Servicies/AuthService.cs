using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDTORespone?> ValidateUserAsync(string account, string password)
        {
            User? user = null;

            user = await _unitOfWork.User.GetByEmailAsync(account);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            return _mapper.Map<UserDTORespone>(user);
        }

        public async Task<UserDTORespone?> CreatedAccountAsync(UserDTORequest userDto)
        {
            var userEntity = _mapper.Map<User>(userDto);
            userEntity.PasswordHash = EncryptPassword(userDto.Password);

            await _unitOfWork.User.AddAsync(userEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDTORespone>(userEntity);
        }

        public async Task<bool> UpdatePasswordAsync(UserChangePasswordDTORequest user)
        {
            var newPasswordHash = EncryptPassword(user.NewPassword);
            var flag =  await _unitOfWork.User.UpdatePasswordAsynce(user.UserId, newPasswordHash);
            if (flag)
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<bool> IsPremiumAsync(int userId)
        {
            return _unitOfWork.User.IsPremiumAsync(userId);
        }
        public string EncryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }

        public async Task<bool> ExsitedUser(string? email)
        {
            var exists = await _unitOfWork.User.ExistsAsync(email);
            if (exists)
                return true;
            else
                return false;
        }
        public async Task<bool> ExsitedRole(int roleId)
        {
            var exists = await _unitOfWork.Role.ExistsByIdAsync(roleId);
            if (exists)
                return true;
            else
                return false;
        }

    }
}
