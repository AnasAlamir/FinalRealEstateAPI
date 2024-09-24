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
    internal class PropertyRepository :BaseRepository<Property>,IPropertyRepository
    {
        protected readonly DbSet<PropertyImage> _dbSetPropertyImage;
        public PropertyRepository(AppDbContext context) :base(context)
        {
            _dbSetPropertyImage = context.Set<PropertyImage>();
        }
        public override IEnumerable<Property> GetAll()
        {
            return _dbSet
                .Include(property => property.User)
                .Include(property => property.City)
                .Include(property => property.PropertyType)
                .Include(property => property.PropertyStatus)
                .Include(property => property.Amenities);

        }
        public override Property? Get(int id)
        {
            return _dbSet
                .Include(property => property.User)
                .Include(property => property.City)
                .Include(property => property.PropertyType)
                .Include(property => property.PropertyStatus)
                .Include(property => property.Amenities)
                .FirstOrDefault(p=>p.Id == id);
        }
        //public IEnumerable<PropertyImage> GetPropertyImages(int propertyId)
        //{
        //    return _dbSetPropertyImage.Where(i => i.PropertyId == propertyId);
        //}
        public IEnumerable<PropertyImage> GetPropertyImages(int propertyId)
        {
            return _dbSetPropertyImage.AsNoTracking().Where(i => i.PropertyId == propertyId);
        }

        public void AddImageToProperty(PropertyImage propertyImage)
        {
            _dbSetPropertyImage.Add(propertyImage);
        }
        public void UpdateImageToProperty(PropertyImage propertyImage)
        { 
                _dbSetPropertyImage.Update(propertyImage);
        }      

        public int GetAmenitiesId(Amenities amenities)
        {
            string numberInBinary = null;
            numberInBinary += amenities.IsFurnished ? "1" : "0";
            numberInBinary += amenities.HasCentralHeating ? "1" : "0";
            numberInBinary += amenities.HasParking ? "1" : "0";
            numberInBinary += amenities.HasBalcony ? "1" : "0";
            numberInBinary += amenities.HasElevator ? "1" : "0";
            numberInBinary += amenities.HasGarden ? "1" : "0";
            numberInBinary += amenities.HasPool ? "1" : "0";
            numberInBinary += amenities.Laundry_Room ? "1" : "0";
            numberInBinary += amenities.Two_Stories ? "1" : "0";
            numberInBinary += amenities.HasGarage ? "1" : "0";
            int amenityId = Convert.ToInt32(numberInBinary, 2) + 1;
            return amenityId;
        }
        public bool IsDuplicateProperty(string name, string address, int userId)
        {
        var value =  _dbSet
                .FirstOrDefault(p => p.Name == name && p.Address == address && p.UserId == userId);
            return value != null;
        }
        public bool IsDuplicatePropertyImage(string path)
        {
            var value = _dbSetPropertyImage
                    .FirstOrDefault(pi => pi.Path == path);
            return value != null;
        }
    }
}
