using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Modal;

namespace User.Application.Interface
{
    public interface ICustomerRepository : IGenericRepository<UserModal>
    {
        Task<bool> GetByEmail(string email, string password);
    }
}
