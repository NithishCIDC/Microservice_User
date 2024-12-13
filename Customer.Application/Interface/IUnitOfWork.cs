namespace Customer.Application.Interface
{
    public interface IUnitOfWork
    {
        public ICustomerRepository CutomerRepository { get; }

        public IProductService ProductService { get; }
    }
}
