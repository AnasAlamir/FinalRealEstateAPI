using Services.Dto.Inquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IInquiryService
    {
        IEnumerable<InquiryDto> GetAllInquiries(); // الحصول على جميع الاستفسارات
        InquiryDto GetInquiryById(int id); // الحصول على استفسار بناءً على المعرف
        IEnumerable<InquiryDto> GetInquiriesByUserId(int userId); // الحصول على استفسارات بناءً على معرف المستخدم
        IEnumerable<InquiryDto> GetInquiriesByPropertyId(int propertyId); // الحصول على استفسارات بناءً على معرف العقار
        void CreateInquiry(InquiryInsertDto inquiryDto); // إنشاء استفسار جديد
        void UpdateInquiry(InquiryUpdateDto inquiryDto); // تحديث استفسار موجود
        void DeleteInquiry(int id); // حذف استفسار بناءً على المعرف
        IEnumerable<InquiryDto> GetInquiriesByDateRange(DateTime startDate, DateTime endDate); // الحصول على الاستفسارات بناءً على النطاق الزمني
    }
}
