using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interface;
using User.Domain.Modal;
using User.infrastructure.Data;

namespace User.infrastructure.Repository
{
    public class UserRepository(ApplicationDbContext _dbContext) : GenericRepository<UserModal>(_dbContext), IUserRepository
    {
        public async Task<bool> IsEmailRegistered(string email)
        {
            var IsRegistered = await _dbContext.User.AnyAsync(cutomer => cutomer.Email == email);
            return IsRegistered;
        }
    }
}
