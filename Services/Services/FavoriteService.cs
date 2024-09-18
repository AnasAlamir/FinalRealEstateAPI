using DataAccess.Contracts;
using DataAccess.Models;
using Services.Contracts;
using Services.Dto.Favorite;
using Services.Dto.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    internal class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FavoriteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<FavoriteDto> GetFavoritesByUserId(int userId)
        {
            try
            {
                var favorites = _unitOfWork.FavoriteRepository.GetAll()
                    .Where(f => f.UserId == userId);

                if (favorites == null || !favorites.Any())
                {
                    throw new KeyNotFoundException("No favorites found for this user.");
                }

                return favorites.Select(favorite => new FavoriteDto
                {
                    FavoriteId = favorite.Id,
                    UserName = favorite.User.FullName,
                    PropertyName = favorite.Property.Name
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving favorites.", ex);
            }
        }

        public void AddToFavorites(FavoriteInsertDto favoriteInsertDto)
        {

            try
            {
                if (_unitOfWork.PropertyRepository.Get(favoriteInsertDto.PropertyId).UserId == favoriteInsertDto.UserId)
                {
                    throw new InvalidOperationException("User Can not add his property to Favorite.");
                }

                if (IsFavorite(favoriteInsertDto.UserId, favoriteInsertDto.PropertyId))
                {
                    throw new InvalidOperationException("Property is already in favorites.");
                }

                var favorite = new Favorite
                {
                    UserId = favoriteInsertDto.UserId,
                    PropertyId = favoriteInsertDto.PropertyId
                };
                ValidateFavorite(favorite);
                _unitOfWork.FavoriteRepository.Insert(favorite);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding to favorites.", ex);
            }
        }

        public void RemoveFromFavorites(int userId, int propertyId)
        {
            try
            {
                var favorite = _unitOfWork.FavoriteRepository.GetAll()
                    .FirstOrDefault(f => f.UserId == userId && f.PropertyId == propertyId);

                if (favorite == null)
                {
                    throw new KeyNotFoundException("Favorite not found.");
                }

                _unitOfWork.FavoriteRepository.Delete(favorite.Id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while removing from favorites.", ex);
            }
        }

        public bool IsFavorite(int userId, int propertyId)
        {
            try
            {
                return _unitOfWork.FavoriteRepository.GetAll()
                    .Any(f => f.UserId == userId && f.PropertyId == propertyId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while checking if the property is a favorite.", ex);
            }
        }

        public IEnumerable<PropertyDto> GetFavoriteProperties(int userId)
        {
            try
            {
                var favorites = _unitOfWork.FavoriteRepository.GetAll()
                    .Where(f => f.UserId == userId);

                var propertyIds = favorites
                     .Select(f => f.PropertyId);

                var properties = _unitOfWork.PropertyRepository.GetAll()
                    .Where(p => propertyIds.Contains(p.Id));

                return properties.Select(property=> new PropertyDto
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
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving favorite properties.", ex);
            }
        }

        public void ClearFavorites(int userId)
        {
            try
            {
                var favorites = _unitOfWork.FavoriteRepository.GetAll()
                   .Where(f => f.UserId == userId)
                   .ToList();

                foreach (var favorite in favorites)
                {
                    _unitOfWork.FavoriteRepository.Delete(favorite.Id);
                }

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while clearing favorites.", ex);
            }
        }

        // Private method to validate favorite
        private void ValidateFavorite(Favorite favoriteDto)
        {
            if (favoriteDto == null)
                throw new ArgumentNullException(nameof(favoriteDto));

            if (favoriteDto.UserId <= 0)
                throw new ArgumentException("Invalid user ID.", nameof(favoriteDto.UserId));

            if (favoriteDto.PropertyId <= 0)
                throw new ArgumentException("Invalid property ID.", nameof(favoriteDto.PropertyId));

            // Add any additional validations as needed
        }
    }
}
