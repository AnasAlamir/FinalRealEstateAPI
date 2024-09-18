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
        IEnumerable<PropertyImage> GetPropertyImages(int propertyId);
        void AddImageToProperty(PropertyImage propertyImage);
        void UpdateImageToProperty(PropertyImage propertyImage);
        int GetAmenitiesId(Amenities amenities);
    }
}
