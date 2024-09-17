using DataAccess.Contracts;
using DataAccess.DataContext;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UOW
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _propertyRepository = new PropertyRepository(context);
            _userRepository = new UserRepository(context);
            _inquiryRepository = new InquiryRepository(context);
            _favoriteRepository = new FavoriteRepository(context);
        }

        public IPropertyRepository PropertyRepository { get { return _propertyRepository; } }
        public IUserRepository UserRepository => _userRepository;
        public IInquiryRepository InquiryRepository => _inquiryRepository;
        public IFavoriteRepository FavoriteRepository => _favoriteRepository;

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}