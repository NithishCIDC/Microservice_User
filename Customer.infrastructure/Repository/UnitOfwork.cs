using Customer.Application.Interface;
using Customer.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.infrastructure.Repository
{
    public class UnitOfwork(ApplicationDbContext _dbContext) : IUnitOfWork
    {
        public ICustomerRepository CutomerRepository { get; private set; } = new CustomerRepository(_dbContext);
    }
}
