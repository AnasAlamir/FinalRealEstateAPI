using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        IPropertyRepository PropertyRepository { get; }
        IUserRepository UserRepository { get; }
        IInquiryRepository InquiryRepository { get; }
        IFavoriteRepository FavoriteRepository { get; }
        void Save();
    }
}
