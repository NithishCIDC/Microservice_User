namespace User.Application.Interface
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        public IProductService ProductService { get; }
    }
}
