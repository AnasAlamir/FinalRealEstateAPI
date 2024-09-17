using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.Favorite
{
    public class FavoriteDto
    {
        public int FavoriteId { get; set; }
        public string UserName { get; set; }
        public string PropertyName { get; set; }
    }
}
