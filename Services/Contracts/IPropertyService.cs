using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Dto.Inquiry;
using Services.Dto.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IPropertyService
    {
        IEnumerable<PropertyDto> GetAllProperties();
        PropertyDto GetPropertyById(int id);
        void CreateProperty(PropertyInsertDto propertyDto);
        void UpdateProperty(PropertyUpdateDto propertyDto);
        void DeleteProperty(int id);
        IEnumerable<PropertyDto> GetPropertiesByPrice(decimal minPrice, decimal maxPrice);
        IEnumerable<PropertyDto> SearchProperties(string searchTerm);
        IEnumerable<PropertyDto> FilterProperties(string location = null, string propertyType = null, int? minBedrooms = null, int? maxBedrooms = null, int? minBathrooms = null, int? maxBathrooms = null); // تصفية العقارات بناءً على معايير متعددة
        IEnumerable<PropertyDto> GetPropertiesOrderedByPrice(bool ascending = true); // الحصول على عقارات مرتبة بناءً على السعر
        IEnumerable<PropertyDto> GetPropertiesOrderedByDateAdded(bool ascending = true); // الحصول على عقارات مرتبة بناءً على تاريخ الإضافة
        IEnumerable<PropertyDto> GetPropertiesByUserId(int userId); // الحصول على العقارات التي يمتلكها مستخدم معين
        IEnumerable<PropertyDto> GetPropertiesOrderedByDate(); // الحصول على العقارات مرتبة حسب تاريخ الإضافة
        IEnumerable<InquiryDto> GetPropertyInquiries(int propertyId);

    }
}
