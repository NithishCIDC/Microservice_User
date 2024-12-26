using Mapster;
using User.Application.DTO;
using User.Application.Interface;
using User.Domain.Modal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace User.Services
{
    public class UserProcessor : IUserProcessor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpContext _context;
        public UserProcessor(IUnitOfWork unitOfWork, IHttpContextAccessor context) 
        {
            _unitOfWork = unitOfWork;
            _context = context.HttpContext!;
        }

        public async Task<UserModal> AddUser(UserDTO entity)
        {
            if (await _unitOfWork.UserRepository.IsEmailRegistered(entity.Email))
            {
                return (UserModal)null!;
            }
            var UserData = entity.Adapt<UserModal>();
            UserData.CreatedDateTime = DateTime.Now;
            UserData.CreatedBy = Convert.ToInt32(_context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserData.UpdatedDateTime = DateTime.Now;
            UserData.UpdatedBy = Convert.ToInt32(_context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _unitOfWork.UserRepository.AddAsync(UserData);
            await _unitOfWork.UserRepository.SaveAsync();
            return UserData;
        }

        public async Task<IEnumerable<UserModal>> GetAllUser()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<UserModal> GetUserbyID(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return user;
        }

        public async Task EditUser(UserModal UserData)
        {
            UserData.UpdatedDateTime = DateTime.Now;
            UserData.UpdatedBy = Convert.ToInt32(_context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _unitOfWork.UserRepository.Update(UserData);
            await _unitOfWork.UserRepository.SaveAsync();
        }

        public async Task<bool> DeleteUser(int id)
        {
            var User = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (User == null)
            {
                return false;
            }
            _unitOfWork.UserRepository.Delete(User);
            await _unitOfWork.UserRepository.SaveAsync();
            return true;
        }
    }
}
