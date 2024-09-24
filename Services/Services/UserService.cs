using DataAccess.Contracts;
using DataAccess.Models;
using Services.Contracts;
using Services.Dto.Favorite;
using Services.Dto.Inquiry;
using Services.Dto.Property;
using Services.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    internal class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public IEnumerable<UserDto> GetAllUsers()
        {
            try
            {
                return _unitOfWork.UserRepository.GetAll()
                    .Select(user=> new UserDto {
                        UserId = user.Id,
                        UserFirstName =user.F_Name,
                        UserLastName =user.L_Name,
                        UserEmail = user.Email,
                        UserPassword = user.Password,
                        UserPhoneNumber = user.PhoneNumber,
                        UserAddress = user.Address,
                        UserProfilePicture = user.ProfilePicture
                    }

                    );               
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving users.", ex);
            }
        }

        public UserDto GetUserById(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(id);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                return new UserDto
                {
                    UserId = user.Id,
                    UserFirstName = user.F_Name,
                    UserLastName = user.L_Name,
                    UserEmail = user.Email,
                    UserPassword = user.Password,
                    UserPhoneNumber = user.PhoneNumber,
                    UserAddress = user.Address,
                    UserProfilePicture = user.ProfilePicture
                };
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving the user.", ex);
            }
        }
        public void CreateUser(UserInsertDto userInsertDto)
        {
            
            try
            {
                var user = new User
                {
                    F_Name = userInsertDto.UserFirstName,
                    L_Name = userInsertDto.UserLastName,
                    Email = userInsertDto.UserEmail,
                    Password = userInsertDto.UserPassword,
                    PhoneNumber = userInsertDto.UserPhoneNumber,
                    Address = userInsertDto.UserAddress,
                    ProfilePicture = userInsertDto.UserProfilePicture
                };
                // Hash the password before saving
                user.Password = HashPassword(user.Password);
                ValidateUserDto(user);
                if (_unitOfWork.UserRepository.IsDuplicateUser(user.Email, user.PhoneNumber))
                {
                    throw new ApplicationException("User already exists can not insert.");
                }
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while creating the user.", ex);
            }
        }

        public void UpdateUser(UserUpdateDto userUpdateDto)//////////////????
        {

            try
            {
                // Optional: update password only if provided
                if (!string.IsNullOrEmpty(userUpdateDto.UserPassword))
                {
                    userUpdateDto.UserPassword = HashPassword(userUpdateDto.UserPassword);
                }

                var user = new User
                {
                    Id = userUpdateDto.UserId,
                    F_Name = userUpdateDto.UserFirstName,
                    L_Name = userUpdateDto.UserLastName,
                    Email = userUpdateDto.UserEmail,
                    Password = userUpdateDto.UserPassword,
                    PhoneNumber = userUpdateDto.UserPhoneNumber,
                    Address = userUpdateDto.UserAddress,
                    ProfilePicture = userUpdateDto.UserProfilePicture
                };
                ValidateUserDto(user);///////////
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while updating the user.", ex);
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(id);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                _unitOfWork.UserRepository.Delete(id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while deleting the user.", ex);
            }
        }

        public UserDto AuthenticateUser(string email, string password)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetAll()
                    .FirstOrDefault(u => u.Email == email && u.Password == HashPassword(password));

                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }
                return new UserDto
                {
                    UserId = user.Id,
                    UserFirstName = user.F_Name,
                    UserLastName = user.L_Name,
                    UserEmail = user.Email,
                    UserPassword = user.Password,
                    UserPhoneNumber = user.PhoneNumber,
                    UserAddress = user.Address,
                    UserProfilePicture = user.ProfilePicture
                };
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while authenticating the user.", ex);
            }
        }
        public IEnumerable<PropertyDto> GetUserProperties(int userId)
        {
            try
            {
                
                var properties = _unitOfWork.PropertyRepository.GetAll();
                var filteredProperties = properties.Where(p => p.UserId == userId);

                return filteredProperties.Select(property => new PropertyDto
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
                // سجل الاستثناء
                throw new ApplicationException("An error occurred while retrieving user's properties.", ex);
            }
        }

        public IEnumerable<InquiryDto> GetUserInquiries(int userId)
        {
            try
            {
                var inquiries = _unitOfWork.InquiryRepository.GetAll()
                    .Where(i => i.UserId == userId);
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
                throw new ApplicationException("An error occurred while retrieving user's inquiries.", ex);
            }
        }

        public IEnumerable<FavoriteDto> GetUserFavorites(int userId)
        {
            try
            {
                var favorites = _unitOfWork.FavoriteRepository.GetAll()
                    .Where(f => f.UserId == userId);
                return favorites.Select(favorite => new FavoriteDto
                {
                    FavoriteId = favorite.Id,
                    UserName = favorite.User.FullName,
                    PropertyName = favorite.Property.Name
                });
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while retrieving user's favorites.", ex);
            }
        }

        public void UpdateUserPassword(int userId, string newPassword)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                user.Password = HashPassword(newPassword);
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException("An error occurred while updating the user's password.", ex);
            }
        }

        // Private method to validate user data
        private void ValidateUserDto(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("User email is required.", nameof(user.Email));

            if (string.IsNullOrEmpty(user.Password))
                throw new ArgumentException("User password is required.", nameof(user.Password));

            // Add any additional validations as needed
        }

        // Private method to hash passwords
        private string HashPassword(string password)
        {
            // Implement password hashing logic here
            return password; // Replace this with actual hashing implementation
        }
    }
}
