using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IPropertyRepository : IBaseRepository<Property>
    {
        void AddImageToProperty(int propertyId, PropertyImage propertyImage);
        public int GetAmenitiesId(Amenities amenities);
    }
}
