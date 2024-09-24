using DataAccess.Contracts;
using DataAccess.Models;
using Services.Contracts;
using Services.Dto.Favorite;
using Services.Dto.Inquiry;
using Services.Dto.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    internal class InquiryService : IInquiryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public InquiryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<InquiryDto> GetAllInquiries()
        {
            try
            {
                return _unitOfWork.InquiryRepository.GetAll()
                    .Select(inquiry => InquiryToInquiryDto(inquiry));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching all inquiries.", ex);
            }
        }

        public InquiryDto GetInquiryById(int id)
        {
            try
            {
                var inquiry = _unitOfWork.InquiryRepository.Get(id);
                if (inquiry == null)
                {
                    throw new KeyNotFoundException("Inquiry not found.");
                }
                return InquiryToInquiryDto(inquiry);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while fetching the inquiry with ID {id}.", ex);
            }
        }

        public IEnumerable<InquiryDto> GetInquiriesByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }

            try
            {
                var inquiries = _unitOfWork.InquiryRepository.GetAll().Where(i => i.UserId == userId);
                return inquiries.
                    Select(inquiry => InquiryToInquiryDto(inquiry));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching the user's inquiries.", ex);
            }
        }

        public IEnumerable<InquiryDto> GetInquiriesByPropertyId(int propertyId)
        {
            if (propertyId <= 0)
            {
                throw new ArgumentException("Invalid property ID.", nameof(propertyId));
            }

            try
            {
                var inquiries = _unitOfWork.InquiryRepository.GetAll().Where(i => i.PropertyId == propertyId).ToList();
                return inquiries.
                    Select(inquiry => InquiryToInquiryDto(inquiry));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching the property inquiries.", ex);
            }
        }

        public void CreateInquiry(InquiryInsertDto inquiryInsertDto)///no repaeat
        {
            if (inquiryInsertDto == null)
            {
                throw new ArgumentNullException(nameof(inquiryInsertDto), "Inquiry cannot be null.");
            }
            if (_unitOfWork.PropertyRepository.Get(inquiryInsertDto.PropertyId).UserId == inquiryInsertDto.UserId)
            {
                throw new InvalidOperationException("User Can not Inquiry his property.");
            }
            if (_unitOfWork.InquiryRepository.IsDuplicateInquiry(inquiryInsertDto.UserId, inquiryInsertDto.PropertyId))
            {
                throw new ApplicationException("Inquiry already exists can not insert.");
            }
            try
            {
                var inquiry = new Inquiry
                {
                    UserId = inquiryInsertDto.UserId,
                    PropertyId = inquiryInsertDto.PropertyId,
                    DateSent = DateTime.Now,
                    Message = inquiryInsertDto.InquiryMessage
                };
                _unitOfWork.InquiryRepository.Insert(inquiry);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the inquiry.", ex);
            }
        }

        public void UpdateInquiry(InquiryUpdateDto inquiryUpdateDto)
        {           
            if (inquiryUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(inquiryUpdateDto), "Inquiry cannot be null.");
            }
            try
            {
                var existInquiry = _unitOfWork.InquiryRepository.Get(inquiryUpdateDto.InquiryId);
                var inquiry = new Inquiry
                {
                    Id = inquiryUpdateDto.InquiryId,
                    UserId = existInquiry.UserId,///null
                    PropertyId = existInquiry.PropertyId,///null
                    Message = inquiryUpdateDto.InquiryMessage
                };
                ValidateInquiryDto(inquiry);
                _unitOfWork.InquiryRepository.Update(inquiry);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the inquiry.", ex);
            }
        }

        public void DeleteInquiry(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid inquiry ID.", nameof(id));
            }

            try
            {
                var inquiry = _unitOfWork.InquiryRepository.Get(id);
                if (inquiry == null)
                {
                    throw new KeyNotFoundException("Inquiry not found.");
                }

                _unitOfWork.InquiryRepository.Delete(id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the inquiry.", ex);
            }
        }

        public IEnumerable<InquiryDto> GetInquiriesByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before the end date.");
            }

            try
            {
                var inquiries = _unitOfWork.InquiryRepository.GetAll()
                    .Where(i => i.DateSent >= startDate && i.DateSent <= endDate)
                    .ToList();
                return inquiries.
                    Select(inquiry => InquiryToInquiryDto(inquiry));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching inquiries based on the date range.", ex);
            }
        }

        private void ValidateInquiryDto(Inquiry inquiry)
        {
            if (inquiry == null)
                throw new ArgumentNullException(nameof(inquiry));

            if (string.IsNullOrEmpty(inquiry.Message))
                throw new ArgumentException("Message is required.", nameof(inquiry.Message));
        }
        private InquiryDto InquiryToInquiryDto(Inquiry inquiry)
        {
            return new InquiryDto
            {
                InquiryId = inquiry.Id,
                UserName = inquiry.User.FullName,
                PropertyName = inquiry.Property.Name,
                InquiryDateSent = inquiry.DateSent,
                InquiryMessage = inquiry.Message
            };
        }

    }
}
