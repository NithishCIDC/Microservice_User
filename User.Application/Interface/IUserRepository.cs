using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Modal;

namespace User.Application.Interface
{
    public interface IUserRepository : IGenericRepository<UserModal>
    {
        Task<bool> IsEmailRegistered(string email);
    }
}
