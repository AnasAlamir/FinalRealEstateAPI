using Services.Dto.Favorite;
using Services.Dto.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IFavoriteService
    {
        IEnumerable<FavoriteDto> GetFavoritesByUserId(int userId); // الحصول على المفضلات بناءً على معرف المستخدم
        void AddToFavorites(FavoriteInsertDto favoriteInsertDto); // إضافة إلى المفضلات
        void RemoveFromFavorites(int userId, int propertyId); // إزالة من المفضلات
        bool IsFavorite(int userId, int propertyId); // التحقق مما إذا كان العقار مفضلاً
        IEnumerable<PropertyDto> GetFavoriteProperties(int userId); // الحصول على خصائص المفضلة
        void ClearFavorites(int userId); // مسح المفضلات
    }
}
