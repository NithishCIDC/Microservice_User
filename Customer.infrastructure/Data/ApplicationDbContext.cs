using Customer.Domain.Modal;
using Microsoft.EntityFrameworkCore;

namespace Customer.infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<CustomerModal> Customer { get; set; }
        public DbSet<OrderModal> Orders { get; set; }
    }
}
