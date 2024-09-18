using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.Favorite
{
    public class FavoriteIdsDto
    {
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
    }
}
