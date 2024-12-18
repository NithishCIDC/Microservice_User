namespace User.Application.Interface
{
    public interface IUnitOfWork
    {
        public IUserRepository CutomerRepository { get; }

        public IProductService ProductService { get; }
    }
}
