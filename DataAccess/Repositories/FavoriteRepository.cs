using DataAccess.Contracts;
using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context)
        {
        }
        public override IEnumerable<Favorite> GetAll()
        {
            return _dbSet
                .Include(favorite => favorite.User)
                .Include(favorite => favorite.Property);
        }
        public override Favorite? Get(int id)
        {
            return _dbSet
                .Include(favorite => favorite.User)
                .Include(favorite => favorite.Property)
                .FirstOrDefault(favorite => favorite.Id == id); ;
        }
    }
}
