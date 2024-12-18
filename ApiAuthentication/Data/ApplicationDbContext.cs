using ApiAuthentication.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<UserModal> User { get; set; }
    }


}
