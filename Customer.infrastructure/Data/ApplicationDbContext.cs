using Microsoft.EntityFrameworkCore;
using User.Domain.Modal;

namespace User.infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<UserModal> Customer { get; set; }
    }
}
