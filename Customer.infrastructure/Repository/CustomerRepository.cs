using Customer.Application.Interface;
using Customer.Domain.Modal;
using Customer.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.infrastructure.Repository
{
    public class CustomerRepository(ApplicationDbContext _dbContext) : GenericRepository<CustomerModal>(_dbContext), ICustomerRepository
    {

    }
}
