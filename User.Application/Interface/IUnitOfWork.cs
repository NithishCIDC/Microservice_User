namespace User.Application.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }

        public IProductService ProductService { get; }
    }
}
