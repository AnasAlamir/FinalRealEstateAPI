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
    internal class InquiryRepository : BaseRepository<Inquiry>, IInquiryRepository
    {
        public InquiryRepository(AppDbContext context) : base(context)
        {
        }
        public override IEnumerable<Inquiry> GetAll()
        {
            return _dbSet
                .Include(Inquiry => Inquiry.User) 
                .Include(Inquiry => Inquiry.Property);
        }
        public override Inquiry? Get(int id)
        {
            return _dbSet
                .Include(Inquiry => Inquiry.User)
                .Include(Inquiry => Inquiry.Property)
                .AsNoTracking()
                .FirstOrDefault(Inquiry => Inquiry.Id == id);
        }
        public bool IsDuplicateInquiry(int userId, int propertyId)
        {
            var value = _dbSet.FirstOrDefault(inquiry => inquiry.UserId == userId && inquiry.PropertyId == propertyId);
            return value != null;
        }
    }
}
