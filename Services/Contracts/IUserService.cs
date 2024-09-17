using Services.Dto.Favorite;
using Services.Dto.Inquiry;
using Services.Dto.Property;
using Services.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAllUsers(); // الحصول على جميع المستخدمين
        UserDto GetUserById(int id); // الحصول على مستخدم بناءً على المعرف
        void CreateUser(UserInsertDto userDto); // إنشاء مستخدم جديد
        void UpdateUser(UserUpdateDto userDto); // تحديث مستخدم موجود
        void DeleteUser(int id); // حذف مستخدم بناءً على المعرف
        UserDto AuthenticateUser(string email, string password); // التحقق من صحة تسجيل دخول المستخدم
        IEnumerable<PropertyDto> GetUserProperties(int userId); // الحصول على العقارات التي يمتلكها مستخدم معين
        IEnumerable<InquiryDto> GetUserInquiries(int userId); // الحصول على الاستفسارات المرسلة من قبل مستخدم معين
        IEnumerable<FavoriteDto> GetUserFavorites(int userId); // الحصول على العقارات المفضلة لمستخدم معين
        void UpdateUserPassword(int userId, string newPassword);
    }
}
