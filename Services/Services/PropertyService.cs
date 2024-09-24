using DataAccess.Contracts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Contracts;
using Services.Dto.Inquiry;
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
        public IEnumerable<InquiryDto> GetPropertyInquiries(int propertyId)
        {
            try
            {
                var inquiries = _unitOfWork.InquiryRepository.GetAll()
                    .Where(i => i.PropertyId == propertyId);
                return inquiries.Select(inquiry => new InquiryDto
                {
                    InquiryId = inquiry.Id,
                    UserName = inquiry.User.FullName,
                    PropertyName = inquiry.Property.Name,
                    InquiryDateSent = inquiry.DateSent,
                    InquiryMessage = inquiry.Message
                });
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving Property's inquiries.", ex);
            }
        }
        public IEnumerable<PropertyDto> GetAllProperties()
        {
            try
            {
                return _unitOfWork.PropertyRepository.GetAll()
                    .Select(property => PropertyToPropertyDto(property));
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
                var property = _unitOfWork.PropertyRepository.Get(id);
                if (property == null)
                {
                    throw new KeyNotFoundException("Property not found.");
                }
                return PropertyToPropertyDto(property);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving the property.", ex);
            }
        }

        public void CreateProperty(PropertyInsertDto propertyInsertDto)//check userid,ids
        {
            var property = new DataAccess.Models.Property
            {
                Name = propertyInsertDto.PropertyName,
                Address = propertyInsertDto.PropertyAddress,
                CityId = propertyInsertDto.PropertyCityId,
                AreaInMeters = propertyInsertDto.AreaInMeters,
                BathroomsNumber = propertyInsertDto.BathroomsNumber,
                BedroomsNumber = propertyInsertDto.BedroomsNumber,
                DateAdded = DateTime.Now,
                Description = propertyInsertDto.Description,
                AdditionalNotes = propertyInsertDto.AdditionalNotes,
                Price = propertyInsertDto.PropertyPrice,
                PropertyStatusId = propertyInsertDto.PropertyStatusId,
                YearBuilt = propertyInsertDto.YearBuilt,
                UserId = propertyInsertDto.UserId,
                PropertyTypeId = propertyInsertDto.PropertyTypeId,
            };
            var propertyAmenities = new Amenities
            {
                HasGarage = propertyInsertDto.HasGarage,
                Two_Stories = propertyInsertDto.Two_Stories,
                Laundry_Room = propertyInsertDto.Laundry_Room,
                HasPool = propertyInsertDto.HasPool,
                HasGarden = propertyInsertDto.HasGarden,
                HasElevator = propertyInsertDto.HasElevator,
                HasBalcony = propertyInsertDto.HasBalcony,
                HasParking = propertyInsertDto.HasParking,
                HasCentralHeating = propertyInsertDto.HasCentralHeating,
                IsFurnished = propertyInsertDto.IsFurnished
            };
            property.AmenitiesId = _unitOfWork.PropertyRepository.GetAmenitiesId(propertyAmenities);


            ValidateProperty(property);

            try
            {
                if(_unitOfWork.PropertyRepository.IsDuplicateProperty(property.Name, property.Address, property.UserId))
                {
                    throw new ApplicationException("Property already with the user can not insert.");
                }
                _unitOfWork.PropertyRepository.Insert(property);
                _unitOfWork.Save();
                var lastInsertedProperty = _unitOfWork.PropertyRepository.GetAll()
                                     .OrderByDescending(p => p.Id)
                                     .FirstOrDefault();
                foreach (string path in propertyInsertDto.PropertyImagePaths)
                {
                    var propertyImage = new PropertyImage
                    {
                        Path = path
                    };
                    if (_unitOfWork.PropertyRepository.IsDuplicatePropertyImage(propertyImage.Path))
                    {
                        throw new ApplicationException("Property image already exists can not insert.");
                    }
                    if (lastInsertedProperty != null)
                    {
                        propertyImage.PropertyId = lastInsertedProperty.Id;
                    }
                    else
                    {
                        throw new ApplicationException("An error occurred while adding the PropertyImage.");
                    }
                    _unitOfWork.PropertyRepository.AddImageToProperty(propertyImage);
                }

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while creating the property.", ex);
            }
        }

        public void UpdateProperty(PropertyUpdateDto propertyUpdateDto)//check userid,ids
        {
            var property = new DataAccess.Models.Property
            {
                Id = propertyUpdateDto.PropertyId,
                Name = propertyUpdateDto.PropertyName,
                Address = propertyUpdateDto.PropertyAddress,
                CityId = propertyUpdateDto.PropertyCityId,
                AreaInMeters = propertyUpdateDto.AreaInMeters,
                BathroomsNumber = propertyUpdateDto.BathroomsNumber,
                BedroomsNumber = propertyUpdateDto.BedroomsNumber,
                Description = propertyUpdateDto.Description,
                AdditionalNotes = propertyUpdateDto.AdditionalNotes,
                Price = propertyUpdateDto.PropertyPrice,
                PropertyStatusId = propertyUpdateDto.PropertyStatusId,
                YearBuilt = propertyUpdateDto.YearBuilt,
                UserId = propertyUpdateDto.UserId,
                PropertyTypeId = propertyUpdateDto.PropertyTypeId,
            };
            var propertyAmenities = new Amenities
            {
                HasGarage = propertyUpdateDto.HasGarage,
                Two_Stories = propertyUpdateDto.Two_Stories,
                Laundry_Room = propertyUpdateDto.Laundry_Room,
                HasPool = propertyUpdateDto.HasPool,
                HasGarden = propertyUpdateDto.HasGarden,
                HasElevator = propertyUpdateDto.HasElevator,
                HasBalcony = propertyUpdateDto.HasBalcony,
                HasParking = propertyUpdateDto.HasParking,
                HasCentralHeating = propertyUpdateDto.HasCentralHeating,
                IsFurnished = propertyUpdateDto.IsFurnished
            };
            property.AmenitiesId = _unitOfWork.PropertyRepository.GetAmenitiesId(propertyAmenities);
            ValidateProperty(property);

            var images = _unitOfWork.PropertyRepository.GetPropertyImages(propertyUpdateDto.PropertyId);
            foreach (string path in propertyUpdateDto.PropertyImagePaths)
            {               
                var existingImage = images.FirstOrDefault(i => i.PropertyId == propertyUpdateDto.PropertyId && i.Path == path);

                if (existingImage != null)
                {
                    existingImage.Path = path;
                    existingImage.PropertyId = propertyUpdateDto.PropertyId;

                    _unitOfWork.PropertyRepository.UpdateImageToProperty(existingImage);
                }
                else
                {
                    throw new InvalidOperationException("No matching image found for the specified property and path.");
                }
            }
            _unitOfWork.Save();
            try
            {
                _unitOfWork.PropertyRepository.Update(property);
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
                var property = _unitOfWork.PropertyRepository.Get(id);
                if (property == null)
                {
                    throw new KeyNotFoundException("Property not found.");
                }

                _unitOfWork.PropertyRepository.Delete(id);
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
                return _unitOfWork.PropertyRepository.GetAll()
                    .Where(property => property.Price >= minPrice && property.Price <= maxPrice)
                    .Select(property => PropertyToPropertyDto(property));
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
                return _unitOfWork.PropertyRepository.GetAll()
                    .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                                p.Description.ToLower().Contains(searchTerm) ||
                                p.Address.ToLower().Contains(searchTerm))
                    .Select(property => PropertyToPropertyDto(property)); ;
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
                var properties = _unitOfWork.PropertyRepository.GetAll().AsQueryable();

                if (!string.IsNullOrEmpty(location))
                    properties = properties.Where(p => p.Address.Contains(location));

                if (!string.IsNullOrEmpty(propertyType))
                    properties = properties.Where(p => p.PropertyType.Type.Contains(propertyType));

                if (minBedrooms.HasValue)
                    properties = properties.Where(p => p.BedroomsNumber >= minBedrooms.Value);

                if (maxBedrooms.HasValue)
                    properties = properties.Where(p => p.BedroomsNumber <= maxBedrooms.Value);

                if (minBathrooms.HasValue)
                    properties = properties.Where(p => p.BathroomsNumber >= minBathrooms.Value);

                if (maxBathrooms.HasValue)
                    properties = properties.Where(p => p.BathroomsNumber <= maxBathrooms.Value);

                return properties.Select(property => PropertyToPropertyDto(property));
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
                    ? _unitOfWork.PropertyRepository.GetAll()
                    .OrderBy(p => p.Price)
                    .Select(property => PropertyToPropertyDto(property))
                    : _unitOfWork.PropertyRepository.GetAll()
                    .OrderByDescending(p => p.Price)
                    .Select(property => PropertyToPropertyDto(property));
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
                    ? _unitOfWork.PropertyRepository.GetAll()
                    .OrderBy(p => p.DateAdded)
                    .Select(property => PropertyToPropertyDto(property))
                    : _unitOfWork.PropertyRepository.GetAll()
                    .OrderByDescending(p => p.DateAdded)
                    .Select(property => PropertyToPropertyDto(property));
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
                return _unitOfWork.PropertyRepository.GetAll()
                    .Where(p => p.UserId == userId)
                    .Select(property => PropertyToPropertyDto(property));
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
                return _unitOfWork.PropertyRepository.GetAll()
                .OrderBy(p => p.DateAdded)
                .Select(property => PropertyToPropertyDto(property));

            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving properties ordered by date.", ex);
            }
        }

        // Private method to validate property data
        private void ValidateProperty(DataAccess.Models.Property property)
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
        private PropertyDto PropertyToPropertyDto(DataAccess.Models.Property property)
        {
            return new PropertyDto
            {
                PropertyId = property.Id,
                PropertyName = property.Name,
                UserFullName = property.User.FullName,
                PropertyPrice = property.Price,
                PropertyAddress = property.Address,
                PropertyCityName = property.City.CityName,
                AreaInMeters = property.AreaInMeters,
                BathroomsNumber = property.BathroomsNumber,
                BedroomsNumber = property.BedroomsNumber,
                PropertyStatusName = property.PropertyStatus.Status,
                PropertyTypeName = property.PropertyType.Type,
                PropertyDateAdded = property.DateAdded,
                YearBuilt = property.YearBuilt,
                Description = property.Description,
                AdditionalNotes = property.AdditionalNotes,

                PropertyImagePaths = _unitOfWork.PropertyRepository.GetPropertyImages(property.Id).Select(i => i.Path),

                HasGarage = property.Amenities.HasGarage,
                Two_Stories = property.Amenities.Two_Stories,
                Laundry_Room = property.Amenities.Laundry_Room,
                HasPool = property.Amenities.HasPool,
                HasGarden = property.Amenities.HasGarden,
                HasElevator = property.Amenities.HasElevator,
                HasBalcony = property.Amenities.HasBalcony,
                HasParking = property.Amenities.HasParking,
                HasCentralHeating = property.Amenities.HasCentralHeating,
                IsFurnished = property.Amenities.IsFurnished
            };
        }
    }
}
