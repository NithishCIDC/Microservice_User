using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interface;
using User.Domain.Modal;
using User.infrastructure.Data;

namespace User.infrastructure.Repository
{
    public class CustomerRepository(ApplicationDbContext _dbContext) : GenericRepository<UserModal>(_dbContext), ICustomerRepository
    {
        public async Task<bool> GetByEmail(string email, string password)
        {
            var customer = await _dbContext.Customer.Where(cutomer => cutomer.Email == email).FirstOrDefaultAsync();
            if (customer == null) return false;
            if (customer.Password != password) return false;
            return true;
        }
    }
}
