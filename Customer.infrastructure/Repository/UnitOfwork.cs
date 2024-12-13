using Customer.Application.Interface;
using Customer.infrastructure.Data;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.infrastructure.Repository
{
    public class UnitOfwork(ApplicationDbContext _dbContext,IHttpClientFactory httpClient) : IUnitOfWork
    {
        public ICustomerRepository CutomerRepository { get; private set; } = new CustomerRepository(_dbContext);

        public IProductService ProductService { get; private set; } = new ProductService(httpClient);
    }
}
