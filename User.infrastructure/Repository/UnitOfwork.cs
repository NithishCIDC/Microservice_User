using User.Application.Interface;
using User.infrastructure.Data;

namespace User.infrastructure.Repository
{
    public class UnitOfwork: IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpClientFactory _httpClient;

        public UnitOfwork(ApplicationDbContext dbContext, IHttpClientFactory httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }
        public IUserRepository UserRepository => new UserRepository(_dbContext);

        public IProductService ProductService => new ProductService(_httpClient);

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
