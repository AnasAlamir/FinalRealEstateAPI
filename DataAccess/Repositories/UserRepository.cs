using DataAccess.Contracts;
using DataAccess.DataContext;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
        public bool IsDuplicateUser(string email, string phoneNumber)
        {
            var value = _dbSet.FirstOrDefault(user => user.Email == email || user.PhoneNumber == phoneNumber);
            return value != null;
        }
    }
}
