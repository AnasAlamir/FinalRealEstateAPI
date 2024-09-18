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
        void AddImageToProperty(int propertyId, PropertyImage propertyImage);
        void UpdateImageToProperty(int propertyId, PropertyImage propertyImage);
        int GetAmenitiesId(Amenities amenities);
    }
}
