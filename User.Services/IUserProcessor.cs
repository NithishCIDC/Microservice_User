using User.Application.DTO;
using User.Domain.Modal;

namespace User.Services
{
    public interface IUserProcessor
    {
        Task<UserModal> AddUser(UserDTO entity);
        Task<IEnumerable<UserModal>> GetAllUser();
        Task<UserModal> GetUserbyID(int id);
        Task EditUser(UserModal entity);
        Task<bool> DeleteUser(int id);
    }
}
