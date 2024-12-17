using Customer.Domain.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Application.Interface
{
    public interface ICustomerRepository : IGenericRepository<CustomerModal>
    {
        Task<bool> GetByEmail(string email, string password);
    }
}
