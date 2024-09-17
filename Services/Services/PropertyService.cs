using DataAccess.Contracts;
using DataAccess.Models;
using Services.Contracts;
using Services.Dto.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    internal class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PropertyDto> GetAllProperties()
        {
            try
            {
                return _unitOfWork.Property.GetAll();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving properties.", ex);
            }
        }

        public PropertyDto GetPropertyById(int id)
        {
            try
            {
                var property = _unitOfWork.Property.Get(id);
                if (property == null)
                {
                    throw new KeyNotFoundException("Property not found.");
                }
                return property;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving the property.", ex);
            }
        }

        public void CreateProperty(PropertyInsertDto property)
        {
            ValidateProperty(property);

            try
            {
                _unitOfWork.Property.Insert(property);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while creating the property.", ex);
            }
        }

        public void UpdateProperty(PropertyUpdateDto property)
        {
            ValidateProperty(property);

            try
            {
                var existingProperty = _unitOfWork.Property.Get(property.Id);
                if (existingProperty == null)
                {
                    throw new KeyNotFoundException("Property not found.");
                }

                _unitOfWork.Property.Update(property);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while updating the property.", ex);
            }
        }

        public void DeleteProperty(int id)
        {
            try
            {
                var property = _unitOfWork.Property.Get(id);
                if (property == null)
                {
                    throw new KeyNotFoundException("Property not found.");
                }

                _unitOfWork.Property.Delete(id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while deleting the property.", ex);
            }
        }

        public IEnumerable<PropertyDto> GetPropertiesByPrice(decimal minPrice, decimal maxPrice)
        {
            try
            {
                return _unitOfWork.Property.GetAll()
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving properties by price.", ex);
            }
        }

        public IEnumerable<PropertyDto> SearchProperties(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    throw new ArgumentException("Search term cannot be null or empty.", nameof(searchTerm));
                }

                searchTerm = searchTerm.ToLower();
                return _unitOfWork.Property.GetAll()
                    .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                                p.Description.ToLower().Contains(searchTerm) ||
                                p.Address.ToLower().Contains(searchTerm))
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while searching for properties.", ex);
            }
        }

        public IEnumerable<PropertyDto> FilterProperties(
            string location = null,
            string propertyType = null,
            int? minBedrooms = null,
            int? maxBedrooms = null,
            int? minBathrooms = null,
            int? maxBathrooms = null)
        {
            try
            {
                var properties = _unitOfWork.Property.GetAll().AsQueryable();

                if (!string.IsNullOrEmpty(location))
                    properties = properties.Where(p => p.Address.Contains(location));

                if (!string.IsNullOrEmpty(propertyType))
                    properties = properties.Where(p => p.PropertyType.Contains(propertyType));

                if (minBedrooms.HasValue)
                    properties = properties.Where(p => p.BedroomsNumber >= minBedrooms.Value);

                if (maxBedrooms.HasValue)
                    properties = properties.Where(p => p.BedroomsNumber <= maxBedrooms.Value);

                if (minBathrooms.HasValue)
                    properties = properties.Where(p => p.BathroomsNumber >= minBathrooms.Value);

                if (maxBathrooms.HasValue)
                    properties = properties.Where(p => p.BathroomsNumber <= maxBathrooms.Value);

                return properties.ToList();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while filtering properties.", ex);
            }
        }

        public IEnumerable<PropertyDto> GetPropertiesOrderedByPrice(bool ascending = true)
        {
            try
            {
                return ascending
                    ? _unitOfWork.Property.GetAll().OrderBy(p => p.Price).ToList()
                    : _unitOfWork.Property.GetAll().OrderByDescending(p => p.Price).ToList();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while ordering properties by price.", ex);
            }
        }

        public IEnumerable<PropertyDto> GetPropertiesOrderedByDateAdded(bool ascending = true)
        {
            try
            {
                return ascending
                    ? _unitOfWork.Property.GetAll().OrderBy(p => p.DateAdded).ToList()
                    : _unitOfWork.Property.GetAll().OrderByDescending(p => p.DateAdded).ToList();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while ordering properties by date added.", ex);
            }
        }

        public IEnumerable<PropertyDto> GetPropertiesByUserId(int userId)
        {
            try
            {
                return _unitOfWork.Property.GetAll()
                    .Where(p => p.UserId == userId)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving properties by user ID.", ex);
            }
        }

        public IEnumerable<PropertyDto> GetPropertiesOrderedByDate()
        {
            try
            {
                return _unitOfWork.Property.GetAll()
                    .OrderBy(p => p.DateAdded)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving properties ordered by date.", ex);
            }
        }

        // Private method to validate property data
        private void ValidateProperty(PropertyDto property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            if (string.IsNullOrEmpty(property.Name))
                throw new ArgumentException("Property title is required.", nameof(property.Name));

            if (property.Price <= 0)
                throw new ArgumentException("Property price must be greater than zero.", nameof(property.Price));

            if (property.BedroomsNumber < 0)
                throw new ArgumentException("Number of bedrooms cannot be negative.", nameof(property.BedroomsNumber));

            if (property.BathroomsNumber < 0)
                throw new ArgumentException("Number of bathrooms cannot be negative.", nameof(property.BathroomsNumber));

            // Add any additional validations as needed
        }
    }
}
